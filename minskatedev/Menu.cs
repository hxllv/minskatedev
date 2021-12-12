using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace minskatedev
{
    public class Menu
    {
        Game game;
        GraphicsDeviceManager graphics;

        int mouseX, mouseY;
        bool mDown = false;
        bool orbit = false;
        bool rightOrbit = true;

        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        ModelHelper[] sk8 = new ModelHelper[7];
        ModelHelper[] sk8Def = new ModelHelper[7];
        ModelHelper[] wheelsFL = new ModelHelper[4];
        ModelHelper[] wheelsFR = new ModelHelper[4];
        ModelHelper[] wheelsBL = new ModelHelper[4];
        ModelHelper[] wheelsBR = new ModelHelper[4];
        ModelHelper[] decks = new ModelHelper[4];
        ModelHelper[] trucksF = new ModelHelper[5];
        ModelHelper[] trucksB = new ModelHelper[5];

        public Menu(Game game, GraphicsDeviceManager graphics,
            Vector3 camTarget, Vector3 camPosition, Matrix projectionMatrix, Matrix viewMatrix, Matrix worldMatrix)
        {
            this.game = game;
            this.graphics = graphics;
            this.camTarget = camTarget;
            this.camPosition = camPosition;
            this.projectionMatrix = projectionMatrix;
            this.viewMatrix = viewMatrix;
            this.worldMatrix = worldMatrix;
        }

        public void MenuInit()
        {
            //Setup Camera
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 2f, -4f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphics.GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0, 1f, 0f));
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);
            bool fDone = false;
            bool fDoneT = false;

            int x = 0;
            foreach (string i in new string[] { "wheell", "wheelr", "wheell", "wheelr", "deck", "trucks", "trucks" })
            {
                Matrix transMat = Matrix.Identity;
                Matrix rotMat = Matrix.Identity;
                if (i == "wheell")
                {
                    rotMat = Matrix.CreateRotationY((float)Math.PI);
                    transMat = Matrix.CreateTranslation(1f, 0, 0.3125f);
                    if (fDone)
                        transMat = Matrix.CreateTranslation(-0.6f, 0, 0.3125f);                
                }
                if (i == "wheelr")
                {
                    transMat = Matrix.CreateTranslation(0.6f, 0, -0.3125f);
                    if (fDone)
                        transMat = Matrix.CreateTranslation(-0.6f, 0, -0.3125f);
                    fDone = true;
                }
                if (i == "trucks")
                {
                    transMat = Matrix.CreateTranslation(0.6f, 0, 0);
                    if (fDoneT)
                        transMat = Matrix.CreateTranslation(-0.6f, 0, 0);
                    fDoneT = true;
                }
                sk8Def[x] = new ModelHelper(game, "models\\sk8\\" + i, transMat, Matrix.Identity, rotMat, Matrix.Identity);
                x++;
            }

            x = 0;
            foreach (string i in new string[] { "black", "navyblue", "purpleish", "turq" })
            {
                wheelsFL[x] = new ModelHelper(game, "models\\sk8\\wheels\\" + i + "l", Matrix.CreateTranslation(0.6f, 0, 0.3125f), Matrix.Identity, Matrix.CreateRotationY((float)Math.PI), Matrix.Identity);
                wheelsFR[x] = new ModelHelper(game, "models\\sk8\\wheels\\" + i + "r", Matrix.CreateTranslation(0.6f, 0, -0.3125f), Matrix.Identity, Matrix.Identity, Matrix.Identity);
                wheelsBL[x] = new ModelHelper(game, "models\\sk8\\wheels\\" + i + "l", Matrix.CreateTranslation(-0.6f, 0, 0.3125f), Matrix.Identity, Matrix.CreateRotationY((float)Math.PI), Matrix.Identity);
                wheelsBR[x] = new ModelHelper(game, "models\\sk8\\wheels\\" + i + "r", Matrix.CreateTranslation(-0.6f, 0, -0.3125f), Matrix.Identity, Matrix.Identity, Matrix.Identity);
                x++;
            }

            x = 0;
            foreach (string i in new string[] { "shake", "sk8" })
            {
                decks[x] = new ModelHelper(game, "models\\sk8\\decks\\" + i, Matrix.CreateTranslation(0, 0, 0), Matrix.Identity, Matrix.Identity, Matrix.Identity);
                x++;
            }

            x = 0;
            foreach (string i in new string[] { "black", "navyblue", "purpleish", "turq", "white" })
            {
                trucksF[x] = new ModelHelper(game, "models\\sk8\\trucks\\" + i, Matrix.CreateTranslation(0.6f, 0, 0), Matrix.Identity, Matrix.Identity, Matrix.Identity);
                trucksB[x] = new ModelHelper(game, "models\\sk8\\trucks\\" + i, Matrix.CreateTranslation(-0.6f, 0, 0), Matrix.Identity, Matrix.Identity, Matrix.Identity);
                x++;
            }

            //wheels

            //front
            sk8[0] = sk8Def[0];
            sk8[1] = sk8Def[1];

            //back
            sk8[2] = sk8Def[2];
            sk8[3] = sk8Def[3];

            //deck
            sk8[4] = sk8Def[4];

            //trucks
            sk8[5] = sk8Def[5];
            sk8[6] = sk8Def[6];
        }

        public void MenuUpdate()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Game.gameState = 1;
                Game.mainGame.MainGameInit(sk8);
            }

            // horrible temp code

            bool d = Keyboard.GetState().IsKeyDown(Keys.D);
            bool t = Keyboard.GetState().IsKeyDown(Keys.T);
            bool w = Keyboard.GetState().IsKeyDown(Keys.W);

            // wheel selection

            if (w && Keyboard.GetState().IsKeyDown(Keys.NumPad1))
            {
                sk8[0] = sk8Def[0];
                sk8[1] = sk8Def[1];
                sk8[2] = sk8Def[2];
                sk8[3] = sk8Def[3];
            }
            if (w && Keyboard.GetState().IsKeyDown(Keys.NumPad2))
            {
                LoopWheels(0);
            }
            if (w && Keyboard.GetState().IsKeyDown(Keys.NumPad3))
            {
                LoopWheels(1);

            }
            if (w && Keyboard.GetState().IsKeyDown(Keys.NumPad4))
            {
                LoopWheels(2);

            }
            if (w && Keyboard.GetState().IsKeyDown(Keys.NumPad5))
            {
                LoopWheels(3);

            }

            // deck selection

            if (d && Keyboard.GetState().IsKeyDown(Keys.NumPad1))
            {
                sk8[4] = sk8Def[4];
            }
            if (d && Keyboard.GetState().IsKeyDown(Keys.NumPad2))
            {
                sk8[4] = decks[0];
            }
            if (d && Keyboard.GetState().IsKeyDown(Keys.NumPad3))
            {
                sk8[4] = decks[1];
            }

            // trucks selection

            if (t && Keyboard.GetState().IsKeyDown(Keys.NumPad1))
            {
                sk8[5] = sk8Def[5];
                sk8[6] = sk8Def[6];
            }
            if (t && Keyboard.GetState().IsKeyDown(Keys.NumPad2))
            {
                LoopTrucks(0);
            }
            if (t && Keyboard.GetState().IsKeyDown(Keys.NumPad3))
            {
                LoopTrucks(1);

            }
            if (t && Keyboard.GetState().IsKeyDown(Keys.NumPad4))
            {
                LoopTrucks(2);

            }
            if (t && Keyboard.GetState().IsKeyDown(Keys.NumPad5))
            {
                LoopTrucks(3);

            }
            if (t && Keyboard.GetState().IsKeyDown(Keys.NumPad6))
            {
                LoopTrucks(4);

            }

            //orbit

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rightOrbit = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rightOrbit = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                orbit = !orbit;
            }

            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(.5f));
                if (!rightOrbit)
                {
                    rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(-.5f));
                }

                camPosition = Vector3.Transform(camPosition, rotationMatrix);
            }

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (!mDown)
                {
                    MouseState currentMouse = Mouse.GetState();
                    mouseX = currentMouse.X;
                    mouseY = currentMouse.Y;
                    mDown = true;
                }

                MouseMove();
            }
            else
            {
                mDown = false;
            }
        }

        private void LoopWheels(int x)
        {
            sk8[0] = wheelsFL[x];
            sk8[1] = wheelsFR[x];
            sk8[2] = wheelsBL[x];
            sk8[3] = wheelsBR[x];
        }

        private void LoopTrucks(int x)
        {
            sk8[5] = trucksF[x];
            sk8[6] = trucksB[x];
        }

        public void MenuDraw()
        {
            foreach (ModelHelper model in sk8)
            {

                //for every model
                foreach (ModelMesh mesh in model.model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.View = viewMatrix;
                        effect.World = model.rotationX * model.rotationY * model.rotationZ * model.translation;
                        effect.Projection = projectionMatrix;
                    }

                    //Draw Model
                    mesh.Draw();
                }
            }
        }

        public void MouseMove()
        {
            MouseState currentMouse = Mouse.GetState();

            int Y = currentMouse.X - mouseX;
            int X = currentMouse.Y - mouseY;
            float TRX = (float)Math.Cos(Math.Acos(viewMatrix.M11)) * X;
            float TRZ = (float)Math.Sin(Math.Acos(viewMatrix.M11)) * X;

            if (viewMatrix.M13 >= 0)
            {
                TRX = (float)Math.Cos(-Math.Acos(viewMatrix.M11)) * X;
                TRZ = (float)Math.Sin(-Math.Acos(viewMatrix.M11)) * X;
            }

            Matrix rotationMatrixY = Matrix.CreateRotationY(-MathHelper.ToRadians(Y));
            Matrix rotationMatrixX = Matrix.CreateRotationX(-MathHelper.ToRadians(TRX));
            Matrix rotationMatrixZ = Matrix.CreateRotationZ(-MathHelper.ToRadians(TRZ));
            camPosition = Vector3.Transform(camPosition, rotationMatrixY);
            camPosition = Vector3.Transform(camPosition, rotationMatrixX);
            camPosition = Vector3.Transform(camPosition, rotationMatrixZ);

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

            mouseX = currentMouse.X;
            mouseY = currentMouse.Y;
        }
    }
}
