using System;

namespace minskatedev
{
    public partial class MainGame
    {
        public partial class Skate 
        {
            public static partial class Input
            {
                public static class Animations
                {

                    public static Skate sk8;

                    public static class Powerslide
                    {
                        static decimal powerSlideAngle = 0;
                        static decimal powerSlideAngleTotal = 0;

                        public static void KeyDown(decimal speed)
                        {
                            if (powerSlideAngleTotal == -1)
                                powerSlideAngleTotal = 0;

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
                            if (powerSlideAngleTotal < 0 && powerSlideAngleTotal != -1)
                            {
                                powerSlideAngle = (decimal)Math.PI / 10;
                                powerSlideAngleTotal += (decimal)Math.PI / 10;
                            }
                            else if (powerSlideAngleTotal == 0)
                            {
                                sk8.ResetRotationY();
                                powerSlideAngleTotal = -1;
                                powerSlideAngle = 0;
                            }
                            sk8.Rotate(0, (float)powerSlideAngle, 0);
                        }
                    }

                    public static class Ollie
                    {
                        public static bool doAnim = false;
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

                            if (olliePitchTotal >= 0.6M && !popDone)
                            {
                                popDone = true;
                                Sounds.PlayPop();
                            }

                            if (!popDone)
                            {
                                olliePitch = 0.15M;
                                olliePitchTotal += 0.15M;
                            }
                            else
                            {
                                olliePitch = -0.075M;
                                olliePitchTotal -= 0.075M;
                            }

                            if (olliePitchTotal == 0)
                            {
                                popDone = false;
                                doAnim = false;
                            }

                            sk8.Rotate((float)olliePitch, 0, 0);
                        }

                        public static void ForceStop()
                        {
                            popDone = false;
                            doAnim = false;
                            olliePitchTotal = 0;
                        }
                    }

                    public static class Flip
                    {
                        static decimal flipRoll = 0;
                        public static decimal flipRollTotal = 0;
                        public static bool doAnim = false;
                        public static int flipType = 0;
                        public static void KeyPress(int type)
                        {
                            if (Physics.jumped && !doingTricks.Contains(1))
                            {
                                doingTricks.Add(1);
                                doAnim = true;
                                flipType = type;
                                flipRollTotal = 0;

                                if (type == 0)
                                {
                                    flipRoll = -(decimal)Math.PI / 13;
                                }
                                    
                                else if (type == 1)
                                {
                                    flipRoll = (decimal)Math.PI / 13;
                                }

                                Ollie.ForceStop();
                                sk8.ResetRotationXandZ();
                            }
                        }
                        public static void Animate()
                        {
                            if (!doAnim) return;

                            flipRollTotal += (decimal)Math.PI / 13;

                            sk8.Rotate(0, 0, (float)flipRoll);
                        }

                        public static bool StopTrick()
                        {
                            TrickNames.CalcTrick();
                            doAnim = false;
                            if ((double)flipRollTotal % (2 * Math.PI) <= 5 * Math.PI / 13 || (double)flipRollTotal % (2 * Math.PI) >= 2 * Math.PI - (5 * Math.PI / 13))
                            {
                                sk8.ResetRotationXandZ();
                                return true;
                            }

                            return false;
                        }
                    }

                    public static class Shuv
                    {
                        static decimal shuvYaw = 0;
                        public static decimal shuvYawTotal = 0;
                        public static bool doAnim = false;
                        public static int shuvType = 0;

                        public static void KeyPress(int type)
                        {
                            if (Physics.jumped && !doingTricks.Contains(2))
                            {
                                doingTricks.Add(2);
                                doAnim = true;
                                shuvType = type;
                                shuvYawTotal = 0;

                                if (type == 0)
                                {
                                    shuvYaw = -(decimal)Math.PI / 18;
                                }

                                else if (type == 1)
                                {
                                    shuvYaw = (decimal)Math.PI / 18;
                                }

                                Ollie.ForceStop();
                                sk8.ResetRotationXandZ();
                            }
                        }
                        public static void Animate()
                        {
                            if (!doAnim) return;

                            shuvYawTotal += (decimal)Math.PI / 18;

                            sk8.Rotate(0, (float)shuvYaw, 0);
                        }

                        public static bool StopTrick()
                        {
                            TrickNames.CalcTrick();
                            doAnim = false;

                            if ((double)shuvYawTotal % Math.PI <= 5 * Math.PI / 18 || (double)shuvYawTotal % Math.PI >= Math.PI - (5 * Math.PI / 18))
                            {
                                sk8.ResetRotationY();
                                return true;
                            }

                            return false;
                        }
                    }
                }
            }
        }
    }
}
