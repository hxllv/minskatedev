using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
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
                private static decimal[] phys;
                static int doingTrick = 0;
                public static bool fuckedTrick = false;

                public static decimal[] UpdateInput()
                {
                    Animations.sk8 = sk8;
                    phys = Physics.UpdatePhysics(sk8, boxes, floor, rails);

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        if (Physics.ExecJump())
                            Animations.Ollie.KeyPress();

                        if (doingTrick != 0)
                        {
                            switch (doingTrick)
                            {
                                case 1:
                                    if (!Animations.Flip.StopTrick())
                                        fuckedTrick = true;
                                    break;
                                case 2:
                                    if (!Animations.Shuv.StopTrick())
                                        fuckedTrick = true;
                                    break;
                            }

                            doingTrick = 0;
                        }
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        Physics.ExecAddSpeed();
                    }
                    else
                    {
                        Physics.ExecDecreaseSpeed();
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.S) && phys[3] == 0)
                    {
                        Physics.ExecBrake();
                        Animations.Powerslide.KeyDown(phys[0]);
                    }
                    else
                    {
                        Animations.Powerslide.KeyUp(phys[0]);
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.A) && !Keyboard.GetState().IsKeyDown(Keys.D) && phys[3] == 0)
                    {
                        Physics.ExecAddLeftTurn();
                    }
                    else
                    {
                        Physics.ExecDecreaseLeftTurn();

                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.D) && !Keyboard.GetState().IsKeyDown(Keys.A) && phys[3] == 0)
                    {
                        Physics.ExecAddRightTurn();
                    }
                    else
                    {
                        Physics.ExecDecreaseRightTurn();
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.A) && Keyboard.GetState().IsKeyDown(Keys.D) && phys[3] == 0)
                    {
                        Physics.ExecStraightenTurn();
                    }

                    if (doingTrick == 0)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                        {
                            Animations.Flip.KeyPress(0);
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                        {
                            Animations.Flip.KeyPress(1);
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                        {
                            Animations.Shuv.KeyPress(0);
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                        {
                            Animations.Shuv.KeyPress(1);
                        }
                    }

                    Animations.Ollie.Animate();
                    Animations.Flip.Animate();
                    Animations.Shuv.Animate();

                    return phys;
                }
            }
        }
    }
}
