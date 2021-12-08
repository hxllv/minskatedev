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

            Matrix identWFL = Matrix.Identity * Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateTranslation(0.6f, 0, 0.3125f);
            Matrix identWFLU = Matrix.Identity * Matrix.CreateTranslation(-0.6f, 0, -0.3125f) * Matrix.CreateRotationY(-(float)Math.PI);
            Matrix identWFR = Matrix.Identity * Matrix.CreateTranslation(0.6f, 0, -0.3125f);
            Matrix identWFRU = Matrix.Identity * Matrix.CreateTranslation(-0.6f, 0, 0.3125f);
            Matrix identWBL = Matrix.Identity * Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateTranslation(-0.6f, 0, 0.3125f);
            Matrix identWBLU = Matrix.Identity * Matrix.CreateTranslation(0.6f, 0, -0.3125f) * Matrix.CreateRotationY(-(float)Math.PI);
            Matrix identWBR = Matrix.Identity * Matrix.CreateTranslation(-0.6f, 0, -0.3125f);
            Matrix identWBRU = Matrix.Identity * Matrix.CreateTranslation(0.6f, 0, 0.3125f);
            Matrix identTF = Matrix.Identity * Matrix.CreateTranslation(0.6f, 0, 0);
            Matrix identTB = Matrix.Identity * Matrix.CreateTranslation(-0.6f, 0, 0);

            public Skate(ModelHelper[] sk8)
            {
                this.wheelFL = sk8[0];
                this.wheelFR = sk8[1];
                this.wheelBL = sk8[2];
                this.wheelBR = sk8[3];
                this.deck = sk8[4];
                this.trucksF = sk8[5];
                this.trucksB = sk8[6];
                this.wheelFL.worldMatrix = Matrix.Identity * Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateTranslation(0.6f, 1f, 0.3125f);
                this.wheelFR.worldMatrix = Matrix.Identity * Matrix.CreateTranslation(0.6f, 1f, -0.3125f);
                this.wheelBL.worldMatrix = Matrix.Identity * Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateTranslation(-0.6f, 1f, 0.3125f);
                this.wheelBR.worldMatrix = Matrix.Identity * Matrix.CreateTranslation(-0.6f, 1f, -0.3125f);
                this.deck.worldMatrix = Matrix.Identity * Matrix.CreateTranslation(0f, 1f, 0f);
                this.trucksF.worldMatrix = Matrix.Identity * Matrix.CreateTranslation(0.6f, 1f, 0f);
                this.trucksB.worldMatrix = Matrix.Identity * Matrix.CreateTranslation(-0.6f, 1f, 0f);
                this.deckBounds = UpdateBoundingBox(this.deck.model, this.deck.worldMatrix);
                this.truckFBounds = UpdateBoundingBox(this.trucksF.model, this.trucksF.worldMatrix);
                this.truckBBounds = UpdateBoundingBox(this.trucksB.model, this.trucksB.worldMatrix);
                this.wheelFLBounds = UpdateBoundingBox(this.wheelFL.model, this.wheelFL.worldMatrix);
                this.wheelFRBounds = UpdateBoundingBox(this.wheelFR.model, this.wheelFR.worldMatrix);
                this.wheelBLBounds = UpdateBoundingBox(this.wheelBL.model, this.wheelBL.worldMatrix);
                this.wheelBRBounds = UpdateBoundingBox(this.wheelBR.model, this.wheelBR.worldMatrix);
            }

            public Matrix Move(ref Vector3 camTarget, ref Vector3 camPosition, float sk8Speed)
            {
                Matrix deckMatrix = this.deck.worldMatrix;
                Matrix wheelsFLMatrix = this.wheelFL.worldMatrix;
                Matrix wheelsFRMatrix = this.wheelFR.worldMatrix;
                Matrix wheelsBLMatrix = this.wheelBL.worldMatrix;
                Matrix wheelsBRMatrix = this.wheelBR.worldMatrix;
                Matrix trucksFMatrix = this.trucksF.worldMatrix;
                Matrix trucksBMatrix = this.trucksB.worldMatrix;

                float cos = (float)Math.Cos((double)this.angle) * sk8Speed;
                float sin = -(float)Math.Sin((double)this.angle) * sk8Speed;
                Matrix fwd = Matrix.CreateTranslation(cos, 0f, sin);

                this.deck.worldMatrix = deckMatrix * fwd;
                this.wheelFL.worldMatrix = wheelsFLMatrix * fwd;
                this.wheelFR.worldMatrix = wheelsFRMatrix * fwd;
                this.wheelBL.worldMatrix = wheelsBLMatrix * fwd;
                this.wheelBR.worldMatrix = wheelsBRMatrix * fwd;
                this.trucksF.worldMatrix = trucksFMatrix * fwd;
                this.trucksB.worldMatrix = trucksBMatrix * fwd;
                this.deckBounds = UpdateBoundingBox(this.deck.model, this.deck.worldMatrix);
                this.truckFBounds = UpdateBoundingBox(this.trucksF.model, this.trucksF.worldMatrix);
                this.truckBBounds = UpdateBoundingBox(this.trucksB.model, this.trucksB.worldMatrix);
                this.wheelFLBounds = UpdateBoundingBox(this.wheelFL.model, this.wheelFL.worldMatrix);
                this.wheelFRBounds = UpdateBoundingBox(this.wheelFR.model, this.wheelFR.worldMatrix);
                this.wheelBLBounds = UpdateBoundingBox(this.wheelBL.model, this.wheelBL.worldMatrix);
                this.wheelBRBounds = UpdateBoundingBox(this.wheelBR.model, this.wheelBR.worldMatrix);

                camTarget.X = camTarget.X + cos;
                camTarget.Z = camTarget.Z + sin;
                camPosition.X = camPosition.X + cos;
                camPosition.Z = camPosition.Z + sin;

                return Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0, 1f, 0f));
            }

            public Matrix VertMove(ref Vector3 camTarget, ref Vector3 camPosition, float sk8VertVel)
            {
                Matrix deckMatrix = this.deck.worldMatrix;
                Matrix wheelsFLMatrix = this.wheelFL.worldMatrix;
                Matrix wheelsFRMatrix = this.wheelFR.worldMatrix;
                Matrix wheelsBLMatrix = this.wheelBL.worldMatrix;
                Matrix wheelsBRMatrix = this.wheelBR.worldMatrix;
                Matrix trucksFMatrix = this.trucksF.worldMatrix;
                Matrix trucksBMatrix = this.trucksB.worldMatrix;
                Matrix vert = Matrix.CreateTranslation(0f, sk8VertVel, 0f);

                this.deck.worldMatrix = deckMatrix * vert;
                this.wheelFL.worldMatrix = wheelsFLMatrix * vert;
                this.wheelFR.worldMatrix = wheelsFRMatrix * vert;
                this.wheelBL.worldMatrix = wheelsBLMatrix * vert;
                this.wheelBR.worldMatrix = wheelsBRMatrix * vert;
                this.trucksF.worldMatrix = trucksFMatrix * vert;
                this.trucksB.worldMatrix = trucksBMatrix * vert;

                this.deckBounds = UpdateBoundingBox(this.deck.model, this.deck.worldMatrix);
                this.truckFBounds = UpdateBoundingBox(this.trucksF.model, this.trucksF.worldMatrix);
                this.truckBBounds = UpdateBoundingBox(this.trucksB.model, this.trucksB.worldMatrix);
                this.wheelFLBounds = UpdateBoundingBox(this.wheelFL.model, this.wheelFL.worldMatrix);
                this.wheelFRBounds = UpdateBoundingBox(this.wheelFR.model, this.wheelFR.worldMatrix);
                this.wheelBLBounds = UpdateBoundingBox(this.wheelBL.model, this.wheelBL.worldMatrix);
                this.wheelBRBounds = UpdateBoundingBox(this.wheelBR.model, this.wheelBR.worldMatrix);

                camTarget.Y = camTarget.Y + sk8VertVel;
                camPosition.Y = camPosition.Y + sk8VertVel;

                return Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0, 1f, 0f));
            }

            public Matrix Turn(ref Vector3 camTarget, ref Vector3 camPosition, float angle)
            {
                Matrix deckMatrix = this.deck.worldMatrix;
                Matrix wheelsFLMatrix = this.wheelFL.worldMatrix;
                Matrix wheelsFRMatrix = this.wheelFR.worldMatrix;
                Matrix wheelsBLMatrix = this.wheelBL.worldMatrix;
                Matrix wheelsBRMatrix = this.wheelBR.worldMatrix;
                Matrix trucksFMatrix = this.trucksF.worldMatrix;
                Matrix trucksBMatrix = this.trucksB.worldMatrix;

                Matrix lft = Matrix.CreateFromYawPitchRoll(angle, 0f, 0f);
                Matrix ident = Matrix.Identity;
                
                this.angle += angle;
                this.deck.worldMatrix = ident * lft * deckMatrix;

                this.wheelFL.worldMatrix = identWFL * lft * identWFLU * wheelsFLMatrix;
                this.wheelFR.worldMatrix = identWFR * lft * identWFRU * wheelsFRMatrix;
                this.wheelBL.worldMatrix = identWBL * lft * identWBLU * wheelsBLMatrix;
                this.wheelBR.worldMatrix = identWBR * lft * identWBRU * wheelsBRMatrix;

                this.trucksF.worldMatrix = identTF * lft * identTB * trucksFMatrix;
                this.trucksB.worldMatrix = identTB * lft * identTF * trucksBMatrix;

                this.deckBounds = UpdateBoundingBox(this.deck.model, this.deck.worldMatrix);
                this.truckFBounds = UpdateBoundingBox(this.trucksF.model, this.trucksF.worldMatrix);
                this.truckBBounds = UpdateBoundingBox(this.trucksB.model, this.trucksB.worldMatrix);
                this.wheelFLBounds = UpdateBoundingBox(this.wheelFL.model, this.wheelFL.worldMatrix);
                this.wheelFRBounds = UpdateBoundingBox(this.wheelFR.model, this.wheelFR.worldMatrix);
                this.wheelBLBounds = UpdateBoundingBox(this.wheelBL.model, this.wheelBL.worldMatrix);
                this.wheelBRBounds = UpdateBoundingBox(this.wheelBR.model, this.wheelBR.worldMatrix);

                camPosition = Vector3.Transform(camPosition - camTarget, lft) + camTarget;

                return Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0, 1f, 0f));
            }

            public void Rotate(float rotX, float rotY, float rotZ)
            {
                Matrix deckMatrix = this.deck.worldMatrix;
                Matrix wheelsFLMatrix = this.wheelFL.worldMatrix;
                Matrix wheelsFRMatrix = this.wheelFR.worldMatrix;
                Matrix wheelsBLMatrix = this.wheelBL.worldMatrix;
                Matrix wheelsBRMatrix = this.wheelBR.worldMatrix;
                Matrix trucksFMatrix = this.trucksF.worldMatrix;
                Matrix trucksBMatrix = this.trucksB.worldMatrix;
                Matrix rotMatrix = Matrix.CreateFromYawPitchRoll(rotY, rotZ, rotX);

                Matrix ident = Matrix.Identity;

                this.deck.worldMatrix = ident * rotMatrix * deckMatrix;
                this.wheelFL.worldMatrix = identWFL * rotMatrix * identWFLU * wheelsFLMatrix;
                this.wheelFR.worldMatrix = identWFR * rotMatrix * identWFRU * wheelsFRMatrix;
                this.wheelBL.worldMatrix = identWBL * rotMatrix * identWBLU * wheelsBLMatrix;
                this.wheelBR.worldMatrix = identWBR * rotMatrix * identWBRU * wheelsBRMatrix;
                this.trucksF.worldMatrix = identTF * rotMatrix * identTB * trucksFMatrix;
                this.trucksB.worldMatrix = identTB * rotMatrix * identTF * trucksBMatrix;
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
                            effect.World = model.worldMatrix;
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
