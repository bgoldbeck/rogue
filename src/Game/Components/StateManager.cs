//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;

using Ecs;

namespace Game.Components
{
    class StateManager : Component
    {
        private int width;
        private int height;

        public enum GameState
        {
            LoadSecreen, Load, Running, GameOver
        }

        private Action[] updateAction = null;
        private GameState currentGameState = GameState.LoadSecreen;
        //private GameManager gameManager = null;

        public StateManager(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;

            updateAction = new Action[]
            {
                LoadScreen, Load, Running, GameOverScreen
            };
        }


        private void LoadScreen()
        {
            //width = Console.WindowWidth;
            //height = Console.WindowHeight;
            FireLogo fl = new FireLogo(width, height);
            fl.Run();


            currentGameState = GameState.Load;
        }

        private void Running()
        {
            return;
        }

        private void Load()
        {

            GameObject.Instantiate("GameManager").AddComponent(new GameManager(width, height));
            //GameObject go = GameObject.FindWithTag("GameManager");

            //go.SetActive(true);

            currentGameState = GameState.Running;
            return;
        }

        public void PlayerIsDead()
        {
            currentGameState = GameState.GameOver;
        }

        private void GameOverScreen()
        {

            GameObject go = GameObject.FindWithTag("GameManager");
            go.SetActive(false);
            GameObject.Destroy(go);

            //GameOver screen = new GameOver(width, height);
            //screen.Run();

            currentGameState = GameState.Load;
            return;
        }

        public override void EarlyUpdate()
        {
            updateAction[(int)currentGameState]();
        }
    }
}
