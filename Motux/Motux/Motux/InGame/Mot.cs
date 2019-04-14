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
using System.IO.IsolatedStorage;

namespace Motux.InGame
{
    class Mot
    {
        private int _lettre_dimension;
        private int _nombre_de_lettres;
        private int _position_Y;
        public List<Lettre> _lettres = new List<Lettre>();
        public bool _valider { get; set; }
        public bool _afficher { get; set; }
        public int loop = 0;

        //Animation fin
        float _timer = 0f;
        int position_mot = 0;

        SpriteFont _font;

        private Texture2D _texture { get; set; }
        private Texture2D _texture_lettre_placee { get; set; }
        private Texture2D _texture_bon { get; set; }
        private Texture2D _texture_anim { get; set; }

        string theme = (string)IsolatedStorageSettings.ApplicationSettings["theme"];

        public Mot(int nombre_lettres, int position_Y, SpriteFont font)
        {
            _valider = false;
            _nombre_de_lettres = nombre_lettres;
            _font = font;
            _position_Y = position_Y;
            _afficher = false;

            _lettre_dimension = TuilesThemeDimension(_nombre_de_lettres);

            switch (_nombre_de_lettres)
            {
                case 5: Si5Lettres(); break;
                case 6: Si6Lettres(); break;
                case 7: Si7Lettres(); break;
                case 8: Si8Lettres(); break;
                case 9: Si9Lettres(); break;
                case 10: Si10Lettres(); break;
                default: Si8Lettres(); break;
            }
        }

        public Mot(int nombre_lettres, int position_Y, SpriteFont font, Texture2D texture, Texture2D texture_lettre_placee, Texture2D texture_bon, Texture2D texture_anim)
        {
            _texture = texture;
            _texture_lettre_placee = texture_lettre_placee;
            _texture_bon = texture_bon;
            _texture_anim = texture_anim;

            _afficher = false;
            _valider = false;
            _nombre_de_lettres = nombre_lettres;
            _font = font;
            _position_Y = position_Y;

            switch (_nombre_de_lettres)
            {
                case 5: Si5Lettres(); break;
                case 6: Si6Lettres(); break;
                case 7: Si7Lettres(); break;
                case 8: Si8Lettres(); break;
                case 9: Si9Lettres(); break;
                case 10: Si10Lettres(); break;
                default: Si8Lettres(); break;
            }
        }

        private int TuilesThemeDimension(int nombre_de_lettre)
        {
            switch (nombre_de_lettre)
            {
                case 10: return (int)((480 / 10) - 2);
                case 9: return (int)((480 / 9) - 2);
                case 8: return (int)((480 / 8) - 2);
                case 7: return (int)((480 / 7) - 2);
                case 6: return (int)((480 / 6) - 2);
                case 5: return (int)((480 / 6) - 2);
                default: return (int)((480 / 8) - 2);
            }
        }

        public bool PeuxAddLettreMot(string lettre)
        {
            bool pd = false;
            foreach (var caca in _lettres)
            {
                if (caca._lettre == "")
                {
                    pd = true;
                    break;
                }
            }
            return pd;
        }

        public void AnimationMotJuste(float timer)
        {
            float _timer_max = 70f;
            _timer += timer;

            if (_timer_max < _timer)
            {
                _timer = 0;
                _lettres.ElementAt(position_mot)._etat = Lettre.ValidationEtat.Animation;
                if (position_mot == 0 && loop == 0)
                {
                }
                else if (position_mot == 0 && loop != 0)
                {
                    _lettres.ElementAt(_nombre_de_lettres - 1)._etat = Lettre.ValidationEtat.Bon;
                }
                else
                {
                    _lettres.ElementAt(position_mot - 1)._etat = Lettre.ValidationEtat.Bon;
                }

                if (position_mot == _nombre_de_lettres - 1)
                {
                    loop++;
                    position_mot = 0;
                }
                else
                {
                    position_mot++;
                }
                if (loop == 3)
                {
                    _lettres.ElementAt(_nombre_de_lettres - 1)._etat = Lettre.ValidationEtat.Bon;
                }


            }

        }

