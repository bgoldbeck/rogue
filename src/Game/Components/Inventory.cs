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
        public List<Item> Bag { get; private set; } = new List<Item>();

        public void Add(Item newItem)
        {
            Bag.Add(newItem);
        }

        public bool Contains(Item item)
        {
            return Bag.Contains(item);
        }

        public bool Contains(String name)
        {
            return Find(name) != null;
        }

        public Item Find(String name)
        {
            foreach (Item item in Bag)
            {
                if (item.name == name)
                    return item;
            }
            return null;
        }


        public void MergeWith(Inventory inv)
        {

            foreach (Item item in Bag)
            {
                inv.Add(item);
            }
            if (Bag.Count > 0)
            {
                //Console.Beep(3000, 100);
            }
            Bag.Clear();
        }

        public bool Remove(Item item)
        {
            return Bag.Remove(item);
        }

        public bool Remove(String name)
        {
            return Bag.Remove(Find(name));
        }


    }
}
