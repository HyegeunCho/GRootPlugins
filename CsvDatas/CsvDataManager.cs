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
                            var values = line.Split(',').ToList();
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

    }

}