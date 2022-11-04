using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HGPlugins.Singleton;
using UnityEngine;

namespace GRootPlugins.ObjectPooling
{
    public sealed class ObjectPoolingManager : MonoSingleton<ObjectPoolingManager>
    {
        private Dictionary<Type, Queue<IPoolingObject>> _pool;

        public Queue<IPoolingObject> GetPool(Type inType)
        {
            if (_pool == null) _pool = new Dictionary<Type, Queue<IPoolingObject>>();

            bool isFetchSuccess = _pool.TryGetValue(inType, out var result);
            if (isFetchSuccess == false || result == null)
            {
                result = new Queue<IPoolingObject>();
                _pool.Add(inType, result);
            }
            
            return result;
        }
        
        public IPoolingObject GetObject<T>(Func<T> inGenerator = null) where T : IPoolingObject
        {
            Queue<IPoolingObject> pool = GetPool(typeof(T));

            if (pool.Count < 1)
            {
                if (inGenerator == null) return null;
                T newObj = inGenerator.Invoke();
                newObj.New();
                return newObj;
            }

            T result = (T)pool.Dequeue();
            if (result == null) return null;

            // SlotMachineMain.Instance.Board.ForEach(v =>
            // {
            //     BlockView target = result as BlockView;
            //     if (target == null) return;
            //     if (System.Object.ReferenceEquals(v, target))
            //     {
            //         Debug.LogError($"{target.name}");
            //     }
            // });
            
            result.New();
            return result;
        }

        public void ReturnObject<T>(T inObject) where T : IPoolingObject
        {
            if (inObject.IsReturned) return;
            Queue<IPoolingObject> pool = GetPool(typeof(T));

            // bool isException = pool.ToList().Exists(v => System.Object.ReferenceEquals(v, inObject));
            // if (isException) Debug.LogError("");
            
            inObject.Free();
            pool.Enqueue(inObject);
        }

    }
    
}
