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

        List<Rectangle> bugs = new List<Rectangle>();
        List<int> x = new List<int>(), y = new List<int>();
        Rectangle window = new Rectangle(0, 0, 800, 500);

        List<Texture2D> bugTextures = new List<Texture2D>();
        Texture2D bedTexture;

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
                bugs.Add(new Rectangle(x[i], y[i], 50, 50));
            }


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < bugs.Count; i++)
                bugTextures.Add(Content.Load<Texture2D>($"bug{random.Next(1, 9)}"));
            bedTexture = Content.Load<Texture2D>("truebed");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
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

                bugs[i] = new Rectangle(x[i], y[i], 50, 50);
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
                    bugs[j] = new Rectangle(x[j], y[j], 50, 50);
                }

                if (bugs[i].Left < 0)
                    x[i] = 1;
                else if (bugs[i].Right > window.Width)
                    x[i] = window.Width - bugs[i].Width - 1;

                if (bugs[i].Top < 0)
                    y[i] = 1;
                else if (bugs[i].Bottom > window.Height)
                    y[i] = window.Height - bugs[i].Height - 1;

                bugs[i] = new Rectangle(x[i], y[i], 50, 50);
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(bedTexture, new Rectangle(-40, -60, window.Width + 300, window.Height + 300), Color.White);
            for (int i = 0; i < bugs.Count; i++)
                _spriteBatch.Draw(bugTextures[i], bugs[i], Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
