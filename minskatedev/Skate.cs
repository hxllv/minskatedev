using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace minskatedev
{
    public partial class MainGame
    {
        public partial class Skate 
        {
            public ModelHelper deck;
            public ModelHelper trucksF;
            public ModelHelper trucksB;
            public ModelHelper wheelFL;
            public ModelHelper wheelFR;
            public ModelHelper wheelBL;
            public ModelHelper wheelBR;
            public BoundingBox deckBounds;
            public BoundingBox truckFBounds;
            public BoundingBox truckBBounds;
            public BoundingBox wheelFLBounds;
            public BoundingBox wheelFRBounds;
            public BoundingBox wheelBLBounds;
            public BoundingBox wheelBRBounds;
            public Vector3 center;
            public float angle;
            public bool isReversing;
            MainGame mainGame;

            Matrix identWFL = Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateTranslation(0.6f, 0, 0.3125f);
            Matrix identWFLU = Matrix.CreateTranslation(-0.6f, 0, -0.3125f) * Matrix.CreateRotationY(-(float)Math.PI);
            Matrix identWFR = Matrix.CreateTranslation(0.6f, 0, -0.3125f);
            Matrix identWFRU = Matrix.CreateTranslation(-0.6f, 0, 0.3125f);
            Matrix identWBL = Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateTranslation(-0.6f, 0, 0.3125f);
            Matrix identWBLU = Matrix.CreateTranslation(0.6f, 0, -0.3125f) * Matrix.CreateRotationY(-(float)Math.PI);
            Matrix identWBR = Matrix.CreateTranslation(-0.6f, 0, -0.3125f);
            Matrix identWBRU = Matrix.CreateTranslation(0.6f, 0, 0.3125f);
            Matrix identTF = Matrix.CreateTranslation(0.6f, 0, 0);
            Matrix identTB = Matrix.CreateTranslation(-0.6f, 0, 0);

            public Skate(ModelHelper[] sk8, MainGame mainGame)
            {
                this.wheelFL = sk8[0];
                this.wheelFR = sk8[1];
                this.wheelBL = sk8[2];
                this.wheelBR = sk8[3];
                this.deck = sk8[4];
                this.trucksF = sk8[5];
                this.trucksB = sk8[6];
                this.wheelFL.translation = Matrix.CreateTranslation(0.6f, 1f, 0.3125f);
                this.wheelFL.rotationY = Matrix.CreateRotationY((float)Math.PI);
                this.wheelFR.translation = Matrix.CreateTranslation(0.6f, 1f, -0.3125f);
                this.wheelBL.translation = Matrix.CreateTranslation(-0.6f, 1f, 0.3125f);
                this.wheelBL.rotationY = Matrix.CreateRotationY((float)Math.PI);
                this.wheelBR.translation = Matrix.CreateTranslation(-0.6f, 1f, -0.3125f);
                this.deck.translation = Matrix.CreateTranslation(0f, 1f, 0f);
                this.trucksF.translation = Matrix.CreateTranslation(0.6f, 1f, 0f);
                this.trucksB.translation = Matrix.CreateTranslation(-0.6f, 1f, 0f);

                this.deckBounds = UpdateBoundingBox(this.deck.model, this.deck.translation);
                this.truckFBounds = UpdateBoundingBox(this.trucksF.model, this.trucksF.translation);
                this.truckBBounds = UpdateBoundingBox(this.trucksB.model, this.trucksB.translation);
                this.wheelFLBounds = UpdateBoundingBox(this.wheelFL.model, this.wheelFL.translation);
                this.wheelFRBounds = UpdateBoundingBox(this.wheelFR.model, this.wheelFR.translation);
                this.wheelBLBounds = UpdateBoundingBox(this.wheelBL.model, this.wheelBL.translation);
                this.wheelBRBounds = UpdateBoundingBox(this.wheelBR.model, this.wheelBR.translation);

                this.mainGame = mainGame;
            }

            public Matrix Move(ref Vector3 camTarget, ref Vector3 camPosition, float sk8Speed)
            {
                Matrix deckMatrix = this.deck.translation;
                Matrix wheelsFLMatrix = this.wheelFL.translation;
                Matrix wheelsFRMatrix = this.wheelFR.translation;
                Matrix wheelsBLMatrix = this.wheelBL.translation;
                Matrix wheelsBRMatrix = this.wheelBR.translation;
                Matrix trucksFMatrix = this.trucksF.translation;
                Matrix trucksBMatrix = this.trucksB.translation;

                float cos = (float)Math.Cos((double)this.angle) * sk8Speed;
                float sin = -(float)Math.Sin((double)this.angle) * sk8Speed;
                Matrix fwd = Matrix.CreateTranslation(cos, 0f, sin);

                this.deck.translation = deckMatrix * fwd;
                this.wheelFL.translation = wheelsFLMatrix * fwd;
                this.wheelFR.translation = wheelsFRMatrix * fwd;
                this.wheelBL.translation = wheelsBLMatrix * fwd;
                this.wheelBR.translation = wheelsBRMatrix * fwd;
                this.trucksF.translation = trucksFMatrix * fwd;
                this.trucksB.translation = trucksBMatrix * fwd;
                this.deckBounds = UpdateBoundingBox(this.deck.model, this.deck.translation);
                this.truckFBounds = UpdateBoundingBox(this.trucksF.model, this.trucksF.translation);
                this.truckBBounds = UpdateBoundingBox(this.trucksB.model, this.trucksB.translation);
                this.wheelFLBounds = UpdateBoundingBox(this.wheelFL.model, this.wheelFL.translation);
                this.wheelFRBounds = UpdateBoundingBox(this.wheelFR.model, this.wheelFR.translation);
                this.wheelBLBounds = UpdateBoundingBox(this.wheelBL.model, this.wheelBL.translation);
                this.wheelBRBounds = UpdateBoundingBox(this.wheelBR.model, this.wheelBR.translation);

                camTarget.X = camTarget.X + cos;
                camTarget.Z = camTarget.Z + sin;
                camPosition.X = camPosition.X + cos;
                camPosition.Z = camPosition.Z + sin;

                return Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0, 1f, 0f));
            }

            public Matrix VertMove(ref Vector3 camTarget, ref Vector3 camPosition, float sk8VertVel)
            {
                Matrix deckMatrix = this.deck.translation;
                Matrix wheelsFLMatrix = this.wheelFL.translation;
                Matrix wheelsFRMatrix = this.wheelFR.translation;
                Matrix wheelsBLMatrix = this.wheelBL.translation;
                Matrix wheelsBRMatrix = this.wheelBR.translation;
                Matrix trucksFMatrix = this.trucksF.translation;
                Matrix trucksBMatrix = this.trucksB.translation;
                Matrix vert = Matrix.CreateTranslation(0f, sk8VertVel, 0f);

                this.deck.translation = deckMatrix * vert;
                this.wheelFL.translation = wheelsFLMatrix * vert;
                this.wheelFR.translation = wheelsFRMatrix * vert;
                this.wheelBL.translation = wheelsBLMatrix * vert;
                this.wheelBR.translation = wheelsBRMatrix * vert;
                this.trucksF.translation = trucksFMatrix * vert;
                this.trucksB.translation = trucksBMatrix * vert;

                this.deckBounds = UpdateBoundingBox(this.deck.model, this.deck.translation);
                this.truckFBounds = UpdateBoundingBox(this.trucksF.model, this.trucksF.translation);
                this.truckBBounds = UpdateBoundingBox(this.trucksB.model, this.trucksB.translation);
                this.wheelFLBounds = UpdateBoundingBox(this.wheelFL.model, this.wheelFL.translation);
                this.wheelFRBounds = UpdateBoundingBox(this.wheelFR.model, this.wheelFR.translation);
                this.wheelBLBounds = UpdateBoundingBox(this.wheelBL.model, this.wheelBL.translation);
                this.wheelBRBounds = UpdateBoundingBox(this.wheelBR.model, this.wheelBR.translation);

                camTarget.Y = camTarget.Y + sk8VertVel;
                camPosition.Y = camPosition.Y + sk8VertVel;

                return Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0, 1f, 0f));
            }

            public Matrix Turn(ref Vector3 camTarget, ref Vector3 camPosition, float angle)
            {
                Matrix deckMatrix = this.deck.rotationY;
                Matrix wheelsFLMatrix = this.wheelFL.rotationY;
                Matrix wheelsFRMatrix = this.wheelFR.rotationY;
                Matrix wheelsBLMatrix = this.wheelBL.rotationY;
                Matrix wheelsBRMatrix = this.wheelBR.rotationY;
                Matrix trucksFMatrix = this.trucksF.rotationY;
                Matrix trucksBMatrix = this.trucksB.rotationY;

                Matrix lft = Matrix.CreateFromYawPitchRoll(angle, 0f, 0f);
                Matrix ident = Matrix.Identity;
                
                this.angle += angle;
                this.deck.rotationY = ident * lft * deckMatrix;

                this.wheelFL.rotationY = identWFL * lft * identWFLU * wheelsFLMatrix;
                this.wheelFR.rotationY = identWFR * lft * identWFRU * wheelsFRMatrix;
                this.wheelBL.rotationY = identWBL * lft * identWBLU * wheelsBLMatrix;
                this.wheelBR.rotationY = identWBR * lft * identWBRU * wheelsBRMatrix;

                this.trucksF.rotationY = identTF * lft * identTB * trucksFMatrix;
                this.trucksB.rotationY = identTB * lft * identTF * trucksBMatrix;

                this.deckBounds = UpdateBoundingBox(this.deck.model, this.deck.translation);
                this.truckFBounds = UpdateBoundingBox(this.trucksF.model, this.trucksF.translation);
                this.truckBBounds = UpdateBoundingBox(this.trucksB.model, this.trucksB.translation);
                this.wheelFLBounds = UpdateBoundingBox(this.wheelFL.model, this.wheelFL.translation);
                this.wheelFRBounds = UpdateBoundingBox(this.wheelFR.model, this.wheelFR.translation);
                this.wheelBLBounds = UpdateBoundingBox(this.wheelBL.model, this.wheelBL.translation);
                this.wheelBRBounds = UpdateBoundingBox(this.wheelBR.model, this.wheelBR.translation);

                camPosition = Vector3.Transform(camPosition - camTarget, lft) + camTarget;

                return Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0, 1f, 0f));
            }

            public void Rotate(float rotX, float rotY, float rotZ)
            {
                Matrix deckMatrixX = this.deck.rotationX;
                Matrix deckMatrixY = this.deck.rotationY;
                Matrix deckMatrixZ = this.deck.rotationZ;
                Matrix wheelsFLMatrixX = this.wheelFL.rotationX;
                Matrix wheelsFLMatrixY = this.wheelFL.rotationY;
                Matrix wheelsFLMatrixZ = this.wheelFL.rotationZ;
                Matrix wheelsFRMatrixX = this.wheelFR.rotationX;
                Matrix wheelsFRMatrixY = this.wheelFR.rotationY;
                Matrix wheelsFRMatrixZ = this.wheelFR.rotationZ;
                Matrix wheelsBLMatrixX = this.wheelBL.rotationX;
                Matrix wheelsBLMatrixY = this.wheelBL.rotationY;
                Matrix wheelsBLMatrixZ = this.wheelBL.rotationZ;
                Matrix wheelsBRMatrixX = this.wheelBR.rotationX;
                Matrix wheelsBRMatrixY = this.wheelBR.rotationY;
                Matrix wheelsBRMatrixZ = this.wheelBR.rotationZ;
                Matrix trucksFMatrixX = this.trucksF.rotationX;
                Matrix trucksFMatrixY = this.trucksF.rotationY;
                Matrix trucksFMatrixZ = this.trucksF.rotationZ;
                Matrix trucksBMatrixX = this.trucksB.rotationX;
                Matrix trucksBMatrixY = this.trucksB.rotationY;
                Matrix trucksBMatrixZ = this.trucksB.rotationZ;

                Matrix rotMatrixX = Matrix.CreateFromYawPitchRoll(0, 0, rotX);
                Matrix rotMatrixY = Matrix.CreateFromYawPitchRoll(rotY, 0, 0);
                Matrix rotMatrixZ = Matrix.CreateFromYawPitchRoll(0, rotZ, 0);

                Matrix ident = Matrix.Identity;

                this.deck.rotationX = ident * rotMatrixX * deckMatrixX;
                this.deck.rotationY = ident * rotMatrixY * deckMatrixY;
                this.deck.rotationZ = ident * rotMatrixZ * deckMatrixZ;
                this.wheelFL.rotationX = identWFL * rotMatrixX * identWFLU * wheelsFLMatrixX;
                this.wheelFL.rotationY = identWFL * rotMatrixY * identWFLU * wheelsFLMatrixY;
                this.wheelFL.rotationZ = identWFL * rotMatrixZ * identWFLU * wheelsFLMatrixZ;
                this.wheelFR.rotationX = identWFR * rotMatrixX * identWFRU * wheelsFRMatrixX;
                this.wheelFR.rotationY = identWFR * rotMatrixY * identWFRU * wheelsFRMatrixY;
                this.wheelFR.rotationZ = identWFR * rotMatrixZ * identWFRU * wheelsFRMatrixZ;
                this.wheelBL.rotationX = identWBL * rotMatrixX * identWBLU * wheelsBLMatrixX;
                this.wheelBL.rotationY = identWBL * rotMatrixY * identWBLU * wheelsBLMatrixY;
                this.wheelBL.rotationZ = identWBL * rotMatrixZ * identWBLU * wheelsBLMatrixZ;
                this.wheelBR.rotationX = identWBR * rotMatrixX * identWBRU * wheelsBRMatrixX;
                this.wheelBR.rotationY = identWBR * rotMatrixY * identWBRU * wheelsBRMatrixY;
                this.wheelBR.rotationZ = identWBR * rotMatrixZ * identWBRU * wheelsBRMatrixZ;
                this.trucksF.rotationX = identTF * rotMatrixX * identTB * trucksFMatrixX;
                this.trucksF.rotationY = identTF * rotMatrixY * identTB * trucksFMatrixY;
                this.trucksF.rotationZ = identTF * rotMatrixZ * identTB * trucksFMatrixZ;
                this.trucksB.rotationX = identTB * rotMatrixX * identTF * trucksBMatrixX;
                this.trucksB.rotationY = identTB * rotMatrixY * identTF * trucksBMatrixY;
                this.trucksB.rotationZ = identTB * rotMatrixZ * identTF * trucksBMatrixZ;
            }

            public void ResetSk8()
            {
                this.wheelFL.translation = Matrix.CreateTranslation(0.6f, 1f, 0.3125f);
                this.wheelFL.rotationX = Matrix.Identity;
                this.wheelFL.rotationY = Matrix.CreateRotationY((float)Math.PI);
                this.wheelFL.rotationZ = Matrix.Identity;

                this.wheelFR.translation = Matrix.CreateTranslation(0.6f, 1f, -0.3125f);
                this.wheelFR.rotationX = Matrix.Identity;
                this.wheelFR.rotationY = Matrix.Identity;
                this.wheelFR.rotationZ = Matrix.Identity;

                this.wheelBL.translation = Matrix.CreateTranslation(-0.6f, 1f, 0.3125f);
                this.wheelBL.rotationX = Matrix.Identity;
                this.wheelBL.rotationY = Matrix.CreateRotationY((float)Math.PI);
                this.wheelBL.rotationZ = Matrix.Identity;

                this.wheelBR.translation = Matrix.CreateTranslation(-0.6f, 1f, -0.3125f);
                this.wheelBR.rotationX = Matrix.Identity;
                this.wheelBR.rotationY = Matrix.Identity;
                this.wheelBR.rotationZ = Matrix.Identity;

                this.deck.translation = Matrix.CreateTranslation(0f, 1f, 0f);
                this.deck.rotationX = Matrix.Identity;
                this.deck.rotationY = Matrix.Identity;
                this.deck.rotationZ = Matrix.Identity;

                this.trucksF.translation = Matrix.CreateTranslation(0.6f, 1f, 0f);
                this.trucksF.rotationX = Matrix.Identity;
                this.trucksF.rotationY = Matrix.Identity;
                this.trucksF.rotationZ = Matrix.Identity;

                this.trucksB.translation = Matrix.CreateTranslation(-0.6f, 1f, 0f);
                this.trucksB.rotationX = Matrix.Identity;
                this.trucksB.rotationY = Matrix.Identity;
                this.trucksB.rotationZ = Matrix.Identity;
                this.angle = 0;

                mainGame.camTarget = new Vector3(0f, 0.8f, 0f);
                mainGame.camPosition = new Vector3(-5f, 2.3f, 0f);
                mainGame.viewMatrix = Matrix.CreateLookAt(mainGame.camPosition, mainGame.camTarget, new Vector3(0, 1f, 0f));
            }

            public void ResetRotationXandZ()
            {
                this.deck.rotationX = Matrix.Identity;
                this.deck.rotationZ = Matrix.Identity;
                this.wheelFL.rotationX = Matrix.Identity;
                this.wheelFL.rotationZ = Matrix.Identity;
                this.wheelFR.rotationX = Matrix.Identity;
                this.wheelFR.rotationZ = Matrix.Identity;
                this.wheelBL.rotationX = Matrix.Identity;
                this.wheelBL.rotationZ = Matrix.Identity;
                this.wheelBR.rotationX = Matrix.Identity;
                this.wheelBR.rotationZ = Matrix.Identity;
                this.trucksF.rotationX = Matrix.Identity;
                this.trucksF.rotationZ = Matrix.Identity;
                this.trucksB.rotationX = Matrix.Identity;
                this.trucksB.rotationZ = Matrix.Identity;
            }

            public void ResetRotationY()
            {
                Matrix lft = Matrix.CreateFromYawPitchRoll(this.angle, 0, 0);
                Matrix deg180 = Matrix.CreateRotationY((float)Math.PI);

                this.deck.rotationY = lft;
                this.wheelFL.rotationY = identWFL * lft * identWFLU * deg180;
                this.wheelFR.rotationY = identWFR * lft * identWFRU;
                this.wheelBL.rotationY = identWBL * lft * identWBLU * deg180;
                this.wheelBR.rotationY = identWBR * lft * identWBRU;
                this.trucksF.rotationY = identTF * lft * identTB;
                this.trucksB.rotationY = identTB * lft * identTF;
            }

            public void SkateDraw(Matrix viewMatrix, Matrix projectionMatrix)
            {
                foreach (ModelHelper model in new ModelHelper[] { deck, trucksF, trucksB, wheelFL, wheelFR, wheelBL, wheelBR })
                {

                    //for every model
                    foreach (ModelMesh mesh in model.model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.View = viewMatrix;
                            effect.World = model.rotationX * model.rotationZ * model.rotationY * model.translation;
                            effect.Projection = projectionMatrix;
                        }

                        //Draw Model
                        mesh.Draw();
                    }
                }
            }
        }
    }
}
