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

            public Floor(Microsoft.Xna.Framework.Game game, Matrix worldMatrix, Matrix translation)
            {
                this.floor = new ModelHelper(game, worldMatrix, "models\\obst\\floor", translation);
                this.bounds = UpdateBoundingBox(this.floor.model, this.floor.worldMatrix);
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
                        effect.World = floor.worldMatrix;
                        effect.Projection = projectionMatrix;
                    }

                    //Draw Model
                    mesh.Draw();
                }
            }
        }
    }
}
