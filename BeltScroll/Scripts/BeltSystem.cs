using System;
using System.Collections;
using System.Collections.Generic;
using HGPlugins.ObjectPooling;
using UnityEngine;

public class BeltSystem : MonoBehaviour
{
    public enum EDirection
    {
        Left, 
        Right, 
        Up, 
        Down,
    }

    public float MinX = -360f;
    public float MinY = 640f;
    public float MaxY = 0f;
    public float MaxX = 360f;
    
    public bool IsRepeat = true;
    public bool IsSequential = true;
    
    public EDirection Direction = EDirection.Left;
    public float Speed = 1f;
    public BeltObject[] BeltPrefabs;

    private List<BeltObject> _beltObjects;

    private void Awake()
    {
        if (_beltObjects == null) _beltObjects = new List<BeltObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ObjectPoolingManager.Instance.GetObject<BeltObject>(() =>
        {
            return Instantiate(BeltPrefabs[0], transform);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
