using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XMLObject;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Phone.Marketplace;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Motux
{
    class MainMenuScreen : PhoneMenuScreen
    {
        ContentManager content;

        string entrainement, acheter, jouer, quitter, newgame, joueur, nombre_lettre, option;

        Languages lang = new Languages();
        Langues langue = new Langues();

        bool trial;

        public MainMenuScreen()
            : base()
        {
            trial = Guide.IsTrialMode;

        }

        private void InitilizeLanguages()
        {
            if (!trial)
            {
                entrainement = lang.AffectationLANG("Entrainement", langue);
                jouer = lang.AffectationLANG("Jouer", langue);
                quitter = lang.AffectationLANG("Quitter", langue);
                newgame = lang.AffectationLANG("NewGame", langue);
                joueur = lang.AffectationLANG("Joueur_String", langue);
                nombre_lettre = lang.AffectationLANG("Nombre_de_lettres", langue);
                option = lang.AffectationLANG("Options", langue);

                Button startRapideGameMenuEntry = new Button(entrainement);
                Button startGameMenuEntry = new Button(jouer);
                Button OptionEntry = new Button(option);
                Button exitMenuEntry = new Button(quitter.ToUpper());

                startRapideGameMenuEntry.Tapped += StartRapideGameMenuEntrySelected;
                startGameMenuEntry.Tapped += StartGameMenuEntrySelected;
                OptionEntry.Tapped += OptionEntrySelected;
                exitMenuEntry.Tapped += Quitter;

                MenuButtons.Add(startRapideGameMenuEntry);
                MenuButtons.Add(startGameMenuEntry);
                MenuButtons.Add(OptionEntry);
                MenuButtons.Add(exitMenuEntry);
            }
            else
            {
                jouer = lang.AffectationLANG("Jouer", langue);
                option = lang.AffectationLANG("Options", langue);
                acheter = lang.AffectationLANG("Acheter", langue);

                Button startGameMenuEntry = new Button(jouer);
                Button OptionEntry = new Button(option);
                Button Acheter = new Button(acheter);

                startGameMenuEntry.Tapped += JouerTrial;
                OptionEntry.Tapped += OptionEntrySelected;
                Acheter.Tapped += AcheterSelected;

                MenuButtons.Add(startGameMenuEntry);
                MenuButtons.Add(OptionEntry);
                MenuButtons.Add(Acheter);
            }
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            //message = ScreenManager.Game.Content.Load<SpriteFont>("Transition");
            InitilizeLanguages();

            base.LoadContent();
        }

        void OptionEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new OptionScreen());
        }

        void StartRapideGameMenuEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new Menu_Nombre_Lettre(InGame.Motus.TypeDePartie.Simple));
        }

        void StartGameMenuEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new NouvellePartieScreen(newgame));
        }

        void Quitter(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        void JouerTrial(object sender, EventArgs e)
        {
            TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new InGame.Motus(true, InGame.Motus.TypeDePartie.Simple));
        }
        void AcheterSelected(object sender, EventArgs e)
        {
            Guide.ShowMarketplace(PlayerIndex.One);
        }



        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.OnCancel(PlayerIndex.One);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.Game.Exit();
        }


        
    }
}
