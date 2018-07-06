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

        private List<Component> components = new List<Component>();
        private bool isActive = true;
        private String tag = "";
        private int id = -1;

        public Transform transform;
        
        private GameObject()
        {
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

        public String Tag()
        {
            return this.tag;
        }

        public void Start()
        {
            foreach (Component component in components)
            {
                if (component.IsActive())
                {
                    component.Start();
                }
            }
            return;
        }

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

        public Component GetComponentInChildren<T>()
        {
            return GetComponentInChildren<T>(transform);
        }

        /// <summary>
        /// Helper function to find a component in the children of a gameobject
        /// </summary>
        /// <param name="transform"></param>
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
                if (component.GetType() == type)
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
                // Remove the game object from the game objects id map.
                gameObjectsIdMap.Remove(go.id);
            }
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
