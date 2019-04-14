using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.GamerServices;
using XMLObject;
using Microsoft.Devices;

namespace Motux
{
    class Aide : GameScreen
    {
        Texture2D plaque_juste;
        Texture2D plaque_cercle;
        Texture2D exemple_faux;

        SpriteFont font_titre;
        SpriteFont font_text;
        SpriteFont font_fat;

        bool vibrate = (bool)IsolatedStorageSettings.ApplicationSettings["vibrate"];

        int page;
        Rectangle r = new Rectangle(0, 0, 480, 800);

        Vector2 position_suivant = new Vector2(300, 720);

        string suivant, aide, terminer;

        Languages lang = new Languages();
        Langues langue = new Langues();

        MouseState state = Mouse.GetState();
        MouseState previousState;

        string P1, P2, P21, P3, P31, P4, P41, P5, P51, P6, P61, P7, P71, P8, P81, P9, P91, P10, P101, P11, P12, P121, P13, P131;

        public Aide()
        {
            page = 1;
        }

        public override void LoadContent()
        {
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();
            font_fat = ScreenManager.Game.Content.Load<SpriteFont>("font_choix_nombre_lettre_fat");
            font_titre = ScreenManager.Game.Content.Load<SpriteFont>("font_choix_nombre_lettre");
            font_text = ScreenManager.Game.Content.Load<SpriteFont>("font_aide");

            plaque_juste = ScreenManager.Game.Content.Load<Texture2D>("Themes/10/plaque_juste");
            plaque_cercle = ScreenManager.Game.Content.Load<Texture2D>("Themes/10/plaque_cercle");
            exemple_faux = ScreenManager.Game.Content.Load<Texture2D>("Icone/aide");

            base.LoadContent();
        }

        private void InitilizeLanguages()
        {
            suivant = lang.AffectationLANG("Suivant", langue);
            aide = lang.AffectationLANG("Aide", langue);
            terminer = lang.AffectationLANG("Terminer",langue);

            if (lang.lang == "fr")
            {
                P1 = "Dans Motux vous devez trouver un mot";
                P2 = "Pour cela à chaque fois la première ";
                P21 = "lettre et un autre lettre est donnée";
                P3 = "Le joueur a 6 essais pour trouver le";
                P31 =  "mot caché";
                P4 = "A chaque validation, les lettres donnent";
                P41 = "une indication";
                P5 = "Si c'est vert ou rouge";
                P51 = "c'est que la lettre est juste";
                P6 = "Si c'est jaune c'est que la lettre"; 
                P61 = "est dans le mot mais mal placée";
                P7 = "Cependant si le mot proposé est";
                P71 = "faux alors il n'y a pas d'indication donnée";
                P8 = "Il y a 2 modes de jeu : Marathon";
                P81 = "Contre la montre";
                P9 = "Dans le mode marathon, vous continuez tant";
                P91 = "que vous trouvez les mots";
                P10 = "Dans contre la montre, vous avez 5 minutes";
                P101 = "pour trouver le plus de mots possible";
                P11 = "Plus les mots ont de lettres à trouver et";
                P12 = "plus vous trouvez en un minimum d'essais";
                P121 = "plus vous gagner de points !";
                P13 = "Comparez votre score aux autres dans le";
                P131 = "classement remis à zero tout les mois!";
            }
            else
            {
                P1 = "In Motux you need to find a word";
                P2 = "For that, at every word, the first and";
                P21 = "another letter is given";
                P3 = "The player have 6 tries to find the";
                P31 = "hidden word";
                P4 = "At every validation, letters give an";
                P41 = "indication";
                P5 = "If it's green or red";
                P51 = "the letter is right";
                P6 = "If it's yellow, the letter is in";
                P61 = "the word";
                P7 = "But if the word is wrong";
                P71 = "there is no indication";
                P8 = "There are 2 game modes : Marathon";
                P81 = "Time trial";
                P9 = "In the mode marathon, you continue";
                P91 = "as long as you find the words";
                P10 = "In time trial, you have 5 minutes";
                P101 = "to find the most word";
                P11 = "More words have letters to find and";
                P12 = "more you find in a minimum try";
                P121 = "more you win points !";
                P13 = "Compare your score to others in the";
                P131 = "ranking, reset every month!";
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void HandleInput(InputState input)
        {
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    if (gesture.Position.X >= position_suivant.X &&
                        gesture.Position.X <= font_titre.MeasureString(suivant).X + position_suivant.X &&
                        gesture.Position.Y >= position_suivant.Y &&
                        gesture.Position.Y <= font_titre.MeasureString(suivant).Y + position_suivant.Y)
                    {
                        if (page < 3)
                        {
                            if (vibrate)
                            {
                                VibrateController vc = VibrateController.Default;
                                vc.Start(TimeSpan.FromMilliseconds(50));
                            }
                            page++;
                        }
                        else
                        {
                            if (vibrate)
                            {
                                VibrateController vc = VibrateController.Default;
                                vc.Start(TimeSpan.FromMilliseconds(50));
                            }
                            this.ExitScreen();
                            ScreenManager.AddScreen(new OptionScreen());
                        }
                    }

                }
            }

            base.HandleInput(input);
        }