        public void AddLettreMot(string lettre)
        {
            foreach (var caca in _lettres)
            {
                if (caca._lettre == "")
                {
                    caca._lettre = lettre; break;
                }
            }
        }
        private void Si5Lettres()
        {
            
            for (int i = 0; i <= 4; i++)
            {
                if (theme == "classic")
                {
                    int centrage = 480 / 2 - ((_texture.Width * 5) / 2);
                    _lettres.Add(new Lettre(_font, new Vector2((_texture.Width * i) + centrage, _position_Y), _texture, _texture_lettre_placee, _texture_bon, _texture_anim));
                }
                else if (theme == "tuiles")
                {
                    int centrage = 480 / 2 - (((_lettre_dimension + 2) * 5) / 2);
                    _lettres.Add(new Lettre(_font, new Vector2(((_lettre_dimension + 2) * i) + centrage, _position_Y), new Vector2(_lettre_dimension, _lettre_dimension)));
                }
            }
        }
        private void Si6Lettres()
        {
            for (int i = 0; i <= 5; i++)
            {
                if (theme == "classic")
                {
                    _lettres.Add(new Lettre(_font, new Vector2((_texture.Width * i), _position_Y), _texture, _texture_lettre_placee, _texture_bon, _texture_anim));
                }
                else if (theme == "tuiles")
                {
                    _lettres.Add(new Lettre(_font, new Vector2((_lettre_dimension + 2) * i, _position_Y), new Vector2(_lettre_dimension, _lettre_dimension)));
                }
            }
        }

        private void Si7Lettres()
        {
            for (int i = 0; i <= 6; i++)
            {
                if (theme == "classic")
                {
                    _lettres.Add(new Lettre(_font, new Vector2((_texture.Width * i), _position_Y), _texture, _texture_lettre_placee, _texture_bon, _texture_anim));
                }
                else if(theme =="tuiles")
                {
                    _lettres.Add(new Lettre(_font, new Vector2((_lettre_dimension + 2) * i, _position_Y), new Vector2(_lettre_dimension, _lettre_dimension)));
                }
            }
        }
        private void Si8Lettres()
        {
            for (int i = 0; i <= 7; i++)
            {
                if (theme == "classic")
                {
                    _lettres.Add(new Lettre(_font, new Vector2((_texture.Width * i), _position_Y), _texture, _texture_lettre_placee, _texture_bon, _texture_anim));
                }
                else if (theme == "tuiles")
                {
                    _lettres.Add(new Lettre(_font, new Vector2((_lettre_dimension + 2) * i, _position_Y), new Vector2(_lettre_dimension, _lettre_dimension)));
                }
            }
        }
        private void Si9Lettres()
        {
            for (int i = 0; i <= 8; i++)
            {
                if (theme == "classic")
                {
                    _lettres.Add(new Lettre(_font, new Vector2((_texture.Width * i), _position_Y), _texture, _texture_lettre_placee, _texture_bon, _texture_anim));
                }
                else if (theme == "tuiles")
                {
                    _lettres.Add(new Lettre(_font, new Vector2((_lettre_dimension + 2) * i, _position_Y), new Vector2(_lettre_dimension, _lettre_dimension)));
                }
            }
        }
        private void Si10Lettres()
        {
            for (int i = 0; i <= 9; i++)
            {
                if (theme == "classic")
                {
                    _lettres.Add(new Lettre(_font, new Vector2((_texture.Width * i), _position_Y), _texture, _texture_lettre_placee, _texture_bon, _texture_anim));
                }
                else if (theme == "tuiles")
                {
                    _lettres.Add(new Lettre(_font, new Vector2((_lettre_dimension + 2) * i, _position_Y), new Vector2(_lettre_dimension, _lettre_dimension)));
                }
            }
        }
    }
}
