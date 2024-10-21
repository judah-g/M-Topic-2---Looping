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
        List<Texture2D> bugTextures = new List<Texture2D>();
        Random random = new Random();
        Rectangle window = new Rectangle(0, 0, 800, 500);
        Texture2D bedTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            for (int i = 0; i < 10; i++)
                bugs.Add(new Rectangle(random.Next(0, window.Width), random.Next(0, window.Height), 50, 50));


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < bugs.Count; i++)
                bugTextures.Add(Content.Load<Texture2D>($"bug{random.Next(1, 9)}"));
            bedTexture = Content.Load<Texture2D>("bugbed");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(bedTexture, new Rectangle(0, 0, window.Width, window.Height), Color.White);
            for (int i = 0; i < bugs.Count; i++)
                _spriteBatch.Draw(bugTextures[i], bugs[i], Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
