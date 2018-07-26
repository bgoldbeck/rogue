//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

namespace Game.Interfaces
{
    public interface IMovable
    {
        void OnMove(int dx, int dy);
        void OnFailedMove();
    }
}
