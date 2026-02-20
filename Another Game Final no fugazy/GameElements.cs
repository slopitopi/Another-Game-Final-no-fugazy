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
using static Another_Game_Final_no_fugazy.GameElements;
using static Another_Game_Final_no_fugazy.ObjectPosTex; //To get to the positions and textures without having to write the full namespace each time.
using static System.Net.Mime.MediaTypeNames;




namespace Another_Game_Final_no_fugazy
{
    /// <summary>
    /// Centralized class for managing all game elements, including player, enemies, cards, backgrounds, buttons, and high scores.
    ///
    /// And is responsible for initializing, updating, and drawing these elements based on the current game state.
    /// </summary>

    internal static class GameElements
    {
        //--------------------------------PlayerSystem--------------------------------//
        private static Player player;
        //--------------------------------PlayerSystem END--------------------------------//



        //--------------------------------Wave System--------------------------------//
        private static int currentWave;
        private static bool WaveOver = true;
        //--------------------------------Wave System END--------------------------------//




        //--------------------------------EnemySystem--------------------------------//
        private static GraphicsDevice _graphicsDevice;
        private static List<CombatEntity> enemys;
        private static CombatEntity clickedEnemy; 
        //--------------------------------EnemySystem END--------------------------------//



        //--------------------------------CardSystem--------------------------------//
        private static Card[] pos1, pos2, pos3;

        private static Random randomCardGen ,randomEnemyGen;
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
        private static Instructions instructions, cardInstrucions, currentWaveInfo;
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
            currentState = State.Menu; // Start in the menu state
            background = new Background();
            currentPlayPhase = PlayPhase.SelectCard; // When in the play state, start with the card selection phase
            currentWave = 1; // Start at wave 1
        }












