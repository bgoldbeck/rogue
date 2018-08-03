//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;

namespace Ecs
{
    /// <summary>
    /// A base class for Components in the Entity Component System.
    /// </summary>
    public class Component
    {
        private bool isActive = false;
        public GameObject gameObject = null;
        public Transform transform = null;

        /// <summary>
        /// Auto property to retrieve the name of the GameObject this component is attached to.
        /// </summary>
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

        /// <summary>
        /// Start is called when the component is added to a GameObject.
        /// </summary>
        public virtual void Start()
        {
            return;
        }

        /// <summary>
        /// EarlyUpdate is called on each frame as the first update method.
        /// </summary>
        public virtual void EarlyUpdate()
        {
            return;
        }

        /// <summary>
        /// Update is called on each frame after EarlyUpdate.
        /// </summary>
        public virtual void Update()
        {
            return;
        }

        /// <summary>
        /// LateUpdate is called on each frame after Update.
        /// </summary>
        public virtual void LateUpdate()
        {
            return;
        }

        /// <summary>
        /// Render is called on each frame after LateUpdate.
        /// </summary>
        public virtual void Render()
        {
            return;
        }

        /// <summary>
        /// OnEnable is called when the component is first attached to a GameObject and 
        /// each time the component is subsequently reactivated by SetActive.
        /// </summary>
        public virtual void OnEnable()
        {
            return;
        }

        /// <summary>
        /// OnDisable is called each time the component is subsequently deactivated by SetActive.
        /// </summary>
        public virtual void OnDisable()
        {
            return;
        }

        /// <summary>
        /// OnResize is called when the console window was resized by the user.
        /// </summary>
        public virtual void OnResize()
        {
            return;
        }

        /// <summary>
        /// OnDestroy is called when the GameObject is removed from the System.
        /// </summary>
        public virtual void OnDestroy()
        {
            return;
        }

        /// <summary>
        /// Calls a method by a name on all components of type 'T' on the GameObject this component is
        /// attached to.
        /// </summary>
        /// <typeparam name="T">The type of component to find on this GameObject.</typeparam>
        /// <param name="name">The name of the function to call.</param>
        /// <param name="parameters">The parameters to use for the function call.</param>
        public void SendMessage<T>(string name, object[] parameters = null)
        {
            this.gameObject.SendMessage<T>(name, parameters);
            return;
        }

        /// <summary>
        /// Retrieve a component from the GameObject this component is attached to.
        /// </summary>
        /// <param name="type">The type of component to retrieve.</param>
        /// <returns></returns>
        public Component GetComponent(Type type)
        {
            return gameObject.GetComponent(type);
        }

        /// <summary>
        /// Retrieve a component from the GameObject this component is attached to.
        /// </summary>
        /// <typeparam name="T">The type of component to retrieve.</typeparam>
        /// <returns></returns>
        public Component GetComponent<T>()
        {
            return gameObject.GetComponent(typeof(T));
        }

        /// <summary>
        /// Returns a single component of type T on this GameObject or any of its children.
        /// </summary>
        /// <returns>Component The component found, if any.</returns>
        public Component GetComponentInChildren<T>()
        {
            return gameObject.GetComponentInChildren<T>();
        }

        /// <summary>
        /// Add a component to the System.
        /// </summary>
        /// <param name="component">The component to add.</param>
        /// <returns>
        /// The component that was added.
        /// </returns>
        public Component AddComponent(Component component)
        {
            return gameObject.AddComponent(component);
        }

        /// <summary>
        /// Add a component to the System.
        /// </summary>
        /// <typeparam name="T">The type of component to add.</typeparam>
        /// <returns>
        /// The component that was added.
        /// </returns>
        public Component AddComponent<T>()
        {
            return gameObject.AddComponent<T>();  
        }

        /// <summary>
        /// Determines the active state of this component.
        /// </summary>
        /// <returns>
        /// True, if the component is active.
        /// </returns>
        public bool IsActive()
        {
            return this.isActive;
        }

        /// <summary>
        /// Set the active state of this component. Responsible for calling OnEnable and
        /// OnDisable respectively.
        /// </summary>
        /// <param name="state">The new active state.</param>
        public void SetActive(bool state)
        {
            if (this.isActive == state)
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

            this.isActive = state;
            return;
        }

    }

}
