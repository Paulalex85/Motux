using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLObject;
using GameStateManagement;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
namespace Motux
{
    class NouvellePartieScreen : PhoneMenuScreen 
    {

        Languages lang = new Languages();
        Langues langue = new Langues();

        string retour, marathon, contre_la_montre;

        public NouvellePartieScreen(string newgame)
            : base(newgame)
        {
        }

        private void InitilizeLanguages()
        {
            retour = lang.AffectationLANG("Retour", langue);
            marathon = lang.AffectationLANG("Marathon", langue);
            contre_la_montre = lang.AffectationLANG("contre_la_montre", langue);

            Button lettre8 = new Button(marathon);
            Button lettre9 = new Button(contre_la_montre);
            Button Menu_retour = new Button(retour.ToUpper());

            lettre8.Tapped += Motus8;
            lettre9.Tapped += Motus9;
            Menu_retour.Tapped += Quitter;

            MenuButtons.Add(lettre8);
            MenuButtons.Add(lettre9);
            MenuButtons.Add(Menu_retour);
        }

        public override void LoadContent()
        {
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();

            base.LoadContent();
        }

        void Motus8(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new Menu_Nombre_Lettre(InGame.Motus.TypeDePartie.Marathon));
        }
        void Motus9(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new Menu_Nombre_Lettre(InGame.Motus.TypeDePartie.Montre));
        }

        void Quitter(object sender, EventArgs e)
        {
            this.ExitScreen();
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
