using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HGPlugins.ObjectPooling
{
    public interface IPoolingObject
    {
        public bool IsReturned { get; }
        public void New();
        public void Free();
    }    
}

