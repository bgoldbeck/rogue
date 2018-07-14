//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecs
{
    public class Component
    {
        private bool isActive = false;
        public GameObject gameObject = null;
        public Transform transform = null;

        public String Name
        {
            get
            {
                return gameObject.Name;
            }
            set
            {
                gameObject.Name = value;
                return;
            }
        }

        public virtual void Start()
        {
            return;
        }

        public virtual void Update()
        {
            return;
        }
        public virtual void LateUpdate()
        {
            return;
        }

        public virtual void Render()
        {
            return;
        }

        public virtual void OnEnable()
        {
            return;
        }

        public virtual void OnDisable()
        {
            return;
        }

        public virtual void OnResize()
        {
            return;
        }

        public void SendMessage<T>(string name, object[] parameters = null)
        {
            this.gameObject.SendMessage<T>(name, parameters);
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
            return gameObject.AddComponent<T>();  
        }

        public bool IsActive()
        {
            return this.isActive;
        }

        public void SetActive(bool active)
        {
            if (this.isActive == active)
            {
                return;
            }

            if (!this.isActive)
            {
                OnEnable();
            }
            else
            {
                OnDisable();
            }

            this.isActive = active;
            return;
        }


    }

}
