using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using TP3_OCR_WPF.DAL;

namespace TP3_OCR_WPF.BLL
{
    /// <summary>
    /// Auteur : Claudel D. Roy
    /// Classe du perceptron. Permet de faire l'apprentissage automatique sur un échantillon d'apprentissage. 
    /// </summary>
    public class Perceptron
    {
        private double _cstApprentissage = CstApplication.CONSTANTEAPPRENTISSAGE;
        private double[] _poidsSynaptique;
        private string _reponse = "?";
        private int MAX_ITERATION = CstApplication.MAXITERATION;



        public string Reponse
        {
            get { return _reponse; }
        }

        /// <summary>
        /// Constructeur de la classe. Crée un perceptron pour une réponse(caractère) qu'on veut identifier le pattern(modèle)
        /// </summary>
        /// <param name="reponse">La classe que défini le perceptron</param>
        public Perceptron(string reponse)
        {
            _reponse = reponse;
            _poidsSynaptique = new double[256];
        }

        /// <summary>
        /// Faire l'apprentissage sur un ensemble de coordonnées. Ces coordonnées sont les coordonnées de tous les caractères analysés. 
        /// </summary>
        /// <param name="coord">La liste de coordonnées pour les caractères à analysés.</param>
        /// <returns>Les paramètres de la console</returns>
        public string Entrainement(List<CoordDessin> coord)
        {
            string sResultat = "";
            int iNbItération = 0;
            int iNbErreur = 0;
            int iResultatEstimer = 0;
            int iErreurLocal = 0;
            int count = 0;
            int ligne = 0;
            int colonne = 0;
            int reponse = 0;
            double dSum = 0.00d;
            double dPourcentageSucces = 0.00d;


            Random rnd = new Random();
            _poidsSynaptique = new double[256];
            for (int i = 0; i < 256; i++)
                _poidsSynaptique[i] = rnd.NextDouble();

            do
            {
                for (int i = 0; i < coord.Count; i++)
                {
                    iNbErreur = 0;
                    List<double> lstDouble = new List<double>();

                    foreach (var element in coord[i].BitArrayDessin)
                    {

                        if ((bool)element)
                            reponse = 1;
                        else
                            reponse = 0;

                        double SArray = reponse;

                        lstDouble.Add(SArray);
                        count++;
                    }
                    for (int m = 0; m < lstDouble.Count; m++)
                    {
                        
                        dSum = _poidsSynaptique[0];
                        for (int j = 1; j < _poidsSynaptique.Length; j++)
                            dSum += _poidsSynaptique[j] * lstDouble[j - 1];

                        iResultatEstimer = (dSum >= 0) ? 1 : 0;
                        iErreurLocal = ((int)lstDouble[m] - iResultatEstimer);

                        if (iErreurLocal != 0)
                        {
                            _poidsSynaptique[0] += _cstApprentissage * iErreurLocal;
                            for (int k = 1; k < _poidsSynaptique.Length; k++)
                                _poidsSynaptique[k] += _cstApprentissage * iErreurLocal * lstDouble[k - 1];

                            iNbErreur++;
                        }

                    }

                }
                iNbItération++;
                sResultat += $"\r\nItération : {iNbItération} Erreur : {iNbErreur} ";
                dPourcentageSucces = ((double)(coord.Count * 256) - iNbErreur) / ((double)coord.Count * 256) * 100.00;
                sResultat += $"Taux de succès : {dPourcentageSucces} %";


            } while (iNbErreur > 0 && iNbItération < MAX_ITERATION);
            return sResultat;

        }

        /// <summary>
        /// Calcul la valeur(vrai ou faux) pour un les coordonnées d'un caractère. Permet au perceptron d'évaluer la valeur de vérité.
        /// </summary>
        /// <param name="vecteurSyn">Les poids synaptiques du perceptron</param>
        /// <param name="entree">Le vecteur de bit correspondant aux couleurs du caractère</param>
        /// <returns>Vrai ou faux</returns>
        public int ValeurEstime(double[] vecteurSyn, BitArray entree)
        {

            string sResultat = "";
            int iNbErreur = 0;
            int iResultatEstimer = 0;
            int iErreurLocal = 0;
            int count = 0;
            int ligne = 0;
            int colonne = 0;
            int reponse = 0;
            double dSum = 0.00d;
            double dPourcentageSucces = 0.00d;


            iNbErreur = 0;
            List<double> lstDouble = new List<double>();

            foreach (var element in entree)
            {

                if ((bool)element)
                    reponse = 1;
                else
                    reponse = 0;

                double SArray = reponse;

                lstDouble.Add(SArray);
                count++;
            }

            for (int m = 0; m < lstDouble.Count; m++)
            {

                for (int k = 0; k < lstDouble.Count; k++)
                {
                    dSum = _poidsSynaptique[0];
                    for (int j = 1; j < _poidsSynaptique.Length; j++)
                        dSum += _poidsSynaptique[j] * lstDouble[j - 1];


                    iResultatEstimer = (dSum >= 0) ? 1 : 0;
                    iErreurLocal = iResultatEstimer;

                    if (iErreurLocal != 0)
                        iNbErreur++;
                }
            }

            dPourcentageSucces = (double)(count - iNbErreur) / (double)count * 100.00;
            sResultat += $"Taux de succès : {dPourcentageSucces} %";

            return dPourcentageSucces >= CstApplication.POURCENTCONVERGENCE ? CstApplication.VRAI : CstApplication.FAUX;
        }

        /// <summary>
        /// Interroge la neuronnes pour un ensembles des coordonnées(d'un caractère).
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public bool TesterNeurone(CoordDessin coord)
        {

            if (ValeurEstime(_poidsSynaptique, coord.BitArrayDessin) == CstApplication.VRAI)
                return true;
            else
                return false;

        }
    }
}
