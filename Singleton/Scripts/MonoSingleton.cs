using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRootPlugins.Singleton
{
    public class MonoSingleton<T> : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    string name = $"[SINGLETON] {typeof(T).Name}";
                    GameObject go = new GameObject(name, typeof(T));
                    go.transform.position = Vector3.zero;
                    _instance = go.GetComponent<T>();
                }
                return _instance;
            }
        }

        protected void SetUpInstance(T inT)
        {
            _instance = inT;
        }
        
        public virtual void Awake()
        {
            DontDestroyOnLoad(this);

            
        }
    }    
}

