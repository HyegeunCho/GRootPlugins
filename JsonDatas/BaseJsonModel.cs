using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace GRootPlugins.JsonDatas
{
    public class BaseJsonModel
    {
        public int ID = 0;
        public string KEY = string.Empty;

        public bool IsKeyStatic => !string.IsNullOrEmpty(KEY);
        public bool IsIDStatic => ID > 0;
        
        
        public virtual void Parse(JObject inJson)
        {
            foreach (var pair in inJson)
            {
                string key = pair.Key;
                JToken value = pair.Value;

                var member = GetType().GetField(key);
                if (member == null) continue;

                if (member.FieldType == typeof(int))
                {
                    int temp = default(int);
                    try
                    {
                        temp = pair.Value.ToObject<int>();
                    }
                    catch
                    {
                        // ignored
                    }

                    member.SetValue(this, temp);
                }
                else if (member.FieldType == typeof(float))
                {
                    float temp = default(float);
                    try
                    {
                        temp = pair.Value.ToObject<float>();
                    }
                    catch
                    {
                        // ignored
                    }
                    member.SetValue(this, temp);
                }
                else
                {
                    string temp = string.Empty;
                    try
                    {
                        temp = pair.Value.ToObject<string>();
                    }
                    catch
                    {
                        // ignored
                    }
                    member.SetValue(this, temp);
                }
            }
        }
    }
}
