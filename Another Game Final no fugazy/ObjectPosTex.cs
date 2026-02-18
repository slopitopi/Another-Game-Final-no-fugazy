using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Another_Game_Final_no_fugazy
{
    internal static class ObjectPosTex
    {
        private static ContentManager content;
        private static GraphicsDevice graphicsDevice;
        private static GameWindow window;


        //--------------------------------CARD--------------------------------//

        private static Texture2D damageCardTex;
        private static Texture2D healCardTex;

        public readonly static Rectangle CardPos1 = new Rectangle(360, 600, 160, 240);
        public readonly static Rectangle CardPos2 = new Rectangle(560, 600, 160, 240);
        public readonly static Rectangle CardPos3 = new Rectangle(760, 600, 160, 240);



        public static Texture2D DamageCardTex
        {
            get { return damageCardTex; }
        }

        public static Texture2D HealCardTex
        {
            get { return healCardTex; }
        }

        //--------------------------------CARD END--------------------------------//




        //--------------------------------ENEMY--------------------------------//
        private static Texture2D enemyBrawlerTex;

        public static Texture2D EnemyBrawlerTex
        {
            get { return enemyBrawlerTex; }
        }

        public readonly static Rectangle EnemyPos1 = new Rectangle(800, 240, 80, 120);
        public readonly static Vector2 EnemyPos1Healthbar = new Vector2(800 + 20, 240 + (120 + 25));

        public readonly static Vector2 EnemyPos1EffectBox = new Vector2(800 + 100, 240 + 20);
        //--------------------------------ENEMY--------------------------------//

        //--------------------------------PLAYER--------------------------------//
        private static Texture2D playerTex;
        public static Texture2D PlayerTex
        {
            get { return playerTex; }
        }

        public readonly static Rectangle PlayerPos = new Rectangle(200, 240, 80, 120);
        public readonly static Vector2 PlayerHealthBarPos = new Vector2(200 + 20, 240 + (120 + 25));

        public readonly static Vector2 PlayerEffectBoxPos = new Vector2(50, 500);

        //--------------------------------PLAYER ENMD--------------------------------//




        //--------------------------------BUTTONS--------------------------------//
        private static Texture2D menuButtonPlayTex;
        private static Texture2D menuButtonHighScoreTex;
        private static Texture2D menuButtonInstructionsTex;
        private static Texture2D menuButtonQuitTex;
        private static Texture2D backButtonTex;


        public static Texture2D BackButtonTex
        {
            get { return backButtonTex; }
        }

        public static Texture2D MenuButtonPlayTex
        {
            get { return menuButtonPlayTex; }
        }

        public static Texture2D MenuButtonHighScoreTex
        {
            get { return menuButtonHighScoreTex; }
        }

        public static Texture2D MenuButtonInstructionsTex
        {
            get { return menuButtonInstructionsTex; }
        }

        public static Texture2D MenuButtonQuitTex
        {
            get { return menuButtonQuitTex; }
        }





        public readonly static Rectangle menuButtonPlayRect = new Rectangle(540, 100, 200, 50);
        public readonly static Rectangle menuButtonHighScoreRect = new Rectangle(540, 170, 200, 50);
        public readonly static Rectangle menuButtonInstructionsRect = new Rectangle(540, 240, 200, 50);
        public readonly static Rectangle MenuButtonQuitRect = new Rectangle(540, 310, 200, 50);
        public readonly static Rectangle backButtonPos = new Rectangle(1280 - 110, 720 - 60, 100, 50);
        //--------------------------------BUTTONS END--------------------------------//




        //--------------------------------BACKGROUNDS--------------------------------//
        //private static Texture2D highScoreBackgroundTex;
        private static Texture2D instructionsBackgroundTex;
        private static Texture2D playBackgroundTex;



        //public static Texture2D HighScoreBackgroundTex
        //{
        //    get { return highScoreBackgroundTex; }
        //}
        public static Texture2D InstructionsBackgroundTex
        {
            get { return instructionsBackgroundTex; }
        }

        public static Texture2D PlayBackgroundTex
        {
            get { return playBackgroundTex; }
        }




        private static Texture2D[] menuBackgroundFrames;

        public static Texture2D[] MenuBackgroundFrames
        {
            get { return menuBackgroundFrames; }
        }


        //--------------------------------BACKGROUNDS END--------------------------------//


        //--------------------------------TEXT/INSRUCTIONS--------------------------------//
        private static SpriteFont mainFont;
        public static SpriteFont MainFont
        {
            get { return mainFont; }
        }

        public readonly static Vector2 CardInstructionsPos = new Vector2(0, 50);
        public readonly static Vector2 EnemyHealthBarPos1 = new Vector2(400, 220);
        //--------------------------------TEXT/INSRUCTIONS END--------------------------------//







        public static void Load_Pos_Tex(ContentManager content, GameWindow window, GraphicsDevice graphicsDevice)
        {
            //--------------------------------CARD--------------------------------//
            damageCardTex = content.Load<Texture2D>("images/Cards/DmgCard");
            healCardTex = content.Load<Texture2D>("images/Cards/HealCard");
            //--------------------------------CARD END--------------------------------//


            //--------------------------------ENEMY--------------------------------//
            enemyBrawlerTex = content.Load<Texture2D>("images/Enemy/EnemySprite");
            //--------------------------------ENEMY END--------------------------------//




            //--------------------------------PLATER--------------------------------//
            playerTex = content.Load<Texture2D>("images/Player/PlayerSprite");
            //--------------------------------PLAYER END--------------------------------//




            //--------------------------------BUTTONS --------------------------------//
            menuButtonPlayTex = content.Load<Texture2D>("images/menu/playButton_");
            menuButtonHighScoreTex = content.Load<Texture2D>("images/menu/highScore_");
            menuButtonInstructionsTex = content.Load<Texture2D>("images/menu/instructions_");
            menuButtonQuitTex = content.Load<Texture2D>("images/menu/quit_");
            backButtonTex = content.Load<Texture2D>("images/Instructions/BackButton");
            //--------------------------------BUTTONS END--------------------------------//






            //--------------------------------BACKGROUNDS--------------------------------//
            instructionsBackgroundTex = content.Load<Texture2D>("images/Instructions/Background");
            playBackgroundTex = content.Load<Texture2D>("images/Battleground/Battleground_BG1");
            //highScoreBackgroundTex = content.Load<Texture2D>("images/highscore/HighScoreBackground");


            menuBackgroundFrames = new Texture2D[]
            {
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

            };
            //--------------------------------BACKGROUNDS END--------------------------------//







            //--------------------------------TEXT/INSRUCTIONS--------------------------------//
            mainFont = content.Load<SpriteFont>("File");
            //--------------------------------TEXT/INSRUCTIONS--------------------------------//

        }






    }
}