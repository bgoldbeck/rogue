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
            Collider collisionDetect = (Collider)this.GetComponent(typeof(Collider));
            if(collisionDetect.handleCollision(dx,dy, out GameObject found) == DataStructures.CollisionTypes.None)
            {
                
                transform.Translate(dx, dy);
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
