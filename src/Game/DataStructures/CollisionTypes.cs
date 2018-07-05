//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

namespace Game.DataStructures
{
    /// <summary>
    /// Used by the Collider component to return whether an object collided with a
    /// wall, object, or nothing at all.
    /// </summary>
    public enum CollisionTypes { None, Wall, ActiveObject };
}