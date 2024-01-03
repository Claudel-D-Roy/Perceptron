using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using TP3_OCR_WPF.BLL;

namespace TP3_OCR_WPF.DAL
{
    /// <summary>
    /// Auteur : Claudel D. Roy
    /// Cette classe gère l'accès aux disques pour le fichiers d'apprentissages. 
    /// Permet de charger ou décharger dans la matrice d'apprentissage.
    /// </summary>
    public class GestionFichiersSorties : IFichierSortant
    {
        private List<CoordDessin> _lstCoord;
        public LstApprentissageAuto _lstApp;

        /// <summary>
        /// Auteur : Claudel D. Roy
        /// Permet d'extraire un fichier texte dans une matrice pour l'apprentissage automatique.
        /// Date: 2032-04-26
        /// </summary>
        /// <param name="fichier">Fichier où extraire les données</param>
        public List<CoordDessin> ChargerCoordonnees(string fichier)
        {
            string Path = AppDomain.CurrentDomain.BaseDirectory;
            int index = Path.IndexOf("bin");
            Path = Path.Substring(0, index);
            string pathEchantillons = "DAL\\Train\\";
            string fullPath = Path + pathEchantillons + fichier;

            List<CoordDessin> lstCoord = new List<CoordDessin>();
            _lstApp = new LstApprentissageAuto();
   
            using (StreamReader lecteur = new StreamReader(fullPath))
            {

                string sLigne = "";
          
                string[] sTabElement = null;


                while (!lecteur.EndOfStream)
                {
                    sLigne = lecteur.ReadLine();
                    _lstApp.NbElement = int.Parse(sLigne);

                    sLigne = lecteur.ReadLine();
                    _lstApp.NbAttribut = int.Parse(sLigne);

      

                    if (_lstApp.NbElement == 0)
                        break;

                    _lstApp.Elements = new double[_lstApp.NbElement, _lstApp.NbAttribut - 1];
                    _lstApp.Resultat = new int[_lstApp.NbElement];


                    for (int i = 0; i < _lstApp.NbElement; i++)
                    {
                        CoordDessin coordCourant = new CoordDessin(CstApplication.TAILLEDESSINX, CstApplication.TAILLEDESSINY);
                        BitArray ba = coordCourant.BitArrayDessin;
                        sLigne = lecteur.ReadLine();
                        sTabElement = sLigne.Split("\t");
    
                        for (int j = 0; j < sTabElement.Length - 2; j++)
                        {
                            _lstApp.Elements[i, j] = Convert.ToDouble(sTabElement[j]);
                            bool valeur = int.Parse(sTabElement[j]) == 1 ? true : false;
                            ba.Set(j, valeur);
                        }
                        _lstApp.Resultat[i] = Convert.ToInt32(sTabElement[sTabElement.Length - 1]);
                        coordCourant.Reponse = _lstApp.Resultat[i].ToString();
                       
                    lstCoord.Add(coordCourant);
                    }
                }
            }

            return lstCoord;
        }


        /// <summary>
        /// Auteur : Claudel D. Roy
        /// Permet de sauvegarder dans fichier texte dans une matrice pour l'apprentissage automatique
        /// Date: 2023-04-26
        /// </summary>
        /// <param name="fichier">Fichier où extraire les données</param>
        public int SauvegarderCoordonnees(string fichier, List<CoordDessin> lstCoord)
        {
            int count = lstCoord.Count - 1;
     
            string Path = AppDomain.CurrentDomain.BaseDirectory;

            int index = Path.IndexOf("bin");
            Path = Path.Substring(0, index);

            string pathEchantillions = "DAL\\Train\\";
            string fullPath = Path + pathEchantillions + fichier;
            int nbLigne = File.ReadLines(fullPath).Count();

            if (nbLigne >= 2)
                nbLigne = nbLigne - 2;

            nbLigne = lstCoord.Count;
                

            string[] lignes = File.ReadAllLines(fullPath);
            lignes[0] = nbLigne.ToString();
            lignes[1] = "256";
            File.WriteAllLines(fullPath, lignes);


            
                CoordDessin coordList = lstCoord[count];
                BitArray bA = coordList.BitArrayDessin;
                List<string[]> lstString = new List<string[]>();
                string reponse = "";

                foreach (var element in bA)
                {
                    if ((bool)element)
                        reponse = "1";
                    else
                        reponse = "0";

                    string[] sArray = new string[] { reponse };

                    lstString.Add(sArray);

                }


                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                    for (int j = 0; j < lstString.Count; j++)
                    {
                        string s = lstString[j][0];
                        sw.Write(s + "\t");
                    }
                    sw.Write(coordList.Reponse);

                }
            
            MelangerEchantillon(lstCoord);


            return CstApplication.OK;
        }


        /// <summary>
        /// Permet d'extraire un fichier texte dans une matrice pour l'apprentissage automatique.
        /// </summary>
        public IList<CoordDessin> ObtenirCoordonnees()
        {

            return _lstCoord;
        }


        /// <summary>
        /// Permet de mélanger aléatoirement les échantillons d'apprentissages(coordonnées) dans le but d'améliorer l'apprentissage.
        /// </summary>
        /// <param name="lstCoord">Les coordonnées à mélanger</param>
        private void MelangerEchantillon(List<CoordDessin> lstCoord)
        {
            Random r1 = new Random();
            Random r2 = new Random();
            int index1;
            int index2;
            CoordDessin coordTemp;

            for (int i = 0; i < CstApplication.MAXITERATION; i++)
            {
                index1 = r1.Next(lstCoord.Count);
                index2 = r2.Next(lstCoord.Count);

                coordTemp = lstCoord[index1];
                lstCoord[index1] = lstCoord[index2];
                lstCoord[index2] = coordTemp;
            }
        }

    }

}