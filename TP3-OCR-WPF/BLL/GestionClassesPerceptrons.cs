using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TP3_OCR_WPF.BLL;
using TP3_OCR_WPF.DAL;

namespace TP3_OCR_WPF
{
    /// <summary>
    /// Gère les fonctionnalités de l'application. Entre autre, permet de :
    /// - Charger les échantillons d'apprentissage,
    /// - Sauvegarder les échantillons d'apprentissage,
    /// - Tester le perceptron
    /// - Entrainer le perceptron
    /// </summary>
    public class GestionClassesPerceptrons : IGestionnairePerceptron
    {
        private Dictionary<string, Perceptron> _lstPerceptrons;
        private IFichierSortant _gestionSortie;
        private List<CoordDessin> _coordDessinList;

        /// <summary>
        /// Constructeur
        /// </summary>
        public GestionClassesPerceptrons()
        {
            _lstPerceptrons = new Dictionary<string, Perceptron>();
            _gestionSortie = new GestionFichiersSorties();
            _coordDessinList = new List<CoordDessin>();

            string fichier = "train.txt";
            ChargerCoordonnees(fichier);

        }

        /// <summary>
        /// Charge les échantillons d'apprentissage sauvegardé sur le disque.
        /// </summary>
        /// <param name="fichier">Le nom du fichier</param>
        public void ChargerCoordonnees(string fichier)
        {
            _coordDessinList = _gestionSortie.ChargerCoordonnees(fichier);
       
        }

        /// <summary>
        /// Sauvegarde les échantillons d'apprentissage sauvegardé sur le disque.
        /// </summary>
        /// <param name="fichier">Le nom du fichier</param>
        /// <returns>En cas d'erreur retourne le code d'erreur</returns>
        public int SauvegarderCoordonnees(string fichier)
        {

            return _gestionSortie.SauvegarderCoordonnees(fichier, _coordDessinList);
        
        }

        /// <summary>
        /// Entraine les perceptrons avec un nouveau caractère
        /// </summary>
        /// <param name="coordo">Les nouvelles coordonnées</param>
        /// <param name="reponse">La réponse associé(caractère) aux coordonnées</param>
        /// <returns>Le résultat de la console</returns>
        public string Entrainement(CoordDessin coordo, string reponse)
        {
            string sConsole = "";
            _coordDessinList.Add(coordo);

            // Si le perceptron n'existe pas, le créer
            if (!_lstPerceptrons.ContainsKey(reponse))
            {
                _lstPerceptrons.Add(reponse, new Perceptron(reponse));
                
            }

            foreach (KeyValuePair<string, Perceptron> perceptron in _lstPerceptrons)
            {
                if(perceptron.Key == reponse)
                {
                    List<CoordDessin> lstCoord = _coordDessinList.Where(x => x.Reponse == reponse).ToList();
                    sConsole =  perceptron.Value.Entrainement(lstCoord);

                }

            }
            SauvegarderCoordonnees("train.txt");
            return sConsole;
        }


        /// <summary>
        /// Test le perceptron avec de nouvelles coordonnées.
        /// </summary>
        /// <param name="coord">Les nouvelles coordonnées</param>
        /// <returns>Retourne la liste des valeurs possibles du perceptron</returns>
        public string TesterPerceptron(CoordDessin coord)
        {
            string sConsole = "";
            bool reponse = false;
            
            foreach (KeyValuePair<string, Perceptron> perceptron in _lstPerceptrons)
            {
                if (reponse) { break; }

                if (perceptron.Value.TesterNeurone(coord))
                {
                    sConsole += perceptron.Key;
                    reponse = true;
                }
                else
                    sConsole = "?";
            }
    
            return sConsole;

        }

        /// <summary>
        /// Obtient une liste des coordonées.
        /// </summary>
        /// <returns>Une liste des coordonées.</returns>
        public IList<CoordDessin> ObtenirCoordonnees()
        {
            return _gestionSortie.ObtenirCoordonnees();
        }
    }
}
