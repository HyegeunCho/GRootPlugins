using GRootPlugins.ObjectPooling;
using UnityEngine;

namespace GRootPlugins.BeltSystem
{
    public class BeltObject : MonoBehaviour, IPoolingObject
    {
        public float Width;
        public float Height;

        public Rect RectValue => new Rect(transform.position.x - (Width / 2f), transform.position.y - (Height / 2f), Width,
            Height); 

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

}