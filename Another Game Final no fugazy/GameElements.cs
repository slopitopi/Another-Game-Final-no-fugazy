using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;

namespace Another_Game_Final_no_fugazy
{
    internal static class GameElements
    {
        //--------------------------------Game State Management--------------------------------//
        public enum State { Menu, Play, Instructions, HighScore };
        public static State currentState;
        //--------------------------------Game State Management END--------------------------------//


        //--------------------------------TEST PLANE--------------------------------//
        private static Background background;
        //--------------------------------TEST PLANE END--------------------------------//



        //--------------------------------Button Lists For GameStates--------------------------------//
        private static List<Button> menuButtons;
        //-------------------------------- Button Lists For GameStates END--------------------------------//

        public static void InitializeGE()
        {
            currentState = State.Menu; // Set initial state to Menu
            background = new Background();
        }

        public static void LoadContentGE(ContentManager content, GameWindow window)
        {


            //--------------------------------TEST PLANE--------------------------------//

            //--------------------------------TEST PLANE END--------------------------------//

            background.LoadAllBackgrounds(content);
            background.SetState(currentState);

            // The lists for buttons in each state are initialized here and also the buttons are created and added to the lists.
            menuButtons = new List<Button>()
            {
                new Button(content.Load<Texture2D>("images/menu/playButton_"), 640 - 200 / 2, 100, 200, 50, () => currentState = State.Play, Color.White, Color.Green),
                new Button(content.Load<Texture2D>("images/menu/highScore_"), 640 - 200 / 2, 150 +20 , 200, 50, () => currentState = State.HighScore, Color.White, Color.Blue),
                new Button(content.Load<Texture2D>("images/menu/instructions_"), 640 - 200 / 2, 200 + 40, 200, 50, () => currentState = State.Instructions, Color.White, Color.Red),
                new Button(content.Load<Texture2D>("images/menu/quit_"), 640 - 200 / 2, 250+ 60, 200, 50, () => Environment.Exit(0), Color.White, Color.Red),
            };



        }


        /// <summary>
        /// Master Update method for Game Elements, calls specific update methods based on the current game state.
        /// 
        /// It makes sure that only the active game state's update logic is executed.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void MASTER_UpdateGE(GameTime gameTime)
        {

            switch (currentState)
            {
                case State.Menu:
                    Menu_UpdateGE(gameTime);
                    break;

                case State.Play:
                    Play_UpdateGE(gameTime);
                    break;

                case State.Instructions:
                    Instructions_UpdateGE(gameTime);
                    break;

                case State.HighScore:
                    HighScore_UpdateGE(gameTime);
                    break;
            }



        }

        /// <summary>
        /// Master Draw method for Game Elements, calls specific draw methods based on the current game state.
        /// 
        /// makres sure that only the active game state's drawing logic is executed.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void MASTER_DrawGE(SpriteBatch spriteBatch)
        {
            switch (currentState)
            {
                case State.Menu:
                    Menu_DrawGE(spriteBatch);
                    break;

                case State.Play:
                    Play_DrawGE(spriteBatch);
                    break;

                case State.Instructions:
                    Instructions_DrawGE(spriteBatch);
                    break;

                case State.HighScore:
                    HighScore_DrawGE(spriteBatch);
                    break;
            }
        }








        public static void Menu_UpdateGE(GameTime gameTime)
        {
            // Menu update logic for game elements can be added here
            background.Update(gameTime);




            MouseState mouse = Mouse.GetState();


            foreach (Button button in menuButtons)
            {
                button.BtnCheck(mouse);
            }



            Debug.WriteLine("In Menu Update GE");
        }



        public static void Menu_DrawGE(SpriteBatch spriteBatch)
        {
            // Drawing logic for game elements can be added here
            background.Draw(spriteBatch);


            foreach (Button button in menuButtons)
            {
                button.BtnMake(spriteBatch);
            }
        }







        public static void Play_UpdateGE(GameTime gameTime)
        {
            // Play update logic for game elements can be added here
            Debug.WriteLine("In Play Update GE");


        }


        public static void Play_DrawGE(SpriteBatch spriteBatch)
        {
            // Drawing logic for game elements can be added here
        }






        public static void Instructions_UpdateGE(GameTime gameTime)
        {
            // Instructions update logic for game elements can be added here
            Debug.WriteLine("In Instructions Update GE");
        }
        public static void Instructions_DrawGE(SpriteBatch spriteBatch)
        {
            // Instructions drawing logic for game elements can be added here
        }







        public static void HighScore_UpdateGE(GameTime gameTime)
        {
            // HighScore update logic for game elements can be added here
            Debug.WriteLine("In HighScore Update GE");
        }


        public static void HighScore_DrawGE(SpriteBatch spriteBatch)
        {
            // HighScore drawing logic for game elements can be added here
        }











        public static void Reset()
        {
            // Credits update logic for game elements can be added here
        }
    }
}