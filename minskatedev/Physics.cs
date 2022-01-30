using Microsoft.Xna.Framework.Input;
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
                public static class Physics
                {
                    public static decimal speed = 0;
                    public static decimal turning = 0;
                    public static decimal vertVel = 0;

                    static bool justLanded = false;
                    public static bool keepVertMomentum = false;
                    public static bool jumped = false;
                    static bool isColliding = false;
                    public static bool isCollidingGround = true;

                    public static bool ExecJump()
                    {
                        if (!jumped)
                        {
                            vertVel = 0.25M;
                            jumped = true;
                            keepVertMomentum = true;
                            return true;
                        }

                        return false;
                    }

                    public static void ExecAddSpeed()
                    {
                        if (!jumped && !isColliding)
                            if (speed < 0.25M)
                                speed += 0.005M;
                    }

                    public static void ExecDecreaseSpeed()
                    {
                        if (!jumped)
                        {
                            if (speed > 0)
                                speed -= 0.00125M;
                            if (speed < 0)
                                speed += 0.00125M;
                        }
                    }

                    public static decimal ExecBrake()
                    {
                        if (speed > 0)
                            speed -= 0.005M;

                        return speed;
                    }

                    public static void ExecAddLeftTurn()
                    {
                        if (turning < 0.026M)
                            turning += 0.002M;
                    }
                    public static void ExecDecreaseLeftTurn()
                    {
                        if (turning > 0)
                        {
                            turning -= 0.002M;
                        }
                    }
                    public static void ExecAddRightTurn()
                    {
                        if (turning > -0.026M)
                            turning -= 0.002M;
                    }
                    public static void ExecDecreaseRightTurn()
                    {
                        if (turning < 0)
                        {
                            turning += 0.002M;
                        }
                    }

                    public static void ExecStraightenTurn()
                    {
                        if (turning != 0)
                        {
                            if (turning > 0)
                                turning -= 0.002M;
                            if (turning < 0)
                                turning += 0.002M;
                        }
                    }

                    public static decimal[] UpdatePhysics(Skate sk8, List<Box> boxes, List<Floor> floor, List<Rail> rails)
                    {
                        isCollidingGround = false;
                        isColliding = false;

                        foreach (Floor floori in floor)
                        {
                            if ((sk8.truckFBounds.Intersects(floori.bounds) || sk8.truckBBounds.Intersects(floori.bounds)) && !keepVertMomentum)
                            {
                                jumped = false;
                                isCollidingGround = true;
                                vertVel = 0;
                            }
                        }

                        foreach (Box box in boxes)
                        {
                            if ((sk8.truckFBounds.Intersects(box.top) || sk8.truckBBounds.Intersects(box.top)) && !keepVertMomentum)
                            {
                                jumped = false;
                                isCollidingGround = true;
                                vertVel = 0;
                            }

                            bool tempCollide = (sk8.deckBoundsF.Intersects(box.bounds) && !isColliding) || 
                                (sk8.deckBoundsB.Intersects(box.bounds) && !isColliding);

                            if (tempCollide)
                            {
                                isColliding = true;
                            }
                        }

                        foreach (Rail rail in rails)
                        {
                            if ((sk8.truckFBounds.Intersects(rail.top) || sk8.truckBBounds.Intersects(rail.top)) && !keepVertMomentum)
                            {
                                jumped = false;
                                isCollidingGround = true;
                                vertVel = 0;
                            }

                            bool tempCollide = (sk8.deckBoundsF.Intersects(rail.bounds) && !isColliding) ||
                                (sk8.deckBoundsB.Intersects(rail.bounds) && !isColliding);

                            if (tempCollide)
                            {
                                isColliding = true;
                            }
                        }

                        if (!isCollidingGround)
                        {
                            if (vertVel > -0.2M)
                            {
                                vertVel -= 0.01M;
                            }

                            keepVertMomentum = false;
                            justLanded = false;
                        }

                        if (isCollidingGround && !keepVertMomentum)
                        {
                            vertVel = 0;

                            if (!justLanded)
                            {
                                justLanded = true;
                                Sounds.PlayLand();
                            }

                            if (fuckedTrick || doingTricks.Count != 0)
                            {   
                                foreach (int doingTrick in doingTricks)
                                {
                                    switch (doingTrick)
                                    {
                                        case 1:
                                            Animations.Flip.doAnim = false;
                                            break;
                                        case 2:
                                            Animations.Shuv.doAnim = false;
                                            break;
                                    }
                                }
 
                                if (!sk8.mainGame.gameOfSkating)
                                {
                                    sk8.ResetSk8();
                                    fuckedTrick = false;
                                    doingTricks.Clear();
                                }
                                    
                                TrickNames.trickName = "";
                            }
                        }

                        if (isColliding)
                        {
                            if (speed <= 0.01M && speed > 0)
                                speed = -0.069M;
                            else if (speed > 0)
                                speed = -speed * 0.5M;
                            if (Math.Abs(turning) > 0)
                                turning = -turning;
                        }

                        if (Math.Abs(speed) < 0.005M)
                            speed = 0;

                        return new decimal[] { speed, vertVel, turning, jumped ? 1 : 0 };
                    }
                }
            }
        }
    }
}
