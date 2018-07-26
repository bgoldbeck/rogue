//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

namespace Game.Interfaces
{
    public interface IHidden
    {
        bool IsHidden { get; }

        void Reveal();
    }
}
