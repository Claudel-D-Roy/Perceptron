using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP3_OCR_WPF.BLL;

namespace TP3_OCR_WPF.DAL
{
    /// <summary>
    /// Auteur : Claudel D. Roy
    /// Interface pour les fichiers sortants.
    /// Date : 2023-04-26
    /// </summary>
    public interface IFichierSortant
    {
        List<CoordDessin> ChargerCoordonnees(string fichier);
        int SauvegarderCoordonnees(string fichier, List<CoordDessin> lstCoord);

        IList<CoordDessin> ObtenirCoordonnees();
    }
}
