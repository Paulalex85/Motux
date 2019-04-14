using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XMLObject;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;

namespace Motux
{
    public class XML_Serializer
    {
        public void SerialiseSauvegarde(Sauvegarde caca)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("Sauvegardes.xml", FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Sauvegarde));
                    using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                    {
                        serializer.Serialize(xmlWriter, caca);
                    }
                }
            }
        }

        public Sauvegarde DeserializeSauvegarde()
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("Sauvegardes.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Sauvegarde));
                        return (Sauvegarde)serializer.Deserialize(stream);
                    }
                }
            }
            catch
            {
                Parties newSave = new Parties()
                {
                    TypeMatch = 1,
                    RoundMatch = 1,
                    NombreDeManches = 1,
                    NombreManchesJouer = 1,
                    Difficulte = 1,
                    ScoreJoueur = "99",
                    ScoreAdversaire = "99",
                    NomAdversaire = "xxx"
                };

                Sauvegarde save = new Sauvegarde();
                save.ListeSave = newSave;
                return save;
            }
        }
    }

}
