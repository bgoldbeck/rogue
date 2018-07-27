//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using IO;

namespace Ecs
{
    public class GameObject
    {
        private static Dictionary<String, List<GameObject>> gameObjectsTagMap = new Dictionary<String, List<GameObject>>();
        private static Dictionary<int, GameObject> gameObjectsIdMap = new Dictionary<int, GameObject>();
        private static int IDCounter = 0;
        private static List<int> deadList = new List<int>();
        private static List<GameObject> gameObjectsToAdd = new List<GameObject>();

        private List<Component> components = new List<Component>();
        private List<Component> componentsToRemove = new List<Component>();


        private bool isActive = true;
        private String tag = "";
        private int id = -1;

        public String Name { get; set; }


        public Transform transform;

        /// <summary>
        /// GameObject private contructor meant to be private/hidden from outside eyes.
        /// </summary>
        private GameObject() { }


        public void SendMessage<T>(string name, object[] parameters = null)
        {
            List<T> interfaceables = GetComponents<T>();
            foreach (T interfaceable in interfaceables)
            {
                bool called = false;

                MethodInfo methodInfo = typeof(T).GetMethod(name);

                if (methodInfo != null)
                {
                    methodInfo.Invoke(interfaceable, parameters);
                    called = true;
                }

                if (!called)
                {
                    Debug.LogError("SendInterfaceMessage<T>() Could not call method named " + name);
                }
            }
        }

