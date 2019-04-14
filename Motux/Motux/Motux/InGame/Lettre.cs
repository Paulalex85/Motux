using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;


namespace Motux.InGame
{
    class Lettre
    {
        public enum ValidationEtat
        {
            Rien,
            Dedans,
            Bon,
            Animation
        }
        private SpriteFont _font;
        private Vector2 _position;
        public string _lettre { get; set; }
        public Vector2 _size = Vector2.Zero;
        public int _border = 4;
        public Color _borderColor = new Color(255, 255, 255);
        public Color FillColor = new Color(115, 197, 255);
        public Color TextColor = Color.White;
        public Color _color_si_bon = new Color(126, 215, 1);
        public Color _color_si_dedans = new Color(254, 217, 6);
        public Color _color_animation = new Color(103,232,14);

        private Texture2D _texture { get; set; }
        private Texture2D _texture_lettre_placee { get; set; }
        private Texture2D _texture_bon { get; set; }
        private Texture2D _texture_anim { get; set; }

        public ValidationEtat _etat = ValidationEtat.Rien;
        public bool _effacable = true;


        public Lettre(SpriteFont font, Vector2 position, Vector2 size)
        {
            _position = position;
            _font = font;
            _size = size;
            _lettre = "";
        }

        public Lettre(SpriteFont font, Vector2 position, Texture2D texture, Texture2D texture_lettre_placee, Texture2D texture_bon , Texture2D texture_anim)
        {
            _texture = texture;
            _texture_lettre_placee = texture_lettre_placee;
            _texture_bon = texture_bon;
            _texture_anim = texture_anim;
            _position = position;
            _font = font;

            _lettre = "";
        }


        public void DrawTexture(GameScreen screen)
        {
            SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;

            if (_etat == ValidationEtat.Rien)
            {
                spriteBatch.Draw(_texture, _position, Color.White);
            }
            else if (_etat == ValidationEtat.Dedans)
            {
                Vector2 position_placee = new Vector2(_position.X + (_texture.Width / 2 - _texture_lettre_placee.Width / 2), _position.Y + (_texture.Height / 2 - _texture_lettre_placee.Height / 2));

                spriteBatch.Draw(_texture, _position, Color.White);
                spriteBatch.Draw(_texture_lettre_placee, position_placee, Color.White);
            }
            else if (_etat == ValidationEtat.Animation)
            {
                spriteBatch.Draw(_texture_anim, _position, Color.White);
            }
            else
            {
                spriteBatch.Draw(_texture_bon, _position, Color.White);
            }


            //Draw Text
            Vector2 textSize = _font.MeasureString(_lettre);
            Vector2 textPosition = new Vector2(_position.X + _texture.Width / 2 - textSize.X / 2, _position.Y +  _texture.Height / 2 - textSize.Y / 2);
            textPosition.X = (int)textPosition.X;
            textPosition.Y = (int)textPosition.Y;
            spriteBatch.DrawString(_font, _lettre, textPosition, TextColor);


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
                (int)_size.X,
                (int)_size.Y);

            // Fill the button
            if (_etat == ValidationEtat.Rien)
            {
                spriteBatch.Draw(blank, r, FillColor);
            }
            else if (_etat == ValidationEtat.Dedans)
            {
                spriteBatch.Draw(blank, r, _color_si_dedans);
            }
            else if (_etat == ValidationEtat.Animation)
            {
                spriteBatch.Draw(blank, r, _color_animation);
            }
            else
            {
                spriteBatch.Draw(blank, r, _color_si_bon);
            }


            // Draw the border
            /*spriteBatch.Draw(
                blank,
                new Rectangle(r.Left, r.Top, r.Width, _border),
                _borderColor);
            spriteBatch.Draw(
                blank,
                new Rectangle(r.Left, r.Top, _border, r.Height),
                _borderColor);
            spriteBatch.Draw(
                blank,
                new Rectangle(r.Right - _border, r.Top, _border, r.Height),
                _borderColor);
            spriteBatch.Draw(
                blank,
                new Rectangle(r.Left, r.Bottom - _border, r.Width, _border),
                _borderColor);*/

            // Draw the text centered in the button
            Vector2 textSize = _font.MeasureString(_lettre);
            Vector2 textPosition = new Vector2(r.Center.X, r.Center.Y) - textSize / 2f;
            textPosition.X = (int)textPosition.X;
            textPosition.Y = (int)textPosition.Y;
            spriteBatch.DrawString(_font, _lettre, textPosition, TextColor);
        }

    }
}
