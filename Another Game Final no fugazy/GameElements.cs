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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

using static Another_Game_Final_no_fugazy.ObjectPosTex;




namespace Another_Game_Final_no_fugazy
{
    internal static class GameElements
    {



        //--------------------------------TEST PLANE--------------------------------//
        private static Player player;
        //--------------------------------TEST PLANE END--------------------------------//





        //--------------------------------EnemySystem--------------------------------//
        private static List<CombatEntity> enemys;
        private static CombatEntity clickedEnemy;
        //--------------------------------EnemySystem END--------------------------------//




        //--------------------------------CardSystem--------------------------------//
        private static Card[] pos1, pos2, pos3;

        private static Random randomCardGen;
        private static int pos1Index, pos2Index, pos3Index;

        private static Card selectedCard;
        private static int selectedCardPos;

        private enum PlayPhase { SelectCard, SelectEnemy };
        private static PlayPhase currentPlayPhase;

        //--------------------------------CardSystem END--------------------------------//








        //--------------------------------HighScore--------------------------------//
        public enum StateHighS { PrintHighScore, EnterHighScore };
        private static StateHighS HighState;
        private static HighScore highscore;
        private static SpriteFont myFont;
        //--------------------------------HighScore--------------------------------//




        //-------------------------------Instructions Text--------------------------------//
        private static Instructions instructions, cardInstrucions;
        private static Button backButton;
        private static SpriteFont insttructionFont;
        private static Vector2 textSize;
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
            currentPlayPhase = PlayPhase.SelectCard;
        }












