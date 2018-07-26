//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using Ecs;

namespace Game.Interfaces
{
    public interface IInteractable
    {
        bool IsInteractable { get; }

        void OnInteract(GameObject interacter,object interactorType);
    }
}
