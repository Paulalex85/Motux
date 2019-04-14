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

namespace Motux
{
    class Menu_Nombre_Lettre : PhoneMenuScreen
    {
        Texture2D gauche;
        Texture2D droite;

        ContentManager content;
        Languages lang = new Languages();
        Langues langue = new Langues();

        SpriteFont font_fat;
        SpriteFont font_MEGAFAT;

        Vector2 gauche_position;
        Vector2 droite_position;
        Vector2 position_difficulte;
        Vector2 position_text1;
        Vector2 position_text2;

        string lettre, retour, valider, facile, difficile, aleatoire, mots_facile;
        bool facile_bool = true;

        string theme = (string)IsolatedStorageSettings.ApplicationSettings["theme"];

        Color couleur_text;

        InGame.Motus.TypeDePartie _typepartie;

        public Menu_Nombre_Lettre(InGame.Motus.TypeDePartie typepartie)
            : base("")
        {
            _typepartie = typepartie;
        }

        private void InitilizeLanguages()
        {
            lettre = lang.AffectationLANG("Lettres_mini_String", langue);
            retour = lang.AffectationLANG("Retour", langue);
            valider = lang.AffectationLANG("BOUTON_Valider", langue);
            facile = lang.AffectationLANG("Facile", langue);
            difficile = lang.AffectationLANG("Difficile", langue);
            aleatoire = lang.AffectationLANG("Aleatoire", langue);
            mots_facile = lang.AffectationLANG("mots_facile", langue);

            facile = facile.ToUpper();
            difficile = difficile.ToUpper();

            Vector2 _size = new Vector2(200, 60);
            Vector2 _position = new Vector2(30,600);

            gauche_position = new Vector2(80, 500);
            droite_position = new Vector2(320, 500);
            position_difficulte = new Vector2(240, 200);
            position_text1 = new Vector2(30, 350);
            position_text2 = new Vector2(30, 400);

            Button Retour = new Button(retour.ToUpper(),_size,_position,new Color(255,65,67),new Color(255,3,2),Color.White);
            Button Valider = new Button(valider.ToUpper(), _size, new Vector2(_position.X + _size.X + 20, _position.Y), new Color(106, 181, 1), new Color(126, 215, 2), Color.White);

            Retour.Tapped += Quitter;
            Valider.Tapped += ValiderSelected;

            MenuButtons.Add(Retour);
            MenuButtons.Add(Valider);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            font_fat = ScreenManager.Game.Content.Load<SpriteFont>("font_choix_nombre_lettre");
            font_MEGAFAT = ScreenManager.Game.Content.Load<SpriteFont>("font_choix_nombre_lettre_fat");
            gauche = ScreenManager.Game.Content.Load<Texture2D>("Icone/fleche_gauche_noir");
            droite = ScreenManager.Game.Content.Load<Texture2D>("Icone/fleche_droite_noir");
            InitilizeLanguages();

            if (theme == "classic")
            {
                couleur_text = Color.Black;
            }
            else if (theme == "tuiles")
            {
                couleur_text = Color.Black;
            }

            base.LoadContent();
        }

        void Quitter(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen());
        }

        void ValiderSelected(object sender, EventArgs e)
        {
            TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new InGame.Motus(facile_bool, _typepartie));
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen());
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputState input)
        {
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    if (gesture.Position.X < gauche_position.X + gauche.Width &&
                        gesture.Position.X > gauche_position.X &&
                        gesture.Position.Y < gauche_position.Y + gauche.Height &&
                        gesture.Position.Y > gauche_position.Y &&
                        facile_bool == false)
                    {
                        facile_bool = true;
                    }
                    if (gesture.Position.X < droite_position.X + droite.Width &&
                        gesture.Position.X > droite_position.X &&
                        gesture.Position.Y < droite_position.Y + droite.Height &&
                        gesture.Position.Y > droite_position.Y &&
                        facile_bool == true)
                    {
                        facile_bool = false;
                    }
                }
            }
            
            base.HandleInput(input);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            if (facile_bool)
            {
                FacileDraw();
            }
            else
            {
                DifficileDraw();
            }

            ScreenManager.SpriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void FacileDraw()
        {
            if (lang.lang == "fr")
            {
                ScreenManager.SpriteBatch.DrawString(font_MEGAFAT, facile, new Vector2(position_difficulte.X - font_MEGAFAT.MeasureString(facile).X / 2, position_difficulte.Y), Color.Black * TransitionAlpha);
                ScreenManager.SpriteBatch.DrawString(font_fat, "5 - 6 - 7 " + lettre + " " + aleatoire, position_text1, Color.Black * TransitionAlpha);
                ScreenManager.SpriteBatch.DrawString(font_fat, mots_facile, position_text2, Color.Black * TransitionAlpha);
                ScreenManager.SpriteBatch.Draw(droite, droite_position, Color.White * TransitionAlpha);
            }
            else
            {
                ScreenManager.SpriteBatch.DrawString(font_MEGAFAT, facile, new Vector2(position_difficulte.X - font_MEGAFAT.MeasureString(facile).X / 2, position_difficulte.Y), Color.Black * TransitionAlpha);
                ScreenManager.SpriteBatch.DrawString(font_fat, "5 - 6 - 7 " + lettre + " " + aleatoire, position_text1, Color.Black * TransitionAlpha);
                ScreenManager.SpriteBatch.Draw(droite, droite_position, Color.White * TransitionAlpha);
            }
        }

        private void DifficileDraw()
        {
            ScreenManager.SpriteBatch.DrawString(font_MEGAFAT, difficile, new Vector2(position_difficulte.X - font_MEGAFAT.MeasureString(difficile).X / 2, position_difficulte.Y), Color.Black * TransitionAlpha);
            ScreenManager.SpriteBatch.DrawString(font_fat, "8 - 9 - 10 " + lettre + " " + aleatoire, position_text1, Color.Black * TransitionAlpha);
            
            ScreenManager.SpriteBatch.Draw(gauche, gauche_position, Color.White * TransitionAlpha);
        }
    }
}
