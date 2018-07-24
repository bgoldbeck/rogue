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
        private delegate bool UpdateDelegate();
        private UpdateDelegate[] updateAction = null;
        private GameState currentGameState = GameState.LoadSecreen;
        GameObject gameManager = null;

        public StateManager(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;
            updateAction = new UpdateDelegate[]
            {
                LoadScreen, Running, GameOverScreen
            };
        }
        public void Initialize()
        {
            GameObject gameManager = GameObject.Instantiate("GameManager");
            gameManager.AddComponent(new GameManager(width, height));
            return;
        }

        private bool LoadScreen()
        {
            width = Console.WindowWidth;
            height = Console.WindowHeight;
            FireLogo fl = new FireLogo(width, height);
            fl.Run();
            currentGameState = GameState.Running;
            return true;
        }

        private bool Running()
        {
            return true;
        }
        private bool GameOverScreen()
        {
            return false;
        }

        public override void EarlyUpdate()
        {
            updateAction[(int)currentGameState]();
        }
    }
}
