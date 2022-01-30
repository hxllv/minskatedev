using System;
using Microsoft.Xna.Framework;

namespace minskatedev
{
    public partial class MainGame
    {
        public partial class Skate 
        {
            public static partial class Input
            {
                public static class TrickNames
                {
                    public static string trickName = "";
                    public static bool didTrick = false;
                    static int frameCounter = 0;

                    public static void CalcTrick()
                    {
                        trickName = "";
                        if (doingTricks.Contains(1) && doingTricks.Contains(2))
                        {
                            double flipCount = Math.Round((double)Animations.Flip.flipRollTotal / 2 * Math.PI);
                            double shuvCount = Math.Round((double)Animations.Shuv.shuvYawTotal / Math.PI);
                            int flipType = Animations.Flip.flipType;
                            int shuvType = Animations.Shuv.shuvType;

                            if (flipType == 0 && shuvType == 0)
                            {
                                trickName = "Varial Heelflip";
                                if (shuvCount == 2)
                                    trickName = "Laser Flip";
                            }
                            if (flipType == 0 && shuvType == 1)
                            {
                                trickName = "Inward Heelflip";
                                if (shuvCount == 2)
                                    trickName = "360 Inward Heelflip";
                            }
                            if (flipType == 1 && shuvType == 0)
                            {
                                trickName = "Hardflip";
                                if (shuvCount == 2)
                                    trickName = "360 Hardflip";
                            }
                            if (flipType == 1 && shuvType == 1)
                            {
                                trickName = "Varial Kickflip";
                                if (shuvCount == 2)
                                    trickName = "Tre Flip";
                            }

                            didTrick = true;
                        }
                        else if (doingTricks.Contains(1))
                        {
                            double flipCount = Math.Round((double)Animations.Flip.flipRollTotal / 2*Math.PI);
                            switch (flipCount)
                            {
                                case 2:
                                    trickName = "Double ";
                                    break;
                                case 3:
                                    trickName = "Triple ";
                                    break;
                                case 4:
                                    trickName = "Quad ";
                                    break;
                                default:
                                    trickName = "";
                                    break;
                            }

                            switch (Animations.Flip.flipType)
                            {
                                case 0:
                                    trickName += "Heelflip";
                                    break;
                                case 1:
                                    trickName += "Kickflip";
                                    break;
                            }

                            didTrick = true;
                        }
                        else if (doingTricks.Contains(2))
                        {
                            double shuvCount = Math.Round((double)Animations.Shuv.shuvYawTotal / Math.PI);
                            switch (shuvCount)
                            {
                                case 2:
                                    trickName = "360 ";
                                    break;
                                case 3:
                                    trickName = "540 ";
                                    break;
                                case 4:
                                    trickName = "720 ";
                                    break;
                                default:
                                    trickName = "";
                                    break;
                            }

                            switch (Animations.Shuv.shuvType)
                            {
                                case 0:
                                    trickName += "Frontside Shove-it";
                                    break;
                                case 1:
                                    trickName += "Backside Shove-it";
                                    break;
                            }

                            didTrick = true;
                        }
                    }

                    public static void DrawTrick()
                    {
                        if (didTrick)
                        {
                            didTrick = false;
                            frameCounter = 180;
                        }

                        if (frameCounter == 0)
                            trickName = "";
                        else if (frameCounter > 0)
                            frameCounter--;

                        sk8.mainGame.spriteBatch.Begin();
                        Vector2 size = sk8.mainGame.font.MeasureString(trickName);
                        sk8.mainGame.spriteBatch.DrawString(sk8.mainGame.font, trickName, 
                            new Vector2(sk8.mainGame.graphics.PreferredBackBufferWidth / 2, 50), 
                            new Color(255, 97, 244), 0f, size / 2, 1, 
                            Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
                        sk8.mainGame.spriteBatch.End();
                    }
                }
            }
        }
    }
}