        public static void LoadContentGE(ContentManager content, GameWindow window, GraphicsDevice graphicsDevice)
        {
            //--------------------------------General--------------------------------//
            Load_Pos_Tex(content, window, graphicsDevice); //Load all positions and textures from the ObjectPosTex class method.
            backButton = new Button(BackButtonTex, backButtonPos, () => currentState = State.Menu, Color.White, Color.Red); //A button that takes you back to the menu, used in both the instructions and highscore

            //--------------------------------General--------------------------------//



            //--------------------------------CardSystem--------------------------------//
            //Initializing the card arrays for each position with the corresponding textures, positions, and onClick actions. 
            //The onClick actions set the card's PosSelectedCard property to true, which is used in the update method to determine which card in that position had been chosen.
            pos1 = new Card[]
            {
                new Card("DamageCard", "Enemy",  DamageCardTex, CardPos1, () =>{ pos1[pos1Index].PosSelectedCard = true; }, Color.White, Color.Red),
                new Card("DebuffCard", "Enemy",  DebuffCardTex, CardPos1, () =>{ pos1[pos1Index].PosSelectedCard = true; }, Color.White, Color.Orange),

                new Card("HealCard", "Self", HealCardTex, CardPos1, () => { pos1[pos1Index].PosSelectedCard = true;}, Color.White, Color.Green)
            };

            pos2 = new Card[]
            {
                new Card("DamageCard", "Enemy", DamageCardTex, CardPos2, () =>{ pos2[pos2Index].PosSelectedCard = true;}, Color.White, Color.Red),
                new Card("DebuffCard", "Enemy",  DebuffCardTex, CardPos2, () =>{ pos2[pos2Index].PosSelectedCard = true; }, Color.White, Color.Orange),

                new Card("HealCard","Self", HealCardTex, CardPos2, () => { pos2[pos2Index].PosSelectedCard = true;}, Color.White, Color.Green)
            };

            pos3 = new Card[]
            {
                new Card("DamageCard", "Enemy", DamageCardTex, CardPos3, () =>{ pos3[pos3Index].PosSelectedCard = true;}, Color.White, Color.Red),
                new Card("DebuffCard", "Enemy",  DebuffCardTex, CardPos3, () =>{ pos3[pos1Index].PosSelectedCard = true; }, Color.White, Color.Orange),

                new Card("HealCard","Self", HealCardTex, CardPos3, () => { pos3[pos3Index].PosSelectedCard = true;}, Color.White, Color.Green)
            };

            // Randomly pick one card for each position. And Capture it in a moment
            randomCardGen = new Random();
            pos1Index = randomCardGen.Next(3);
            pos2Index = randomCardGen.Next(3);
            pos3Index = randomCardGen.Next(3);
            //--------------------------------CardSystem END--------------------------------//



            //--------------------------------HighScore--------------------------------//
            highscore = new HighScore(10); // List holds a maximum of 10 scores
            myFont = MainFont; // The font used for the highscore text is the same as the main font

            highscore.LoadFromFile("highscore.txt"); // Load existing high scores from a file.
            //--------------------------------HighScore END--------------------------------//




            //-------------------------------Instructions Text--------------------------------//
            insttructionFont = MainFont; // The font used for instructions is the same as the main font but seperated for clarity

            string instructionsText =    // The instructions text that will be shown in the instructions menu, it explains the player how to play the game and what to expect.
                "Instructions:\n\n" +

                "1. Use the mouse to interact with everything.\n" +
                "2. Each wave increases the difficulty but also your cards damage and healing.\n" +
                "3. Click on the card you want to play and then press the enemy you wish to attack.\n" +
                "4. Have Fun.\n";

            string SelectCardText = "Select an Enemy"; //Text that will show up once you press a card. Instructing you to click an enemy


            textSize = insttructionFont.MeasureString(instructionsText); //Checks how long the string is

            instructions = new Instructions(insttructionFont, instructionsText, graphicsDevice, new Vector2((1280 - textSize.X) / 2, (720 - textSize.Y) / 2 - 50));
            cardInstrucions = new Instructions(insttructionFont, SelectCardText, graphicsDevice, CardInstructionsPos);
            //-------------------------------Instructions Text END--------------------------------//








            //--------------------------------Background--------------------------------//
            // Menu background
            background.AddBackground(State.Menu, new BackgroundAsset(
                MenuBackgroundFrames, // Array of textures for the menu background animation
                isAnimated: true, // Indicates that this background should be animated
                frameDuration: 0.09f // Duration of each frame in seconds (0.09 seconds per frame for a smooth animation)
            ));


            // Play background
            background.AddBackground(State.Play, new BackgroundAsset(
                new[] { PlayBackgroundTex }, // Single texture for the play background (not animated)
                isAnimated: false // No animation for the play background (Dosent cykle through any frames)
            ));


            // Instructions background
            background.AddBackground(State.Instructions, new BackgroundAsset(
                new[] { InstructionsBackgroundTex },
                isAnimated: false
            ));

            // HighScore background
            background.AddBackground(State.HighScore, new BackgroundAsset(
                new[] { HighScoreBackgroundTex },
                isAnimated: false
            ));

            background.SetState(currentState); // Sets the initial menu background
            //--------------------------------Background--------------------------------//






            //-------------------------------MenuButtons--------------------------------//
            // Buttons that change between the different states of the game via their onClick actions.
            menuButtons = new List<Button>()
            {
                new Button(MenuButtonPlayTex ,menuButtonPlayRect, () => currentState = State.Play, Color.White, Color.Green), //Takes you to the play state
                new Button(MenuButtonHighScoreTex ,menuButtonHighScoreRect, () => currentState = State.HighScore, Color.White, Color.Blue), //Takes you to the highscore state
                new Button(MenuButtonInstructionsTex, menuButtonInstructionsRect, () => currentState = State.Instructions, Color.White, Color.Red), //Takes you to the instructions state
                new Button(MenuButtonQuitTex, MenuButtonQuitRect, () => Environment.Exit(0), Color.White, Color.Red), //Quits the game by exiting the environment
            };
            //-------------------------------MenuButtons END--------------------------------//




            //--------------------------------Player--------------------------------//
            player = new Player(PlayerTex, PlayerPos, null, Color.White, Color.Blue, 10, 0); //The player is effectively a placeholder that cant be clicked or hoverd over. All it dose is position it and sets the MaxHP

            player.HealthBar = new HealthBar(insttructionFont, graphicsDevice, PlayerHealthBarPos, player); // Creates the healthbar for the player and links it to the player

            player.EffectBoxes = new Instructions(insttructionFont, "", graphicsDevice, PlayerEffectBoxPos); // Creates the effectbox for the player, which will show any active debuffs or buffs. It is also linked to the player so that it can check for any active effects when updating the text.
            player.UpdateEffectBoxText(); // Sets the initial text for the effect box, which will be empty since there are no active effects at the start of the game.
            player.CurrentWave = currentWave; // Links the player to the current wave, which is used to better show the effects of the waves upgrades. (Your damage increases every wave)
            //--------------------------------Player END--------------------------------//



            //--------------------------------EnemySystem--------------------------------//
            enemys = new List<CombatEntity>(); // List that will hold the current enemys on the field, it will be cleared and refilled with new enemys every wave in the SpawnEnemys method.
            randomEnemyGen = new Random(); // Generator used to randomize the enemy spawn order every wave in the SpawnEnemys method.

            currentWaveInfo = new Instructions(insttructionFont, "", graphicsDevice, CurrentWaveInfoPos); // Draws the current wave info text at the top of the screen, it is updated every frame in the play update method to show the current wave number.
            _graphicsDevice = graphicsDevice; // To get the SpawnEnemys method working


            SpawnEnemys(graphicsDevice); // Called to spawn the first wave of enemys.
            //--------------------------------EnemySystem END--------------------------------//

        }










