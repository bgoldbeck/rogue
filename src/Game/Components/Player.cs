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
    class Player : Actor, IMovable, IDamageable
    {

        public override void Start()
        {
            //Add player to map
            int newX = transform.position.x;
            int newY = transform.position.y;
            Map map = (Map)GameObject.FindWithTag("Map").GetComponent(typeof(Map));
            map.AddObject(newX, newY, gameObject);

            //Console.Out.WriteLine("Player started " + name);
            //Console.ReadKey();
            return;
        }

        public override void Update()
        {
            //System.out.println("Player updated");
            return;
        }

        public override void Render()
        {
            //System.out.println("Player rendered");
            return;
        }

        public override void OnEnable()
        {
            //Console.WriteLine("Player Enabled");
            return;
        }

        public override void OnDisable()
        {
            //Console.WriteLine("Player Disabled");
            return;
        }

        public void Move(int dx, int dy)
        {
            HUD hud = (HUD)GameObject.FindWithTag("HUD").GetComponent(typeof(HUD));
            Collider collisionDetect = (Collider)this.GetComponent(typeof(Collider));
            if (collisionDetect.HandleCollision(dx,dy, out GameObject found) == DataStructures.CollisionTypes.None)
            {
                int oldX = transform.position.x;
                int oldY = transform.position.y;
                transform.Translate(dx, dy);
                int newX = transform.position.x;
                int newY = transform.position.y;
                Map map = (Map)GameObject.FindWithTag("Map").GetComponent(typeof(Map));
                map.PopObject(oldX, oldY);
                map.AddObject(newX, newY, gameObject);
                hud.Log("You walked successfully.");
            }
            else
            {
                hud.Log("You walked right into something!");
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
