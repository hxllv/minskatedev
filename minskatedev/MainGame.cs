using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace minskatedev
{
    public partial class MainGame
    {
        Microsoft.Xna.Framework.Game game;
        GraphicsDeviceManager graphics;

        public SpriteBatch spriteBatch;
        public SpriteFont font;

        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        public Skate sk8;
        List<Box> boxes = new List<Box>();
        List<Floor> floor = new List<Floor>();
        List<Rail> rails = new List<Rail>();

        List<Vector3> floorCoords = new List<Vector3>();
        List<List<Matrix>> boxCoords = new List<List<Matrix>>();
        List<List<Matrix>> railCoords = new List<List<Matrix>>();

        bool firstPressE;
        bool editing;

        bool firstPressG;
        public bool gameOfSkating;

        bool firstPressEsc;
        public bool isPaused;

        public MainGame(Game game, GraphicsDeviceManager graphics, 
            Vector3 camTarget, Vector3 camPosition, Matrix projectionMatrix, Matrix viewMatrix, Matrix worldMatrix)
        {
            this.game = game;
            this.graphics = graphics;
            this.camTarget = camTarget;
            this.camPosition = camPosition;
            this.projectionMatrix = projectionMatrix;
            this.viewMatrix = viewMatrix;
            this.worldMatrix = worldMatrix;
            this.firstPressE = false;
            this.editing = false;
            this.firstPressG = false;
            this.gameOfSkating = false;
            this.firstPressEsc = false;
            this.isPaused = false;
        }

        void AddFloors()
        {
            if (floorCoords.Count == 0)
            {
                this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(0f, 0f, 0f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
                this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(0f, 0f, 12.5f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
                this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(0f, 0f, -12.5f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
                this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(12.5f, 0f, 0f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
                this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(12.5f, 0f, 12.5f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
                this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(12.5f, 0f, -12.5f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
                this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(-12.5f, 0f, 0f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
                this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(-12.5f, 0f, 12.5f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
                this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(-12.5f, 0f, -12.5f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
                return;
            }
            
            foreach (Vector3 coord in floorCoords)
            {
                this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(coord), Matrix.Identity, Matrix.Identity, Matrix.Identity));
            }
        }

        void AddObjects()
        {
            foreach (List<Matrix> coord in boxCoords)
            {
                this.boxes.Add(new Box(this.game, coord[0], Matrix.Identity, coord[1], Matrix.Identity));
            }

            foreach (List<Matrix> coord in railCoords)
            {
                this.rails.Add(new Rail(this.game, coord[0], Matrix.Identity, coord[1], Matrix.Identity));
            }
        }

        void SaveObjects()
        {
            floorCoords.Clear();
            boxCoords.Clear();
            railCoords.Clear();

            foreach (Floor temp in floor)
            {
                floorCoords.Add(temp.floor.translation.Translation);
            }

            foreach (Box temp in boxes)
            {
                boxCoords.Add(new List<Matrix>() { temp.box.translation, temp.box.rotationY });
            }

            foreach (Rail temp in rails)
            {
                railCoords.Add(new List<Matrix>() { temp.rail.translation, temp.rail.rotationY });
            }
        }

        void AddFloorsGameOfSkate()
        {
            this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(0f, 0f, 0f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
            this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(12.5f, 0f, 0f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
            this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(25f, 0f, 0f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
            this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(37.5f, 0f, 0f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
            this.floor.Add(new Floor(this.game, Matrix.CreateTranslation(50f, 0f, 0f), Matrix.Identity, Matrix.Identity, Matrix.Identity));
        }

        public void MainGameInit(ModelHelper[] sk8)
        {
            //Setup Camera
            camTarget = new Vector3(0f, 0.8f, 0f);
            camPosition = new Vector3(-5f, 2.3f, 0f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphics.GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0, 1f, 0f));
            worldMatrix = Matrix.CreateWorld(new Vector3(0f, 0f, 0f), Vector3.Forward, Vector3.Up);
            this.sk8 = new Skate(sk8, this);
            Skate.Input.Physics.speed = 0;
            Skate.Input.Physics.turning = 0;
            Skate.Input.Physics.vertVel = 0;

            AddFloors();

            EditWorld.InitEditWorld(this.game, worldMatrix);
            GameOfSkate.InitGameOfSkate();

            Skate.Input.Animations.sk8 = this.sk8;
            Skate.Input.Animations.Flip.StopTrick();
            Skate.Input.Animations.Shuv.StopTrick();
            Skate.Input.Animations.Ollie.ForceStop();
            Skate.Input.fuckedTrick = false;
            Skate.Input.doingTricks = new List<int>();
            Skate.Input.TrickNames.trickName = "";
            this.sk8.ResetSk8();
        }

        public void MainGameUpdate()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !firstPressEsc)
            {
                isPaused = !isPaused;
                firstPressEsc = true;
                Skate.Input.Sounds.StopRoll();
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Escape))
            {
                firstPressEsc = false;
            }

            if (isPaused) return;

            Skate.Input.sk8 = sk8;
            Skate.Input.boxes = boxes;
            Skate.Input.floor = floor;
            Skate.Input.rails = rails;

            decimal[] sk8Vals = Skate.Input.UpdateInput();

            if (Keyboard.GetState().IsKeyDown(Keys.E) && !this.firstPressE && !this.gameOfSkating)
            {
                this.editing = !this.editing;
                this.firstPressE = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.E))
            {
                this.firstPressE = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.G) && !this.firstPressG && !this.editing)
            {
                this.gameOfSkating = !this.gameOfSkating;
                if (this.gameOfSkating)
                {
                    SaveObjects();
                    floor.Clear();
                    boxes.Clear();
                    rails.Clear();
                    AddFloorsGameOfSkate();
                    GameOfSkate.InitGameOfSkate();
                    sk8.ResetSk8(new Vector3(0, 1f, 0), 0, new Vector3(0f, 0.8f, 0f), new Vector3(-5f, 2.3f, 0f));
                }
                else
                {
                    floor.Clear();
                    AddFloors();
                    AddObjects();
                    sk8.ResetSk8();
                }
                this.firstPressG = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.G))
            {
                this.firstPressG = false;
            }

            viewMatrix = this.sk8.Move(ref camTarget, ref camPosition, (float)sk8Vals[0]);
            viewMatrix = this.sk8.VertMove(ref camTarget, ref camPosition, (float)sk8Vals[1]);
            viewMatrix = this.sk8.Turn(ref camTarget, ref camPosition, (float)sk8Vals[2]);

            if (this.sk8.deck.translation.M42 < -5f)
                this.sk8.ResetSk8();

            if (this.editing)
                EditWorld.UpdateEditWorld(this, game, this.sk8);

            if (this.gameOfSkating)
                GameOfSkate.UpdateGameOfSkate(this, game, this.sk8);
        }

        public void MainGameDraw()
        {
            foreach (Box box in boxes)
            {
                box.BoxDraw(viewMatrix, projectionMatrix);
            }
            foreach (Floor floor in floor)
            {
                floor.FloorDraw(viewMatrix, projectionMatrix);
            }
            foreach (Rail rail in rails)
            {
                rail.RailDraw(viewMatrix, projectionMatrix);
            }

            sk8.SkateDraw(viewMatrix, projectionMatrix);

            if (this.editing)
                EditWorld.DrawEditWorld(this.viewMatrix, this.projectionMatrix, this.sk8);

            if (this.gameOfSkating && !isPaused)
                GameOfSkate.DrawGameOfSkate(this);

            if (Skate.Input.Physics.isCollidingGround && !Skate.Input.Physics.keepVertMomentum && !isPaused)
                Skate.Input.TrickNames.DrawTrick();
        }

        static BoundingBox UpdateBoundingBox(Model model, Matrix worldTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            max.Y += 0.1f;

            // Create and return bounding box
            return new BoundingBox(min, max);
        }

        static BoundingSphere UpdateBoundingSphere(Vector3 pos, float angle, bool front = true)
        {
            Vector3 newPos = pos;
            if (front)
                newPos = pos + new Vector3((float)Math.Cos(angle) * 0.61f, 0.3125f, -(float)Math.Sin(angle) * 0.61f);
            else
                newPos = pos + new Vector3(-(float)Math.Cos(angle) * 0.61f, 0.3125f, (float)Math.Sin(angle) * 0.61f);

            return new BoundingSphere(newPos, 0.3125f);
        }
    }
}
