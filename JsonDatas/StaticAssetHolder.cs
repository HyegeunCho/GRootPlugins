using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace GRootPlugins.JsonDatas
{
    public class StaticAssetHolder : MonoBehaviour
    {
        public enum EType
        {
            Unified,
            Separated
        }
        
        public EType HolderType = EType.Unified;
        
        // Separated Asset
        [SerializeField] private List<TextAsset> _idJsonAssets;
        [SerializeField] private List<TextAsset> _keyJsonAssets;
        
        // Unified Asset
        [SerializeField] private TextAsset _jsonAsset;
        [SerializeField] private List<string> _keyStatics;
    
        private Dictionary<Type, Dictionary<int, BaseJsonModel>> _idJsonDatas;
        private Dictionary<Type, Dictionary<string, BaseJsonModel>> _keyJsonDatas;

        public void Clear()
        {
            _idJsonDatas ??= new Dictionary<Type, Dictionary<int, BaseJsonModel>>();
            _idJsonDatas.Clear();
            
            _keyJsonDatas ??= new Dictionary<Type, Dictionary<string, BaseJsonModel>>();
            _keyJsonDatas.Clear();
        }

        public void Initialize()
        {
            Clear();
            
            if (HolderType == EType.Unified)
            {
                LoadUnifiedAsset();
            }
            else
            {
                LoadSeparatedAsset();
            }
        }
        
        #region Load Separated Json Asset

        private void LoadSeparatedAsset()
        {
            foreach (var json in _idJsonAssets)
            {
                string name = json.name;
                Type type = Type.GetType($"JsonDatas.{name},Assembly-CSharp");
                
                if (type == null) continue;

                if (_idJsonDatas.ContainsKey(type)) _idJsonDatas[type].Clear();
                _idJsonDatas[type] = ParseIntoId(type, json);
            }

            foreach (var json in _keyJsonAssets)
            {
                string name = json.name;
                Type type = Type.GetType($"JsonDatas.{name},Assembly-CSharp");
                
                if (type == null) continue;

                if (_keyJsonDatas.ContainsKey(type)) _keyJsonDatas[type].Clear();
                _keyJsonDatas[type] = ParseIntoKey(type, json);
            }
        }
        
        #endregion Load Separated Json Asset
        
        
        #region Load Unified Json Asset

        private void LoadUnifiedAsset()
        {
            JObject jsonData = JObject.Parse(_jsonAsset.text);

            foreach (var pair in jsonData)
            {
                string name = pair.Key;
                JArray jsonArr = (JArray) pair.Value;
                if (jsonArr == null) continue;
                
                Type staticType = GetStaticType(name);
                if (staticType == null) continue;
                
                bool isKeyStatic = _keyStatics.Contains(name);
                if (isKeyStatic) _keyJsonDatas.TryAdd(staticType, ParseIntoKey(staticType, jsonArr));
                else _idJsonDatas.TryAdd(staticType, ParseIntoId(staticType, jsonArr));
            }
        }
        
        private Type GetStaticType(string inName)
        {
            Type type = Type.GetType($"JsonDatas.{name},Assembly-CSharp");
            if (type == null) return null;
            return type;
        }

        private Dictionary<int, BaseJsonModel> ParseIntoId(Type inType, TextAsset inAsset)
        {
            JArray staticData = JArray.Parse(inAsset.text);
            return ParseIntoId(inType, staticData);
        }
        
        private Dictionary<int, BaseJsonModel> ParseIntoId(Type inType, JArray inDatas)
        {
            Dictionary<int, BaseJsonModel> result = new Dictionary<int, BaseJsonModel>();
            if (inDatas == null) return result;
            
            foreach (JObject jsonObj in inDatas)
            {
                BaseJsonModel obj = Activator.CreateInstance(inType) as BaseJsonModel;
                obj.Parse(jsonObj);

                if (result.ContainsKey(obj.ID))
                {
                    Debug.LogError($"[{inType}] {obj.ID} 데이터가 이미 파싱되었습니다.");
                    continue;
                }
                result.Add(obj.ID, obj);
            }
            return result;
        }

        private Dictionary<string, BaseJsonModel> ParseIntoKey(Type inType, TextAsset inAsset)
        {
            JArray staticData = JArray.Parse(inAsset.text);
            return ParseIntoKey(inType, staticData);
        }
        
        private Dictionary<string, BaseJsonModel> ParseIntoKey(Type inType, JArray inDatas)
        {
            Dictionary<string, BaseJsonModel> result = new Dictionary<string, BaseJsonModel>();
            if (inDatas == null) return result;
            
            foreach (JObject jsonObj in inDatas)
            {
                BaseJsonModel obj = Activator.CreateInstance(inType) as BaseJsonModel;
                obj.Parse(jsonObj);

                if (result.ContainsKey(obj.KEY))
                {
                    Debug.LogError($"[{inType}] {obj.KEY} 데이터가 이미 파싱되었습니다.");
                    continue;
                }
                result.Add(obj.KEY, obj);
            }
            return result;
        }
        
        
        #endregion Load Unified Json Asset
        
        
        
        

        #region Getters

        public Dictionary<Type, Dictionary<int, BaseJsonModel>> GetIdJsonData()
        {
            _idJsonDatas ??= new Dictionary<Type, Dictionary<int, BaseJsonModel>>();
            return _idJsonDatas;
        }
        
        public Dictionary<Type, Dictionary<string, BaseJsonModel>> GetKeyJsonData()
        {
            _keyJsonDatas ??= new Dictionary<Type, Dictionary<string, BaseJsonModel>>();
            return _keyJsonDatas;
        }

        #endregion Getters
    }
}
