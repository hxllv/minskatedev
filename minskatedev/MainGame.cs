using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace minskatedev
{
    public partial class MainGame
    {
        Microsoft.Xna.Framework.Game game;
        GraphicsDeviceManager graphics;

        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        Skate sk8;
        List<Box> boxes = new List<Box>();
        List<Floor> floor = new List<Floor>();
        List<Rail> rails = new List<Rail>();

        bool firstPressE;
        bool editing;

        public MainGame(Microsoft.Xna.Framework.Game game, GraphicsDeviceManager graphics,
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
        }

        public void MainGameInit(ModelHelper[] sk8)
        {
            //Setup Camera
            camTarget = new Vector3(0f, 0.8f, 0f);
            camPosition = new Vector3(-5f, 2.3f, 0f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphics.GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0, 1f, 0f));
            worldMatrix = Matrix.CreateWorld(new Vector3(0f, 0f, 0f), Vector3.Forward, Vector3.Up);
            this.sk8 = new Skate(sk8);
            this.floor.Add(new Floor(this.game, this.worldMatrix, Matrix.CreateTranslation(0f, 0f, 0f)));
            this.floor.Add(new Floor(this.game, this.worldMatrix, Matrix.CreateTranslation(0f, 0f, 12.5f)));
            this.floor.Add(new Floor(this.game, this.worldMatrix, Matrix.CreateTranslation(0f, 0f, -12.5f)));
            this.floor.Add(new Floor(this.game, this.worldMatrix, Matrix.CreateTranslation(12.5f, 0f, 0f)));
            this.floor.Add(new Floor(this.game, this.worldMatrix, Matrix.CreateTranslation(12.5f, 0f, 12.5f)));
            this.floor.Add(new Floor(this.game, this.worldMatrix, Matrix.CreateTranslation(12.5f, 0f, -12.5f)));
            this.floor.Add(new Floor(this.game, this.worldMatrix, Matrix.CreateTranslation(-12.5f, 0f, 0f)));
            this.floor.Add(new Floor(this.game, this.worldMatrix, Matrix.CreateTranslation(-12.5f, 0f, 12.5f)));
            this.floor.Add(new Floor(this.game, this.worldMatrix, Matrix.CreateTranslation(-12.5f, 0f, -12.5f)));

            EditWorld.InitEditWorld(this.game, worldMatrix);
            System.Diagnostics.Debug.WriteLine(floor[0]);
        }
        public void ResetSk8()
        {
            this.sk8.wheelFL.worldMatrix = Matrix.Identity * Matrix.CreateTranslation(0f, 1f, 0f);
            this.sk8.deck.worldMatrix = Matrix.Identity * Matrix.CreateTranslation(0f, 1f, 0f);
            this.sk8.trucksF.worldMatrix = Matrix.Identity * Matrix.CreateTranslation(0f, 1f, 0f);
            this.sk8.angle = 0;
            camTarget = new Vector3(0f, 0.8f, 0f);
            camPosition = new Vector3(-5f, 2.3f, 0f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0, 1f, 0f));
        }

        public void MainGameUpdate()
        {
            Skate.Input.sk8 = sk8;
            Skate.Input.boxes = boxes;
            Skate.Input.floor = floor;
            Skate.Input.rails = rails;

            decimal[] sk8Vals = Skate.Input.UpdateInput();

            if (Keyboard.GetState().IsKeyDown(Keys.E) && !this.firstPressE)
            {
                this.editing = !this.editing;
                this.firstPressE = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.E))
            {
                this.firstPressE = false;
            }

            viewMatrix = this.sk8.Move(ref camTarget, ref camPosition, (float)sk8Vals[0]);
            viewMatrix = this.sk8.VertMove(ref camTarget, ref camPosition, (float)sk8Vals[1]);
            viewMatrix = this.sk8.Turn(ref camTarget, ref camPosition, (float)sk8Vals[2]);

            if (this.sk8.deck.worldMatrix.M42 < -5f)
                ResetSk8();

            if (this.editing)
                EditWorld.UpdateEditWorld(this, game);
        }

        public void MainGameDraw()
        {
            sk8.SkateDraw(viewMatrix, projectionMatrix);
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

            if (this.editing)
                EditWorld.DrawEditWorld(this.viewMatrix, this.projectionMatrix, this.sk8);
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
                    meshPart.VertexBuffer.GetData<float>(vertexData);

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
    }
}
