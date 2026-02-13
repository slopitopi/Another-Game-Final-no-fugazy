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



        //--------------------------------TEST PLANE--------------------------------//
        private static TurnLogic turnLogic;
        //--------------------------------TEST PLANE END--------------------------------//







        //--------------------------------CardSystem--------------------------------//
        private static Card[] deck, pos1, pos2, pos3;

        private static int EnemyHP = 100;
        private static int PlayerHP = 10;

        private static Random randomCardGen;
        private static int pos1Index, pos2Index, pos3Index;
        //--------------------------------CardSystem END--------------------------------//








        //--------------------------------HighScore--------------------------------//
        public enum StateHighS { PrintHighScore, EnterHighScore };
        private static StateHighS HighState;
        private static HighScore highscore;
        private static SpriteFont myFont;
        //--------------------------------HighScore--------------------------------//




        //-------------------------------Instructions Text--------------------------------//
        private static Instructions instructions;
        private static Button backButton;
        private static SpriteFont insttructionFont;
        //-------------------------------Instructions Text END--------------------------------//



        //--------------------------------Game State Management--------------------------------//
        public enum State { Menu, Play, Instructions, HighScore };
        public static State currentState;
        private static State _previousState;
        //--------------------------------Game State Management END--------------------------------//


        //--------------------------------Background--------------------------------//
        private static Background background;
        //--------------------------------Background END--------------------------------//




        //--------------------------------Button Lists For GameStates--------------------------------//
        private static List<Button> menuButtons;
        //-------------------------------- Button Lists For GameStates END--------------------------------//










        public static void InitializeGE()
        {
            currentState = State.Menu; // Set initial state to Menu
            background = new Background();
        }












        public static void LoadContentGE(ContentManager content, GameWindow window, GraphicsDevice graphicsDevice)
        {

            //--------------------------------TEST PLANE--------------------------------//

            //--------------------------------TEST PLANE END--------------------------------//




















            //--------------------------------CardSystem--------------------------------//
            pos1 = new Card[]
            {
                new Card("DamageCard", content.Load<Texture2D>("images/Cards/DmgCard"), 300, 100, 80, 120, () =>{ EnemyHP -= 10; pos1Index = randomCardGen.Next(pos1.Length); Debug.WriteLine("DmgCardPressed");}, Color.White, Color.Red),
                new Card("HealCard", content.Load<Texture2D>("images/Cards/HealCard"), 300, 100, 80, 120, () => { PlayerHP += 10; pos1Index = randomCardGen.Next(pos1.Length); Debug.WriteLine("HealCardPressed");}, Color.White, Color.Green)
            };

            pos2 = new Card[]
            {
                new Card("DamageCard", content.Load<Texture2D>("images/Cards/DmgCard"), 600, 100, 80, 120, () =>{ EnemyHP -= 10; pos2Index = randomCardGen.Next(pos2.Length); Debug.WriteLine("DmgCardPressed");}, Color.White, Color.Red),
                new Card("HealCard", content.Load<Texture2D>("images/Cards/HealCard"), 600, 100, 80, 120, () => { PlayerHP += 10; pos2Index = randomCardGen.Next(pos2.Length); Debug.WriteLine("HealCardPressed");}, Color.White, Color.Green)
            };

            pos3 = new Card[]
            {
                new Card("DamageCard", content.Load<Texture2D>("images/Cards/DmgCard"), 900, 100, 80, 120, () =>{ EnemyHP -= 10; pos3Index = randomCardGen.Next(pos3.Length); Debug.WriteLine("DmgCardPressed");}, Color.White, Color.Red),
                new Card("HealCard", content.Load<Texture2D>("images/Cards/HealCard"), 900, 100, 80, 120, () => { PlayerHP += 10; pos3Index = randomCardGen.Next(pos3.Length); Debug.WriteLine("HealCardPressed");}, Color.White, Color.Green)
            };


            randomCardGen = new Random();
            pos1Index = randomCardGen.Next(2);
            pos2Index = randomCardGen.Next(2);
            pos3Index = randomCardGen.Next(2);
            //--------------------------------CardSystem END--------------------------------//






            //--------------------------------Universal Stuff--------------------------------//
            backButton = new Button(content.Load<Texture2D>("images/Instructions/BackButton"), window.ClientBounds.Width - 110, window.ClientBounds.Height - 60, 100, 50, () => currentState = State.Menu, Color.White, Color.Red);
            //--------------------------------Universal Stuff END--------------------------------//










            //--------------------------------HighScore--------------------------------//
            highscore = new HighScore(10); // List holds a maximum of 10 scores
            myFont = content.Load<SpriteFont>("File");

            highscore.LoadFromFile("highscore.txt");
            //--------------------------------HighScore END--------------------------------//


            //-------------------------------Instructions Text--------------------------------//
            insttructionFont = content.Load<SpriteFont>("File");

            string instructionsText =

                "Instructions:\n\n" +

                "1. Use the mouse to interact with everything.\n" +
                "2. xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.\n" +
                "3. xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.\n" +
                "4. xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.\n" +
                "5. xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\n\n";



            instructions = new Instructions(insttructionFont, instructionsText, graphicsDevice);
            //-------------------------------Instructions Text END--------------------------------//



            //--------------------------------Background--------------------------------//
            // Menu background
            background.AddBackground(State.Menu, new BackgroundAsset(
                new[] { 
                    content.Load<Texture2D>("images/menu/frame_00"),
                    content.Load<Texture2D>("images/menu/frame_01"),
                    content.Load<Texture2D>("images/menu/frame_02"),
                    content.Load<Texture2D>("images/menu/frame_03"),
                    content.Load<Texture2D>("images/menu/frame_04"),
                    content.Load<Texture2D>("images/menu/frame_05"),
                    content.Load<Texture2D>("images/menu/frame_06"),
                    content.Load<Texture2D>("images/menu/frame_07"),
                    content.Load<Texture2D>("images/menu/frame_08"),
                    content.Load<Texture2D>("images/menu/frame_09"),
                    content.Load<Texture2D>("images/menu/frame_10"),
                    content.Load<Texture2D>("images/menu/frame_11"),
                    content.Load<Texture2D>("images/menu/frame_12"),
                    content.Load<Texture2D>("images/menu/frame_13"),
                    content.Load<Texture2D>("images/menu/frame_14"),


                },
                isAnimated: true,
                frameDuration: 0.09f
            ));

            // Play background
            background.AddBackground(State.Play, new BackgroundAsset(
                new[] { content.Load<Texture2D>("images/Battleground/Battleground_BG1") },
                isAnimated: false
            ));

            // Instructions background
            background.AddBackground(State.Instructions, new BackgroundAsset(
                new[] { content.Load<Texture2D>("images/Instructions/Background") },
                isAnimated: false
            ));

            //// HighScore background
            //background.AddBackground(State.HighScore, new BackgroundAsset(
            //    new[] { content.Load<Texture2D>("images/highscore/HighScoreBackground") },
            //    isAnimated: false
            //));

            background.SetState(currentState);
            //--------------------------------Background--------------------------------//



            //-------------------------------MenuButtons--------------------------------//
            // The lists for buttons in each state are initialized here and also the buttons are created and added to the lists.
            menuButtons = new List<Button>()
            {
                new Button(content.Load<Texture2D>("images/menu/playButton_"), 640 - 200 / 2, 100, 200, 50, () => currentState = State.Play, Color.White, Color.Green),
                new Button(content.Load<Texture2D>("images/menu/highScore_"), 640 - 200 / 2, 150 +20 , 200, 50, () => currentState = State.HighScore, Color.White, Color.Blue),
                new Button(content.Load<Texture2D>("images/menu/instructions_"), 640 - 200 / 2, 200 + 40, 200, 50, () => currentState = State.Instructions, Color.White, Color.Red),
                new Button(content.Load<Texture2D>("images/menu/quit_"), 640 - 200 / 2, 250+ 60, 200, 50, () => Environment.Exit(0), Color.White, Color.Red),
            };
            //-------------------------------MenuButtons END--------------------------------//


        }


        /// <summary>
        /// Master Update method for Game Elements, calls specific update methods based on the current game state.
        /// 
        /// It makes sure that only the active game state's update logic is executed.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void MASTER_UpdateGE(GameTime gameTime)
        {
            if (currentState != _previousState)
            {
                background.SetState(currentState);
                _previousState = currentState;
            }

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
            //--------------------------------Background--------------------------------//
            background.Update(gameTime);
            //--------------------------------Background END--------------------------------//




            //-------------------------------MenuButtons--------------------------------//
            MouseState mouse = Mouse.GetState();
            
            foreach (Button button in menuButtons)
            {
                button.BtnCheck(mouse);
            }
            //-------------------------------MenuButtons--------------------------------//


            Debug.WriteLine("In Menu Update GE");
        }



        public static void Menu_DrawGE(SpriteBatch spriteBatch)
        {
            // Drawing logic for game elements can be added here
            //--------------------------------Background--------------------------------//
            background.Draw(spriteBatch);
            //--------------------------------Background--------------------------------//


            //-------------------------------MenuButtons--------------------------------//
            foreach (Button button in menuButtons)
            {
                button.BtnMake(spriteBatch);
            }
            //-------------------------------MenuButtons END--------------------------------//

        }
















        public static void Play_UpdateGE(GameTime gameTime)
        {
            //--------------------------------Background--------------------------------//
            background.Update(gameTime);
            //--------------------------------Background END--------------------------------//
            // Play update logic for game elements can be added here







            //--------------------------------TEST PLANE--------------------------------//

            //--------------------------------TEST PLANE--------------------------------//




            //--------------------------------CardSystem--------------------------------//
            MouseState mouse = Mouse.GetState();

            pos1[pos1Index].BtnCheck(mouse);
            pos2[pos2Index].BtnCheck(mouse);
            pos3[pos3Index].BtnCheck(mouse);

            Debug.WriteLine($"Enemy HP: {EnemyHP}, Player HP: {PlayerHP}");
            //--------------------------------CardSystem END--------------------------------//




            Debug.WriteLine("In Play Update GE");
        }


        public static void Play_DrawGE(SpriteBatch spriteBatch)
        {

            //--------------------------------Background--------------------------------//
            background.Draw(spriteBatch);
            //--------------------------------Background--------------------------------//



            // Drawing logic for game elements can be added here
            //--------------------------------TEST PLANE--------------------------------//
            pos1[pos1Index].BtnMake(spriteBatch);
            pos2[pos2Index].BtnMake(spriteBatch);
            pos3[pos3Index].BtnMake(spriteBatch);
            //--------------------------------TEST PLANE--------------------------------//

        }
















        public static void Instructions_UpdateGE(GameTime gameTime)
        {
            // Instructions update logic for game elements can be added here

            //--------------------------------Background--------------------------------//
            background.Update(gameTime);
            //--------------------------------Background END--------------------------------//


            //-------------------------------Back Button--------------------------------//
            MouseState mouse = Mouse.GetState();
            backButton.BtnCheck(mouse);
            //-------------------------------Back Button END--------------------------------//


            Debug.WriteLine("In Instructions Update GE");
        }

        public static void Instructions_DrawGE(SpriteBatch spriteBatch)
        {
            // Instructions drawing logic for game elements can be added here
            //--------------------------------Background--------------------------------//
            background.Draw(spriteBatch);
            //--------------------------------Background--------------------------------//

            //-------------------------------Instructions Text--------------------------------//
            instructions.Draw(spriteBatch);
            //-------------------------------Instructions Text END--------------------------------//


            //-------------------------------Back Button--------------------------------//
            backButton.BtnMake(spriteBatch);
            //-------------------------------Back Button--------------------------------//

        }



















        public static void HighScore_UpdateGE(GameTime gameTime)
        {
            // HighScore update logic for game elements can be added here
            //--------------------------------Background--------------------------------//
            background.Update(gameTime);
            //--------------------------------Background END--------------------------------//

            //-------------------------------Back Button--------------------------------//
            MouseState mouse = Mouse.GetState();
            backButton.BtnCheck(mouse);
            //-------------------------------Back Button END--------------------------------//

            //--------------------------------HighScore--------------------------------//
            switch (HighState)
            {
                case StateHighS.EnterHighScore:
                    // Continue while EnterUpdate returns false. Pass in GameTime and a score (e.g., 10).
                    if (highscore.EnterUpdate(gameTime, 10))
                    {
                        highscore.SaveToFile("highscore.txt");
                        HighState = StateHighS.PrintHighScore; // Switch state when entry is complete
                    }
                    break;


                default: // State.PrintHighScore
                    KeyboardState keyboardState = Keyboard.GetState();
                    if (keyboardState.IsKeyDown(Keys.E)) // Press 'E' to enter a new score
                    {
                        HighState = StateHighS.EnterHighScore;
                    }

                    if (keyboardState.IsKeyDown(Keys.Back) || keyboardState.IsKeyDown(Keys.Escape))
                    {
                        currentState = State.Menu;
                    }
                    break;
            }
            //--------------------------------HighScore END--------------------------------//





            Debug.WriteLine("In HighScore Update GE");
        }


        public static void HighScore_DrawGE(SpriteBatch spriteBatch)
        {
            // HighScore drawing logic for game elements can be added here
            //--------------------------------Background--------------------------------//
            background.Draw(spriteBatch);
            //--------------------------------Background--------------------------------//


            //-------------------------------Back Button--------------------------------//
            backButton.BtnMake(spriteBatch);
            //-------------------------------Back Button--------------------------------//

            //--------------------------------HighScore--------------------------------//
            switch (HighState)
            {
                case StateHighS.EnterHighScore:
                    highscore.EnterDraw(spriteBatch, myFont);
                    break;
                default: // PrintHighScore
                    highscore.PrintDraw(spriteBatch, myFont);
                    break;
            }
            //--------------------------------HighScore END--------------------------------//


        }











        public static void Reset()
        {
            // Credits update logic for game elements can be added here
        }
    }
}