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

namespace XMLObject
{
    
    public class Sauvegarde
    {
        public Parties ListeSave { get; set; }

        public Sauvegarde() { }
    }

    public class Parties
    {
        public int TypeMatch { get; set; }
        public int RoundMatch { get; set; }
        public int NombreDeManches { get; set; }
        public int NombreManchesJouer { get; set; }
        public int Difficulte { get; set; }
        public string ScoreJoueur { get; set; }
        public string ScoreAdversaire { get; set; }
        public string NomAdversaire { get; set; }

        public Parties() { }
    }

    public class Difficulte
    {
        public Niveaux[] ListeDifficulte;

        public Difficulte() { }
    }

    public class Niveaux
    {
        public string Difficulte;
        public string ListePourcentageChiffres;
        public string ListePourcetageLettres;
        public int TempsLettres;
        public int TempsChiffres;

        public Niveaux() { }
    }

    public class Langues
    {
        public Valeurs[] Values;

        public Langues() { }
    }

    public class Valeurs
    {
        public string ID;
        public string caca;

        public Valeurs() { }
    }
}
