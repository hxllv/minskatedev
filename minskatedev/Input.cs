using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace minskatedev
{
    public partial class MainGame
    {
        public partial class Skate 
        {
            public static partial class Input
            {
                public static Skate sk8;
                public static List<Box> boxes;
                public static List<Rail> rails;
                public static List<Floor> floor;

                public static decimal[] UpdateInput()
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        Physics.ExecJump();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        Physics.ExecAddSpeed();
                    }
                    else
                    {
                        Physics.ExecDecreaseSpeed();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        Physics.ExecBrake();
                    }
                    //else if (powerSlideAngleTotal < 0)
                    //{
                    //    powerSlideAngle = (decimal)Math.PI / 10;
                    //    powerSlideAngleTotal += (decimal)Math.PI / 10;
                    //}
                    //else if (powerSlideAngleTotal == 0)
                    //{
                    //    powerSlideAngle = 0;
                    //}

                    if (Keyboard.GetState().IsKeyDown(Keys.A) && !Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        Physics.ExecAddLeftTurn();
                    }
                    else
                    {
                        Physics.ExecDecreaseLeftTurn();

                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.D) && !Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        Physics.ExecAddRightTurn();
                    }
                    else
                    {
                        Physics.ExecDecreaseRightTurn();
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.A) && Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        Physics.ExecStraightenTurn();
                    }

                    return Physics.UpdatePhysics(sk8, boxes, floor, rails);
                }
            }
        }
    }
}
