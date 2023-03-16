using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GRootPlugins.HPBar
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _bg;
        [SerializeField] private SpriteRenderer _hpBar;
        [SerializeField] private TextMeshPro TXT_Hp;
    
        public void SetHP(float inValue, float inMaxValue)
        {
            inMaxValue = Mathf.Max(1, inMaxValue);
            float curtValue = Mathf.Clamp(inValue, 0, inMaxValue);
            float sizeX = ((float)curtValue / (float)inMaxValue) * _bg.size.x;
            _hpBar.size = new Vector2(sizeX, _hpBar.size.y);
            TXT_Hp.text = $"{(int)inValue}";
        }
    }

}
