using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;
using Microsoft.Devices;
using System.IO.IsolatedStorage;

namespace Motux.InGame
{
    class Keyboard_Lettre
    {
        bool vibrate = (bool)IsolatedStorageSettings.ApplicationSettings["vibrate"];

        public Motux.InGame.Keyboard.TypeLettre _type;
        public Texture2D _icone;
        public string _lettre = "";
        public Vector2 _position = Vector2.Zero;
        public Vector2 size = new Vector2(40, 60);
        public Color _color = new Color(100, 100, 100);
        private Color _color_when_tap = new Color(0, 174, 196);
        public Color _textColor = Color.White;
        public event EventHandler<EventArgs> Tapped;
        SpriteFont font;

        //Timer tap
        public bool _tap_bool = false;
        public float _tap_timer_max_anim = 70f;
        public float _tap_timer = 0f;

        public float Alpha = 1;

        public Keyboard_Lettre(SpriteFont _font,string lettre, Vector2 position,Motux.InGame.Keyboard.TypeLettre type)
        {
            _type = type;
            font = _font;
            _lettre = lettre;
            _position = position;
        }

        public Keyboard_Lettre(Vector2 _size,Vector2 position, Texture2D icone, Motux.InGame.Keyboard.TypeLettre type)
        {
            _type = type;
            size = _size;
            _position = position;
            _icone = icone;
        }

        protected virtual void OnTapped()
        {
            if (Tapped != null)
                Tapped(this, EventArgs.Empty);

            _tap_bool = true;
            _tap_timer = 0f;

            if (vibrate)
            {
                VibrateController vc = VibrateController.Default;
                vc.Start(TimeSpan.FromMilliseconds(50));
            }

        }

        public bool HandleTap(Vector2 tap)
        {
            if (tap.X >= _position.X &&
                tap.Y >= _position.Y &&
                tap.X <= _position.X + size.X &&
                tap.Y <= _position.Y + size.Y)
            {
                OnTapped();
                return true;
            }

            return false;
        }

        public void DrawLettre(GameScreen screen)
        {
            // Grab some common items from the ScreenManager
            SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;
            Texture2D blank = screen.ScreenManager.BlankTexture;

            // Compute the button's rectangle
            Rectangle r = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)size.X,
                (int)size.Y);

            // Fill the button
            if (_tap_bool)
            {
                spriteBatch.Draw(blank, r, _color_when_tap);
            }
            else
            {
                spriteBatch.Draw(blank, r, _color * Alpha);
            }

            if (_lettre != "")
            {
                // Draw the text centered in the button
                Vector2 textSize = font.MeasureString(_lettre);
                Vector2 textPosition = new Vector2(r.Center.X, r.Center.Y) - textSize / 2f;
                textPosition.X = (int)textPosition.X;
                textPosition.Y = (int)textPosition.Y;
                spriteBatch.DrawString(font, _lettre, textPosition, _textColor * Alpha);
            }
            else
            {
                Vector2 textureSize = new Vector2(_icone.Width, _icone.Height);
                Vector2 texturePosition = new Vector2(r.Center.X, r.Center.Y) - textureSize / 2;
                texturePosition.X = (int)texturePosition.X;
                texturePosition.Y = (int)texturePosition.Y;
                spriteBatch.Draw(_icone, texturePosition, Color.White * Alpha);
            }
        }
    }
}