        /// <summary>
        /// Determines if this GameObject is active in the game.
        /// </summary>
        public bool IsActiveSelf()
        {
            return this.isActive;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsActiveInHierarchy()
        {
            return IsActiveInHierarchy(transform);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private bool IsActiveInHierarchy(Transform current)
        {
            if (current == null || !current.gameObject.IsActiveSelf())
            {
                return false;
            }

            // We made it to the root parent if the current.Parent is null.
            return current.Parent == null ? true : IsActiveInHierarchy(current.Parent);
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
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void SetActiveRecursively(bool state)
        {
            SetActive(state);
            if (transform != null)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActiveRecursively(state);
                }
            }
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
        /// It calls the EarlyUpdate() method on every Component attached to this
        /// GameObject.
        /// </summary>
        public static void EarlyUpdate()
        {
            // TODO: Can improve this loop performance by updating from the root game objects, recursively
            // down the tree.
            foreach (KeyValuePair<int, GameObject> entry in gameObjectsIdMap)
            {
                if (entry.Value.IsActiveInHierarchy())
                { 
                    foreach (Component component in entry.Value.GetComponents<Component>())
                    {
                        if (component.IsActive())
                        {
                            component.EarlyUpdate();
                        }
                    }
                }
            }

            return;
        }

        /// <summary>
        /// This function is called by the Application on every updated frame. 
        /// It calls the Update() method on every Component attached to this
        /// GameObject.
        /// </summary>
        public static void Update()
        {
            // TODO: Can improve this loop performance by updating from the root game objects, recursively
            // down the tree.
            foreach (KeyValuePair<int, GameObject> entry in gameObjectsIdMap)
            {
                if (entry.Value.IsActiveInHierarchy())
                {
                    foreach (Component component in entry.Value.GetComponents<Component>())
                    {
                        if (component.IsActive())
                        {
                            component.Update();
                        }
                    }
                }
            }

            return;
        }

        /// <summary>
        /// This function is called by the Application on every updated frame, but only
        /// after Update() has been invoked on each GameObject. It will call LateUpdate()
        /// on every Component attached to this GameObject.
        /// </summary>
        public static void LateUpdate()
        {
            // TODO: Can improve this loop performance by updating from the root game objects, recursively
            // down the tree.
            foreach (KeyValuePair<int, GameObject> entry in gameObjectsIdMap)
            {
                if (entry.Value.IsActiveInHierarchy())
                {
                    foreach (Component component in entry.Value.GetComponents<Component>())
                    {
                        if (component.IsActive())
                        {
                            component.LateUpdate();
                        }
                    }
                }
            }

            return;
        }

        /// <summary>
        /// This function is called by the Application if the window was resized.
        /// </summary>
        public static void OnResize()
        {
            foreach (KeyValuePair<int, GameObject> entry in gameObjectsIdMap)
            {
                foreach (Component component in entry.Value.GetComponents<Component>())
                {
                    if (component.IsActive())
                    {
                        component.OnResize();
                    }
                }
            }
            return;
        }

        private static bool drawDebug = false;
        /// <summary>
        /// Called by the Application on every updated frame during the
        /// rendering phase. It will call Render() on every Component attached to this GameObject.
        /// </summary>
        public static void Render()
        {
            
            ForceFlush();
#if DEBUG
            if (Input.ReadKey().Key == ConsoleKey.D)
            {
                drawDebug = !drawDebug;
            }
            if (drawDebug)
            {
                int line = 1;
                SortedSet<int> drawnObjects = new SortedSet<int>();
                foreach (KeyValuePair<int, GameObject> entry in gameObjectsIdMap)
                {
                    if (entry.Value.transform.Parent == null)
                    {
                        DebugDrawRecursive(entry.Value, 0, ref line);
                    }
                }
                return;
            }
#endif
            foreach (KeyValuePair<int, GameObject> entry in gameObjectsIdMap)
            {
                foreach (Component component in entry.Value.GetComponents<Component>())
                {
                    if (component.IsActive())
                    {
                        component.Render();
                    }
                }
            }

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        /// <param name="level"></param>
        /// <param name="line"></param>
        private static void DebugDrawRecursive(GameObject go, int level, ref int line)
        {
            StringBuilder sb = new StringBuilder(" ");
            foreach (Component component in go.components)
            {
                sb.Append("(" + component.GetType().Name + ") ");
            }

            Color color = go.isActive ? Color.Gold : Color.Gray;

            ConsoleUI.Write(0, ConsoleUI.MaxHeight() - (line++), "-".PadRight(level, '-') + go.Name + sb, color);

            if (go.transform.ChildCount() < 5)
            { 
                foreach (Transform child in go.transform)
                {
                    DebugDrawRecursive(child.gameObject, level + 2, ref (line));
                }
            }
            else
            {
                ConsoleUI.Write(0, ConsoleUI.MaxHeight() - (line++), "-".PadRight(level + 2, '-') + "Children Collapsed", Color.Gold);
            }
            return;
        }

        /// <summary>
        /// Called by the Destroy method when called by some user. It will call OnDestroy()
        /// for each component in this GameObject.
        /// </summary>
        private void OnDestroy()
        {
            foreach (Component component in this.components)
            {
                component.OnDestroy();
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

                foreach (Transform child in transform)
                {
                    return GetComponentInChildren<T>(child);
                }

            }
            return null;
        }

        public Component AddComponent(Component component)
        {
            //if (GetComponent(component.GetType()) == null)
            //{
            // We point the component's game object to point to this game object.
            component.gameObject = this;
            component.transform = this.transform;

            this.components.Add(component);
            component.SetActive(true);
            component.Start();

            //}
            //else
            //{
                //Debug.LogWarning("Game object already has component of type: " + component.GetType());
            //}
            return component;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Component AddComponent<T>()
        {
            if (typeof(Component).IsAssignableFrom(typeof(T)))
            {
                var obj = (T)Activator.CreateInstance(typeof(T));
                return AddComponent(obj as Component);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Component GetComponent<T>()
        {
            return GetComponent(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int InstanceID()
        {
            return this.id;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        public static GameObject Instantiate()
        {
            return GameObject.Instantiate("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static GameObject Instantiate(String tag)
        {
            GameObject go = new GameObject();

            // Every game object will have a transform component.
            Transform transform = new Transform();
            go.AddComponent(transform);
            go.transform = transform;
            go.id = IDCounter++;
            go.tag = tag;
            gameObjectsToAdd.Add(go);

            if (tag != null && tag != "")
            {
                // Add the game object to the data structures.
                if (!gameObjectsTagMap.ContainsKey(tag))
                {
                    gameObjectsTagMap.Add(tag, new List<GameObject>());
                }

                if (gameObjectsTagMap.TryGetValue(tag, out List<GameObject> goList))
                {
                    goList.Add(go);
                }
            }

            return go;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        public static void Destroy(Component component)
        {
            if (component != null)
            {
                // OnDestroy? Probably
                if (component.gameObject != null)
                {
                    component.OnDestroy();
                }
                component.gameObject.componentsToRemove.Add(component);
            }
            return;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        public static void Destroy(GameObject go)
        {      
            if (go != null)
            {
                go.OnDestroy();
                // Remove all the children game objects along with this game object.
                foreach (Transform t in go.transform)
                {
                    Destroy(t.gameObject);
                }
                if (go.tag != "")
                {
                    // If the game object has a tag value, we need to remove it from the tag map.
                    if (gameObjectsTagMap.TryGetValue(go.tag, out List<GameObject> goList))
                    {
                        goList.Remove(go);
                        if (goList.Count == 0)
                        {
                            gameObjectsTagMap.Remove(go.tag);
                        }
                    }
                    
                }
                // Add the game object from the game objects to the dead list.
                deadList.Add(go.id);
                
            }
            return;
        }

        /// <summary>
        /// This function is exposed for testing, do not try to use it, or the application will
        /// crash.
        /// This adds the newly created game objects for this frame as well ass clearing out the
        /// dead ones.
        /// </summary>
        private static void ForceFlush()
        {
            AddNewGameObjects();
            ClearDeadGameObjects();

            foreach (KeyValuePair<int, GameObject> entry in gameObjectsIdMap)
            {
                entry.Value.ClearDeadComponents();
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearDeadComponents()
        {
            foreach (Component component in componentsToRemove)
            {
                components.Remove(component);
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ClearDeadGameObjects()
        {
            foreach (int id in deadList)
            {
                if (gameObjectsIdMap.TryGetValue(id, out GameObject go))
                { 
                    go.componentsToRemove.AddRange(go.components);
                    go.ClearDeadComponents();
                    gameObjectsIdMap.Remove(go.id);
                }

            }
            deadList.Clear();

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        private static void AddNewGameObjects()
        {
            foreach (GameObject go in gameObjectsToAdd)
            {
                gameObjectsIdMap.Add(go.id, go);
                
            }
            gameObjectsToAdd.Clear();

            return;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static List<GameObject> FindGameObjectsWithTag(String tag)
        {
            if (gameObjectsTagMap.TryGetValue(tag, out List<GameObject> goList))
            {
                return goList;
            }
            return null;
        }

        

    }


}
