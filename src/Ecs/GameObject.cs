//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecs
{    
    public class GameObject
    {
        private static Dictionary<String, List<GameObject>> gameObjectsTagMap= new Dictionary<String, List<GameObject>>();
        private static Dictionary<int, GameObject> gameObjectsIdMap = new Dictionary<int, GameObject>();
        private static int IDCounter = 0;
        private static List<int> deadList = new List<int>();


        private List<Component> components = new List<Component>();
        private bool isActive = true;
        private String tag = "";
        private int id = -1;


        public Transform transform;
        
        /// <summary>
        /// GameObject private contructor meant to be private/hidden from outside eyes.
        /// </summary>
        private GameObject() { }

        /// <summary>
        /// Determines if this GameObject is active in the game.
        /// </summary>
        public bool IsActive()
        {
            return this.isActive;
        }

        /// <summary>
        /// Changes this GameObject's active state.
        /// </summary>
        /// <param name="active">The true/false state to modify this GameObject as.</param>
        /// <example>
        /// <code>
        /// GameObject go = GameObject.Instantiate();
        /// go.SetActive(false)
        /// </code>
        /// </example>
        public void SetActive(bool active)
        {
            if (this.isActive == active)
            {
                return;
            }
            foreach (Component component in components)
            { 
                if (!this.isActive && component.IsActive())
                {
                    component.OnEnable();
                }
                else
                {
                    component.OnDisable();
                }
            }

            this.isActive = active;
            return;
        }

        /// <summary>
        /// Accessor method for retrieving the tag on this GameObject.
        /// </summary>
        /// <returns>The string representing the tag on this GameObject</returns>
        public String Tag()
        {
            return this.tag;
        }

        /// <summary>
        /// This function is called by the Application on every updated frame. 
        /// It calls the Update() method on every Component attached to this
        /// GameObject.
        /// </summary>
        public void Update()
        {
            foreach (Component component in components)
            {
                if (component.IsActive())
                {
                    component.Update();
                }
            }
            return;
        }

        /// <summary>
        /// This function is called by the Application on every updated frame, but only
        /// after Update() has been invoked on each GameObject. It will call LateUpdate()
        /// on every Component attached to this GameObject.
        /// </summary>
        public void LateUpdate()
        {
            foreach (Component component in components)
            {
                if (component.IsActive())
                {
                    component.LateUpdate();
                }
            }
            return;
        }

        /// <summary>
        /// Called by the Application on every updated frame during the
        /// rendering phase. It will call Render() on every Component attached to this GameObject.
        /// </summary>
        public void Render()
        {
            foreach (Component component in components)
            {
                if (component.IsActive())
                {
                    component.Render();
                }
            }
            return;
        }

        /// <summary>
        /// Returns a single component of type T on this GameObject or any of its children.
        /// </summary>
        /// <returns>Component The component found, if any.</returns>
        public Component GetComponentInChildren<T>()
        {
            return GetComponentInChildren<T>(transform);
        }

        /// <summary>
        /// Helper function to find a component in the children of a GameObject.
        /// </summary>
        /// <param name="transform">The transform node to start from.</param>
        /// <returns></returns>
        private Component GetComponentInChildren<T>(Transform transform)
        {
            if (transform != null)
            {
                if (transform.gameObject.GetComponent<T>() != null)
                {
                    return transform.gameObject.GetComponent<T>();
                }

                foreach (Transform child in transform.children)
                {
                    return GetComponentInChildren<T>(child);
                }

            }
            return null;
        }

        public Component AddComponent(Component component)
        {
            if (GetComponent(component.GetType()) == null)
            {
                // We point the component's game object to point to this game object.
                component.gameObject = this;
                component.transform = this.transform;

                this.components.Add(component);
                component.Start();
                component.SetActive(true);

            }
            return component;
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

        public Component GetComponent(Type type)
        {
            Component retrieved = null;

            foreach (Component component in components)
            {
                if (type.IsAssignableFrom(component.GetType()))
                {
                    retrieved = component;
                    break;
                }
            }

            return retrieved;
        }

        public Component GetComponent<T>()
        {
            return GetComponent(typeof(T));
        }

        public List<T> GetComponents<T>()
        {
            List<T> retrieved = new List<T>();

            foreach (Component component in components)
            {
                if (component is T)
                {
                    retrieved.Add((T)(object)component);
                }
            }
            return retrieved;
        }

        public int InstanceID()
        {
            return this.id;
        }

        public void RemoveComponent(Type type)
        {
            // No idea if this works?
            components.Remove((Component)Activator.CreateInstance(type));
            return;
        }

        public static GameObject Instantiate()
        {
            GameObject go = new GameObject();

            // Every game object will have a transform component.
            Transform transform = new Transform();
            go.AddComponent(transform);
            go.transform = transform;
            go.id = IDCounter++;

            gameObjectsIdMap.Add(go.id, go);

            return go;
        }

        public static GameObject Instantiate(String tag)
        {
            GameObject go = new GameObject();

            // Every game object will have a transform component.
            Transform transform = new Transform();
            go.AddComponent(transform);
            go.transform = transform;
            go.tag = tag;
            go.id = IDCounter++;

            // Add the game object to the data structures.
            if (!gameObjectsTagMap.ContainsKey(tag))
            {
                gameObjectsTagMap.Add(tag, new List<GameObject>());
            }

            if (gameObjectsTagMap.TryGetValue(tag, out List<GameObject> goList))
            {
                goList.Add(go);
            }
            gameObjectsIdMap.Add(go.id, go);

            return go;
        }


        public static void Destroy(GameObject go)
        {      
            if (go != null)
            { 
                // Remove all the children game objects along with this game object.
                foreach (Transform trans in go.transform.children)
                {
                    Destroy(go);
                }
                if (go.tag != "")
                {
                    // If the game object has a tag value, we need to remove it from the tag map.
                    if (gameObjectsTagMap.TryGetValue(go.tag, out List<GameObject> goList))
                    {
                        goList.Remove(go);
                    }
                }
                // Add the game object from the game objects to the dead list.
                deadList.Add(go.id);
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ClearDeadGameObjects()
        {
            foreach (int id in deadList)
            {
                gameObjectsIdMap.Remove(id);
            }
            deadList.Clear();
            return;
        }

        public static GameObject FindWithTag(String tag)
        {
            GameObject go = null;
            if (gameObjectsTagMap.TryGetValue(tag, out List<GameObject> goList))
            {
                if (goList.Count > 0)
                {
                    go = goList.ElementAt<GameObject>(0);
                }
            }
            return go;
        }

        public static List<GameObject> FindGameObjectsWithTag(String tag)
        {
            if (gameObjectsTagMap.TryGetValue(tag, out List<GameObject> goList))
            {
                return goList;
            }
            return null;
        }

        public static Dictionary<int, GameObject> GetGameObjects()
        {
            return gameObjectsIdMap;
        }
    }


}
