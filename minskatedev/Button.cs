using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace minskatedev
{
    public partial class Game
    {
        public class Button
        {
            public static Game game;
            public int buttonX, buttonY;
            float scale;
            string name;
            public Texture2D text;
            public Texture2D textHover;
            static bool firstPress = false;
            static string onButton = null;
            string stringText;
            Rectangle rect;
            Vector2 size;

            public Button(string name, Texture2D text, Texture2D textHover, int buttonX, int buttonY, float scale)
            {
                this.name = name;
                this.text = text;
                this.textHover = textHover;
                this.buttonX = buttonX;
                this.buttonY = buttonY;
                this.scale = scale;
                this.rect = new Rectangle(0,0,0,0);
            }

            public Button(string name, string stringText, int buttonX, int buttonY, float scale)
            {
                this.name = name;
                this.stringText = stringText;
                this.buttonX = buttonX;
                this.buttonY = buttonY;
                this.scale = scale;

                this.size = game.font.MeasureString(this.stringText);
                this.rect = new Rectangle(buttonX, buttonY, (int)((size.X + 40) * scale), (int)((size.Y + 20) * scale));

                this.text = new Texture2D(game.GraphicsDevice, 1, 1);
                this.text.SetData(new Color[] { new Color(255, 97, 244) });
                this.textHover = new Texture2D(game.GraphicsDevice, 1, 1);
                this.textHover.SetData(new Color[] { new Color(179, 69, 172) });
            }

            public bool OnButton()
            {
                MouseState currentMouse = Mouse.GetState();

                if ((currentMouse.X < buttonX + text.Width || currentMouse.X < buttonX + size.X + 40) &&
                        currentMouse.X > buttonX &&
                        (currentMouse.Y < buttonY + text.Height || currentMouse.Y < buttonY + size.Y + 20) &&
                        currentMouse.Y > buttonY)
                    return true;
                return false;
            }

            public void Update()
            {
                if (OnButton() && Mouse.GetState().LeftButton == ButtonState.Pressed && !firstPress)
                {
                    firstPress = true;
                    onButton = this.name;
                }

                if (Mouse.GetState().LeftButton == ButtonState.Released && firstPress && onButton == this.name)
                {
                    firstPress = false;
                    onButton = null;
                    // button handler
                    switch (name)
                    {
                        case "resume":
                            mainGame.isPaused = false;
                            break;
                        case "backtomenu2":
                            gameState = 0;
                            mainGame = new MainGame(game, game.graphics, game.camTarget, game.camPosition, game.projectionMatrix, game.viewMatrix, game.worldMatrix);
                            mainGame.spriteBatch = new SpriteBatch(game.GraphicsDevice);
                            mainGame.font = game.font;
                            MainGame.Skate.Input.Sounds.roll = game.Content.Load<SoundEffect>("sounds/roll");
                            MainGame.Skate.Input.Sounds.pop = game.Content.Load<SoundEffect>("sounds/pop");
                            MainGame.Skate.Input.Sounds.land = game.Content.Load<SoundEffect>("sounds/land");
                            MainGame.Skate.Input.Sounds.rollInstance = MainGame.Skate.Input.Sounds.roll.CreateInstance();
                            menu.MenuInit();
                            break;
                        case "setreset":
                            mainGame.sk8.SetResetPoint();
                            MainGame.Skate.Input.TrickNames.trickName = "Reset point set!";
                            MainGame.Skate.Input.TrickNames.didTrick = true;
                            break;
                        case "reset":
                            mainGame.sk8.ResetSk8();
                            break;
                        case "play":
                            gameState = 1;
                            game.SaveSk8(menu.deckInd, menu.truckInd, menu.wheelInd);
                            mainGame.MainGameInit(menu.sk8);
                            break;
                        case "editsk8":
                            menu.menuState = 1;
                            menu.editState = 0;
                            break;
                        case "quit":
                            game.SaveSk8(menu.deckInd, menu.truckInd, menu.wheelInd);
                            game.Exit();
                            break;
                        case "backtomenu":
                            menu.menuState = 0;
                            break;
                        case "editback":
                            menu.editState = 0;
                            break;
                        case "wheels":
                            menu.editState = 1;
                            break;
                        case "wdef":
                            menu.sk8[0] = menu.sk8Def[0];
                            menu.sk8[1] = menu.sk8Def[1];
                            menu.sk8[2] = menu.sk8Def[2];
                            menu.sk8[3] = menu.sk8Def[3];
                            menu.wheelInd = -1;
                            break;
                        case "wblack":
                            menu.LoopWheels(0);
                            break;
                        case "wnavy":
                            menu.LoopWheels(1);
                            break;
                        case "wpurp":
                            menu.LoopWheels(2);
                            break;
                        case "wturq":
                            menu.LoopWheels(3);
                            break;
                        case "trucks":
                            menu.editState = 2;
                            break;
                        case "tdef":
                            menu.sk8[5] = menu.sk8Def[5];
                            menu.sk8[6] = menu.sk8Def[6];
                            menu.truckInd = -1;
                            break;
                        case "tblack":
                            menu.LoopTrucks(0);
                            break;
                        case "tnavy":
                            menu.LoopTrucks(1);
                            break;
                        case "tpurp":
                            menu.LoopTrucks(2);
                            break;
                        case "tturq":
                            menu.LoopTrucks(3);
                            break;
                        case "twhite":
                            menu.LoopTrucks(4);
                            break;
                        case "deck":
                            menu.editState = 3;
                            break;
                        case "ddef":
                            menu.sk8[4] = menu.sk8Def[4];
                            menu.deckInd = -1;
                            break;
                        case "dshake":
                            menu.sk8[4] = menu.decks[0];
                            menu.deckInd = 0;
                            break;
                        case "dsk8":
                            menu.sk8[4] = menu.decks[1];
                            menu.deckInd = 1;
                            break;
                        default:
                            break;
                    }
                }
            }
            public void Draw(SpriteBatch spriteBatch)
            {
                if (OnButton()) 
                {
                    if (rect != new Rectangle(0, 0, 0, 0))
                    {
                        spriteBatch.Draw(textHover, rect, Color.White);
                        spriteBatch.DrawString(game.font, stringText, new Vector2(buttonX + 20 * scale, buttonY + 10 * scale), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
                        return;
                    }
                    spriteBatch.Draw(textHover, new Rectangle(buttonX, buttonY, (int)(text.Width * scale), (int)(text.Height * scale)), Color.White);
                }
                else {
                    if (rect != new Rectangle(0, 0, 0, 0))
                    {
                        spriteBatch.Draw(text, rect, Color.White);
                        spriteBatch.DrawString(game.font, stringText, new Vector2(buttonX + 20 * scale, buttonY + 10 * scale), Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
                        return;
                    }
                    spriteBatch.Draw(text, new Rectangle(buttonX, buttonY, (int)(text.Width * scale), (int)(text.Height * scale)), Color.White);
                }
            }
        }
    }
}
