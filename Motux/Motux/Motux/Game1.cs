using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using GameStateManagement;
using Microsoft.Advertising.Mobile.Xna;
using System.IO.IsolatedStorage;
using XMLObject;

namespace Motux
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        string theme;

        Texture2D background;
        Languages lang = new Languages();
        Langues langue = new Langues();

        bool first = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            Content.RootDirectory = "Content";


            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            InitializationThemes();

            screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new LoadingScreen(first));
        }

        /// <summary>
        /// LoadContent est appelé une fois par partie. Emplacement de chargement
        /// de tout votre contenu.
        /// </summary>
        protected override void LoadContent()
        {
            /*InitializationThemes();

            if (theme == "classic")
            {

                // Créer un SpriteBatch, qui peut être utilisé pour dessiner des textures.
                GraphicsDevice.Clear(Color.CornflowerBlue);
                SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
                background = Content.Load<Texture2D>("background");
                spriteBatch.Begin();

                spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

                spriteBatch.End();
                GraphicsDevice.Present();
            }
            else if (theme == "tuiles")
            {
                GraphicsDevice.Clear(Color.White);
            }*/

            // TODO: utilisez this.Content pour charger votre contenu de jeu ici
        }

        private void InitializationThemes()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("theme"))
            {
                IsolatedStorageSettings.ApplicationSettings["theme"] = "classic";
                first = true;
            }
            else
            {
                theme = (string)IsolatedStorageSettings.ApplicationSettings["theme"];
            }

            if (!IsolatedStorageSettings.ApplicationSettings.Contains("vibrate"))
            {
                IsolatedStorageSettings.ApplicationSettings["vibrate"] = true;
            }

            if (!IsolatedStorageSettings.ApplicationSettings.Contains("player1"))
            {
                Random random = new Random();
                int blbl = random.Next(1, 100001);
                if (lang.lang == "fr")
                {
                    IsolatedStorageSettings.ApplicationSettings["player1"] = "Joueur" + blbl.ToString();
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["player1"] = "Player" + blbl.ToString();
                }
            }
        }

        /// <summary>
        /// UnloadContent est appelé une fois par partie. Emplacement de déchargement
        /// de tout votre contenu.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Déchargez le contenu non ContentManager ici
        }

        /// <summary>
        /// Permet au jeu d’exécuter la logique de mise à jour du monde,
        /// de vérifier les collisions, de gérer les entrées et de lire l’audio.
        /// </summary>
        /// <param name="gameTime">Fournit un aperçu des valeurs de temps.</param>
        protected override void Update(GameTime gameTime)
        {

            // TODO: Ajoutez votre logique de mise à jour ici

            base.Update(gameTime);
        }

        /// <summary>
        /// Appelé quand le jeu doit se dessiner.
        /// </summary>
        /// <param name="gameTime">Fournit un aperçu des valeurs de temps.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
