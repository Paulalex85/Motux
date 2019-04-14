using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using XMLObject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Motux
{
    class ScoreVisualisation_TypePartie : PhoneMenuScreen
    {
        Languages lang = new Languages();
        Langues langue = new Langues();

        string marathon, montre, retour;

        public ScoreVisualisation_TypePartie()
            : base("")
        {
        }

        private void InitilizeLanguages()
        {
            marathon = lang.AffectationLANG("Marathon", langue);
            montre = lang.AffectationLANG("contre_la_montre", langue);
            retour = lang.AffectationLANG("Retour", langue);

            Button Marathon = new Button(marathon);
            Button Montre = new Button(montre);
            Button Retour = new Button(retour);

            Marathon.Tapped += MarathonSelected;
            Montre.Tapped += MontreSelected;
            Retour.Tapped += RetourSelected;

            MenuButtons.Add(Marathon);
            MenuButtons.Add(Montre);
            MenuButtons.Add(Retour);
        }

        public override void LoadContent()
        {
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();

            base.LoadContent();
        }

        void MarathonSelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new InGame.ScoreScreen(InGame.Motus.TypeDePartie.Marathon));
        }

        void MontreSelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new InGame.ScoreScreen(InGame.Motus.TypeDePartie.Montre));
        }

        void RetourSelected(object sender, EventArgs e)
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
