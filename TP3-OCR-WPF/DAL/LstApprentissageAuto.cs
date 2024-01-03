using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3_OCR_WPF.DAL
{
    /// <summary>
    /// Auteur : Claudel D. Roy
    /// Classe pour les propriétés des fichiers.
    /// Date : 2023-04-26
    /// </summary>
    public class LstApprentissageAuto
    {


        public int NbElement { get; set; }
        public int NbAttribut { get; set; }
        public double[,] Elements { get; set; }
        public int[] Resultat { get; set; }
   
    }
}
