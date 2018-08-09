#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System;
using Xunit;
using Ecs;
using System.Collections.Generic;

namespace XUnitTestProject
{
    public class GameObjectTest
    {
        public class TestComponent : Component, ITestInterface
        {
            public bool something = false;
            public int id = 0;

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
        public void InactiveGameObjectIsInactive()
        {
            GameObject go = GameObject.Instantiate();
            go.SetActive(false);
            Assert.False(go.IsActiveSelf());
        }

        [Fact]
        public void NewGameObjectIsActive()
        {
            GameObject go = GameObject.Instantiate();
            Assert.True(go.IsActiveSelf());
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
            //GameObject.ForceFlush();
            Assert.True(found != null);
        }

        [Fact]
        public void MultipleGameObjectsWithTagCanBeFound()
        {
            GameObject.Instantiate("findmemultiple");
            GameObject.Instantiate("findmemultiple");
            //GameObject.ForceFlush();
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
            child.Transform.SetParent(parent.Transform);
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
                child.Transform.SetParent(parent.Transform);
            }

            GameObject.Render();
            List<GameObject> preKill = GameObject.FindGameObjectsWithTag("destroychildtest");
            Assert.True(preKill.Count == 5);
            GameObject.Destroy(parent);
            List<GameObject> postKill = GameObject.FindGameObjectsWithTag("destroychildtest");
            Assert.Null(postKill);
            return;
        }


        [Fact]
        public void GameObjectWithComponentGetComponentsNotNull()
        {
            GameObject go = GameObject.Instantiate();
            go.AddComponent<TestComponent>();

            List<TestComponent> components = go.GetComponents<TestComponent>();

            Assert.True(components != null && components.Count == 1);
            return;
        }

        [Fact]
        public void GameObjectWithIdenticalComponentsDestroyOneOfThemIsTheOneDestroyed()
        {
            GameObject go = GameObject.Instantiate();

            TestComponent c1 = new TestComponent
            {
                id = 1
            };

            TestComponent c2 = new TestComponent
            {
                id = 2
            };

            TestComponent c3 = new TestComponent
            {
                id = 3
            };

            go.AddComponent(c1);
            go.AddComponent(c2);
            go.AddComponent(c3);

            go.Destroy(c2);

            GameObject.Render();

            List<TestComponent> components = go.GetComponents<TestComponent>();
            Assert.Contains(c1, components);
            Assert.Contains(c3, components);
            Assert.DoesNotContain(c2, components);
            return;
        }

        [Fact]
        public void GameObjectWithIdenticalComponentGetComponentsEqualsCount()
        {
            GameObject go = GameObject.Instantiate();
            go.AddComponent<TestComponent>();
            go.AddComponent<TestComponent>();
            go.AddComponent<TestComponent>();
            go.AddComponent<TestComponent>();

            List<TestComponent> components = go.GetComponents<TestComponent>();

            Assert.True(components != null && components.Count == 4);
            return;
        }

        [Fact]
        public void GameObjectDestroyComponent()
        {
            GameObject go = GameObject.Instantiate();
            go.AddComponent<TestComponent>();
            Assert.True(go.GetComponent<TestComponent>() != null);
            go.Destroy(go.GetComponent<TestComponent>());
            GameObject.Render();
            Assert.True(go.GetComponent<TestComponent>() == null);
        }

        public interface ITestInterface
        {
            void OnAnything();
            void OnThisValue(bool value);
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

        [Fact]
        public void GameObjectWithInactiveAncestorIsNotActiveInHierarchy()
        {
            GameObject greatGrandpa = GameObject.Instantiate();

            GameObject grandpa = GameObject.Instantiate();
            grandpa.Transform.SetParent(greatGrandpa.Transform);

            GameObject dad = GameObject.Instantiate();
            dad.Transform.SetParent(grandpa.Transform);

            GameObject child = GameObject.Instantiate();
            child.Transform.SetParent(dad.Transform);

            Assert.True(child.IsActiveInHierarchy() == true);

            greatGrandpa.SetActive(false);

            Assert.True(child.IsActiveInHierarchy() == false);

            greatGrandpa.SetActive(true);
            grandpa.SetActive(false);

            Assert.True(child.IsActiveInHierarchy() == false);

            return;
        }

    }
}
