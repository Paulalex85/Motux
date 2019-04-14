using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO.IsolatedStorage;

namespace Motux
{
    class BackgroundScreen : GameScreen
    {
        Texture2D background;
        string theme = (string)IsolatedStorageSettings.ApplicationSettings["theme"];

        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            background = ScreenManager.Game.Content.Load<Texture2D>("background");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            spriteBatch.Begin();

            if (theme == "classic")
            {
                ScreenManager.SpriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            }
            else
            {
                Rectangle r = new Rectangle(0, 0, 480, 800);
                ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, r, Color.White);
            }

            spriteBatch.End();
        }
    }
}
