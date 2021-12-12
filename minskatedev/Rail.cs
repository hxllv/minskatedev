using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace minskatedev
{
    public partial class MainGame
    {
        public class Rail
        {
            public ModelHelper rail;
            public BoundingBox bounds;
            public BoundingBox top;

            public Rail(Microsoft.Xna.Framework.Game game, Matrix translation, Matrix rotationX, Matrix rotationY, Matrix rotationZ)
            {
                this.rail = new ModelHelper(game, "models\\obst\\rail", translation, rotationX, rotationY, rotationZ);
                this.bounds = UpdateBoundingBox(this.rail.model, this.rail.translation);
                Vector3 max = this.bounds.Max;
                Vector3 min = this.bounds.Min;
                float y = Math.Max(max.Y, min.Y);
                max.Y = y + 0.1f;
                min.Y = y;
                this.top = new BoundingBox(min, max);
            }
            public void RailDraw(Matrix viewMatrix, Matrix projectionMatrix)
            {
                //for every model
                foreach (ModelMesh mesh in rail.model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.View = viewMatrix;
                        effect.World = rail.rotationX * rail.rotationY * rail.rotationZ * rail.translation;
                        effect.Projection = projectionMatrix;
                    }

                    //Draw Model
                    mesh.Draw();
                }
            }
        }
    }
}
