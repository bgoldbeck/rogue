using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using Game.Interfaces;

namespace Game.Components
{
    class GameManager : Component
    {
        public int gameWidth;
        public int gameHeight;

        public GameManager(int width, int height)
        {
            this.gameWidth = width;
            this.gameHeight = height;

        }

        public override void Start()
        {
            GameObject map = GameObject.Instantiate("Map");
            map.AddComponent(new Map(gameWidth, gameHeight));

            GameObject player = GameObject.Instantiate("Player");
            player.AddComponent(new Player());
            player.AddComponent(new PlayerController());
            player.AddComponent(new Model());

            Model playerModel = (Model)player.GetComponent(typeof(Model));
            playerModel.model.Clear();
            playerModel.model.Add("$");
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
    }
}
