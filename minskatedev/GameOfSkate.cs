using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace minskatedev
{
    public partial class MainGame
    {
        public static partial class GameOfSkate
        {
            public static bool playerTurn;
            static string landedTrick;
            static int frameCounter;
            static bool endOfTurn;
            static bool showTurn;

            static int playerNumLetters;
            static int botNumLetters;

            static bool landedShow;
            static string botPotential;

            static List<string> tricks;

            public static void InitGameOfSkate()
            {
                playerTurn = true;
                Skate.Input.TrickNames.trickName = "";
                landedTrick = "";
                frameCounter = 0;
                endOfTurn = false;
                showTurn = true;
                playerNumLetters = 0;
                botNumLetters = 0;
                landedShow = false;
                botPotential = "";
                tricks = new List<string> {"Kickflip", "Heelflip", "Backside Shove-it" ,
                    "Frontside Shove-it", "360 Backside Shove-it", "360 Frontside Shove-it",
                    "Varial Heelflip", "Laser Flip", "Tre Flip", "Varial Kickflip",
                    "Hardflip", "Inward Heelflip", "360 Hardflip", "360 Inward Heelflip"
                };
            }

            public static void UpdateGameOfSkate(MainGame mainGame, Microsoft.Xna.Framework.Game game, Skate sk8)
            {
                Bot.sk8 = sk8;

                if (frameCounter == 0 && endOfTurn)
                {
                    Skate.Input.fuckedTrick = false;
                    Skate.Input.doingTricks.Clear();
                    sk8.ResetSk8(new Vector3(0, 1f, 0), 0, new Vector3(0f, 0.8f, 0f), new Vector3(-5f, 2.3f, 0f));
                    endOfTurn = false;
                    Skate.Input.TrickNames.trickName = "";
                    playerTurn = !playerTurn;

                    if (showTurn)
                        landedTrick = "";

                    if (!showTurn && !playerTurn)
                    {
                        Bot.SetTrick(landedTrick);
                    }

                    if (showTurn && !playerTurn)
                    {
                        Random rnd = new Random();
                        int chance = rnd.Next(0, tricks.Count);
                        botPotential = tricks[chance];
                        System.Diagnostics.Debug.WriteLine(tricks[chance]);
                        Bot.SetTrick(tricks[chance]);
                    }

                    if (playerNumLetters >= 5 || botNumLetters >= 5)
                    {
                        mainGame.gameOfSkating = false;
                        mainGame.floor.Clear();
                        mainGame.AddFloors();
                        sk8.ResetSk8(new Vector3(0, 1f, 0), 0, new Vector3(0f, 0.8f, 0f), new Vector3(-5f, 2.3f, 0f));
                    }
                }
                    
                if (frameCounter > 0)
                    frameCounter--;

                if (Skate.Input.Physics.isCollidingGround && !Skate.Input.Physics.keepVertMomentum && 
                    (Skate.Input.TrickNames.trickName != "" || Skate.Input.fuckedTrick || Skate.Input.doingTricks.Count != 0) 
                    && !endOfTurn)
                {
                    if (showTurn)
                    {
                        landedTrick = Skate.Input.TrickNames.trickName;
                        if (Bot.catchIndex == 1 && !playerTurn)
                            landedTrick = botPotential;
                        landedShow = false;

                        if (tricks.Contains(landedTrick))
                        {
                            tricks.Remove(landedTrick);
                            landedShow = true;
                        }
                        else
                        {
                            landedTrick = "";
                        }
                    }

                    if (!showTurn)
                    {
                        if (playerTurn && (Skate.Input.TrickNames.trickName != landedTrick || Skate.Input.fuckedTrick || Skate.Input.doingTricks.Count != 0))
                        {
                            playerNumLetters++;
                        }
                        else if (!playerTurn && Skate.Input.fuckedTrick)
                        {
                            botNumLetters++;
                        }
                    }
                    frameCounter = 120;
                    endOfTurn = true;
                    if (landedShow)
                        showTurn = !showTurn;
                }

                if (!playerTurn)
                {
                    mainGame.camPosition = new Vector3(25f, 3f, 5f);
                    Bot.ExecTrick();
                }
            }

            public static void DrawGameOfSkate(MainGame mainGame)
            {
                int i = 1;
                foreach (int ltrsNum in new int[] {playerNumLetters, botNumLetters })
                {
                    string ltrs = i == 1 ? "Player letters: " : "Bot letters: ";

                    switch (ltrsNum)
                    {
                        case 1:
                            ltrs += "S";
                            break;
                        case 2:
                            ltrs += "S K";
                            break;
                        case 3:
                            ltrs += "S K A";
                            break;
                        case 4:
                            ltrs += "S K A T";
                            break;
                        case 5:
                            ltrs += "S K A T E";
                            break;
                    }

                    mainGame.spriteBatch.Begin();
                    mainGame.spriteBatch.DrawString(mainGame.font, ltrs, new Vector2(10f, 40f + 20f * i), Color.Black);
                    mainGame.spriteBatch.End();
                    i++;
                }

                mainGame.spriteBatch.Begin();
                mainGame.spriteBatch.DrawString(mainGame.font, "Landed trick: " + landedTrick, new Vector2(10f, 100f), Color.Black);
                mainGame.spriteBatch.End();
            }
        }
    }
}
