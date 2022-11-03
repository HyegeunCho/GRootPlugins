using System.Collections;
using System.Collections.Generic;
using HGPlugins.ObjectPooling;
using UnityEngine;

public class BeltObject : MonoBehaviour, IPoolingObject
{
    public float Width;
    public float Height;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool IsReturned { private set; get; }
    public void New()
    {
        gameObject.SetActive(true);
        IsReturned = false;
        
    }

    public void Free()
    {
        gameObject.SetActive(false);
        IsReturned = false;
    }
}
