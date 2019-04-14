using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using XMLObject;
using GameStateManagement;

namespace Motux
{
    class Languages
    {
        CultureInfo Culture;
        public string lang;
        public string path;

        public Languages()
        {
            Culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            lang = Culture.ToString().Substring(0, 2);
            path = PathDictionnaire();
        }

        public string AffectationLANG(string id, Langues langue)
        {
            string facial = "";

            foreach (var caca in langue.Values)
            {
                if (caca.ID == id)
                {
                    facial = caca.caca;
                    break;
                }
            }
            return facial;
        }


        public string PathDictionnaire()
        {
            switch (lang)
            {
                case "fr": return "Languages/French/";
                //case "nl": return "Languages/Dutch/";
                case "en": return "Languages/English/";
                //case "de": return "Languages/German/";
                //case "it": return "Languages/Italian/";
                //case "pt": return "Languages/Portuguese/";
                //case "es": return "Languages/Spanish/";
                default: return "Languages/English/";
            }
        }
        public List<string> LettresTirage(bool voyelle)
        {
            List<string> caca;
            if (voyelle)
            {
                switch (lang)
                {
                    case "fr": caca = new List<string> { "A", "A", "A", "A", "A", "A", "A", "A", "A", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "I", "I", "I", "I", "I", "I", "I", "I", "O", "O", "O", "O", "O", "O", "U", "U", "U", "U", "U", "U", "Y" }; break;
                    //case "nl": caca = new List<string> { "A", "A", "A", "A", "A", "A", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "I", "I", "I", "I", "I", "I", "O", "O", "O", "O", "O", "O", "U", "U", "U", "Y" }; break;
                    case "en": caca = new List<string> { "A", "A", "A", "A", "A", "A", "A", "A", "A", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "I", "I", "I", "I", "I", "I", "I", "I", "I", "O", "O", "O", "O", "O", "O", "O", "O", "U", "U", "U", "U", "Y", "Y" }; break;
                    //case "de": caca = new List<string> { "A", "A", "A", "A", "A", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "I", "I", "I", "I", "I", "I", "O", "O", "O", "U", "U", "U", "U", "U", "U", "U", "Y" }; break;
                    //case "it": caca = new List<string> { "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "O", "O", "O", "O", "O", "O", "O", "O", "O", "O", "O", "O", "U", "U", "U", "U", "U", "Y" }; break;
                    //case "pt": caca = new List<string> { "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "E", "E", "E", "E", "E", "I", "I", "I", "I", "I", "I", "I", "I", "I", "I", "O", "O", "O", "O", "O", "O", "O", "O", "O", "O", "U", "U", "U", "U", "U", "U", "U" }; break;
                    //case "es": caca = new List<string> { "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "I", "I", "I", "I", "I", "I", "O", "O", "O", "O", "O", "O", "O", "O", "O", "U", "U", "U", "U", "U", "Y" }; break;
                    default: caca = new List<string> { "A", "A", "A", "A", "A", "A", "A", "A", "A", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "I", "I", "I", "I", "I", "I", "I", "I", "I", "O", "O", "O", "O", "O", "O", "O", "O", "U", "U", "U", "U", "Y", "Y" }; break;
                }
            }
            else
            {
                switch (lang)
                {
                    case "fr": caca = new List<string> { "B", "B", "C", "C", "D", "D", "D", "F", "F", "G", "G", "H", "H", "J", "K", "L", "L", "L", "L", "L", "M", "M", "M", "N", "N", "N", "N", "N", "N", "P", "P", "Q", "R", "R", "R", "R", "R", "R", "S", "S", "S", "S", "S", "S", "T", "T", "T", "T", "T", "T", "V", "V", "W", "X", "Z" }; break;
                    //case "nl": caca = new List<string> { "B", "B", "C", "C", "D", "D", "D", "D", "D", "F", "G", "G", "G", "H", "H", "J", "J", "J", "J", "K", "K", "K", "L", "L", "L", "M", "M", "M", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "P", "P", "Q", "R", "R", "R", "R", "R", "S", "S", "S", "S", "T", "T", "T", "T", "T", "V", "V", "W", "W", "X", "Z", "Z" }; break;
                    case "en": caca = new List<string> { "B", "B", "C", "C", "D", "D", "D", "D", "F", "F", "G", "G", "G", "H", "H", "J", "K", "L", "L", "L", "L", "M", "M", "N", "N", "N", "N", "N", "N", "P", "P", "Q", "R", "R", "R", "R", "R", "R", "S", "S", "S", "S", "T", "T", "T", "T", "T", "T", "V", "V", "W", "W", "X", "Z" }; break;
                    //case "de": caca = new List<string> { "B", "B", "C", "C", "D", "D", "D", "D", "F", "F", "G", "G", "G", "H", "H", "H", "H", "J", "L", "L", "L", "M", "M", "M", "M", "N", "N", "N", "N", "N", "N", "N", "N", "N", "P", "Q", "R", "R", "R", "R", "R", "R", "S", "S", "S", "S", "S", "S", "S", "T", "T", "T", "T", "T", "T", "V", "W", "X", "Z" }; break;
                    //case "it": caca = new List<string> { "B", "B", "B", "C", "C", "C", "C", "D", "D", "D", "F", "F", "G", "G", "G", "H", "H", "J", "K", "L", "L", "L", "L", "L", "M", "M", "M", "M", "M", "N", "N", "N", "N", "N", "N", "P", "P", "P", "Q", "R", "R", "R", "R", "R", "R", "S", "S", "S", "S", "S", "S", "T", "T", "T", "T", "T", "T", "V", "V", "V", "V", "W", "X", "Z", "Z" }; break;
                    //case "pt": caca = new List<string> { "B", "B", "B", "C", "C", "C", "C", "D", "D", "D", "D", "D", "F", "F", "G", "G", "H", "H", "J", "J", "L", "L", "L", "L", "L", "M", "M", "M", "M", "M", "M", "N", "N", "N", "N", "P", "P", "P", "P", "Q", "R", "R", "R", "R", "R", "R", "S", "S", "S", "S", "S", "S", "S", "S", "T", "T", "T", "T", "T", "V", "V", "X", "Z" }; break;
                    //case "es": caca = new List<string> { "B", "B", "C", "C", "C", "C", "D", "D", "D", "D", "D", "F", "G", "G", "H", "H", "J", "L", "L", "L", "L", "L", "L", "L", "M", "M", "N", "N", "N", "N", "N", "N", "P", "P", "Q", "R", "R", "R", "R", "R", "R", "R", "S", "S", "S", "S", "S", "S", "T", "T", "T", "T", "V", "X", "Z" }; break;
                    default: caca = new List<string> { "B", "B", "C", "C", "D", "D", "D", "D", "F", "F", "G", "G", "G", "H", "H", "J", "K", "L", "L", "L", "L", "M", "M", "N", "N", "N", "N", "N", "N", "P", "P", "Q", "R", "R", "R", "R", "R", "R", "S", "S", "S", "S", "T", "T", "T", "T", "T", "T", "V", "V", "W", "W", "X", "Z" }; break;
                }
            }
            return caca;
        }

