using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using GRootPlugins.Singleton;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GRootPlugins.JsonDatas
{
    [RequireComponent(typeof(StaticAssetHolder))]
    public class JsonDataManager : MonoSingleton<JsonDataManager>
    {
        private StaticAssetHolder _assetHolder;
        
        private Dictionary<Type, Dictionary<int, BaseJsonModel>> _idJsonDatas;
        private Dictionary<Type, Dictionary<string, BaseJsonModel>> _keyJsonDatas;

        public override void Awake()
        {
            SetUpInstance(this);
            base.Awake();
        }

        private void Start()
        {
            _assetHolder ??= GetComponent<StaticAssetHolder>();
            _assetHolder.Initialize();
            
            Initialize();
        }

        public void Initialize()
        {
            _idJsonDatas = _assetHolder.GetIdJsonData();
            _keyJsonDatas = _assetHolder.GetKeyJsonData();
        }
        
        public T GetData<T>(int inID) where T : BaseJsonModel
        {
            Type type = typeof(T);
            if (!_idJsonDatas.ContainsKey(type)) return null;
            if (_idJsonDatas[type] == null || _idJsonDatas[type].Count == 0) return null;

            if (!_idJsonDatas[type].TryGetValue(inID, out BaseJsonModel result))
            {
                return null;
            }

            return result as T;
        }

        public T GetData<T>(string inKEY) where T : BaseJsonModel
        {
            Type type = typeof(T);
            if (!_keyJsonDatas.ContainsKey(type)) return null;
            if (_keyJsonDatas[type] == null || _keyJsonDatas[type].Count == 0) return null;

            if (!_keyJsonDatas[type].TryGetValue(inKEY, out BaseJsonModel result))
            {
                return null;
            }

            return result as T;
        }

        public T GetData<T>(Predicate<T> inPredicate) where T : BaseJsonModel
        {
            Type type = typeof(T);

            if (_idJsonDatas.ContainsKey(type) && _idJsonDatas[type] != null)
            {
                var result = _idJsonDatas[type].Values.FirstOrDefault(v => inPredicate(v as T));
                return result as T;    
            }
            
            if (_keyJsonDatas.ContainsKey(type) && _keyJsonDatas[type] != null)
            {
                var result = _keyJsonDatas[type].Values.FirstOrDefault(v => inPredicate(v as T));
                return result as T;    
            }

            return null;
        }

        public void ForEach<T>(Action<T> inDelegate) where T : BaseJsonModel
        {
            Type type = typeof(T);

            if (_idJsonDatas.ContainsKey(type) && _idJsonDatas[type] != null)
            {
                foreach (var baseCsvModel in _idJsonDatas[type].Values)
                {
                    var value = (T)baseCsvModel;
                    if (value == null) continue;
                    inDelegate.Invoke(value);
                }
            }
            
            if (_keyJsonDatas.ContainsKey(type) && _keyJsonDatas[type] != null)
            {
                foreach (var baseCsvModel in _keyJsonDatas[type].Values)
                {
                    var value = (T)baseCsvModel;
                    if (value == null) continue;
                    inDelegate.Invoke(value);
                }
            }
        }

        public List<T> GetDatas<T>(Predicate<T> inPredicate) where T : BaseJsonModel
        {
            Type type = typeof(T);

            if (_idJsonDatas.ContainsKey(type) && _idJsonDatas[type] != null)
            {
                return _idJsonDatas[type].Values.Where(v => inPredicate(v as T)).Select(v => v as T).ToList();    
            }

            if (_keyJsonDatas.ContainsKey(type) && _keyJsonDatas[type] != null)
            {
                return _keyJsonDatas[type].Values.Where(v => inPredicate(v as T)).Select(v => v as T).ToList();
            }

            return null;
        }
        
        public List<T> GetList<T>() where T : BaseJsonModel
        {
            Type type = typeof(T);
            
            if (_idJsonDatas.ContainsKey(type) && _idJsonDatas[type] != null)
            {
                return _idJsonDatas[type].Values.Select(item => item as T).Where(item => item !=null).ToList();    
            }

            if (_keyJsonDatas.ContainsKey(type) && _keyJsonDatas[type] != null)
            {
                return _keyJsonDatas[type].Values.Select(item => item as T).Where(item => item !=null).ToList();
            }

            return null;
        }
        
        public int GetCount<T>() where T : BaseJsonModel
        {
            Type type = typeof(T);
            
            if (_idJsonDatas.ContainsKey(type) && _idJsonDatas[type] != null)
            {
                return _idJsonDatas[type].Values.Count;    
            }

            if (_keyJsonDatas.ContainsKey(type) && _keyJsonDatas[type] != null)
            {
                return _keyJsonDatas[type].Values.Count;
            }
            return 0;
        }
    }

}
