using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;

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
                public static List<int> doingTricks = new List<int>();
                public static bool fuckedTrick = false;

                public static decimal[] UpdateInput()
                {
                    Animations.sk8 = sk8;
                    phys = Physics.UpdatePhysics(sk8, boxes, floor, rails);

                    if (sk8.mainGame.gameOfSkating && !GameOfSkate.playerTurn)
                    {
                        Animations.Ollie.Animate();
                        Animations.Flip.Animate();
                        Animations.Shuv.Animate();

                        if (phys[0] > 0 && phys[3] == 0)
                            Sounds.PlayRoll();
                        else
                            Sounds.StopRoll();

                        Sounds.RollVolume((float)phys[0]);

                        return phys;
                    }
                        

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        if (Physics.ExecJump())
                        {
                            Animations.Ollie.KeyPress();
                            TrickNames.trickName = "";
                        }

                        if (doingTricks.Count != 0)
                        {
                            foreach (int doingTrick in doingTricks)
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
                            }

                            doingTricks.Clear();
                        }
                    }

                    if (phys[0] > 0 && phys[3] == 0)
                        Sounds.PlayRoll();
                    else 
                        Sounds.StopRoll();

                    Sounds.RollVolume((float)phys[0]);

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

                    Animations.Ollie.Animate();
                    Animations.Flip.Animate();
                    Animations.Shuv.Animate();
                    
                    return phys;
                }

                public static class Sounds
                {
                    public static SoundEffect roll = null;
                    public static SoundEffect pop = null;
                    public static SoundEffect land = null;
                    public static SoundEffectInstance rollInstance = null;

                    public static void PlayRoll()
                    {
                        rollInstance.IsLooped = true;
                        rollInstance.Play();
                    }

                    public static void StopRoll()
                    {
                        rollInstance.Stop();
                    }

                    public static void RollVolume(float x)
                    {
                        x = Math.Abs(x);
                        if (x > 0.25f)
                            x = 0.25f;

                        rollInstance.Volume = x * 4;
                    }

                    public static void PlayPop()
                    {
                        pop.Play();
                    }

                    public static void PlayLand()
                    {
                        land.Play();
                    }
                }
            }
        }
    }
}
