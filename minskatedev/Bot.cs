using System;

namespace minskatedev
{
    public partial class MainGame
    {

        public static partial class GameOfSkate
        {
            static class Bot
            {
                public static Skate sk8;

                static readonly decimal twopi = (decimal)(2 * Math.PI);
                static readonly decimal pi = (decimal)(Math.PI);
                static readonly decimal piover13 = (decimal)(6 * Math.PI / 13);
                static readonly decimal piover18 = (decimal)(6 * Math.PI / 18);
                static readonly decimal[] flip = new decimal[] { twopi - piover13, twopi, twopi + piover13  };
                static readonly decimal[] shuv = new decimal[] { pi - piover18, pi, pi + piover18 };
                static readonly decimal[] threeshuv = new decimal[] { twopi - piover18, twopi, twopi + piover18 };

                public static int catchIndex;
                static int frameCounter = 0;
                static string trickName = "";
                static Random rnd = new Random();
                static bool rollUp = true;
                static bool trickExecdFlip = false;
                static bool trickExecdShuv = false;
                static bool trickExecdThreeShuv = false;

                public static void SetTrick(string trick)
                {
                    rollUp = true;
                    trickExecdFlip = false;
                    trickExecdShuv = false;
                    trickExecdThreeShuv = false;

                    int chance = rnd.Next(0, 101);
                    catchIndex = 1;

                    frameCounter = 80;

                    if (chance <= 25)
                        catchIndex = 0;
                    else if (chance >= 75)
                        catchIndex = 2;

                    trickName = trick;
                }

                public static void ExecTrick()
                {
                    if (frameCounter > 0)
                    {
                        frameCounter--;
                        Skate.Input.Physics.ExecAddSpeed();
                    }

                    if (frameCounter == 0)
                    {
                        if (rollUp)
                        {
                            Skate.Input.Animations.Ollie.KeyPress();
                            Skate.Input.Physics.ExecJump();
                            rollUp = false;
                            frameCounter = 5;
                        }
                        else
                        {
                            if (catchIndex != 1)
                                Skate.Input.fuckedTrick = true;
                            switch (trickName)
                            {
                                case "Kickflip":
                                    FlipExec(1);
                                    break;
                                case "Heelflip":
                                    FlipExec(0);
                                    break;
                                case "Backside Shove-it":
                                    ShuvExec(1);
                                    break;
                                case "Frontside Shove-it":
                                    ShuvExec(0);
                                    break;
                                case "360 Backside Shove-it":
                                    ThreeShuvExec(1);
                                    break;
                                case "360 Frontside Shove-it":
                                    ThreeShuvExec(0);
                                    break;
                                case "Varial Kickflip":
                                    ShuvExec(1);
                                    FlipExec(1);
                                    break;
                                case "Varial Heelflip":
                                    ShuvExec(0);
                                    FlipExec(0);
                                    break;
                                case "Hardflip":
                                    ShuvExec(0);
                                    FlipExec(1);
                                    break;
                                case "Inward Heelflip":
                                    ShuvExec(1);
                                    FlipExec(0);
                                    break;
                                case "Tre Flip":
                                    ThreeShuvExec(1);
                                    FlipExec(1);
                                    break;
                                case "Laser Flip":
                                    ThreeShuvExec(0);
                                    FlipExec(0);
                                    break;
                                case "360 Hardflip":
                                    ThreeShuvExec(0);
                                    FlipExec(1);
                                    break;
                                case "360 Inward Heelflip":
                                    ThreeShuvExec(1);
                                    FlipExec(0);
                                    break;
                            }
                        }
                    }
                }

                private static void FlipExec(int type)
                {
                    if (!trickExecdFlip)
                    {
                        Skate.Input.Animations.Flip.KeyPress(type);
                        trickExecdFlip = true;
                    }
                    if (Skate.Input.Animations.Flip.flipRollTotal > flip[catchIndex])
                    {
                        Skate.Input.Animations.Flip.StopTrick();
                        if (catchIndex == 1)
                            Skate.Input.TrickNames.trickName = trickName;
                    }
                }

                private static void ShuvExec(int type)
                {
                    if (!trickExecdShuv)
                    {
                        Skate.Input.Animations.Shuv.KeyPress(type);
                        trickExecdShuv = true;
                    }
                    if (Skate.Input.Animations.Shuv.shuvYawTotal > shuv[catchIndex])
                    {
                        Skate.Input.Animations.Shuv.StopTrick();
                        if (catchIndex == 1)
                            Skate.Input.TrickNames.trickName = trickName;
                    }
                }

                private static void ThreeShuvExec(int type)
                {
                    if (!trickExecdThreeShuv)
                    {
                        Skate.Input.Animations.Shuv.KeyPress(type);
                        trickExecdThreeShuv = true;
                    }
                    if (Skate.Input.Animations.Shuv.shuvYawTotal > threeshuv[catchIndex])
                    {
                        Skate.Input.Animations.Shuv.StopTrick();
                        if (catchIndex == 1)
                            Skate.Input.TrickNames.trickName = trickName;
                    }
                }
            }
        }
    }
}
