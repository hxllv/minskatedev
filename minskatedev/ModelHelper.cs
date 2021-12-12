using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace minskatedev
{
    public class ModelHelper
    {
        public Model model;
        public Matrix translation;
        public Matrix rotationX;
        public Matrix rotationY;
        public Matrix rotationZ;

        public ModelHelper(Microsoft.Xna.Framework.Game game, string modelName, 
            Matrix translation, 
            Matrix rotationX,
            Matrix rotationY,
            Matrix rotationZ)
        {
            this.model = game.Content.Load<Model>(modelName);
            this.translation = translation;
            this.rotationX = rotationX;
            this.rotationY = rotationY;
            this.rotationZ = rotationZ;
        }
    }
}
