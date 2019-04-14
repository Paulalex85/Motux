using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XMLObject;
using Microsoft.Xna.Framework.Input;
using System.IO.IsolatedStorage;

namespace Motux
{
    class LoadingScreen : GameScreen
    {
        Texture2D _loading2D;

        string theme = (string)IsolatedStorageSettings.ApplicationSettings["theme"];

        private Thread backgroundThread;
        SpriteFont Font_pourcentage;

        int _frame = 0;
        int _nombre_frame = 7;
        int _loadingWidth = 64;
        float _timeframe = 200;
        float _timeloading;
        int pourcentage = 0;

        Languages lang = new Languages();
        LoadingSprite loading = new LoadingSprite();

        bool _caca;

        public LoadingScreen(bool caca)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.0);
            loading.start = true;
            loading.AfficherPourcentage = true;
            _caca = caca;
        }

        void BackgroundLoadContent()
        {
            ScreenManager.Game.Content.Load<object>("menufont");
            if (lang.lang == "fr")
            {
                ScreenManager.Game.Content.Load<object>(lang.path + "dico10");
                pourcentage = 20;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico9");
                pourcentage = 30;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico8");
                pourcentage = 40;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico7");
                pourcentage = 50;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico6");
                pourcentage = 60;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico5");
                pourcentage = 70;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico7_easy");
                pourcentage = 80;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico6_easy");
                pourcentage = 90;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico5_easy");
                pourcentage = 100;
            }
            else
            {
                ScreenManager.Game.Content.Load<object>(lang.path + "dico10");
                pourcentage = 30;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico9");
                pourcentage = 45;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico8");
                pourcentage = 60;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico7");
                pourcentage = 75;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico6");
                pourcentage = 90;
                ScreenManager.Game.Content.Load<object>(lang.path + "dico5");
                pourcentage = 100;
            }
            
            loading.start = false;
            loading.AfficherPourcentage = false;
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
            Font_pourcentage = ScreenManager.Game.Content.Load<SpriteFont>("font_info");

            if (backgroundThread == null)
            {
                backgroundThread = new Thread(BackgroundLoadContent);
                backgroundThread.Start();
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                ScreenManager.Game.Exit();
            }

            _timeloading += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Loading();

            if (backgroundThread != null && backgroundThread.Join(10))
            {
                backgroundThread = null;
                this.ExitScreen();
                if (_caca)
                {
                    ScreenManager.AddScreen(new Aide());
                }
                else
                {
                    ScreenManager.AddScreen(new MainMenuScreen());
                }
                ScreenManager.Game.ResetElapsedTime();
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Loading()
        {
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


        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            LoadingDraw();

            ScreenManager.SpriteBatch.End();
        }

        private void LoadingDraw()
        {
            int _afficherWidth = _frame * _loadingWidth;
            Rectangle source = new Rectangle(_afficherWidth, 0, _loadingWidth, _loading2D.Height);
            ScreenManager.SpriteBatch.Draw(_loading2D, new Vector2(210, 600), source, Color.White);
            ScreenManager.SpriteBatch.DrawString(Font_pourcentage, pourcentage + " %", new Vector2(220, 700), Color.Black);
        }
    }
}
