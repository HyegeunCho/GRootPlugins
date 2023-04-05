using System.Collections.Generic;
using UnityEngine;

namespace GRootPlugins.CsvDatas
{
    public class BaseCsvModel
    {
        public int ID;
        
        public virtual void Parse(List<string> inHeader, List<string> inData)
        {
            int length = Mathf.Min(inHeader.Count, inData.Count);

            for (int i = 0; i < length; i++)
            {
                string name = inHeader[i];
                
                var member = GetType().GetField(name);
                if (member == null) continue;

                string value = inData[i];

                if (member.FieldType == typeof(int))
                {
                    member.SetValue(this, int.Parse(value));
                }
                else if (member.FieldType == typeof(float))
                {
                    member.SetValue(this, float.Parse(value));
                }
                else
                {
                    member.SetValue(this, value);
                }
            }
        }
    }
    
}
