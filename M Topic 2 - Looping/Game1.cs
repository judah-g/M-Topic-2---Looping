using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace M_Topic_2___Looping
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        List<Rectangle> bugs = new List<Rectangle>(), deadBugs = new List<Rectangle>();
        List<int> x = new List<int>(), y = new List<int>(), size = new List<int>(), textureNumber = new List<int>();
        List<float> deathTimer = new List<float>();
        Rectangle window = new Rectangle(0, 0, 800, 500);
        MouseState mouseState, prevMouseState;

        List<Texture2D> bugTextures = new List<Texture2D>(), deadBugTextures = new List<Texture2D>();
        Texture2D bedTexture, downSwatterTexture, upSwatterTexture;

        Random random = new Random();
        int movementHelper;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            for (int i = 0; i < 20; i++)
            {
                x.Add(random.Next(0, window.Width - 50));
                y.Add(random.Next(0, window.Height - 50));
                size.Add(random.Next(25, 60));
                bugs.Add(new Rectangle(x[i], y[i], size[i], size[i]));
            }


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < bugs.Count; i++)
            {
                textureNumber.Add(random.Next(1, 9));
                bugTextures.Add(Content.Load<Texture2D>($"bug{textureNumber[i]}"));
            }
            bedTexture = Content.Load<Texture2D>("truebed");
            upSwatterTexture = Content.Load<Texture2D>("flyup");
            downSwatterTexture = Content.Load<Texture2D>("flydown");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            //movement
            for (int i = 0; i < bugs.Count; i++)
            {
                movementHelper = random.Next(0, 4);

                if (movementHelper == 0)
                    x[i] += random.Next(0, 3);
                else if (movementHelper == 1)
                    y[i] += random.Next(0, 3);
                else if (movementHelper == 2)
                    x[i] -= random.Next(0, 3);
                else if (movementHelper == 3)
                    y[i] -= random.Next(0, 3);

                bugs[i] = new Rectangle(x[i], y[i], size[i], size[i]);
            }

            //collision
            for (int i = 0; i < bugs.Count; i++)
            {
                for (int j = 0; j < bugs.Count; j++)
                {
                    if (bugs[i].Intersects(bugs[j]) && i != j)
                    {
                        if (bugs[i].X > bugs[j].X)
                        {
                            x[i] += 1;
                            x[j] -= 1;
                        }
                        else
                        {
                            x[i] -= 1;
                            x[j] += 1;
                        }

                        if (bugs[i].Y > bugs[j].Y)
                        {
                            y[i] += 1;
                            y[j] -= 1;
                        }
                        else
                        {
                            y[i] -= 1;
                            y[j] += 1;
                        }
                    }
                    bugs[j] = new Rectangle(x[j], y[j], size[j], size[j]);
                }

                if (bugs[i].Left < 0)
                    x[i] = 1;
                else if (bugs[i].Right > window.Width)
                    x[i] = window.Width - bugs[i].Width - 1;

                if (bugs[i].Top < 0)
                    y[i] = 1;
                else if (bugs[i].Bottom > window.Height)
                    y[i] = window.Height - bugs[i].Height - 1;

                bugs[i] = new Rectangle(x[i], y[i], size[i], size[i]);
            }

            //clicking
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < bugs.Count; i++)
                {
                    if (bugs[i].Contains(mouseState.Position))
                    {
                        deadBugs.Add(bugs[i]);
                        deathTimer.Add((float)gameTime.ElapsedGameTime.TotalSeconds);
                        deadBugTextures.Add(Content.Load<Texture2D>($"bugdead{textureNumber[i]}"));

                        x[i] = random.Next(0, window.Width - 50);
                        y[i] = random.Next(0, window.Height - 50);
                        size[i] = random.Next(25, 60);
                        bugs[i] = new Rectangle(x[i], y[i], size[i], size[i]);

                        textureNumber[i] = random.Next(1, 9);
                        bugTextures[i] = Content.Load<Texture2D>($"bug{textureNumber[i]}");
                    }
                }
            }

            //dead bug logic
            for (int i = 0; i < deadBugs.Count; i++)
            {
                deathTimer[i] += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (deathTimer[i] > 2)
                {
                    deadBugs.RemoveAt(i);
                    deathTimer.RemoveAt(i);
                    deadBugTextures.RemoveAt(i);
                    i--;
                }
            }

            if (mouseState.ScrollWheelValue == prevMouseState.ScrollWheelValue + 120)
            {
                x.Add(random.Next(0, window.Width - 50));
                y.Add(random.Next(0, window.Height - 50));
                size.Add(random.Next(25, 60));
                bugs.Add(new Rectangle(x[x.Count - 1], y[y.Count - 1], size[size.Count - 1], size[size.Count - 1]));

                textureNumber.Add(random.Next(1, 9));
                bugTextures.Add(Content.Load<Texture2D>($"bug{textureNumber[textureNumber.Count - 1]}"));
            }

            Window.Title = mouseState.ScrollWheelValue.ToString();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(bedTexture, new Rectangle(-40, -60, window.Width + 300, window.Height + 300), Color.White);

            for (int i = 0; i < deadBugs.Count; i++)
                _spriteBatch.Draw(deadBugTextures[i], deadBugs[i], Color.White);
            for (int i = 0; i < bugs.Count; i++)
                _spriteBatch.Draw(bugTextures[i], bugs[i], Color.White);

            if (mouseState.LeftButton == ButtonState.Pressed)
                _spriteBatch.Draw(downSwatterTexture, new Rectangle(mouseState.X - 32, mouseState.Y - 20, 64, 64), Color.White);
            else
                _spriteBatch.Draw(upSwatterTexture, new Rectangle(mouseState.X - 32, mouseState.Y - 20, 64, 64), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
