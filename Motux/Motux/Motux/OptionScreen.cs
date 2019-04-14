using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLObject;
using GameStateManagement;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Phone.Tasks;

namespace Motux
{
    class OptionScreen : PhoneMenuScreen
    {
        SpriteFont menufont;

        Texture2D check;
        Texture2D uncheck;

        Vector2 pos1;
        Vector2 pos2;

        int align_x1 = 20;
        int align_x2 = 210;
        int y1 = 100;
        int y2 = 200;
        int y3 = 300;
        int y4 = 400;
        int y5 = 500;
        int y6 = 600;

        string theme = (string)IsolatedStorageSettings.ApplicationSettings["theme"];
        bool vibrate = (bool)IsolatedStorageSettings.ApplicationSettings["vibrate"];
        string _nom_joueur = (string)IsolatedStorageSettings.ApplicationSettings["player1"];
        string valider, changer_nom, lettre_max, nom, voir_score, aide, theme_string, vibration_string, rate;

        ContentManager content;
        Languages lang = new Languages();
        Langues langue = new Langues();

        public OptionScreen()
            : base("Options")
        {
            pos1 = new Vector2(align_x1, y4);
            pos2 = new Vector2(align_x1, y5);
        }
        
        private void InitilizeLanguages()
        {
            valider = lang.AffectationLANG("BOUTON_Valider", langue);
            changer_nom = lang.AffectationLANG("Menu_changer_nom", langue);
            lettre_max = lang.AffectationLANG("Menu_changer_nom_lettres_max", langue);
            nom = lang.AffectationLANG("Nom", langue);
            theme_string = lang.AffectationLANG("Themes", langue);
            vibration_string = lang.AffectationLANG("Vibration", langue);
            voir_score = lang.AffectationLANG("Score_String", langue);
            aide = lang.AffectationLANG("Aide", langue);
            rate = lang.AffectationLANG("Noter", langue);

            Button theme1;
            if (theme == "classic")
            {
                theme1 = new Button("Classic", new Vector2(align_x2,y1));
            }
            else
            {
                theme1 = new Button("Tuiles", new Vector2(align_x2, y1));
            }

            Button nom_joueur = new Button(_nom_joueur, new Vector2(align_x2,y2));
            Button _voir_score = new Button(voir_score, new Vector2(align_x2, y4));
            Button AideBouton = new Button(aide, new Vector2(align_x2, y5));
            Button RateBouton = new Button(rate, new Vector2(align_x2, y6));

            BooleanButton vibrationButton = new BooleanButton(vibrate, new Vector2(align_x2,y3));

            Vector2 _size = new Vector2(200, 60);
            Vector2 _position = new Vector2(150, 700);
            Button Retour = new Button(valider.ToUpper(), _size, _position, new Color(106, 181, 1), new Color(126, 215, 2), Color.White);
            

            theme1.Tapped += ThemeVoid;
            nom_joueur.Tapped += ChangerNom;
            vibrationButton.Tapped += ChangerVibrate;
            _voir_score.Tapped += VoirScore;
            AideBouton.Tapped += GoAide;
            RateBouton.Tapped += Rating;
            Retour.Tapped += Quitter;

            MenuButtons.Add(theme1);
            MenuButtons.Add(nom_joueur);
            MenuButtons.Add(vibrationButton);
            MenuButtons.Add(_voir_score);
            MenuButtons.Add(AideBouton);
            MenuButtons.Add(RateBouton);
            MenuButtons.Add(Retour);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            menufont = ScreenManager.Game.Content.Load<SpriteFont>("menufont");
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();

            check = ScreenManager.Game.Content.Load<Texture2D>("Icone/checkSquare");
            uncheck = ScreenManager.Game.Content.Load<Texture2D>("Icone/uncheckSquare");

            base.LoadContent();
        }

        void ThemeVoid(object sender, EventArgs e)
        {
            if (theme == "classic")
            {
                IsolatedStorageSettings.ApplicationSettings["theme"] = "tuiles";
                Renitialization();
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings["theme"] = "classic";
                Renitialization();
            }
        }

        void ChangerNom(object sender, EventArgs e)
        {
            Guide.BeginShowKeyboardInput(PlayerIndex.One, changer_nom, lettre_max, _nom_joueur, new AsyncCallback(gotText), null);
        }

        void Rating(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }

        private void gotText(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                string caca = Guide.EndShowKeyboardInput(result);
                if (caca != null && caca.Count() < 13)
                {
                    IsolatedStorageSettings.ApplicationSettings["player1"] = Guide.EndShowKeyboardInput(result);
                }
            }
            this.ExitScreen();
            ScreenManager.AddScreen(new OptionScreen());
        }

        void ChangerVibrate(object sender, EventArgs e)
        {
            if (vibrate)
            {
                IsolatedStorageSettings.ApplicationSettings["vibrate"] = false;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings["vibrate"] = true;
            }
        }

        void VoirScore(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new ScoreVisualisation_TypePartie());
        }

        void GoAide(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new Aide());
        }

        void Quitter(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen());
        }

        void Renitialization()
        {
            foreach (var caca in ScreenManager.GetScreens())
            {
                caca.ExitScreen();
            }
            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(new OptionScreen());
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

        public override void Draw(GameTime gameTime)
        {
            int align = 25;
            ScreenManager.SpriteBatch.Begin();

            ScreenManager.SpriteBatch.DrawString(menufont, theme_string.ToUpper(), new Vector2(align_x1, y1 + align), Color.Black * TransitionAlpha);
            ScreenManager.SpriteBatch.DrawString(menufont, nom.ToUpper(), new Vector2(align_x1, y2 + align), Color.Black * TransitionAlpha);
            ScreenManager.SpriteBatch.DrawString(menufont, vibration_string.ToUpper(), new Vector2(align_x1, y3 + align), Color.Black * TransitionAlpha);

            ScreenManager.SpriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
