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

        public Player player;
        public MapManager mapManager;
        public NavigatorMap navigatorMap;
        public HUD hud;
        public Map currentMap;

        public GameManager(int width, int height)
        {
            this.gameWidth = width;
            this.gameHeight = height;

        }

        public override void Start()
        {
            GameObject mapObject = GameObject.Instantiate("MapManager");

            mapManager = (MapManager)mapObject.AddComponent(new MapManager(80, 40, 1));
            mapObject.transform.position = new Vec2i(mapObject.transform.position.x, gameHeight - 1);

            navigatorMap = (NavigatorMap)GameObject.Instantiate("NavigatorMap").AddComponent(new NavigatorMap());

            Map map = MapManager.CurrentMap();
            currentMap = map;

            player = (Player)GameObject.Instantiate("MainPlayer").AddComponent(new Player("Sneaky McDevious", "Thiefy rogue", 1, 10, 1, 2));

            player.AddComponent(new PlayerController());
            player.AddComponent(new Model());
            player.AddComponent(new LightSource(10.0f));
            player.AddComponent(new Camera(gameWidth - hudWidth, gameHeight));
            player.AddComponent(new MapTile('$', new Color(255, 255, 255), true));
            player.AddComponent(new Inventory());
            player.AddComponent(new Sound());

            

            player.transform.position = new Vec2i(map.startingX, map.startingY);
    
            map.AddObject(map.startingX, map.startingY, player.gameObject);
  

            // Setup HUD for stats and info
            hud = (HUD)GameObject.Instantiate("HUD").AddComponent(new HUD(hudWidth, gameHeight));
            
            Model hudModel = (Model)hud.AddComponent(new Model());
            hudModel.color.Set(180, 180, 180);
            hud.transform.position = new Vec2i(gameWidth - hudWidth, gameHeight - 1);
        

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

        public override void Update()
        {
            return;
        }

        public override void LateUpdate()
        {
            return;
        }

        public override void OnDestroy()
        {
            GameObject.Destroy(player.gameObject);
            GameObject.Destroy(mapManager.gameObject);
            GameObject.Destroy(navigatorMap.gameObject);
            GameObject.Destroy(hud.gameObject);
            GameObject.Destroy(currentMap.gameObject);
            return;
        }

    }
}