        /// <summary>
        /// Spawns a new set of enemys based on the current wave number. It randomizes the spawn order and increases the enemys difficulty every wave
        /// <param name="graphicsDevice"></param>
        private static void SpawnEnemys(GraphicsDevice graphicsDevice)
        {
            enemys.Clear(); // Clear existing enemys from the previous wave

            int[] positions = { 0, 1, 2 }; // Array representing the three enemy positions
            positions = positions.OrderBy(x => randomEnemyGen.Next()).ToArray(); // Randomize the order of the positions to create variety in enemy spawns each wave

            // Arrays to easily access the positions for the enemys, their healthbars, and their effectboxes based on the randomized spawn order.
            Rectangle[] enemyPositions = { EnemyPos1, EnemyPos2, EnemyPos3 }; 
            Vector2[] healthBarPositions = { EnemyPos1Healthbar, EnemyPos2Healthbar, EnemyPos3Healthbar };
            Vector2[] effectBoxPositions = { EnemyPos1EffectBox, EnemyPos2EffectBox, EnemyPos3EffectBox };

            int waveBonus = (currentWave - 1) * 5; // Health increase for each enemy every wave.

            for (int i = 0; i < 1; i++) // Spawns 1 Brawler every wave
            {
                int idx = positions[0]; // Get the index for the first enemy position based on the randomized order
                int brawlerBaseDmg = 900 + (currentWave - 1) * 2; // Base damage for the brawler, it increases every wave by 2 times
                EnemyBrawler temp = new EnemyBrawler(EnemyBrawlerTex, enemyPositions[idx], null, Color.White, Color.Red, 30 + waveBonus, brawlerBaseDmg, player); // Create a new brawler enemy
                temp.SetOnClick(() => // Cant call temp directly in the lambda since its not assigned yet, so we use a temp variable to capture it in the moment
                {
                    if (currentPlayPhase == PlayPhase.SelectEnemy)
                    {
                        clickedEnemy = temp;
                    }
                });

                Vector2 healthBarPos = EnemyPos1Healthbar; // Get the healthbar position for this enemy based on the randomized order
                temp.HealthBar = new HealthBar(insttructionFont, graphicsDevice, healthBarPositions[idx], temp); // Create a healthbar for the enemy and link it to the enemy so that it can update based on the enemys health.


                Vector2 effectBoxPos = EnemyPos1EffectBox; // Get the effectbox position for this enemy based on the randomized order
                temp.EffectBoxes = new Instructions(insttructionFont, "", graphicsDevice, effectBoxPositions[idx]); // Create an effectbox for the enemy, which will show any active debuffs or buffs. It is also linked to the enemy so that it can check for any active effects when updating the text.
                temp.UpdateEffectBoxText(); // Sets the initial text for the effect box, which will be empty since there are no active effects at the start of the wave.

                enemys.Add(temp); // Add the enemy to the list of current enemys on the field so that it can be updated and drawn in the play update and draw methods.
            }





            for (int i = 0; i < 1; i++) 
            {

                int idx = positions[1];
                int mageBaseDps = 3 + (currentWave - 1); // Base damage per second for the mage, it increases every wave by 1 times
                EnemyMage temp = new EnemyMage(EnemyMageTex, enemyPositions[idx], null, Color.White, Color.Red, 20 + waveBonus, mageBaseDps, player);
                temp.SetOnClick(() =>
                {
                    if (currentPlayPhase == PlayPhase.SelectEnemy)
                    {
                        clickedEnemy = temp;
                    }
                });

                Vector2 healthBarPos = EnemyPos2Healthbar;
                temp.HealthBar = new HealthBar(insttructionFont, graphicsDevice, healthBarPositions[idx], temp);

                Vector2 effectBoxPos = EnemyPos2EffectBox;
                temp.EffectBoxes = new Instructions(insttructionFont, "", graphicsDevice, effectBoxPositions[idx]);
                temp.UpdateEffectBoxText();


                enemys.Add(temp);
            }



            for (int i = 0; i < 1; i++) 
            {
                int idx = positions[2];
                int healerBaseDps = 3 + (currentWave - 1); 
                int healAmount = 7 + (currentWave - 1); // Base heal amount for the healer, it increases every wave by 1 times
                EnemyHealer temp = new EnemyHealer(EnemyHealerTex, enemyPositions[idx], null, Color.White, Color.Red, 20 + waveBonus, healerBaseDps, healAmount, player, enemys);
                temp.SetOnClick(() =>
                {
                    if (currentPlayPhase == PlayPhase.SelectEnemy)
                    {
                        clickedEnemy = temp;
                    }
                });

                Vector2 healthBarPos = EnemyPos3Healthbar;
                temp.HealthBar = new HealthBar(insttructionFont, graphicsDevice, healthBarPositions[idx], temp);

                Vector2 effectBoxPos = EnemyPos3EffectBox;
                temp.EffectBoxes = new Instructions(insttructionFont, "", graphicsDevice, effectBoxPositions[idx]);
                temp.UpdateEffectBoxText();

                enemys.Add(temp);
            }

            //--------------------------------EnemySystem END--------------------------------//
        }



