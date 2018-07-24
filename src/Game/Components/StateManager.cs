//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Text;

using Ecs;

namespace Game.Components
{
    class StateManager : Component
    {
        private int width;
        private int height;
        public enum GameState
        {
            LoadSecreen, Running, GameOver
        }
        private Action[] updateAction = null;
        private GameState currentGameState = GameState.LoadSecreen;
        private GameManager gameManager = null;

        public StateManager(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;
            updateAction = new Action[]
            {
                LoadScreen, Running, GameOverScreen
            };
        }
        public void Initialize()
        {
            GameObject newObject = GameObject.Instantiate("StateManager");
            gameManager = new GameManager(width, height);
            newObject.AddComponent(gameManager);
            return;
        }

        private void LoadScreen()
        {
            width = Console.WindowWidth;
            height = Console.WindowHeight;
            FireLogo fl = new FireLogo(width, height);
            fl.Run();
            currentGameState = GameState.Running;
        }

        private void Running()
        {
            return;
        }
        public void PlayerIsDead()
        {
            currentGameState = GameState.GameOver;
        }

        private void GameOverScreen()
        {
            //GameOver screen = new GameOver(width, height);
            //screen.Run();

            //gameObject.Destroy(gameManager);

            //gameManager = GameObject.Instantiate("GameManager");
            //gameManager.AddComponent(new GameManager(width, height));
            currentGameState = GameState.Running;
            return;
        }

        public override void EarlyUpdate()
        {
            updateAction[(int)currentGameState]();
        }
    }
}
