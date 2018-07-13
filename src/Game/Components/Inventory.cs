//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using Game.Interfaces;

namespace Game.Components
{
    class Inventory : Component
    {
        public List<Item> inventory = new List<Item>();

        public void Add(Item newItem)
        {
            inventory.Add(newItem);
        }

        public Item Find(String name)
        {
            foreach (Item i in inventory)
            {
                if (i.name == name)
                    return i;
            }
            return null;
        }

        public void MergeWith(Inventory inv)
        {

            foreach (Item i in inventory)
            {
                inv.Add(i);
            }
            if (inventory.Count > 0)
            {
                Console.Beep(3000, 100);
            }
            inventory.Clear();
        }

        public void Remove(String name)
        {
            Item toRemove = null;
            foreach (Item i in inventory)
            {
                if (i.name == name)
                    toRemove = i;
            }
            if (toRemove != null)
                inventory.Remove(toRemove);
        }

        public override void Start()
        {
            return;
        }

        public override void Update()
        {
            return;
        }

        public override void Render()
        {
            return;
        }

    }
}