        public List<string> NomsAdversaires()
        {
            List<string> caca;
            switch (lang)
            {
                case "fr": caca = new List<string> { "Roger", "Pascal", "Bertrand", "René", "Serge", "Louis", "Emile", "Patrick", "Jean", "Jacques", "Paul", "Pierre", "Thomas", "Florian", "Mathieu", "Sasha", "Emma", "Valentin" }; break;
                //case "nl": caca = new List<string> { "Jasper", "Bauke", "Tom", "Nick", "Thijs", "Rik", "Joost", "Chris", "Laura", "Marieke", "Iris", "Kim", "Emma", "Sasha", "Julia" }; break;
                case "en": caca = new List<string> { "James", "Barney", "Patrick", "Michael", "Wiliam", "David", "Tyler", "Kayla", "Justin", "Brian", "Dylan", "Caleb", "Paul", "Emma", "Sasha" }; break;
                //case "de": caca = new List<string> { "Jan", "Fabian", "Ludwig", "Marcel", "John", "Tony", "Paul", "Pascal", "Stefan", "Eva", "Julia", "Anna", "Sarah", "Anne", "Leonie", "Marie" }; break;
                //case "it": caca = new List<string> { "Damiano", "Riccardo", "Marco", "Luca", "Danilo", "Matteo", "Daniele", "Filippo", "Sara", "Giorgia", "Silvia", "Giulia", "Elena", "Sasha", "Simona", "Camilla" }; break;
                //case "pt": caca = new List<string> { "Pedro", "Rui", "Ricardo", "Miguel", "Marco", "Carlos", "Filipe", "Antonio", "Tiago", "Ana", "Rita", "Marta", "Maria", "Joana", "Telma", "Sara" }; break;
                //case "es": caca = new List<string> { "Alejandro", "Pablo", "Pedro", "Diego", "Daniel", "Sergio", "David", "Raul", "Jesus", "Maria", "Marta", "Laura", "Sara", "Ana", "Eva", "Lucia" }; break;
                default: caca = new List<string> { "James", "Barney", "Patrick", "Michael", "Wiliam", "David", "Tyler", "Kayla", "Justin", "Brian", "Dylan", "Caleb", "Paul", "Emma", "Sasha" }; break;
            }
            return caca;
        }
    }
}
