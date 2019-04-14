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

namespace Motux
{
    class ThemesScreen : PhoneMenuScreen 
    {
        string theme = (string)IsolatedStorageSettings.ApplicationSettings["theme"];
        string retour;

        ContentManager content;
        Languages lang = new Languages();
        Langues langue = new Langues();

        public ThemesScreen()
            : base("Theme")
        {
        }
        
        private void InitilizeLanguages()
        {
            retour = lang.AffectationLANG("Retour", langue);

            Button theme1 = new Button("Classic");
            Button theme2 = new Button("Light Color");
            Button Menu_retour = new Button(retour.ToUpper());

            theme1.Tapped += Classic;
            theme2.Tapped += Tuiles;
            Menu_retour.Tapped += Quitter;

            MenuButtons.Add(theme1);
            MenuButtons.Add(theme2);
            MenuButtons.Add(Menu_retour);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();


            base.LoadContent();
        }

        void Classic(object sender, EventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["theme"] = "classic";
            Renitialization();
        }

        void Tuiles(object sender, EventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["theme"] = "tuiles";
            Renitialization();
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
            ScreenManager.AddScreen(new MainMenuScreen());
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
    }
}
