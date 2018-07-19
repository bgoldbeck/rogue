//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Ecs;
using Game.Interfaces;
using IO;

namespace Game.Components
{
    class GameManager : Component
    {
        private const int hudWidth = 36;
        public int gameWidth;
        public int gameHeight;

        public GameManager(int width, int height)
        {
            this.gameWidth = width;
            this.gameHeight = height;

        }

        public override void Start()
        {
            // Play intro music in separate thread
            Thread music = new Thread(PlayIntroMusic);
            music.Start();

            GameObject player = GameObject.Instantiate("MainPlayer");
            GameObject mapObject = GameObject.Instantiate("Map");
            //Map map = new Map(gameWidth - hudWidth, gameHeight);
            Map map = new Map(60, 30);
            mapObject.AddComponent(map);
            mapObject.transform.position = new Vec2i(mapObject.transform.position.x, gameHeight - 1);

            GameObject navigatorMapObject = GameObject.Instantiate("NavigatorMap");
            navigatorMapObject.AddComponent(new NavigatorMap());

            player.AddComponent(new Player("Sneaky McDevious", "Thiefy rogue", 1, 10, 1, 2));
            player.AddComponent(new PlayerController());
            player.AddComponent(new Model());
            player.AddComponent(new LightSource(10.0f));
            player.transform.position = new Vec2i(map.startingX, map.startingY);
            //player.transform.position.x = map.startingX;
            //player.transform.position.y = map.startingY;
            map.AddObject(map.startingX, map.startingY, player);
            player.AddComponent(new Camera(gameWidth - hudWidth, gameHeight));
            player.AddComponent(new MapTile('$', new Color(255, 255, 255), true));
            player.AddComponent(new Inventory());
            player.AddComponent(new Sound());
            //player.AddComponent(new NavigatorAgent());

            // Setup HUD for stats and info
            GameObject hud = GameObject.Instantiate("HUD");
            hud.AddComponent(new HUD(hudWidth, gameHeight));
            Model hudModel = (Model)hud.AddComponent(new Model());
            hudModel.color.Set(180, 180, 180);
            hud.transform.position = new Vec2i(gameWidth - hudWidth, gameHeight - 1);
            //hud.transform.position.x = gameWidth - hudWidth;
            //hud.transform.position.y = gameHeight - 1;

            Debug.Log("GameManager added all components on start.");
            return;
        }

        public override void OnResize()
        {
            gameWidth = ConsoleUI.MaxWidth();
            gameHeight = ConsoleUI.MaxHeight();

            //Camera camera = (Camera)GameObject.FindWithTag("Player").GetComponent(typeof(Camera));

            Camera camera = Camera.CacheInstance();
            camera.Resize(gameWidth - hudWidth, gameHeight);

            GameObject hudObject = GameObject.FindWithTag("HUD");
            hudObject.transform.position = new Vec2i(gameWidth - hudWidth, gameHeight - 1);
            //hudObject.transform.position.x = gameWidth - hudWidth;
            //hudObject.transform.position.y = gameHeight - 1;

            HUD hud = (HUD)hudObject.GetComponent(typeof(HUD));
            hud.Resize(hudWidth, gameHeight);

            Debug.Log("Resized game to " + gameWidth + " x " + gameHeight + ".");
        }

        /// <summary>
        /// This function plays a Castlevania-esque ditty.
        /// </summary>
        private void PlayIntroMusic()
        {
            Console.Beep(220, 150); //A3
            Console.Beep(165, 150); //E3
            Console.Beep(247, 150); //A3
            Console.Beep(165, 150); //E3
            Console.Beep(262, 150); //A3
            Console.Beep(165, 150); //E3
            Console.Beep(294, 150); //A3
            Console.Beep(165, 150); //E3
            Console.Beep(220, 500); //A3
        }

        public override void Update()
        {
            return;
        }

        public override void LateUpdate()
        {
            return;
        }

        public override void Render()
        {
            return;
        }
    }
}
