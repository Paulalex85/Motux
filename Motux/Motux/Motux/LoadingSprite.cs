using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Motux
{
    class LoadingSprite : GameScreen
    {
        SpriteFont _font;
        Texture2D _loading2D;
        Vector2 _position { get; set; }

        int _frame = 0;
        int _nombre_frame = 7;
        int _loadingWidth = 64;
        public float _timeframe = 200;
        float _timeloading;
        public bool start { get; set; }
        public int pourcentage { get; set; }
        public bool AfficherPourcentage { get; set; }

        public LoadingSprite()
        {
            start = false;
        }

        public override void LoadContent()
        {

            _font = ScreenManager.Game.Content.Load<SpriteFont>("Transition");
            _loading2D = ScreenManager.Game.Content.Load<Texture2D>("loading");
            _position = new Vector2(800 / 2 - _loading2D.Width / 2, 480 / 2 - _loading2D.Height / 2);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            if (start)
            {
                Loading();
                _timeloading += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Loading()
        {
            if (_timeloading > _timeframe)
            {
                if (_frame == _nombre_frame)
                {
                    _frame = 0;
                }
                else
                {
                    _frame++;
                }
                _timeloading = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            if (start) { LoadingDraw(); }

            ScreenManager.SpriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void LoadingDraw()
        {
            int _afficherWidth = _frame * _loadingWidth;
            Rectangle source = new Rectangle(_afficherWidth, 0, _loadingWidth, _loading2D.Height);
            ScreenManager.SpriteBatch.Draw(_loading2D, _position, source, Color.White);
            if (AfficherPourcentage)
            {
                ScreenManager.SpriteBatch.DrawString(_font, pourcentage + " %", new Vector2(220, 400), Color.White);
            }
        }
    }
}