        public static void LoadContentGE(ContentManager content, GameWindow window, GraphicsDevice graphicsDevice)
        {
            Load_Pos_Tex(content, window, graphicsDevice);






            //--------------------------------TEST PLANE--------------------------------//

            //--------------------------------TEST PLANE END--------------------------------//

























            //--------------------------------CardSystem--------------------------------//


            pos1 = new Card[]
            {
                new Card("DamageCard", "Enemy",  DamageCardTex, CardPos1, () =>{ pos1[pos1Index].selectableCard = true; }, Color.White, Color.Red),
                new Card("HealCard", "Self", HealCardTex, CardPos1, () => { pos1[pos1Index].selectableCard = true;}, Color.White, Color.Green)
            };

            pos2 = new Card[]
            {
                new Card("DamageCard", "Enemy", DamageCardTex, CardPos2, () =>{ pos2[pos2Index].selectableCard = true;}, Color.White, Color.Red),
                new Card("HealCard","Self", HealCardTex, CardPos2, () => { pos2[pos2Index].selectableCard = true;}, Color.White, Color.Green)
            };

            pos3 = new Card[]
            {
                new Card("DamageCard", "Enemy", DamageCardTex, CardPos3, () =>{ pos3[pos3Index].selectableCard = true;}, Color.White, Color.Red),
                new Card("HealCard","Self", HealCardTex, CardPos3, () => { pos3[pos3Index].selectableCard = true;}, Color.White, Color.Green)
            };


            randomCardGen = new Random();
            pos1Index = randomCardGen.Next(2);
            pos2Index = randomCardGen.Next(2);
            pos3Index = randomCardGen.Next(2);
            //--------------------------------CardSystem END--------------------------------//






            //--------------------------------Universal Stuff--------------------------------//
            backButton = new Button(BackButtonTex, backButtonPos, () => currentState = State.Menu, Color.White, Color.Red);
            //--------------------------------Universal Stuff END--------------------------------//










            //--------------------------------HighScore--------------------------------//
            highscore = new HighScore(10); // List holds a maximum of 10 scores
            myFont = MainFont;

            highscore.LoadFromFile("highscore.txt");
            //--------------------------------HighScore END--------------------------------//




            //-------------------------------Instructions Text--------------------------------//
            insttructionFont = MainFont;

            string instructionsText =

                "Instructions:\n\n" +

                "1. Use the mouse to interact with everything.\n" +
                "2. xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.\n" +
                "3. xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.\n" +
                "4. xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.\n" +
                "5. xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\n\n";

            string SelectCardText = "Select an Enemy";


            textSize = insttructionFont.MeasureString(instructionsText);

            instructions = new Instructions(insttructionFont, instructionsText, graphicsDevice, new Vector2((1280 - textSize.X) / 2, (720 - textSize.Y) / 2 - 50));

            cardInstrucions = new Instructions(insttructionFont, SelectCardText, graphicsDevice, CardInstructionsPos);
            //-------------------------------Instructions Text END--------------------------------//








            //--------------------------------Background--------------------------------//
            // Menu background
            background.AddBackground(State.Menu, new BackgroundAsset(
                MenuBackgroundFrames,
                isAnimated: true,
                frameDuration: 0.09f
            ));


            // Play background
            background.AddBackground(State.Play, new BackgroundAsset(
                new[] { PlayBackgroundTex },
                isAnimated: false
            ));


            // Instructions background
            background.AddBackground(State.Instructions, new BackgroundAsset(
                new[] { InstructionsBackgroundTex },
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
                new Button(MenuButtonPlayTex ,menuButtonPlayRect, () => currentState = State.Play, Color.White, Color.Green),
                new Button(MenuButtonHighScoreTex ,menuButtonHighScoreRect, () => currentState = State.HighScore, Color.White, Color.Blue),
                new Button(MenuButtonInstructionsTex, menuButtonInstructionsRect, () => currentState = State.Instructions, Color.White, Color.Red),
                new Button(MenuButtonQuitTex, MenuButtonQuitRect, () => Environment.Exit(0), Color.White, Color.Red),
            };
            //-------------------------------MenuButtons END--------------------------------//




            //--------------------------------Player--------------------------------//
            player = new Player(PlayerTex, PlayerPos, null, Color.White, Color.Blue, 100, 0);
            player.HealthBar = new HealthBar(insttructionFont, graphicsDevice, PlayerHealthBarPos, player);
            //--------------------------------Player END--------------------------------//



            //--------------------------------EnemySystem--------------------------------//
            enemys = new List<CombatEntity>();

            for (int i = 0; i < 1; i++)
            {
                EnemyBrawler temp = new EnemyBrawler(EnemyBrawlerTex, EnemyPos1, null, Color.White, Color.Red, 30, 8, player);

                temp.SetOnClick(() =>
                {
                    if (currentPlayPhase == PlayPhase.SelectEnemy)
                    {
                        clickedEnemy = temp;
                    }
                });

                Vector2 healthBarPos = EnemyPos1Healthbar;

                temp.HealthBar = new HealthBar(insttructionFont, graphicsDevice, healthBarPos, temp);

                enemys.Add(temp);
            }
            //--------------------------------EnemySystem END--------------------------------//













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
                button.Update(mouse);
            }
            //-------------------------------MenuButtons--------------------------------//


            //Debug.WriteLine("In Menu Update GE");
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
                button.Draw(spriteBatch);
            }
            //-------------------------------MenuButtons END--------------------------------//

        }
















        public static void Play_UpdateGE(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            //--------------------------------Background--------------------------------//
            background.Update(gameTime);
            //--------------------------------Background END--------------------------------//
            // Play update logic for game elements can be added here












            //--------------------------------MINI EnemySystem END--------------------------------//
            foreach (CombatEntity enemy in enemys)
            {
                if (enemy.IsAlive)
                {
                    enemy.Update(mouse);
                    enemy.HealthBar.UpdateHealth();
                }
            }

            player.Update(mouse);
            player.HealthBar.UpdateHealth();
            //--------------------------------MINI EnemySystem END--------------------------------//





            //--------------------------------CardSystem--------------------------------//
            switch (currentPlayPhase)
            {
                case PlayPhase.SelectCard:
                    pos1[pos1Index].Update(mouse);
                    pos2[pos2Index].Update(mouse);
                    pos3[pos3Index].Update(mouse);
                    




                    selectedCard = null;
                    selectedCardPos = 0;

                    if (pos1[pos1Index].selectableCard)
                    {
                        selectedCard = pos1[pos1Index];
                        selectedCardPos = 1;
                    }
                    else if (pos2[pos2Index].selectableCard)
                    {
                        selectedCard = pos2[pos2Index];
                        selectedCardPos = 2;

                    }
                    else if (pos3[pos3Index].selectableCard)
                    {
                        selectedCard = pos3[pos3Index];
                        selectedCardPos = 3;
                    }



                    if (selectedCard != null)
                    {
                        pos1[pos1Index].selectableCard = false;
                        pos2[pos2Index].selectableCard = false;
                        pos3[pos3Index].selectableCard = false;


                        if (selectedCard.target == "Self")
                        {
                            switch (selectedCard.name)
                            {
                                case "HealCard":
                                    player.Heal(10);
                                    player.HealthBar.UpdateHealth();
                                    break;
                            }
                            Debug.WriteLine("Self Card has been activated");

                            foreach (CombatEntity enemy in enemys)
                            {
                                if (enemy.IsAlive)
                                {
                                    enemy.WaitTurns();
                                    enemy.HealthBar.UpdateHealth();
                                }

                            }

                            switch (selectedCardPos)
                            {
                                case 1: pos1Index = randomCardGen.Next(pos1.Length); break;
                                case 2: pos2Index = randomCardGen.Next(pos2.Length); break;
                                case 3: pos3Index = randomCardGen.Next(pos3.Length); break;
                            }


                            currentPlayPhase = PlayPhase.SelectCard;
                        }

                        else if (selectedCard.target == "Enemy")
                        {
                            Debug.WriteLine("CARD HAS BEED SELECTED");

                            currentPlayPhase = PlayPhase.SelectEnemy;
                        }
                    }
                    break;






                case PlayPhase.SelectEnemy:
                    //Show instructions



                    if (clickedEnemy != null)
                    {
                        Debug.WriteLine("ENEMY HAS BEEN CLICKED");

                        switch (selectedCard.name)
                        {
                            case "DamageCard":
                                clickedEnemy.TakeDamage(1);
                                clickedEnemy.HealthBar.UpdateHealth();
                                break;

                        }

                        //--EnemySysten--//
                        foreach (CombatEntity enemy in enemys)
                        {
                            if (enemy.IsAlive)
                            {
                                enemy.WaitTurns();
                            }

                        }
                        //--EnemySysten--//

                        if (!player.IsAlive)
                        {
                            currentState = State.Menu;
                            Debug.WriteLine("Player has died! Game Over!");

                        }


                        switch (selectedCardPos)
                        {
                            case 1: pos1Index = randomCardGen.Next(pos1.Length); break;
                            case 2: pos2Index = randomCardGen.Next(pos2.Length); break;
                            case 3: pos3Index = randomCardGen.Next(pos3.Length); break;
                        }


                        selectedCard = null;
                        clickedEnemy = null;
                        currentPlayPhase = PlayPhase.SelectCard;
                    }

                    break;
            }

            //--------------------------------CardSystem END--------------------------------//













            //--------------------------------TEST PLANE--------------------------------//

            //--------------------------------TEST PLANE--------------------------------//

            //Debug.WriteLine($"PlayPhase {currentPlayPhase}");
        }


        public static void Play_DrawGE(SpriteBatch spriteBatch)
        {

            //--------------------------------Background--------------------------------//
            background.Draw(spriteBatch);
            //--------------------------------Background--------------------------------//

            //--------------------------------CardSystem--------------------------------//
            if (currentPlayPhase == PlayPhase.SelectEnemy)
            {
                cardInstrucions.Draw(spriteBatch);
            }

            pos1[pos1Index].Draw(spriteBatch);
            pos2[pos2Index].Draw(spriteBatch);
            pos3[pos3Index].Draw(spriteBatch);

            if (selectedCard != null)
            {
                selectedCard.Draw(spriteBatch);
            }
            //--------------------------------CardSystem END--------------------------------//




            //--------------------------------EnemySystem--------------------------------//
            foreach (CombatEntity enemy in enemys)
            {
                if (enemy.IsAlive)
                {
                    enemy.Draw(spriteBatch);
                    enemy.HealthBar.Draw(spriteBatch);
                }
            }
            //--------------------------------EnemySystem END--------------------------------//










            //--------------------------------TEST PLANE--------------------------------//

            player.Draw(spriteBatch);
            player.HealthBar.Draw(spriteBatch);


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
            backButton.Update(mouse);
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
            backButton.Draw(spriteBatch);
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
            backButton.Update(mouse);
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
            backButton.Draw(spriteBatch);
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