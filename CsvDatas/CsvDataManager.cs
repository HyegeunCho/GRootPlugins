using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GRootPlugins.Singleton;
using UnityEngine;

namespace GRootPlugins.CsvDatas 
{
    public class CsvDataManager : MonoSingleton<CsvDataManager>
    {
        [SerializeField] private List<TextAsset> _csvs;

        private Dictionary<Type, Dictionary<int, BaseCsvModel>> _csvDatas;
        
        public override void Awake()
        {
            SetUpInstance(this);
            base.Awake();
            
            Initialize();
        }

        public void Initialize()
        {
            if (_csvDatas == null) _csvDatas = new Dictionary<Type, Dictionary<int, BaseCsvModel>>();

            foreach (var csv in _csvs)
            {
                string name = csv.name;
                Type type = Type.GetType($"CsvDatas.{name},Assembly-CSharp");

                if (type == null) continue;
                
                if (_csvDatas.ContainsKey(type)) _csvDatas[type].Clear();
                _csvDatas[type] = LoadCsvAsset(type, csv);
            }
        }

        private Dictionary<int, BaseCsvModel> LoadCsvAsset(Type inType, TextAsset inAsset)
        {
            List<string> header = null;
            Dictionary<int, BaseCsvModel> data = new Dictionary<int, BaseCsvModel>();
            
            using (Stream s = new MemoryStream(inAsset.bytes))
            {
                using (StreamReader reader = new StreamReader(s))
                {
                    int i = 0;
                    do
                    {
                        string line = reader.ReadLine();
                        if (i == 0)
                        {
                            header = line.Split(',').ToList();
                        }
                        else
                        {
                            //var values = line.Split(',').ToList();
                            var values = CustomSplit(line);
                            BaseCsvModel obj = Activator.CreateInstance(inType) as BaseCsvModel;
                            if (obj == null) continue;
                            
                            obj.Parse(header, values);
                            data.Add(obj.ID, obj);
                        }

                        i++;
                    } while (!reader.EndOfStream);
                }
            }

            return data;
        }

        private List<string> CustomSplit(string inValue)
        {
            List<string> result = new List<string>();

            var buffer = inValue.Split(',');

            bool isNested = false;
            string aggregateBuffer = string.Empty;
            for (int i = 0; i < buffer.Length; i++)
            {
                var curBuffer = buffer[i];
                if (!isNested)
                {
                    if (!curBuffer.Contains('\"'))
                    {
                        result.Add(curBuffer);
                        continue;    
                    }
                    
                    isNested = true;
                    curBuffer = curBuffer.Remove(curBuffer.IndexOf('\"'), 1);
                    aggregateBuffer = aggregateBuffer == string.Empty ? curBuffer : $"{aggregateBuffer},{curBuffer}";
                    continue;
                }

                if (curBuffer.Contains('\"'))
                {
                    isNested = false;
                    curBuffer = curBuffer.Remove(curBuffer.IndexOf('\"'), 1);
                    aggregateBuffer = aggregateBuffer == string.Empty ? curBuffer : $"{aggregateBuffer},{curBuffer}";
                    result.Add(aggregateBuffer);
                    aggregateBuffer = string.Empty;
                    continue;
                }
                
                aggregateBuffer = aggregateBuffer == string.Empty ? curBuffer : $"{aggregateBuffer},{curBuffer}";
            }

            return result;
        }
        
        public T GetData<T>(int inID) where T : BaseCsvModel
        {
            Type type = typeof(T);
            if (!_csvDatas.ContainsKey(type)) return null;
            if (_csvDatas[type] == null || _csvDatas[type].Count == 0) return null;

            if (!_csvDatas[type].TryGetValue(inID, out BaseCsvModel result))
            {
                return null;
            }

            return result as T;
        }

        public T GetData<T>(Predicate<T> inPredicate) where T : BaseCsvModel
        {
            Type type = typeof(T);
            if (!_csvDatas.ContainsKey(type)) return null;
            if (_csvDatas[type] == null || _csvDatas[type].Count == 0) return null;

            var result = _csvDatas[type].Values.FirstOrDefault(v => inPredicate(v as T));
            return result as T;
        }

        public void ForEach<T>(Action<T> inDelegate) where T : BaseCsvModel
        {
            Type type = typeof(T);
            if (!_csvDatas.ContainsKey(type)) return;
            if (_csvDatas[type] == null || _csvDatas[type].Count == 0) return;

            foreach (var baseCsvModel in _csvDatas[type].Values)
            {
                var value = (T)baseCsvModel;
                if (value == null) continue;
                inDelegate.Invoke(value);
            }
        }
    }
}