        /// <summary>
        /// Master Update method for Game Elements, calls specific update methods based on the current game state.
        /// 
        /// It makes sure that only the active game state's update logic is executed.
        /// </summary>
        public static void MASTER_UpdateGE(GameTime gameTime)
        {
            if (currentState != _previousState) // Check if the game state has changed since the last update
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







        /// <summary>
        /// Update method for the Menu game state, it updates the background and the menu buttons.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Menu_UpdateGE(GameTime gameTime)
        {
            //--------------------------------Background--------------------------------//
            background.Update(gameTime); // Updates the background animation if there is one, or just keeps it ready to be drawn if its not animated.
            //--------------------------------Background END--------------------------------//



            //-------------------------------MenuButtons--------------------------------//
            MouseState mouse = Mouse.GetState(); // Gets the current position of the mouse, which is used to update the buttons and check for clicks.

            foreach (Button button in menuButtons) // Updates each button in the menuButtons in the list and checks if the mouse is hovering over it or if it has been clicked, and changes its state accordingly.
            {
                button.Update(mouse);
            }
            //-------------------------------MenuButtons--------------------------------//
        }


        /// <summary>
        /// Draws the game menu, including the background and all menu buttons, to the specified sprite batch.
        /// </summary>
        public static void Menu_DrawGE(SpriteBatch spriteBatch)
        {
            //--------------------------------Background--------------------------------//
            background.Draw(spriteBatch); // Draws the background for the menu, if its animated it will draw the current frame of the animation, if not it will just draw the single texture.
            //--------------------------------Background--------------------------------//


            //-------------------------------MenuButtons--------------------------------//
            foreach (Button button in menuButtons) // Draws each button in the menuButtons list to the screen, including any hover or click effects based on their current state.
            {
                button.Draw(spriteBatch);
            }
            //-------------------------------MenuButtons END--------------------------------//

        }















        /// <summary>
        /// Updates the state of the game during the play phase, including player actions, enemy behavior, card
        /// selection, and wave progression.
        /// </summary>
        /// <remarks>This method is called once per frame while the game is in the play state. It
        /// manages user input, updates all active entities, processes card effects, and handles transitions between
        /// waves. The method assumes that all required game components and state variables are properly initialized
        /// before invocation.</remarks>
        public static void Play_UpdateGE(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState(); // Gets the current state of the mouse, which is used for player input to select cards and enemies during the play phase.

            //--------------------------------Background--------------------------------//
            background.Update(gameTime);
            //--------------------------------Background END--------------------------------//
            // Play update logic for game elements can be added here












            //--------------------------------MINI EnemySystem END--------------------------------//


            if (WaveOver) // If the wave is over, spawn a new wave of enemys and set the WaveOver flag to false until the next wave is completed.
            {
                SpawnEnemys(_graphicsDevice);
                WaveOver = false; // Reset the wave over flag for the next wave
            }

            currentWaveInfo.SetText($"Wave: {currentWave}"); // Update the current wave info text in the corner of the schreen.


            foreach (CombatEntity enemy in enemys) // Update each enemy in the enemys list, which includes their behavior and any active effects. It also updates their health bars to reflect any changes in health.
            {
                if (enemy.IsAlive) // Only update enemies that are still alive, dead enemies will not perform any actions or update their health bars.
                {
                    enemy.Update(mouse);
                    enemy.HealthBar.UpdateHealth();
                }
            }

            player.HealthBar.UpdateHealth(); // Update the player's health bar to reflect any changes in health.





            //--------------------------------CardSystem--------------------------------//
            switch (currentPlayPhase) // The play phase has two main states: selecting a card to play, and then selecting an enemy to target with that card. This switch statement manages the logic for both of those states.
            {
                case PlayPhase.SelectCard: // In the select card phase, the player can click on one of the three cards in their hand to select it.


                    // Update each card in the three card positions, which checks for mouse input to see if the player has clicked on any of the cards to select them.
                    pos1[pos1Index].Update(mouse);
                    pos2[pos2Index].Update(mouse);
                    pos3[pos3Index].Update(mouse);





                    selectedCard = null; // Reset the selected card and its position before checking which card has been selected
                    selectedCardPos = 0; // 0 means no card selected, 1 means card in pos1 selected, 2 means card in pos2 selected, 3 means card in pos3 selected

                    if (pos1[pos1Index].PosSelectedCard) // Check if the card in position 1 has been selected, if so set it as the selected card and record its position.
                    {
                        selectedCard = pos1[pos1Index];
                        selectedCardPos = 1;
                    }
                    else if (pos2[pos2Index].PosSelectedCard)
                    {
                        selectedCard = pos2[pos2Index];
                        selectedCardPos = 2;

                    }
                    else if (pos3[pos3Index].PosSelectedCard)
                    {
                        selectedCard = pos3[pos3Index];
                        selectedCardPos = 3;
                    }



                    if (selectedCard != null) // Makes sure youve actually selected a card before trying to activate it
                    {
                        // Sets the selected card's PosSelectedCard property back to false so that the loop can be restarted
                        pos1[pos1Index].PosSelectedCard = false;
                        pos2[pos2Index].PosSelectedCard = false;
                        pos3[pos3Index].PosSelectedCard = false;




                        if (selectedCard.target == "Self") // If the selected card is a self-targeting card, activate its effect immediately without needing to select an enemy target.
                        {
                            switch (selectedCard.name)
                            {
                                case "HealCard":
                                    player.Heal(5 + (currentWave - 1)); // Heals player for a base amount of 5, plus an additional amount based on the current wave to make the heal more effective as the game progresses.
                                    player.HealthBar.UpdateHealth(); // Update the player's health bar to reflect the healing effect.
                                    break;
                            }

                            player.WaitTurns(); // In case player has a debuff this makes the debuff lose a turn

                            //Checks if each enemy is alive and then passes a turn for them (They have a base number of turns they have to wait before they can perform an action so this passes a turn when you heal.
                            foreach (CombatEntity enemy in enemys)
                            {
                                if (enemy.IsAlive)
                                {
                                    enemy.WaitTurns();
                                    enemy.HealthBar.UpdateHealth();
                                }
                            }

                            if (!player.IsAlive) // If player dies, switch to the highscore state and forces you to enter a name for the highscore entry.
                            {
                                HighState = StateHighS.EnterHighScore;
                                currentState = State.HighScore;
                            }


                            switch (selectedCardPos) // Checks which card position the selected card was in and then randomizes that specific position to give you a new card in that position for the next turn.
                            {
                                case 1: pos1Index = randomCardGen.Next(pos1.Length); break;
                                case 2: pos2Index = randomCardGen.Next(pos2.Length); break;
                                case 3: pos3Index = randomCardGen.Next(pos3.Length); break;
                            }

                            currentPlayPhase = PlayPhase.SelectCard; // Resets you back to the select card phase because you used a self targeting card and dont need to select an enemy.
                        }


                        else if (selectedCard.target == "Enemy") // If the selected card is an enemy-targeting card, switch to the select enemy phase so that the player can choose which enemy to target with the card.
                        {
                            currentPlayPhase = PlayPhase.SelectEnemy;
                            break;
                        }
                    }
                    break;



                case PlayPhase.SelectEnemy:

                    if (clickedEnemy != null) // Check if an enemy has been clicked, if so apply the effect of the selected card to the clicked enemy based on the type of card that was selected.
                    {
                        switch (selectedCard.name)// Checks the name of the selected card to determine which effect to apply to the clicked enemy.
                        {
                            case "DamageCard": // If the card is a damage card, calculate the damage to deal to the enemy.
                                int baseDamage = 10;

                                if (player.IsDebuffed()) // Checks if the player is debuffed and decreases the damage accordingly,
                                {
                                    baseDamage = baseDamage - 2 - (currentWave - 1);
                                    clickedEnemy.TakeDamage(baseDamage);
                                }

                                else // If the player is not debuffed, deal the full damage to the enemy.
                                {
                                    clickedEnemy.TakeDamage(baseDamage);
                                }
                                break;


                            case "DebuffCard": // If the card is a debuff card, apply a debuff to the enemy that decreases their damage done by half for 3 turns.
                                clickedEnemy.GiveDebuff(4);
                                clickedEnemy.HealthBar.UpdateHealth();
                                break;
                        }
                        player.WaitTurns(); // Passes a turn for the player




                        foreach (CombatEntity enemy in enemys) // Passes a turn for the Enemy
                        {
                            if (enemy.IsAlive)
                            {
                                enemy.WaitTurns();
                            }
                        }

                        if (enemys.All(enemy => !enemy.IsAlive)) // Checks if all enemys are dead. if they are.
                        {
                            currentWave++; // Increase the wave number for the next wave, which will make the next wave harder but also increase the damage and healing of the players cards.
                            player.CurrentWave = currentWave; // Update the player's current wave property.
                            WaveOver = true; // Set the wave over flag to true, which will trigger the spawning of a new wave of enemys in the next update cycle.
                        }

                        if (!player.IsAlive) // If player dies, switch to the highscore state and forces you to enter a name for the highscore entry.
                        {
                            HighState = StateHighS.EnterHighScore;
                            currentState = State.HighScore;
                        }


                        switch (selectedCardPos) // resetst the card
                        {
                            case 1: pos1Index = randomCardGen.Next(pos1.Length); break;
                            case 2: pos2Index = randomCardGen.Next(pos2.Length); break;
                            case 3: pos3Index = randomCardGen.Next(pos3.Length); break;
                        }

                        //Reset the selected card and clicked enemy for the next turn, and switch back to the select card phase to start the next turn.
                        selectedCard = null;
                        clickedEnemy = null;
                        currentPlayPhase = PlayPhase.SelectCard;
                    }

                    break;
            }
            //--------------------------------CardSystem END--------------------------------//
        }



        /// <summary>
        /// Draws the current game state to the screen, including the background, player, enemies, cards, and related UI
        /// elements.
        /// </summary>
        /// <remarks>This method should be called during the game's draw phase to ensure all gameplay and
        /// UI elements are rendered in the correct order. Only alive enemies are drawn. The method also displays
        /// contextual instructions and highlights based on the current play phase.</remarks>
        public static void Play_DrawGE(SpriteBatch spriteBatch)
        {
            //--------------------------------Background--------------------------------//
            background.Draw(spriteBatch);
            //--------------------------------Background--------------------------------//



            //--------------------------------CardSystem--------------------------------//
            if (currentPlayPhase == PlayPhase.SelectEnemy) // If the player is in the select enemy phase, draw the instructions for selecting an enemy to target with the card.
            {
                cardInstrucions.Draw(spriteBatch);
            }

            pos1[pos1Index].Draw(spriteBatch);
            pos2[pos2Index].Draw(spriteBatch);
            pos3[pos3Index].Draw(spriteBatch);

            if (selectedCard != null) // Redraws the selected card
            {
                selectedCard.Draw(spriteBatch);
            }
            //--------------------------------CardSystem END--------------------------------//




            //--------------------------------EnemySystem--------------------------------//
            foreach (CombatEntity enemy in enemys) // Draw each enemy in the enemys list, which includes their current health and any active effects. Only alive enemies are drawn, dead enemies are not shown on the screen.
            {
                if (enemy.IsAlive)
                {
                    enemy.Draw(spriteBatch);
                    enemy.HealthBar.Draw(spriteBatch);
                    enemy.EffectBoxes.Draw(spriteBatch);
                }
            }
            //--------------------------------EnemySystem END--------------------------------//



            //--------------------------------Player--------------------------------//
            // Draw the player, which is effectively just a placeholder sprite, but also draws the player's health bar and effect box to show the player's current health and any active effects.
            player.Draw(spriteBatch);
            player.HealthBar.Draw(spriteBatch);
            player.EffectBoxes.Draw(spriteBatch);
            //--------------------------------Player END--------------------------------//



            //--------------------------------Wave Box--------------------------------//
            currentWaveInfo.Draw(spriteBatch);
            //--------------------------------Wave Box END--------------------------------//
        }















        /// <summary>
        /// Updates the state of the instructions screen, including the background and back button, based on the current
        /// game time and user input.
        /// </summary>
        public static void Instructions_UpdateGE(GameTime gameTime)
        {
            //--------------------------------Background--------------------------------//
            background.Update(gameTime);
            //--------------------------------Background END--------------------------------//



            //-------------------------------Back Button--------------------------------//
            MouseState mouse = Mouse.GetState(); // Gets mouse position
            backButton.Update(mouse); // Uses it to check if the back button is being hovered over or clicked, and brings you back to the menu if you click it.
            //-------------------------------Back Button END--------------------------------//
        }


        /// <summary>
        /// Draws the instructions screen, including the background, instructional text, and back button, using the
        /// specified sprite batch.
        /// </summary>
        public static void Instructions_DrawGE(SpriteBatch spriteBatch)
        {
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


















        /// <summary>
        /// Updates the high score screen, handling user input and state transitions for entering or displaying high
        /// scores.
        /// </summary>
        /// <remarks>This method manages both the process of entering a new high score and displaying
        /// existing high scores. It updates the background and back button, processes keyboard and mouse input, and
        /// transitions between states based on user actions. Pressing 'E' allows the user to enter a new high score,
        /// while pressing 'Backspace' or 'Escape' returns to the main menu.</remarks>
        public static void HighScore_UpdateGE(GameTime gameTime)
        {
            //--------------------------------Background--------------------------------//
            background.Update(gameTime);
            //--------------------------------Background END--------------------------------//





            //-------------------------------Back Button--------------------------------//
            MouseState mouse = Mouse.GetState();
            backButton.Update(mouse);
            //-------------------------------Back Button END--------------------------------//



            //--------------------------------HighScore--------------------------------//
            switch (HighState) // The highscore state has two main states: entering a new highscore, and printing the existing highscores. This switch statement manages the logic for both of those states.
            {
                case StateHighS.EnterHighScore: // In the enter highscore state, the player can type in their name for the highscore entry and then press enter to save it.

                    if (highscore.EnterUpdate(gameTime, currentWave))
                    {
                        highscore.SaveToFile("highscore.txt");
                        HighState = StateHighS.PrintHighScore; // Switch state when entry is complete
                        Reset();
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
        }


        public static void HighScore_DrawGE(SpriteBatch spriteBatch)
        {
            //--------------------------------Background--------------------------------//
            background.Draw(spriteBatch);
            //--------------------------------Background--------------------------------//



            //--------------------------------HighScore--------------------------------//
            switch (HighState)
            {
                case StateHighS.EnterHighScore: // Draws the Enter Highscore screen and the text that shows the current score you got and prompts you to enter your name for the highscore entry.
                    highscore.EnterDraw(spriteBatch, myFont);
                    break;
                default: // PrintHighScore. Shows the existing highscores
                    highscore.PrintDraw(spriteBatch, myFont);
                    backButton.Draw(spriteBatch);
                    break;
            }
            //--------------------------------HighScore END--------------------------------//
        }










        /// <summary>
        /// Resets the game state to start a new game, including resetting the player, spawning the first wave of enemies, and resetting all relevant gameplay variables to their initial states.
        /// </summary>
        public static void Reset()
        {
            player = new Player(PlayerTex, PlayerPos, null, Color.White, Color.Blue, 100, 0);
            player.HealthBar = new HealthBar(insttructionFont, _graphicsDevice, PlayerHealthBarPos, player);
            player.EffectBoxes = new Instructions(insttructionFont, "", _graphicsDevice, PlayerEffectBoxPos);
            player.UpdateEffectBoxText();

            // Reset wave
            currentWave = 1;

            // Spawn first wave enemies
            SpawnEnemys(_graphicsDevice);

            // Reset other gameplay variables
            selectedCard = null;
            clickedEnemy = null;
            currentPlayPhase = PlayPhase.SelectCard;
            WaveOver = false;
        }
    }
}