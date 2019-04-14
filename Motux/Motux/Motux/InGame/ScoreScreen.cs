using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLObject;
using GameStateManagement;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Input.Touch;
using System.Threading;

namespace Motux.InGame
{
    class ScoreScreen : GameScreen
    {
        private Thread cacaThread;

        Texture2D _loading2D;

        SpriteFont font, font_titre;

        Languages lang = new Languages();
        Langues langue = new Langues();
        Multi multi;

        Motus.TypeDePartie typepartie;

        string marathon, montre, nom, points, titre, score;

        // BLBLBLB
        string nom_partie;
        int _score_joueur;

        string theme = (string)IsolatedStorageSettings.ApplicationSettings["theme"];
        string nom_joueur = (string)IsolatedStorageSettings.ApplicationSettings["player1"];

        string ApiKey = "b09272e973b4687c3cc277c61e5deb805e186e46";
        string GameId = "";

        Color theme_color;
        Color theme_color_rectangle;

        bool visualisation;

        int _frame = 0;
        int _nombre_frame = 7;
        int _loadingWidth = 64;
        float _timeframe = 200;
        float _timeloading;

        public ScoreScreen(Motus.TypeDePartie _type_partie, int score_joueur)
        {
            _score_joueur = score_joueur;
            typepartie = _type_partie;
            visualisation = false;
            ThemeColor();
            TypePartie(typepartie);
            multi = new Multi(ApiKey, GameId);
        }

        public ScoreScreen(Motus.TypeDePartie _type_partie)
        {
            typepartie = _type_partie;
            visualisation = true;
            ThemeColor();
            TypePartie(typepartie);
            multi = new Multi(ApiKey, GameId);
        }

        private void ThemeColor()
        {
            if (theme == "classic")
            {
                theme_color = Color.White;
                theme_color_rectangle = new Color(251, 118, 21);
            }
            else
            {
                theme_color = Color.Black;
                theme_color_rectangle = new Color(192, 192, 192);
            }
        }


        public override void LoadContent()
        {
            if (theme == "classic")
            {
                _loading2D = ScreenManager.Game.Content.Load<Texture2D>("loading");
            }
            else
            {
                _loading2D = ScreenManager.Game.Content.Load<Texture2D>("loading_black");
            }

            if (cacaThread == null)
            {
                cacaThread = new Thread(BackgroundLoadContent);
                cacaThread.Start();
            }

            font_titre = ScreenManager.Game.Content.Load<SpriteFont>("font_choix_nombre_lettre");
            font = ScreenManager.Game.Content.Load<SpriteFont>("font_info");

            langue = ScreenManager.Game.Content.Load<Langues>(lang.PathDictionnaire() + "LANG");
            InitilizeLanguages();
            base.LoadContent();
        }

        private void BackgroundLoadContent()
        {
            if (visualisation)
            {
                Visualisation(nom_partie);
            }
            else
            {
                SaveScore(nom_partie);
            }
        }

        private void InitilizeLanguages()
        {
            montre = lang.AffectationLANG("contre_la_montre", langue);
            marathon = lang.AffectationLANG("Marathon", langue);
            nom = lang.AffectationLANG("Joueur_String", langue);
            points = lang.AffectationLANG("Transition_Points", langue);
            score = lang.AffectationLANG("Score_String", langue);
        }

