//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;

namespace Game.Interfaces
{
    public interface IInteractable
    {
        void OnInteract(GameObject interacter);
        void OnOpened(GameObject interacter);
        void OnLocked(GameObject interacter);
    }
}
