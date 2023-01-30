using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace minskatedev
{
    public partial class Game : Microsoft.Xna.Framework.Game
    {
        public static int gameState = 0;

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteFont font;

        //Camera
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        public static Menu menu;
        Button playButton;
        Button quitButton;
        Button editSk8Button;
        Button backToMenuButton;
        Button editBackButton;

        Button wheelsButton;
        Button wheelsDefaultButton;
        Button wheelsBlackButton;
        Button wheelsNavyButton;
        Button wheelsPurpleButton;
        Button wheelsTurqButton;

        Button trucksButton;
        Button trucksDefaultButton;
        Button trucksBlackButton;
        Button trucksNavyButton;
        Button trucksPurpleButton;
        Button trucksTurqButton;
        Button trucksWhiteButton;

        Button deckButton;
        Button deckDefaultButton;
        Button deckShakeButton;
        Button deckSk8Button;

        public static MainGame mainGame;
        Button setResetButton;
        Button resetButton;

        Button resumeButton;
        Button backToMenu2Button;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            menu = new Menu(this, graphics, camTarget, camPosition, projectionMatrix, viewMatrix, worldMatrix);
            mainGame = new MainGame(this, graphics, camTarget, camPosition, projectionMatrix, viewMatrix, worldMatrix);
            Button.game = this;
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            base.Initialize();

            menu.MenuInit();
        }

        protected override void LoadContent()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            mainGame.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.font = Content.Load<SpriteFont>("Default");
            mainGame.font = this.font;
            MainGame.Skate.Input.Sounds.roll = Content.Load<SoundEffect>("sounds/roll");
            MainGame.Skate.Input.Sounds.pop = Content.Load<SoundEffect>("sounds/pop");
            MainGame.Skate.Input.Sounds.land = Content.Load<SoundEffect>("sounds/land");
            MainGame.Skate.Input.Sounds.rollInstance = MainGame.Skate.Input.Sounds.roll.CreateInstance();

            resumeButton = new Button("resume", "Resume", 30, 60, 1);
            backToMenu2Button = new Button("backtomenu2", "Quit to Main Menu", 30, 120, 1);
            setResetButton = new Button("setreset", Content.Load<Texture2D>("imgs/menu/setreset"), 
                Content.Load<Texture2D>("imgs/menu/setresethover"),
                graphics.PreferredBackBufferWidth - 150, 10, 0.7f);
            resetButton = new Button("reset", Content.Load<Texture2D>("imgs/menu/reset"), 
                Content.Load<Texture2D>("imgs/menu/resethover"),
                graphics.PreferredBackBufferWidth - 150, (int)(setResetButton.text.Height * 0.7f + 30), 0.7f);
            resetButton.buttonX += (int)(setResetButton.text.Width * 0.7f - resetButton.text.Width * 0.7f);

            playButton = new Button("play", "Play", 30, 60, 1);
            editSk8Button = new Button("editsk8", "Edit Skate", 30, 120, 1);
            quitButton = new Button("quit", "Quit", 30, 180, 1);
            backToMenuButton = new Button("backtomenu", "Back to Main Menu", 5, 5, 0.6f);
            editBackButton = new Button("editback", "Back", 30, 60, 1);

            wheelsButton = new Button("wheels", "Wheels", 30, 60, 1);
            wheelsDefaultButton = new Button("wdef", "Default", 30, 120, 1);
            wheelsBlackButton = new Button("wblack", "Black", 30, 180, 1);
            wheelsNavyButton = new Button("wnavy", "Navy Blue", 30, 240, 1);
            wheelsPurpleButton = new Button("wpurp", "Purple", 30, 300, 1);
            wheelsTurqButton = new Button("wturq", "Turqoise", 30, 360, 1);

            trucksButton = new Button("trucks", "Trucks", 30, 120, 1);
            trucksDefaultButton = new Button("tdef", "Default", 30, 120, 1);
            trucksBlackButton = new Button("tblack", "Black", 30, 180, 1);
            trucksNavyButton = new Button("tnavy", "Navy Blue", 30, 240, 1);
            trucksPurpleButton = new Button("tpurp", "Purple", 30, 300, 1);
            trucksTurqButton = new Button("tturq", "Turqoise", 30, 360, 1);
            trucksWhiteButton = new Button("twhite", "White", 30, 420, 1);

            deckButton = new Button("deck", "Deck", 30, 180, 1);
            deckDefaultButton = new Button("ddef", "Default", 30, 120, 1);
            deckShakeButton = new Button("dshake", "Shake Junt", 30, 180, 1);
            deckSk8Button = new Button("dsk8", "Sk8", 30, 240, 1);
        }

        protected override void UnloadContent()
        {
        }

        [STAThread]
        protected override void Update(GameTime gameTime)
        {
            if (gameState == 0)
            {
                menu.MenuUpdate();
                if (menu.menuState == 0)
                {
                    playButton.Update();
                    editSk8Button.Update();
                    quitButton.Update();
                }
                // edit skate menu
                else if (menu.menuState == 1)
                {
                    if (menu.editState != 0)
                        editBackButton.Update();

                    if (menu.editState == 0)
                    {
                        backToMenuButton.Update();
                        wheelsButton.Update();
                        trucksButton.Update();
                        deckButton.Update();
                    }
                    else if (menu.editState == 1)
                    {
                        wheelsDefaultButton.Update();
                        wheelsBlackButton.Update();
                        wheelsNavyButton.Update();
                        wheelsPurpleButton.Update();
                        wheelsTurqButton.Update();
                    }
                    else if (menu.editState == 2)
                    {
                        trucksDefaultButton.Update();
                        trucksBlackButton.Update();
                        trucksNavyButton.Update();
                        trucksPurpleButton.Update();
                        trucksTurqButton.Update();
                        trucksWhiteButton.Update();
                    }
                    else if (menu.editState == 3)
                    {
                        deckDefaultButton.Update();
                        deckShakeButton.Update();
                        deckSk8Button.Update();
                    }
                }
            }

            if (gameState == 1)
            {
                mainGame.MainGameUpdate();

                if (mainGame.isPaused)
                {
                    resumeButton.Update();
                    backToMenu2Button.Update();
                }
                else
                {
                    resetButton.Update();
                    setResetButton.Update();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (gameState == 0)
            {
                menu.MenuDraw();
                spriteBatch.Begin();
                if (menu.menuState == 0)
                {
                    playButton.Draw(spriteBatch);
                    editSk8Button.Draw(spriteBatch);
                    quitButton.Draw(spriteBatch);
                }
                // edit skate menu
                else if (menu.menuState == 1)
                {
                    if (menu.editState != 0)
                        editBackButton.Draw(spriteBatch);

                    if (menu.editState == 0)
                    {
                        backToMenuButton.Draw(spriteBatch);
                        wheelsButton.Draw(spriteBatch);
                        trucksButton.Draw(spriteBatch);
                        deckButton.Draw(spriteBatch);
                    }
                    else if (menu.editState == 1)
                    {
                        wheelsDefaultButton.Draw(spriteBatch);
                        wheelsBlackButton.Draw(spriteBatch);
                        wheelsNavyButton.Draw(spriteBatch);
                        wheelsPurpleButton.Draw(spriteBatch);
                        wheelsTurqButton.Draw(spriteBatch);
                    }
                    else if (menu.editState == 2)
                    {
                        trucksDefaultButton.Draw(spriteBatch);
                        trucksBlackButton.Draw(spriteBatch);
                        trucksNavyButton.Draw(spriteBatch);
                        trucksPurpleButton.Draw(spriteBatch);
                        trucksTurqButton.Draw(spriteBatch);
                        trucksWhiteButton.Draw(spriteBatch);
                    }
                    else if (menu.editState == 3)
                    {
                        deckDefaultButton.Draw(spriteBatch);
                        deckShakeButton.Draw(spriteBatch);
                        deckSk8Button.Draw(spriteBatch);
                    }
                }

                spriteBatch.End();
            }

            if (gameState == 1)
            {
                mainGame.MainGameDraw();
                spriteBatch.Begin();

                if (mainGame.isPaused)
                {
                    resumeButton.Draw(spriteBatch);
                    backToMenu2Button.Draw(spriteBatch);
                }
                else if (!mainGame.gameOfSkating)
                { 
                    resetButton.Draw(spriteBatch);
                    setResetButton.Draw(spriteBatch);
                }
                spriteBatch.End();
            }

            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw(gameTime);
        }

    }
}
