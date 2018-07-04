using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecs
{
    public class Component
    {
        private bool isActive = true;
        public GameObject gameObject = null;
        public Transform transform = null;

        

        public virtual void Start()
        {
            return;
        }

        public virtual void Update()
        {
            return;
        }

        public virtual void Render()
        {
            return;
        }

        public Component GetComponent(Type type)
        {
            return gameObject.GetComponent(type);
        }

        public Component GetComponent<T>()
        {
            return gameObject.GetComponent(typeof(T));
        }

        public Component GetComponentInChildren<T>()
        {

            return null;
        }

        public Component AddComponent(Component component)
        {
            return gameObject.AddComponent(component);
        }

        public Component AddComponent<T>()
        {
            if (typeof(Component).IsAssignableFrom(typeof(T)))
            {
            
                var obj = (T)Activator.CreateInstance(typeof(T));
                return AddComponent(obj as Component);
            }
            
            return null;    
        }

        public bool IsActive()
        {
            return this.isActive;
        }

        public void SetActive(bool active)
        {
            this.isActive = active;
            return;
        }

        public String Tag()
        {
            return this.gameObject.Tag();
        }


    }

}
