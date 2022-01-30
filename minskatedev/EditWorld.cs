using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace minskatedev
{
    public partial class MainGame
    {
        public static class EditWorld
        {
            static ModelHelper floorE;
            static ModelHelper boxE;
            static ModelHelper railE;
            static ModelHelper toDraw;
            static float offset;
            static Matrix rotation;
            static bool firstPressR;
            static bool firstPressT;
            static bool firstPressEnter;
            static int selectedItem;

            public static void InitEditWorld(Microsoft.Xna.Framework.Game game, Matrix worldMatrix)
            {
                floorE = new ModelHelper(game, "models\\obst\\floor", Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)), Matrix.Identity, Matrix.Identity, Matrix.Identity);
                boxE = new ModelHelper(game, "models\\obst\\box", Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)), Matrix.Identity, Matrix.Identity, Matrix.Identity);
                railE = new ModelHelper(game, "models\\obst\\rail", Matrix.CreateTranslation(new Vector3(0f, 0f, 0f)), Matrix.Identity, Matrix.Identity, Matrix.Identity);
                toDraw = floorE;
                offset = 10f;
                rotation = Matrix.Identity;
                firstPressR = false;
                firstPressT = false;
                firstPressEnter = false;
                selectedItem = 0;
            }

            public static void UpdateEditWorld(MainGame mainGame, Microsoft.Xna.Framework.Game game, Skate sk8)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                {
                    selectedItem = 0;
                    toDraw = floorE;
                    offset = 10f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                {
                    selectedItem = 1;
                    toDraw = boxE;
                    offset = 5f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                {
                    selectedItem = 2;
                    toDraw = railE;
                    offset = 5f;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.R) && !firstPressR)
                {
                    rotation *= Matrix.CreateRotationY((float)Math.PI / 2);
                    firstPressR = true;
                }
                else if (Keyboard.GetState().IsKeyUp(Keys.R))
                    firstPressR = false;

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !firstPressEnter)
                {
                    float cos = (float)Math.Cos((double)sk8.angle) * offset;
                    float sin = -(float)Math.Sin((double)sk8.angle) * offset;
                    float x = sk8.deck.translation.M41;
                    float z = sk8.deck.translation.M43;

                    Matrix translation = toDraw.translation;
                    if (selectedItem != 0)
                        translation = Matrix.CreateTranslation(x, 0f, z) * Matrix.CreateTranslation(cos, 0, sin);

                    switch (selectedItem)
                    {
                        case 0:
                            mainGame.floor.Add(new Floor(game, translation, Matrix.Identity, rotation, Matrix.Identity));
                            break;
                        case 1:
                            mainGame.boxes.Add(new Box(game, translation, Matrix.Identity, rotation, Matrix.Identity));
                            break;
                        case 2:
                            mainGame.rails.Add(new Rail(game, translation, Matrix.Identity, rotation, Matrix.Identity));
                            break;
                    }

                    firstPressEnter = true;
                }
                else if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                    firstPressEnter = false;

                if (Keyboard.GetState().IsKeyDown(Keys.T) && !firstPressT)
                {
                    firstPressT = true;

                    List<float> boxDists = new List<float>();
                    List<float> railDists = new List<float>();
                    List<float> floorDists = new List<float>();

                    Vector3 toDrawVec = new Vector3(toDraw.translation.M41, toDraw.translation.M42, toDraw.translation.M43);

                    foreach (Box box in mainGame.boxes)
                    {
                        Vector3 boxVec = new Vector3(box.box.translation.M41, box.box.translation.M42, box.box.translation.M43);
                        float dist = Vector3.Distance(toDrawVec, boxVec);
                        boxDists.Add(dist);
                    }
                    foreach (Rail rail in mainGame.rails)
                    {
                        Vector3 railVec = new Vector3(rail.rail.translation.M41, rail.rail.translation.M42, rail.rail.translation.M43);
                        float dist = Vector3.Distance(toDrawVec, railVec);
                        railDists.Add(dist);
                    }
                    foreach (Floor floor in mainGame.floor)
                    {
                        Vector3 floorVec = new Vector3(floor.floor.translation.M41, floor.floor.translation.M42, floor.floor.translation.M43);
                        float dist = Vector3.Distance(toDrawVec, floorVec);
                        floorDists.Add(dist);
                    }

                    float minBox = boxDists.Any() ? boxDists.Min() : 99999;
                    int minBoxIndex = boxDists.IndexOf(minBox);
                    float minRail = railDists.Any() ? railDists.Min() : 99999;
                    int minRailIndex = railDists.IndexOf(minRail);
                    float minFloor = floorDists.Any() ? floorDists.Min() : 99999;
                    int minFloorIndex = floorDists.IndexOf(minFloor);

                    List<float> minList = new List<float> { minBox, minRail, minFloor };
                    float minIndex = minList.IndexOf(minList.Min());

                    switch (minIndex)
                    {
                        case 0:
                            mainGame.boxes.RemoveAt(minBoxIndex);
                            break;
                        case 1:
                            mainGame.rails.RemoveAt(minRailIndex);
                            break;
                        case 2:
                            mainGame.floor.RemoveAt(minFloorIndex);
                            break;
                    }
                }
                else if (Keyboard.GetState().IsKeyUp(Keys.T))
                    firstPressT = false;
            }

            private static float roundUp(float numToRound, float multiple)
            {
                if (multiple == 0)
                    return numToRound;

                float remainder = Math.Abs(numToRound) % multiple;
                if (remainder == 0)
                    return numToRound;

                if (numToRound < 0)
                    return -(Math.Abs(numToRound) - remainder);
                else
                    return numToRound + multiple - remainder;
            }

            public static void DrawEditWorld(Matrix viewMatrix, Matrix projectionMatrix, Skate sk8)
            {
                float cos = (float)Math.Cos((double)sk8.angle) * offset;
                float sin = -(float)Math.Sin((double)sk8.angle) * offset;
                float x = sk8.deck.translation.M41;
                float z = sk8.deck.translation.M43;

                toDraw.translation = Matrix.CreateTranslation(x, 0f, z) * Matrix.CreateTranslation(cos, 0, sin);
                toDraw.rotationY = rotation;

                if (selectedItem == 0)
                {
                    float tDX = toDraw.translation.M41;
                    float tDZ = toDraw.translation.M43;

                    toDraw.translation.M41 = roundUp(tDX, 12.5f);
                    toDraw.translation.M43 = roundUp(tDZ, 12.5f);
                }

                //for every model
                foreach (ModelMesh mesh in toDraw.model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.View = viewMatrix;
                        effect.World = toDraw.rotationX * toDraw.rotationY * toDraw.rotationZ * toDraw.translation;
                        effect.Projection = projectionMatrix;
                    }

                    //Draw Model
                    mesh.Draw();
                }
            }
        }
    }
}
