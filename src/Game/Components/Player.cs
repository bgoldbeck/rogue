//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using Game.Interfaces;
using IO;

namespace Game.Components
{
    class Player : Actor, IMovable, IDamageable
    {
        public Player(string name, string description, int level, int hp, int armor, int attack)
            : base(name, description, level, hp, armor, attack)
        {
        }

        public override void Start()
        {
            base.Start();
            return;
        }

        public override void Update()
        {
            base.Update();
            return;
        }

        public void Move(int dx, int dy)
        {
            if (collider == null)
            {
                Debug.LogError("Could not find collider component on Player!");
            }

            HUD hud = (HUD)GameObject.FindWithTag("HUD").GetComponent(typeof(HUD));
            Map map = (Map)GameObject.FindWithTag("Map").GetComponent(typeof(Map));
            int newX = transform.position.x + dx;
            int newY = transform.position.y + dy;
            if (collider.HandleCollision(dx, dy, out GameObject found) == DataStructures.CollisionTypes.None)
            {
                int oldX = transform.position.x;
                int oldY = transform.position.y;
                transform.Translate(dx, dy);
                map.PopObject(oldX, oldY);
                map.AddObject(newX, newY, gameObject);
                hud.Log("You walked successfully.");
            }
            else
            {
                GameObject go = map.PeekObject(newX, newY);
                List<IInteractable> inter = go.GetComponents<IInteractable>();
                if (inter.Count > 0)
                {
                    hud.Log("Interacted with a " + go.Tag() + ".");
                    inter[0].Interact(gameObject);
                }
            }
            return;
        }

        public void ApplyDamage(int damage)
        {
            return;
        }

        public void OnDeath()
        {
            return;
        }
    }
}
