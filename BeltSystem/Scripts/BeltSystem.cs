using System;
using System.Collections.Generic;
using System.Linq;
using GRootPlugins.ObjectPooling;
using UnityEngine;

namespace GRootPlugins.BeltSystem
{
    public class BeltSystem : MonoBehaviour
    {
        public enum EDirection
        {
            Left, 
            Right, 
            Up, 
            Down,
        }

        public Rect Screen = new Rect(-360f, 0f, 720f, 640f);

        public float MiddleX => Screen.x + (Screen.width / 2f);
        public float MiddleY => Screen.y + (Screen.height / 2f);
        
        
        public bool IsRepeat = true;
        public bool IsSequential = true;
        
        public EDirection Direction = EDirection.Left;
        public float Speed = 1f;
        public BeltObject BeltPrefab;

        private Queue<BeltObject> _beltObjects;

        private void Awake()
        {
            if (_beltObjects == null) _beltObjects = new Queue<BeltObject>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // FillScreen();
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var obj in _beltObjects)
            {
                obj.transform.Translate(Vector3.left * Speed);
            }
            
            
            BeltObject frontObject = _beltObjects.Count == 0 ? null : _beltObjects.Peek();
            if (frontObject != null)
            {
                if (!IsInScreen(frontObject))
                {
                    ObjectPoolingManager.Instance.ReturnObject<BeltObject>(_beltObjects.Dequeue());
                }
            }
            
            BeltObject lastCreated = _beltObjects.Count == 0 ? null : _beltObjects.Last();
            if (lastCreated != null)
            {
                if (IsInScreen(lastCreated))
                {
                    Vector2 targetPos = GetNextPos(lastCreated, BeltPrefab);
                    var created = CreateBeltObject(targetPos); 
                    if (created == null) return;
                    _beltObjects.Enqueue(created);
                }
            }
            else
            {
                Vector2 targetPos = GetNextPos(lastCreated, BeltPrefab);
                var created = CreateBeltObject(targetPos); 
                if (created == null) return;
                _beltObjects.Enqueue(created); 
            }

        }

        private Vector2 GetNextPos(BeltObject inLastBeltObj, BeltObject inBeltPrefab)
        {
            Vector2 result = new Vector2();
            
            if (inLastBeltObj == null)
            {
                result.x = -360f + (inBeltPrefab.Width / 2f);
                result.y = inBeltPrefab.Height / 2f;
            }
            else
            {
                result.x = inLastBeltObj.transform.position.x + inBeltPrefab.Width;
                result.y = inBeltPrefab.Height / 2f;
            }

            return result;
        }

        private bool IsInScreen(BeltObject inObject)
        {
            var objRect = inObject.RectValue;

            Vector2 minXminY = new Vector2(objRect.x, objRect.y);
            Vector2 maxXminY = new Vector2(objRect.x + objRect.width, objRect.y);
            Vector2 minXmaxY = new Vector2(objRect.x, objRect.y + objRect.height);
            Vector2 maxXmaxY = new Vector2(objRect.x + objRect.width, objRect.y + objRect.height);

            return Screen.Contains(minXminY) 
                || Screen.Contains(minXmaxY) 
                || Screen.Contains(maxXminY) 
                || Screen.Contains(maxXmaxY);
        }
        
        private void FillScreen()
        {

            BeltObject lastCreated = null;
            do
            {
                lastCreated = _beltObjects.Count == 0 ? null : _beltObjects.Last();
                Vector2 targetPos = GetNextPos(lastCreated, BeltPrefab);
                var created = CreateBeltObject(targetPos); 
                if (created == null) break;
                _beltObjects.Enqueue(created);

                if (_beltObjects.Count > 100) break;
            } while (lastCreated == null || IsInScreen(lastCreated));
        }

        private BeltObject CreateBeltObject(Vector2 inTargetPos)
        {
            BeltObject result = ObjectPoolingManager.Instance.GetObject<BeltObject>(() =>
            {
                return Instantiate(BeltPrefab, transform);
            }) as BeltObject;

            if (result == null) throw new Exception("Cannot Generate BeltObject");
            result.transform.position = new Vector3(inTargetPos.x, inTargetPos.y, 0f);

            return result;
        }
    }

}
