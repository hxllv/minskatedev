using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace minskatedev
{
    public partial class MainGame
    {
        public class Box
        {
            public ModelHelper box;
            public BoundingBox bounds;
            public BoundingBox top;

            public Box(Microsoft.Xna.Framework.Game game, Matrix worldMatrix, Matrix translation)
            {
                this.box = new ModelHelper(game, worldMatrix, "models\\obst\\box", translation);
                this.bounds = UpdateBoundingBox(this.box.model, this.box.worldMatrix);
                Vector3 max = this.bounds.Max;
                Vector3 min = this.bounds.Min;
                float y = Math.Max(max.Y, min.Y);
                max.Y = y + 0.1f;
                min.Y = y;
                this.top = new BoundingBox(min, max);
            }

            public void BoxDraw(Matrix viewMatrix, Matrix projectionMatrix)
            {
                //for every model
                foreach (ModelMesh mesh in box.model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.View = viewMatrix;
                        effect.World = box.worldMatrix;
                        effect.Projection = projectionMatrix;
                    }

                    //Draw Model
                    mesh.Draw();
                }
            }
        }
    }
}