        private void Input()
        {
            state = Mouse.GetState();

            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released)
            {
                if (state.X >= position_suivant.X &&
                        state.X <= font_titre.MeasureString(suivant).X + position_suivant.X &&
                        state.Y >= position_suivant.Y &&
                        state.Y <= font_titre.MeasureString(suivant).Y + position_suivant.Y)
                {
                    if (page < 3)
                    {
                        page++;
                    }
                    else
                    {
                        this.ExitScreen();
                        ScreenManager.AddScreen(new OptionScreen());
                    }
                }
            }
            previousState = state;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen());
            }

            Input();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, r, Color.Black);
            ScreenManager.SpriteBatch.DrawString(font_fat, aide, new Vector2(240 - font_fat.MeasureString(aide).X / 2, 10), new Color(38, 180, 212));

            switch (page)
            {
                case 1: DrawPageOne(); break;
                case 2: DrawPageTwo(); break;
                case 3: DrawPageThree(); break;
                default: DrawPageOne(); break;
            }

            if (page <3)
            {
                ScreenManager.SpriteBatch.DrawString(font_titre, suivant, position_suivant, Color.White);
            }
            else
            {
                ScreenManager.SpriteBatch.DrawString(font_titre, terminer, position_suivant, Color.White);
            }

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawPageOne()
        {
            ScreenManager.SpriteBatch.DrawString(font_text, P1, new Vector2(20, 150), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P2, new Vector2(20, 220), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P21, new Vector2(18, 240), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P3, new Vector2(20, 350), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P31, new Vector2(18, 370), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P4, new Vector2(20, 500), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P41, new Vector2(18, 520), Color.White);
        }

        private void DrawPageTwo()
        {
            ScreenManager.SpriteBatch.DrawString(font_text, P5, new Vector2(20, 150), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P51, new Vector2(18, 170), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P6, new Vector2(18, 270), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P61, new Vector2(20, 290), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P7, new Vector2(18, 350), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P71, new Vector2(20, 370), Color.White);

            ScreenManager.SpriteBatch.Draw(plaque_juste, new Vector2(360, 150), Color.White);
            ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture,new Rectangle(420,150,45,45),new Color(126, 215, 1));

            ScreenManager.SpriteBatch.Draw(plaque_cercle, new Vector2(360, 270), Color.White);
            ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle(420, 270, 45, 45), new Color(254, 217, 6));

            ScreenManager.SpriteBatch.Draw(exemple_faux, new Vector2(75,450), Color.White);
        }

        private void DrawPageThree()
        {
            ScreenManager.SpriteBatch.DrawString(font_text, P8, new Vector2(20, 200), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P81, new Vector2(260, 220), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P9, new Vector2(20, 270), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P91, new Vector2(18, 290), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P10, new Vector2(20, 350), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P101, new Vector2(18, 370), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P11, new Vector2(20, 480), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P12, new Vector2(20, 500), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P121, new Vector2(18, 520), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P13, new Vector2(20, 570), Color.White);
            ScreenManager.SpriteBatch.DrawString(font_text, P131, new Vector2(18, 590), Color.White);
        }
    }
}
