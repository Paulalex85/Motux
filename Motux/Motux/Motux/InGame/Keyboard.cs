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
    class Keyboard :GameScreen
    {
        public enum TypeLettre
        {
            Lettre,
            Effacer,
            Valider
        }

        public List<Keyboard_Lettre> liste_lettre = new List<Keyboard_Lettre>();
        int _position_Y;
        SpriteFont _font_keyboard;
        Texture2D check;
        Texture2D clear;

        public bool _tapped = false;
        public string _letter_tapped = "";
        public TypeLettre _type_tapped;

        public Keyboard(int position_Y, SpriteFont font, Texture2D _check, Texture2D _clear)
        {
            check = _check;
            clear = _clear;
            _font_keyboard = font;
            _position_Y = position_Y;
            Lettre_Initialize();
        }

        private void Lettre_Initialize()
        {
            List<string> rangee_fr = new List<string> { "A", "Z", "E", "R", "T", "Y", "U", "I", "O", "P", "Q", "S", "D", "F", "G", "H", "J", "K", "L", "M", "W", "X", "C", "V", "B", "N" };
            for (int i = 0; i < 10; i++)
            {
                liste_lettre.Add(new Keyboard_Lettre(_font_keyboard, rangee_fr[i], new Vector2(15 + (45 * i), (_position_Y + 20)),TypeLettre.Lettre));
            }
            for (int i = 10; i < 20; i++)
            {
                liste_lettre.Add(new Keyboard_Lettre(_font_keyboard, rangee_fr[i], new Vector2(15 + (45 * (i - 10)), (_position_Y + 85)), TypeLettre.Lettre));
            }
            for (int i = 20; i < 26; i++)
            {
                liste_lettre.Add(new Keyboard_Lettre(_font_keyboard, rangee_fr[i], new Vector2(15 + (45 * (i - 18)), (_position_Y + 150)), TypeLettre.Lettre));
            }

            liste_lettre.Add(new Keyboard_Lettre(new Vector2(85, 60), new Vector2(15, _position_Y + 150),check,TypeLettre.Valider));
            liste_lettre.Add(new Keyboard_Lettre(new Vector2(85, 60), new Vector2(15 + 45 * 8, _position_Y + 150),clear,TypeLettre.Effacer));

        }
    }
}
