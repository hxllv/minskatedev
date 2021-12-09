using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
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

                public static decimal[] UpdateInput()
                {
                    Animations.sk8 = sk8;
                    phys = Physics.UpdatePhysics(sk8, boxes, floor, rails);

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        if (Physics.ExecJump())
                            Animations.Ollie.KeyPress();
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

                    Animations.Ollie.Animate();

                    return phys;
                }

                public static class Animations
                {

                    public static Skate sk8;

                    public static class Powerslide
                    {
                        static decimal powerSlideAngle = 0;
                        static decimal powerSlideAngleTotal = 0;

                        public static void KeyDown(decimal speed)
                        {
                            if (speed > 0)
                            {
                                if (powerSlideAngleTotal > -(decimal)Math.PI / 2)
                                {
                                    powerSlideAngle = -(decimal)Math.PI / 10;
                                    powerSlideAngleTotal -= (decimal)Math.PI / 10;
                                }
                                else
                                {
                                    powerSlideAngle = 0;
                                }
                            }
                            else
                            {
                                powerSlideAngle = 0;
                            }
                            sk8.Rotate(0, (float)powerSlideAngle, 0);
                        }

                        public static void KeyUp(decimal speed)
                        {
                            if (powerSlideAngleTotal < 0)
                            {
                                powerSlideAngle = (decimal)Math.PI / 10;
                                powerSlideAngleTotal += (decimal)Math.PI / 10;
                            }
                            else if (powerSlideAngleTotal == 0)
                            {
                                powerSlideAngle = 0;
                            }
                            sk8.Rotate(0, (float)powerSlideAngle, 0);
                        }
                    }

                    public static class Ollie
                    {
                        static bool doAnim = false;
                        static bool popDone = false;
                        static decimal olliePitch = 0;
                        static decimal olliePitchTotal = 0;

                        public static void KeyPress()
                        {
                            doAnim = true;
                        }

                        public static void Animate()
                        {
                            if (!doAnim) return;

                            if (olliePitchTotal >= 0.6M)
                                popDone = true;

                            if (!popDone)
                            {
                                olliePitch = 0.12M;
                                olliePitchTotal += 0.12M;
                            }
                            else
                            {
                                olliePitch = -0.05M;
                                olliePitchTotal -= 0.05M;
                            }

                            if (olliePitchTotal == 0)
                            {
                                popDone = false;
                                doAnim = false;
                            }

                            sk8.Rotate((float)olliePitch, 0, 0);
                        }
                    }
                }
            }
        }
    }
}
