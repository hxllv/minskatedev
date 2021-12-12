using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace minskatedev
{
    public partial class MainGame
    {
        public class Floor
        {
            public ModelHelper floor;
            public BoundingBox bounds;

            public Floor(Microsoft.Xna.Framework.Game game, Matrix translation, Matrix rotationX, Matrix rotationY, Matrix rotationZ)
            {
                this.floor = new ModelHelper(game, "models\\obst\\floor", translation, rotationX, rotationY, rotationZ);
                this.bounds = UpdateBoundingBox(this.floor.model, this.floor.translation);
            }
            public void FloorDraw(Matrix viewMatrix, Matrix projectionMatrix)
            {
                //for every model
                foreach (ModelMesh mesh in floor.model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.View = viewMatrix;
                        effect.World = floor.rotationX * floor.rotationY * floor.rotationZ * floor.translation;
                        effect.Projection = projectionMatrix;
                    }

                    //Draw Model
                    mesh.Draw();
                }
            }
        }
    }
}
