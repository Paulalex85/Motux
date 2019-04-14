using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using XMLObject;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.GamerServices;

namespace Motux.InGame
{
    class Motus : GameScreen
    {
        #region VARIABLES
        public enum TypeDePartie
        {
            Simple,
            Marathon,
            Montre
        }

        TypeDePartie _type_partie;

        Dictionnaire dico1;
        Dictionnaire dico2;
        Dictionnaire dico3;
        Dictionnaire dico_facile1;
        Dictionnaire dico_facile2;
        Dictionnaire dico_facile3;

        SpriteFont _font_lettre;
        SpriteFont _font_keyboard;
        SpriteFont _font_info;
        SpriteFont _font_sympa;

        //ICONES
        Texture2D check;
        Texture2D clear;
        Texture2D rejouer_icone;
        Texture2D menu_icone;
        Texture2D scores_multi;

        List<Texture2D> _texture_list;

        Mot[] liste_mot;

        Languages lang = new Languages();
        Langues langue = new Langues();
        Random random = new Random();

        //Keyboard
        Keyboard keyboard;
        int _positionKeyboard_Y = 550;
        static int marge_top = 30;
        float timer;
        bool _transition_keyboard = false;

        //DIVERS
        int _nombre_de_lettre;
        int _nombre_de_point = 0;
        float _time_montre = 300000f;
        bool _facile;
        string _mot_a_trouver;
        List<string> _list_mot_a_trouver = new List<string>();
        string essai, rejouer_string, menu_string, score_string, point_string, temps_ecoule_string1, temps_ecoule_string2, mot_errone;
        int _nombre_dessai_restant;
        bool _motfauxmontre = false;
        float _motfaux_time = 0f;
        bool _temps_ecoule = false;
        bool _trial = false;

        // STATUT PARTIE
        bool _nouvelle_partie = false;
        bool _gagner = false;
        bool _perdu = false;
        bool _validation_mot = false;
        bool _mot_faux = false;
        bool _mot_faux_affichage_animation = false;
        float _mot_faux_affichage_animation_time = 0f;
        bool _animation_mot_juste = false;
        string[] _list_des_lettres_trouver_juste;

        //TIMER PERDU AFFICHAGE SOLUTION
        bool _timer_attente = false;
        bool _timer_attente_effacer = false;
        bool _timer_attente_ajouterlettre = false;
        float _timer_attente_final = 0f;
        float _timer_attente_final_fin = 500f;

        //Transition Keyboard-Fin de partie
        private enum StatutTransition
        {
            Vers_le_plus,
            Vers_le_moins
        }

        StatutTransition _transition_statut;
        float _transition_fin_alpha = 1;
        float _transition_timer;
        OptionFinPartie optionFinPartie;

        // Animation verification mot
        int _nombre_i_bis = 0;
        int _nombre_i_a_verifier = 0;
        float _timer_verification = 0f;
        float _timer_next = 150f;
        int _position_verification_mot;

        TransitionDebutScreen transition = new TransitionDebutScreen();

        //THEME
        string theme = (string)IsolatedStorageSettings.ApplicationSettings["theme"];
        Color _theme_color;

        #endregion

