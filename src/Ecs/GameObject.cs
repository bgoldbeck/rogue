using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecs
{    
    public class GameObject
    {
        private static Dictionary<String, GameObject> gameObjects = new Dictionary<String, GameObject>();

        private List<Component> components;
        private bool isActive = true;
        private String tag;

        public Transform transform;
        
        public GameObject()
        {
            this.tag = "";
            this.isActive = true;
            this.components = new List<Component>();
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
            //System.out.println("Update GameObject " + this.tag);
            foreach (Component component in components)
            {
                if (component.IsActive())
                {
                    component.Update();
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

        public Component AddComponent(Component component)
        {
            if (GetComponent(component.GetType()) == null)
            {
                // We point the component's game object to point to this game object.
                component.gameObject = this;
                component.transform = this.transform;

                this.components.Add(component);
                component.Start();

            }
            return component;
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

        public void RemoveComponent(Type type)
        {
            // No idea if this works?
            components.Remove((Component)Activator.CreateInstance(type));
            return;
        }

        public static GameObject Instantiate(String tag)
        {
            // Game object tags must be unique.
            if (gameObjects.ContainsKey(tag))
            {
                return null;
            }

            GameObject go = new GameObject();

            // Every game object will have a transform component.
            Transform transform = new Transform();
            go.AddComponent(transform);
            go.transform = transform;
            go.tag = tag;

            // Add the game object to the data structure.
            gameObjects.Add(tag, go);

            return go;
        }


        public static void Destroy(String tag)
        {
            GameObject go = GameObject.Find(tag);
            if (go != null)
            { 
                // Remove all the children game objects along with this game object.
                foreach (Transform trans in go.transform.children)
                {
                    gameObjects.Remove(trans.gameObject.tag);
                }
                gameObjects.Remove(tag);
            }
            return;
        }

        public static GameObject Find(String tag)
        {
            gameObjects.TryGetValue(tag, out GameObject go);
            return go;
        }

        public static Dictionary<String, GameObject> GetGameObjects()
        {
            return gameObjects;
        }
    }


}
