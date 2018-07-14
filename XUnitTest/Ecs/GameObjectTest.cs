using System;
using Xunit;
using Ecs;
using System.Collections.Generic;

namespace XUnitTestProject
{
    public class GameObjectTest
    {
        [Fact]
        public void InactiveGameObjectIsInactive()
        {
            GameObject go = GameObject.Instantiate();
            go.SetActive(false);
            Assert.False(go.IsActive());
        }

        [Fact]
        public void NewGameObjectIsActive()
        {
            GameObject go = GameObject.Instantiate();
            Assert.True(go.IsActive());
        }

        [Fact]
        public void NewGameObjectWithTagHasTag()
        {
            GameObject go = GameObject.Instantiate("test");
            Assert.True(go.Tag() == "test");
        }

        [Fact]
        public void GameObjectWithTagCanBeFound()
        {
            GameObject go = GameObject.Instantiate("findme");
            GameObject found = GameObject.FindWithTag("findme");
            Assert.True(found != null);
        }

        [Fact]
        public void MultipleGameObjectsWithTagCanBeFound()
        {
            GameObject.Instantiate("findmemultiple");
            GameObject.Instantiate("findmemultiple");

            List<GameObject> found = GameObject.FindGameObjectsWithTag("findmemultiple");

            Assert.True(found != null && found.Count == 2);
        }

        [Fact]
        public void DestroyedGameObjectCantBeFound()
        {
            GameObject go = GameObject.Instantiate("destroyed");
            // Find it once.
            GameObject destroyed = GameObject.FindWithTag("destroyed");
            // Destroy it.
            GameObject.Destroy(destroyed);
            // We no longer can find it, hopefully.
            Assert.True(GameObject.FindWithTag("destroyed") == null);
        }


        [Fact]
        public void GameObjectCanGetComponentInChildrenNoHierarchy()
        {

            GameObject go = GameObject.Instantiate();
            go.AddComponent<Component>();
            Component component = go.GetComponentInChildren<Component>();
            Assert.True(component != null);
        }

        [Fact]
        public void GameObjectCanGetComponentInChildrenWithHierarchy()
        {
            // Setup hierarchy.
            GameObject parent = GameObject.Instantiate();
            GameObject child = GameObject.Instantiate();
            child.AddComponent<Component>();
            child.transform.SetParent(parent.transform);
            // Action.
            Component component = parent.GetComponentInChildren<Component>();
            // Expect.
            Assert.True(component != null);
        }

        [Fact]
        public void GameObjectDestroyKillsAllChildren()
        {
            GameObject parent = GameObject.Instantiate();
            for (int i = 0; i < 5; ++i)
            {
                GameObject child = GameObject.Instantiate("destroychildtest");
                child.transform.SetParent(parent.transform);
            }
            List<GameObject> preKill = GameObject.FindGameObjectsWithTag("destroychildtest");
            Assert.True(preKill.Count == 5);
            GameObject.Destroy(parent);
            List<GameObject> postKill = GameObject.FindGameObjectsWithTag("destroychildtest");
            Assert.True(postKill.Count == 0);
        }


        [Fact]
        public void GameObjectWithComponentGetComponentsNotNull()
        {
            GameObject go = GameObject.Instantiate();
            go.AddComponent<Component>();

            List<Component> components = go.GetComponents<Component>();

            Assert.True(components != null && components.Count == 1);

        }

        [Fact]
        public void GameObjectDestroyComponent()
        {
            GameObject go = GameObject.Instantiate();
            go.AddComponent<Component>();
            Assert.True(go.GetComponent<Component>() != null);
            go.Destroy(go.GetComponent<Component>());
            GameObject.ForceFlush();
            Assert.True(go.GetComponent<Component>() == null);
        }

        public interface ITestInterface
        {
            void OnAnything();
            void OnThisValue(bool value);
        }

        public class TestComponent : Component, ITestInterface
        {
            public bool something = false;
            public void OnAnything()
            {
                something = true;
            }
            public void OnThisValue(bool value)
            {
                something = value;
            }
        }

        [Fact]
        public void TestSendInterfaceMessageCalledOnValidInterface()
        {
            GameObject go = GameObject.Instantiate();
            go.AddComponent<TestComponent>();

            go.SendMessage<ITestInterface>("OnAnything");
            TestComponent component = (TestComponent)go.GetComponent<TestComponent>();
            
            Assert.True(component.something == true);

        }

        [Fact]
        public void TestSendInterfaceMessageCalledOnValidInterfaceWithParameter()
        {
            GameObject go = GameObject.Instantiate();
            go.AddComponent<TestComponent>();

            go.SendMessage<ITestInterface>("OnThisValue", new object[] { true });
            TestComponent component = (TestComponent)go.GetComponent<TestComponent>();

            Assert.True(component.something == true);

        }

        [Fact]
        public void TestSendInterfaceMessageCalledOnInvalidParameters()
        {
            try
            {
                GameObject go = GameObject.Instantiate();
                go.AddComponent<TestComponent>();

                go.SendMessage<ITestInterface>("DoesNotExist", new object[] { true });
                go.SendMessage<ITestInterface>("DoesNotExist", null);
                // Valid interface, invalid parameters. Throws parameter count mismatch.
                go.SendMessage<ITestInterface>("OnAnything", new object[] { 1,2,3 });
            }
            catch (Exception e)
            {
                Assert.Equal("Parameter count mismatch.", e.Message);
            }
            

        }

      
    }
}
