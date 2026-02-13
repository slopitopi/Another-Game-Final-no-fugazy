using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Another_Game_Final_no_fugazy
{
    internal class TurnLogic
    {
        int Turncounter;
        protected Action DidSomething;
        protected bool PlayerTurn = false;
        private bool CardEnabled = true;
        private float TurnTimer = 3f;

        public TurnLogic(int turnCounter)
        {
            Turncounter =  turnCounter;
        }

        public void StartTurn(GameTime gameTime)
        {
            PlayerTurn = true;
            CardEnabled = true;
            if (PlayerTurn)
            {
                Console.WriteLine("Player's turn");

            }
        }

        public void PlayerTurnOver(GameTime gameTime)
        {
            Console.WriteLine("Player's turn is over \n");
            Console.WriteLine("Enemy's turn");

            if (TurnTimer != 0)
            {
                TurnTimer = TurnTimer - (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            PlayerTurn = false;
            CardEnabled = false;
            Turncounter++;

            StartTurn(gameTime);

            Debug.WriteLine($"{Turncounter}");
        }

    }
}