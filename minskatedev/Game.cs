using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace minskatedev
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static int gameState = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Camera
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        public static Menu menu;
        public static MainGame mainGame;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            menu = new Menu(this, graphics, camTarget, camPosition, projectionMatrix, viewMatrix, worldMatrix);
            mainGame = new MainGame(this, graphics, camTarget, camPosition, projectionMatrix, viewMatrix, worldMatrix);
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            base.Initialize();

            menu.MenuInit();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        [STAThread]
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();

            if (gameState == 0)
                menu.MenuUpdate();
            if (gameState == 1)
                mainGame.MainGameUpdate();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (gameState == 0)
                menu.MenuDraw();
            if (gameState == 1)
                mainGame.MainGameDraw();

            base.Draw(gameTime);
        }
    }
}