        public Motus(bool facile_bool, TypeDePartie type_de_partie)
        {
            _type_partie = type_de_partie;
            _facile = facile_bool;
            _nouvelle_partie = true;
            EnabledGestures = GestureType.Tap;

            TransitionOnTime = TimeSpan.FromSeconds(2.5);
            TransitionOffTime = TimeSpan.FromSeconds(1);

            if (type_de_partie == TypeDePartie.Simple)
            {
                _trial = Guide.IsTrialMode;
            }
           
        }
        #region INITIALIZE
        public override void LoadContent()
        {
            _texture_list = new List<Texture2D>();
            AleatoireNombreLettre();
            //Chargement dictionnaire
            if (_facile)
            {
                if (lang.lang == "fr")
                {
                    dico1 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico5");
                    dico2 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico6");
                    dico3 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico7");
                    dico_facile1 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico5_easy");
                    dico_facile2 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico6_easy");
                    dico_facile3 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico7_easy");
                }
                else
                {
                    dico1 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico5");
                    dico2 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico6");
                    dico3 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico7");
                }
            }
            else
            {
                dico1 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico8");
                dico2 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico9");
                dico3 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico10");
            }

            langue = ScreenManager.Game.Content.Load<Langues>(lang.PathDictionnaire() + "LANG");
            InitilizeLanguages();

            //Chargement Icones Keyboard
            clear = ScreenManager.Game.Content.Load<Texture2D>("Icone/clear");
            check = ScreenManager.Game.Content.Load<Texture2D>("Icone/check");
            menu_icone = ScreenManager.Game.Content.Load<Texture2D>("Icone/home");
            rejouer_icone = ScreenManager.Game.Content.Load<Texture2D>("Icone/retry");
            scores_multi = ScreenManager.Game.Content.Load<Texture2D>("Icone/scores_multi");

            //Chargement Keyboard
            _font_keyboard = ScreenManager.Game.Content.Load<SpriteFont>("Lettre_keyboard");
            keyboard = new Keyboard(_positionKeyboard_Y, _font_keyboard, check, clear);

            //Chargement themes
            string path_chargement = PathChargement(_nombre_de_lettre);

            _font_info = ScreenManager.Game.Content.Load<SpriteFont>("font_info");
            _font_sympa = ScreenManager.Game.Content.Load<SpriteFont>("font_choix_nombre_lettre_fat");

            ThemeColorText();

            if (theme == "classic")
            {
                if (_facile)
                {
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/5_6/plaque_lettre"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/5_6/plaque_cercle"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/5_6/plaque_juste"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/5_6/plaque_bon_mot"));

                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/7/plaque_lettre"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/7/plaque_cercle"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/7/plaque_juste"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/7/plaque_bon_mot"));

                }
                else
                {
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/8/plaque_lettre"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/8/plaque_cercle"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/8/plaque_juste"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/8/plaque_bon_mot"));

                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/9/plaque_lettre"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/9/plaque_cercle"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/9/plaque_juste"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/9/plaque_bon_mot"));

                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/10/plaque_lettre"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/10/plaque_cercle"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/10/plaque_juste"));
                    _texture_list.Add(ScreenManager.Game.Content.Load<Texture2D>("Themes/10/plaque_bon_mot"));
                }
                
                _font_lettre = ScreenManager.Game.Content.Load<SpriteFont>("Themes/" + path_chargement + "/font_lettre");
            }
            else if(theme == "tuiles")
            {
                _font_lettre = ScreenManager.Game.Content.Load<SpriteFont>("Themes/" + path_chargement + "/font_lettre");
            }

            if (_type_partie == TypeDePartie.Simple)
            {
                optionFinPartie = new OptionFinPartie(_positionKeyboard_Y + 75, _font_info, rejouer_icone, menu_icone, rejouer_string, menu_string, TypeDePartie.Simple);
            }
            else
            {
                optionFinPartie = new OptionFinPartie(_positionKeyboard_Y + 75, _font_info, rejouer_icone, scores_multi, rejouer_string, score_string, _type_partie);
            }
            base.LoadContent();
        }

        private void ThemeColorText()
        {
            if (theme == "classic")
            {
                _theme_color = Color.Black;
            }
            else if (theme == "tuiles")
            {
                _theme_color = Color.Black;
            }
        }

        private string PathChargement(int nombre_de_lettre)
        {
            switch (nombre_de_lettre)
            {
                case 10: return "10";
                case 9: return "9";
                case 8: return "8";
                case 7: return "7";
                case 6: return "5_6";
                case 5: return "5_6";
                default: return "8";
            }
        }

        private int TuilesThemeDimension(int nombre_de_lettre)
        {
            switch (nombre_de_lettre)
            {
                case 10: return (int)(480 / 10);
                case 9: return (int)(480 / 9);
                case 8: return (int)(480 / 8);
                case 7: return (int)(480 / 7);
                case 6: return (int)(480 / 6);
                case 5: return (int)(480 / 6);
                default: return (int)(480 / 8);
            }
        }

        private void InitializationThemes()
        {
            if (theme == "classic")
            {
                int k = CurrentTexturePositionListe(_nombre_de_lettre);
                liste_mot = new Mot[6] { new Mot(_nombre_de_lettre,marge_top,_font_lettre,_texture_list[k],_texture_list[k+1],_texture_list[k+2],_texture_list[k+3]),
                new Mot(_nombre_de_lettre,marge_top + (1*_texture_list[k].Height),_font_lettre,_texture_list[k],_texture_list[k+1],_texture_list[k+2],_texture_list[k+3]),
                new Mot(_nombre_de_lettre,marge_top + (2*_texture_list[k].Height),_font_lettre,_texture_list[k],_texture_list[k+1],_texture_list[k+2],_texture_list[k+3]),
                new Mot(_nombre_de_lettre,marge_top + (3*_texture_list[k].Height),_font_lettre,_texture_list[k],_texture_list[k+1],_texture_list[k+2],_texture_list[k+3]),
                new Mot(_nombre_de_lettre,marge_top + (4*_texture_list[k].Height),_font_lettre,_texture_list[k],_texture_list[k+1],_texture_list[k+2],_texture_list[k+3]),
                new Mot(_nombre_de_lettre,marge_top + (5*_texture_list[k].Height),_font_lettre,_texture_list[k],_texture_list[k+1],_texture_list[k+2],_texture_list[k+3])};
            }
            else if (theme == "tuiles")
            {
                int k = TuilesThemeDimension(_nombre_de_lettre);
                liste_mot = new Mot[6] { new Mot(_nombre_de_lettre,marge_top,_font_lettre),
                new Mot(_nombre_de_lettre,marge_top + (1*k),_font_lettre),
                new Mot(_nombre_de_lettre,marge_top + (2*k),_font_lettre),
                new Mot(_nombre_de_lettre,marge_top + (3*k),_font_lettre),
                new Mot(_nombre_de_lettre,marge_top + (4*k),_font_lettre),
                new Mot(_nombre_de_lettre,marge_top + (5*k),_font_lettre)};
            }
        }

        private int CurrentTexturePositionListe(int nombre_de_lettre)
        {
            switch (nombre_de_lettre)
            {
                case 5: return 0;
                case 6: return 0;
                case 7: return 4;
                case 8: return 0;
                case 9: return 4;
                case 10: return 8;
                default: return 0;
            }
        }

        private void InitilizeLanguages()
        {
            essai = lang.AffectationLANG("Essai", langue);
            menu_string = lang.AffectationLANG("Menu", langue);
            rejouer_string = lang.AffectationLANG("Recommencer_String", langue);
            score_string = lang.AffectationLANG("Score_String", langue);
            point_string = lang.AffectationLANG("Transition_Points", langue);
            temps_ecoule_string1 = lang.AffectationLANG("Temps", langue);
            temps_ecoule_string2 = lang.AffectationLANG("Ecoule", langue);
            mot_errone = lang.AffectationLANG("Mot_invalide_String", langue);
        }

        private string PathDico(int nombredelettre)
        {
            switch (nombredelettre)
            {
                case 5: return "dico5";
                case 6: return "dico6";
                case 7: return "dico7";
                case 8: return "dico8"; 
                case 9: return "dico9";
                case 10: return "dico10"; 
                default: return "dico8";
            }
        }

        private void ChercheMotXML(int nombrelettre, Dictionnaire _dico)
        {
            int rand = random.Next(0, _dico.DicoMots.Length);
            XMLObject.Mots elementAt = _dico.DicoMots.ElementAt(rand);
            _mot_a_trouver = elementAt.a;

        }
        #endregion
        #region INPUT
        public override void HandleInput(InputState input)
        {
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    if (keyboard.liste_lettre[0].Alpha == 1)
                    {
                        foreach (Keyboard_Lettre caca in keyboard.liste_lettre)
                        {
                            if (caca.HandleTap(gesture.Position))
                            {
                                keyboard._letter_tapped = caca._lettre;
                                keyboard._tapped = true;
                                keyboard._type_tapped = caca._type;
                                break;
                            }
                        }
                    }
                    if (optionFinPartie.Alpha == 1)
                    {
                        if (_type_partie == TypeDePartie.Simple)
                        {
                            if (optionFinPartie.HandleTapMenu(gesture.Position))
                            {
                                TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new MainMenuScreen());
                            }

                            if (optionFinPartie.HandleTapRejouer(gesture.Position))
                            {
                                TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new InGame.Motus(_facile, TypeDePartie.Simple));
                            }
                        }
                        else
                        {
                            if (optionFinPartie.HandleTapScore(gesture.Position))
                            {
                                TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new ScoreScreen(_type_partie, _nombre_de_point));
                            }

                            if (optionFinPartie.HandleTapRejouer(gesture.Position))
                            {
                                TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new InGame.Motus(_facile, _type_partie));
                            }
                        }
                    }

                }
            }

            base.HandleInput(input);
        }
        #endregion

        #region UPDATE
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                TransitionScreen.Load(ScreenManager, this, true, PlayerIndex.One, new MainMenuScreen());
            }

            if (_nouvelle_partie) { NouvellePartie(); }

            timer = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            transition.UpdateAlpha(timer);

            UpdateClavier();
            KeyboardAdd();

            if (_transition_keyboard) { TransitionPerduGagner(timer); }

            if (_validation_mot) { ValiderVerificationLettre(liste_mot.ElementAt(_position_verification_mot), timer); }
            if (_mot_faux) { MotInvalide_PasserSuivant(); }
            if (_timer_attente) { PerduAffichageSolution(timer); }
            if (_animation_mot_juste) { AnimMotJuste(timer); }
            if (_type_partie == TypeDePartie.Montre && !_temps_ecoule) { Timer(timer); }
            if (_motfauxmontre) { TimerMontreMotFaux(timer); }
            if (_mot_faux_affichage_animation) { TimerAnimationMotFaux(timer); }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void TimerAnimationMotFaux(float timer)
        {
            _mot_faux_affichage_animation_time += timer;
            if (_mot_faux_affichage_animation_time > 2000f)
            {
                _mot_faux_affichage_animation_time = 0f;
                _mot_faux_affichage_animation = false;
            }
        }

        private void TimerMontreMotFaux(float timer)
        {
            _motfaux_time += timer;
            if (_motfaux_time > 1000f)
            {
                _list_mot_a_trouver = new List<string>();
                _motfaux_time = 0f;
                _motfauxmontre = false;
                _nouvelle_partie = true;
            }
        }

        private void Timer(float timer)
        {
            if (_time_montre < 0f)
            {
                _temps_ecoule = true;
                _transition_keyboard = true;
                _transition_statut = StatutTransition.Vers_le_moins;
            }
            else
            {
                _time_montre -= timer;
            }
        }

        private void AnimMotJuste(float timer)
        {
            int mot_position;
            if (_gagner)
            {
                mot_position = _position_verification_mot;
            }
            else
            {
                mot_position = 5;
            }
            liste_mot[mot_position].AnimationMotJuste(timer);

            if (liste_mot[mot_position].loop == 3)
            {
                _animation_mot_juste = false;
                if (_gagner)
                {
                    if (_type_partie == TypeDePartie.Marathon || _type_partie == TypeDePartie.Montre)
                    {
                        Points();
                        _list_mot_a_trouver = new List<string>();
                        _nouvelle_partie = true;
                    }
                }
                else
                {
                    if (_type_partie == TypeDePartie.Montre)
                    {
                        _motfauxmontre = true;
                    }
                }

            }
        }

        private void MotInvalide_PasserSuivant()
        {
            
            LettreMotSuivant_StatutPartie();
            _mot_faux_affichage_animation = true;
            _mot_faux = false;
        }

        private void TransitionPerduGagner(float timer)
        {
            float _transition_max = 500f;
            _transition_timer += timer;

            if (_transition_statut == StatutTransition.Vers_le_moins)
            {
                _transition_fin_alpha = (_transition_max - _transition_timer) / _transition_max;
                foreach (var caca in keyboard.liste_lettre)
                {
                    caca.Alpha = _transition_fin_alpha;
                }
            }
            else if (_transition_statut == StatutTransition.Vers_le_plus)
            {
                _transition_fin_alpha = _transition_timer / _transition_max;

                optionFinPartie.Alpha = _transition_fin_alpha;
            }

            if (_transition_statut == StatutTransition.Vers_le_moins && _transition_timer > _transition_max)
            {
                _transition_statut = StatutTransition.Vers_le_plus;
                _transition_timer = 0;
                foreach (var caca in keyboard.liste_lettre)
                {
                    caca.Alpha = 0;
                }
            }
            else if (_transition_statut == StatutTransition.Vers_le_plus && _transition_timer > _transition_max)
            {
                _transition_timer = 0;
                _transition_keyboard = false;
                optionFinPartie.Alpha = 1;
            }

        }
        
        private void KeyboardAdd()
        {
            if (keyboard._tapped)
            {
                if (keyboard._type_tapped == Keyboard.TypeLettre.Lettre)
                {
                    int i = 0;
                    foreach (var caca in liste_mot)
                    {
                        if (caca.PeuxAddLettreMot(keyboard._letter_tapped))
                        {
                            if (i == 0 || liste_mot.ElementAt(i - 1)._valider)
                            {
                                caca.AddLettreMot(keyboard._letter_tapped);
                                keyboard._letter_tapped = "";
                                keyboard._tapped = false;
                                break;
                            }
                        }
                        i++;
                    }
                }
                else if (keyboard._type_tapped == Keyboard.TypeLettre.Effacer)
                {
                    EffacerLettre();
                    keyboard._tapped = false;
                }
                else if (keyboard._type_tapped == Keyboard.TypeLettre.Valider)
                {
                    ValiderMot();
                    keyboard._tapped = false;
                }
            }
        }

        private void ValiderMot()
        {
            _position_verification_mot = 0;
            foreach (var caca in liste_mot)
            {
                if (!caca._valider && MotsCompletDeLettre(caca))
                {
                    if (VerificationMotXML(caca, CurrentDictionnaire(_nombre_de_lettre)))
                    {
                        caca._valider = true;
                        _nombre_i_a_verifier = 0;
                        _validation_mot = true;
                        break;
                    }
                    else
                    {
                        _mot_faux = true;
                        caca._valider = true; break;
                    }
                }
                else
                {
                    _position_verification_mot++;
                }
            }
        }

        private void LettreMotSuivant_StatutPartie()
        {
            if (!_perdu)
            {
                if (Gagner(liste_mot.ElementAt(_position_verification_mot)))
                {
                    _gagner = true;
                    QuandGagnerOuPerdu();
                }
                else if (_position_verification_mot == 5)
                {
                    _perdu = true;
                    QuandGagnerOuPerdu();
                }
                else
                {
                    _nombre_dessai_restant--;
                    AjoutMotSuivantLettreJuste(_position_verification_mot);
                }
            }
        }

        private void QuandGagnerOuPerdu()
        {
            if (_type_partie == TypeDePartie.Simple)
            {
                if (_gagner)
                {
                    _transition_keyboard = true;
                    _transition_statut = StatutTransition.Vers_le_moins;
                    _animation_mot_juste = true;
                    _nombre_i_a_verifier = 0;
                }
                else
                {
                    _transition_keyboard = true;
                    _transition_statut = StatutTransition.Vers_le_moins;
                    _timer_attente = true;
                    _nombre_i_a_verifier = 0;
                    liste_mot[5]._valider = false;
                    _nombre_dessai_restant--;
                }
            }
            else if (_type_partie == TypeDePartie.Marathon)
            {
                if (_gagner)
                {
                    _animation_mot_juste = true;
                    _nombre_i_a_verifier = 0;
                }
                else
                {
                    _transition_keyboard = true;
                    _transition_statut = StatutTransition.Vers_le_moins;
                    _timer_attente = true;
                    _nombre_i_a_verifier = 0;
                    liste_mot[5]._valider = false;
                    _nombre_dessai_restant--;
                }
            }
            else
            {
                if (_gagner)
                {
                    _animation_mot_juste = true;
                    _nombre_i_a_verifier = 0;
                }
                else
                {
                    _timer_attente = true;
                    _nombre_i_a_verifier = 0;
                    liste_mot[5]._valider = false;
                    _nombre_dessai_restant--;
                }
            }
        }

        private void PerduAffichageSolution(float timer)
        {
            _timer_attente_final += timer;

            if (!_timer_attente_effacer && !_timer_attente_ajouterlettre)
            {
                if (_timer_attente_final > _timer_attente_final_fin)
                {
                    _timer_attente_final = 0;
                    foreach (var caca in liste_mot[_position_verification_mot]._lettres)
                    {
                        caca._lettre = "";
                        caca._etat = Lettre.ValidationEtat.Rien;
                    }
                    _timer_attente_effacer = true;
                }
            }
            else if (_timer_attente_effacer && !_timer_attente_ajouterlettre)
            {
                if (_timer_attente_final > _timer_next)
                {
                    _timer_attente_final = 0;
                    liste_mot[_position_verification_mot]._lettres.ElementAt(_nombre_i_bis)._lettre = _list_mot_a_trouver[_nombre_i_bis];
                    _nombre_i_bis++;
                    if (_nombre_i_bis == _nombre_de_lettre)
                    {
                        _timer_attente_ajouterlettre = true;
                    }
                }
            }
            else 
            {
                _validation_mot = true;
                _timer_attente = false;
                _nombre_i_a_verifier = 0;
                _animation_mot_juste = true;
            }

        }

        private bool Gagner(Mot mot)
        {
            bool caca = true;
            foreach (var pd in mot._lettres)
            {
                if (pd._etat != Lettre.ValidationEtat.Bon)
                {
                    caca = false;
                }
            }
            return caca;
        }

        private void AjoutMotSuivantLettreJuste(int position_ancien)
        {
            liste_mot[position_ancien + 1]._afficher = true;
            for (int i = 0; i < _nombre_de_lettre; i++)
            {
                if (liste_mot[position_ancien]._lettres[i]._etat == Lettre.ValidationEtat.Bon)
                {
                    if (_list_des_lettres_trouver_juste[i] == "" || _list_des_lettres_trouver_juste[i] == null)
                    {
                        _list_des_lettres_trouver_juste[i] = liste_mot[position_ancien]._lettres[i]._lettre;
                    }
                    
                }
                if (_list_des_lettres_trouver_juste[i] != null)
                {
                    liste_mot[position_ancien + 1]._lettres[i]._lettre = _list_des_lettres_trouver_juste[i];
                }
            }
        }

        private bool VerificationMotXML(Mot mot, Dictionnaire dico)
        {
            string mot_string = "";
            for(int i = 0; i < mot._lettres.Count();i++)
            {
                mot_string += mot._lettres.ElementAt(i)._lettre;
            }

            bool pd = false; 
            foreach(var caca in dico.DicoMots)
            {
                if (caca.a == mot_string)
                {
                    pd = true;
                    break;
                }
            }
            return pd;
        }

        private void ValiderVerificationLettre(Mot mot, float timer)
        {
            _timer_verification += timer;
            if (_timer_verification > _timer_next)
            {
                if (mot._lettres[_nombre_i_a_verifier]._lettre == _list_mot_a_trouver[_nombre_i_a_verifier])
                {
                    mot._lettres[_nombre_i_a_verifier]._etat = Lettre.ValidationEtat.Bon;
                }
                else if (_list_mot_a_trouver.Contains(mot._lettres[_nombre_i_a_verifier]._lettre))
                {
                    mot._lettres[_nombre_i_a_verifier]._etat = Lettre.ValidationEtat.Dedans;
                }
                else
                {
                    mot._lettres[_nombre_i_a_verifier]._etat = Lettre.ValidationEtat.Rien;
                }
                if (_nombre_i_a_verifier == _nombre_de_lettre-1)
                {
                    _validation_mot = false;
                    LettreMotSuivant_StatutPartie();
                }
                else
                {
                    _nombre_i_a_verifier++;
                }
                _timer_verification = 0f;
            }
        }

        private void Points()
        {
            int point_match = (_nombre_de_lettre * 10) - ((6 - _nombre_dessai_restant) * _nombre_de_lettre);
            _nombre_de_point += point_match;
        }

        private bool MotsCompletDeLettre(Mot mot)
        {
            bool pd = true;
            foreach(Lettre caca in mot._lettres)
            {
                if (caca._lettre == "")
                {
                    pd = false;
                    break;
                }
            }
            return pd;
        }

        private void EffacerLettre()
        {
            bool end = false;
            foreach (var caca in liste_mot)
            {
                if (end) { break; }
                if (!caca._valider)
                {
                    foreach(Lettre pd in caca._lettres.ToArray().Reverse())
                    {
                        if (pd._lettre != "" && pd._effacable)
                        {
                            pd._lettre = "";
                            end = true;
                            break;

                        }
                    }
                }
            }
        }

        private void NouvellePartie()
        {
            AleatoireNombreLettre();
            InitializationThemes();

            if (_facile && lang.lang == "fr")
            {
                ChercheMotXML(_nombre_de_lettre, CurrentDictionnaireFacile(_nombre_de_lettre));
            }
            else
            {
                ChercheMotXML(_nombre_de_lettre, CurrentDictionnaire(_nombre_de_lettre));
            }

            for (int i = 0; i < _nombre_de_lettre; i++)
            {
                _list_mot_a_trouver.Add(_mot_a_trouver.Substring(i,1));
            }

            liste_mot[0]._lettres[0]._lettre = _list_mot_a_trouver[0];

            int rand = random.Next(1, _nombre_de_lettre);
            liste_mot[0]._lettres[rand]._lettre = _list_mot_a_trouver[rand];

            liste_mot[0]._lettres[0]._effacable = false;
            liste_mot[1]._lettres[0]._effacable = false;
            liste_mot[2]._lettres[0]._effacable = false;
            liste_mot[3]._lettres[0]._effacable = false;
            liste_mot[4]._lettres[0]._effacable = false;
            liste_mot[5]._lettres[0]._effacable = false;

            _list_des_lettres_trouver_juste = new string[_nombre_de_lettre];
            _list_des_lettres_trouver_juste[0] = _list_mot_a_trouver[0];
            _list_des_lettres_trouver_juste[rand] = _list_mot_a_trouver[rand];

            liste_mot[0]._afficher = true;

            _nombre_dessai_restant = 6;
            _gagner = false;
            _perdu = false;
            _nombre_i_a_verifier = 0;
            _nombre_i_bis = 0;

            _nouvelle_partie = false;
        }

        private Dictionnaire CurrentDictionnaire(int nombre_de_lettre)
        {
            switch (nombre_de_lettre)
            {
                case 5: return dico1;
                case 6: return dico2;
                case 7: return dico3;
                case 8: return dico1;
                case 9: return dico2;
                case 10: return dico3;
                default: return dico1;
            }
        }

        private Dictionnaire CurrentDictionnaireFacile(int nombre_de_lettre)
        {
            switch (nombre_de_lettre)
            {
                case 5: return dico_facile1;
                case 6: return dico_facile2;
                case 7: return dico_facile3;
                default: return dico_facile1;
            }
        }

        private void AleatoireNombreLettre()
        {
            if (_type_partie == TypeDePartie.Simple && _trial)
            {
                _nombre_de_lettre = 5;
            }
            else
            {
                int rand = random.Next(1, 4);

                if (_facile)
                {
                    switch (rand)
                    {
                        case 1: _nombre_de_lettre = 5; break;
                        case 2: _nombre_de_lettre = 6; break;
                        case 3: _nombre_de_lettre = 7; break;
                    }
                }
                else
                {
                    switch (rand)
                    {
                        case 1: _nombre_de_lettre = 8; break;
                        case 2: _nombre_de_lettre = 9; break;
                        case 3: _nombre_de_lettre = 10; break;
                    }
                }
            }
        }

        private void UpdateClavier()
        {
            foreach (Keyboard_Lettre caca in keyboard.liste_lettre)
            {
                if (caca._tap_timer > caca._tap_timer_max_anim)
                {
                    caca._tap_bool = false;
                }
                if (caca._tap_bool)
                {
                    caca._tap_timer += timer;
                }
            }
        }
        #endregion
        #region DRAW
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            if (!_nouvelle_partie) { DrawCasesLettres(); }
            DrawKeyboard();
            DrawInformation();
            if (_type_partie == TypeDePartie.Marathon || _type_partie == TypeDePartie.Montre) { DrawScores(); }
            if (_type_partie == TypeDePartie.Montre) { DrawTimer(); }
            if (_temps_ecoule) { DrawTempsEND(); }
            if (_mot_faux_affichage_animation) { DrawMotFaux(); }
            optionFinPartie.DrawIcone(this);

            transition.DrawAlpha(this);

            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawTimer()
        {
            string minutes, secondes;
            if(_time_montre > 60000)
            {
                minutes = (_time_montre / 60000).ToString().Substring(0, 1);
                if ((_time_montre - int.Parse(minutes) * 60000) != 0)
                {
                    if ((_time_montre - (int.Parse(minutes) * 60000)) > 10000)
                    {
                        secondes = (_time_montre - int.Parse(minutes) * 60000).ToString().Substring(0, 2);
                    }
                    else if ((_time_montre - (int.Parse(minutes) * 60000)) < 1000)
                    {
                        secondes = "00";
                    }
                    else
                    {
                        secondes = "0" + ((_time_montre - int.Parse(minutes) * 60000)).ToString().Substring(0, 1);
                    }
                }
                else
                {
                    secondes = "00";
                }
            }
            else
            {
                minutes = "0";

                if (_time_montre  > 10000)
                {
                    secondes = (_time_montre - int.Parse(minutes) * 60000).ToString().Substring(0, 2);
                }
                else if (_time_montre< 1000)
                {
                    secondes = "00";
                }
                else
                {
                    secondes = "0" + (_time_montre).ToString().Substring(0, 1);
                }
            }
            ScreenManager.SpriteBatch.DrawString(_font_info, minutes +" : "+ secondes , new Vector2(200, 0), _theme_color);
        }

        private void DrawTempsEND()
        {
            ScreenManager.SpriteBatch.DrawString(_font_sympa, temps_ecoule_string1, new Vector2(480 / 2 - _font_sympa.MeasureString(temps_ecoule_string1).X / 2, 300), _theme_color);
            ScreenManager.SpriteBatch.DrawString(_font_sympa, temps_ecoule_string2, new Vector2(480 / 2 - _font_sympa.MeasureString(temps_ecoule_string2).X / 2, 400), _theme_color);
        }

        private void DrawMotFaux()
        {
            float transition = 0f;
            if (_mot_faux_affichage_animation_time < 1000f)
            {
                transition = 1;
            }
            else
            {
                transition = ((2000f - _mot_faux_affichage_animation_time) / 1000f);
            }
            ScreenManager.SpriteBatch.DrawString(_font_sympa, mot_errone, new Vector2(480 / 2 - _font_sympa.MeasureString(mot_errone).X / 2, 300), (new Color(189,3,3)) * transition);
        }

        private void DrawInformation()
        {
            ScreenManager.SpriteBatch.DrawString(_font_info, essai + " " + _nombre_dessai_restant.ToString(), new Vector2(20, _positionKeyboard_Y - 40), _theme_color);
        }

        private void DrawScores()
        {
            ScreenManager.SpriteBatch.DrawString(_font_info, point_string + " : " + _nombre_de_point.ToString(), new Vector2(300, _positionKeyboard_Y - 40), _theme_color);
        }

        private void DrawCasesLettres()
        {
            foreach (var caca in liste_mot)
            {
                if (caca._afficher)
                {
                    foreach (var caca2 in caca._lettres)
                    {
                        if (theme == "classic")
                        {
                            caca2.DrawTexture(this);
                        }
                        else if (theme == "tuiles")
                        {
                            caca2.DrawLettre(this);
                        }
                    }
                }
            }
        }

        private void DrawKeyboard()
        {
            Rectangle fond_keyboard = new Rectangle(0, _positionKeyboard_Y, 480, 800 - _positionKeyboard_Y);
            ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, fond_keyboard, Color.Black);
            foreach (Keyboard_Lettre caca in keyboard.liste_lettre)
            {
                caca.DrawLettre(this);
            }

        }
        #endregion
    }
}