        #region UPDATE
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if (visualisation)
                {
                    this.ExitScreen();
                    ScreenManager.AddScreen(new ScoreVisualisation_TypePartie());
                }
                else
                {
                    TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new MainMenuScreen());
                }
            }
            if (!multi.HighScoreLoaded)
            {
                _timeloading += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                Loading(_timeloading);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Loading(float time)
        {
            _timeloading += time;
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

        private void TypePartie(Motus.TypeDePartie type_partie)
        {
            if (type_partie == Motus.TypeDePartie.Marathon)
            {
                nom_partie = "Marathon";
                GameId = "5173f2a1a5";
            }
            else
            {
                nom_partie = "Montre";
                GameId = "a9f0052330";
            }
        }

        private void SaveScore(string k)
        {
            multi.Submit(nom_joueur, _score_joueur);
            multi.LoadLeaderBoards();
        }

        private void Visualisation(string k)
        {
            multi.GetPlayerRank(nom_joueur);
            multi.GetPlayerScore(nom_joueur);
            multi.LoadLeaderBoards();
        }

        #endregion

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // h = hauteur d'une ligne
            int h = 40;
            int marge_haut = 100;

            ScreenManager.SpriteBatch.Begin();

            if (theme == "classic")
            {
                Rectangle r = new Rectangle(0, 0, 480, 800);
                ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, r, new Color(255, 89, 0));
            }

            if (multi == null || !multi.HighScoreLoaded)
            {
                DrawLoading();
            }
            else
            {
                for (int i = 0; i < multi.players.Count(); i++)
                {
                    if (IsPair(i))
                    {
                        Rectangle r = new Rectangle(0, marge_haut + (h * i), 480, h);
                        ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, r, theme_color_rectangle);
                    }
                    ScreenManager.SpriteBatch.DrawString(font, (i + 1).ToString(), new Vector2(30, marge_haut + (h * i)), theme_color);
                    ScreenManager.SpriteBatch.DrawString(font, multi.players[i], new Vector2(150, marge_haut + (h * i)), theme_color);
                    ScreenManager.SpriteBatch.DrawString(font, multi.score_players[i], new Vector2(375 + (80 - font.MeasureString(multi.score_players[i]).X), marge_haut + (h * i)), theme_color);
                }

                if (!visualisation)
                {
                    if (IsPair(16))
                    {
                        Rectangle r = new Rectangle(0, marge_haut + (h * 16), 480, h);
                        ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, r, theme_color_rectangle);
                    }
                    ScreenManager.SpriteBatch.DrawString(font, "", new Vector2(30, marge_haut + (h * 16)), theme_color);
                    ScreenManager.SpriteBatch.DrawString(font, nom_joueur, new Vector2(150, marge_haut + (h * 16)), theme_color);
                    ScreenManager.SpriteBatch.DrawString(font, _score_joueur.ToString(), new Vector2(375 + (80 - font.MeasureString(_score_joueur.ToString()).X), marge_haut + (h * 16)), theme_color);
                }
                else
                {
                    if (multi.best_score_player != null && multi.best_score_player.Count() > 0)
                    {
                        if (IsPair(16))
                        {
                            Rectangle r = new Rectangle(0, marge_haut + (h * 16), 480, h);
                            ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, r, theme_color_rectangle);
                        }
                        ScreenManager.SpriteBatch.DrawString(font, multi.best_score_player[1], new Vector2(30, marge_haut + (h * 16)), theme_color);
                        ScreenManager.SpriteBatch.DrawString(font, nom_joueur, new Vector2(150, marge_haut + (h * 16)), theme_color);
                        if (multi.best_score_player.Count() > 1) { ScreenManager.SpriteBatch.DrawString(font, multi.best_score_player[0], new Vector2(375 + (80 - font.MeasureString(multi.best_score_player[0]).X), marge_haut + (h * 16)), theme_color); }
                    }
                }
            }
                
            

            if (nom_partie == "Marathon")
            {
                ScreenManager.SpriteBatch.DrawString(font_titre, score + " " + marathon, new Vector2(480 /2 - font_titre.MeasureString(score + " " + marathon).X/2, 20), theme_color);
            }
            else
            {
                ScreenManager.SpriteBatch.DrawString(font_titre, score + " " + montre, new Vector2(480 / 2 - font_titre.MeasureString(score + " " + montre).X / 2, 20), theme_color);
            }
            ScreenManager.SpriteBatch.DrawString(font, nom, new Vector2(125, marge_haut - h), theme_color);
            ScreenManager.SpriteBatch.DrawString(font, points, new Vector2(375, marge_haut -h), theme_color);

            /*if (multi.players.Count() != 0)
            {
                
            }*/


            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawLoading()
        {
            int _afficherWidth = _frame * _loadingWidth;
            Rectangle source = new Rectangle(_afficherWidth, 0, _loadingWidth, _loading2D.Height);
            ScreenManager.SpriteBatch.Draw(_loading2D, new Vector2(210, 300), source, Color.White);
        }

        private bool IsPair(int i)
        {
            switch (i)
            {
                case 0: return true;
                case 1: return false;
                case 2: return true;
                case 3: return false;
                case 4: return true;
                case 5: return false;
                case 6: return true;
                case 7: return false;
                case 8: return true;
                case 9: return false;
                case 10: return true;
                case 11: return false;
                case 12: return true;
                case 13: return false;
                case 14: return true;
                case 15: return false;
                case 16: return true;
                default: return false;
            }
        }
    }
}

