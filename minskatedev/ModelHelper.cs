using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace minskatedev
{
    public class ModelHelper
    {
        public Model model;
        public Matrix worldMatrix;

        public ModelHelper(Microsoft.Xna.Framework.Game game, Matrix worldMatrix, string modelName, Matrix translation)
        {
            this.model = game.Content.Load<Model>(modelName);
            this.worldMatrix = worldMatrix * translation;
        }
    }
}
