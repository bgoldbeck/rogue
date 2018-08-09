#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System;

using Ecs;
using IO;

namespace Game.Components
{
    class StateManager : Component
    {
        private int width;
        private int height;

        public enum GameState
        {
            LoadScreen, Load, Running, GameOver
        }

        private Action[] updateAction = null;
        private GameState currentGameState = GameState.LoadScreen;
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
            return;
        }

        private void Running()
        {
            return;
        }

        public override void OnResize()
        {
            height = ConsoleUI.MaxHeight();
            width = ConsoleUI.MaxWidth();
            return;
        }

        private void Load()
        {

            GameManager gm = (GameManager)GameObject.Instantiate("GameManager").AddComponent(new GameManager(width, height));
            gm.Name = "GameManager";

            //GameObject go = GameObject.FindWithTag("GameManager");

            //go.SetActive(true);

            currentGameState = GameState.Running;
            return;
        }

        public void PlayerIsDead()
        {
            currentGameState = GameState.GameOver;
            return;
        }

        private void GameOverScreen()
        {

            GameObject go = GameObject.FindWithTag("GameManager");
            //go.SetActive(false);
            GameObject.Destroy(go);

            GameOver screen = new GameOver("Sneaky McDevious");
            screen.Run();

            currentGameState = GameState.Load;
            return;
        }

        public override void EarlyUpdate()
        {
            updateAction[(int)currentGameState]();
        }
    }
}
