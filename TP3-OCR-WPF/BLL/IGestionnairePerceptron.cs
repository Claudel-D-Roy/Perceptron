using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3_OCR_WPF.BLL
{
    /// <summary>
    /// Auteur : Claudel D. Roy
    /// Interface pour la gestion des perceptrons
    /// </summary>
    public interface IGestionnairePerceptron
    {

        void ChargerCoordonnees(string fichier);
        int SauvegarderCoordonnees(string fichier);
        string Entrainement(CoordDessin coordo, string reponse);
        string TesterPerceptron(CoordDessin coord);

    }
}
