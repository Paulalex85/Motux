using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace Motux.InGame
{
    class OptionFinPartie : GameScreen
    {
        Motus.TypeDePartie _type_partie;

        SpriteFont _font;
        Texture2D _rejouer;
        Texture2D _menu;

        string _rejouer_string, _menu_string;

        int _position_Y;

        Vector2 _position1;
        Vector2 _position2;
        Vector2 _size1;
        Vector2 _size2;

        public float Alpha = 0;


        public OptionFinPartie(int position_Y, SpriteFont font, Texture2D texture_lambda, Texture2D menu, string rejouer_string, string menu_string, Motus.TypeDePartie type_partie)
        {
            _type_partie = type_partie;
            _font = font;
            _rejouer = texture_lambda;
            _menu = menu;
            _position_Y = position_Y;
            _rejouer_string = rejouer_string;
            _menu_string = menu_string;

            _position1 = new Vector2(120, _position_Y);
            _position2 = new Vector2(300, _position_Y);
            _size1 = new Vector2(_font.MeasureString(rejouer_string).X+30, _rejouer.Height + 100);
            _size2 = new Vector2(menu.Width+50, _rejouer.Height + 100);
        }

        public bool HandleTapRejouer(Vector2 tap)
        {
            if (tap.X >= _position1.X &&
                tap.Y >= _position1.Y &&
                tap.X <= _position1.X + _size1.X &&
                tap.Y <= _position1.Y + _size1.Y)
            {
                return true;
            }

            return false;
        }

        public bool HandleTapContinuer(Vector2 tap)
        {
            if (tap.X >= _position1.X &&
                tap.Y >= _position1.Y &&
                tap.X <= _position1.X + _size1.X &&
                tap.Y <= _position1.Y + _size1.Y)
            {
                return true;
            }

            return false;
        }

        public bool HandleTapMenu(Vector2 tap)
        {
            if (tap.X >= _position2.X &&
                    tap.Y >= _position2.Y &&
                    tap.X <= _position2.X + _size2.X &&
                    tap.Y <= _position2.Y + _size2.Y)
            {
                return true;
            }
            return false;
        }

        public bool HandleTapScore(Vector2 tap)
        {
            if (tap.X >= _position2.X &&
                    tap.Y >= _position2.Y &&
                    tap.X <= _position2.X + _size2.X &&
                    tap.Y <= _position2.Y + _size2.Y)
            {
                return true;
            }
            return false;
        }

        public void DrawIcone(GameScreen screen)
        {
            SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;

            IconeRecommencer(spriteBatch);
            IconeMenu(spriteBatch);
        }

        private void IconeRecommencer(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_rejouer, _position1, Color.White * Alpha);
            spritebatch.DrawString(_font, _rejouer_string, new Vector2(_position1.X + _rejouer.Width/2- _font.MeasureString(_rejouer_string).X / 2, _position1.Y + _rejouer.Height + 20), Color.White * Alpha);
        }

        private void IconeMenu(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_menu, _position2, Color.White * Alpha);
            spritebatch.DrawString(_font, _menu_string, new Vector2(_position2.X + _menu.Width / 2- _font.MeasureString(_menu_string).X / 2, _position2.Y + _menu.Height + 20), Color.White * Alpha);
        }
    }
}
