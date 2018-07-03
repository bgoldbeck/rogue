using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using Game.Interfaces;

namespace Game.Components
{
    public class Sound : Component, IMovable
    {
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

        public void Move(int dx, int dy)
        {
            //Console.Beep(400, 90);
            return;
        }
    }
}
