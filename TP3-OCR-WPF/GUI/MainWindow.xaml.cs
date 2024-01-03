using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TP3_OCR_WPF.DAL;

namespace TP3_OCR_WPF
{
    /// <summary>
    /// Permet d'afficher l'interface pour la reconnaissance de caractères. 
    /// Cet interface fera appel au Perceptron pour identifier le caractère dessiné.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Le gestionnaire des perceptrons.
        private GestionClassesPerceptrons _gcpAnalyseEcriture;
        public MainWindow()
        {
            InitializeComponent();

            ucDessin.Width = CstApplication.TAILLEDESSINX + 6;
            ucDessin.Height = CstApplication.TAILLEDESSINY + 6;

            _gcpAnalyseEcriture = new GestionClassesPerceptrons();
        }

        /// <summary>
        /// Efface le caractère dessiné et sa matrice.
        /// </summary>
        /// <param name="sender">L'objet qui à envoyé cet événement.</param>
        /// <param name="e">Les arguments de cet événement.</param>

        private void btnEffacer_Click(object sender, RoutedEventArgs e)
        {
            ucDessin.EffacerDessin();
        }
        /// <summary>
        /// Entraine le bon Perceptron avec la valeur entrée dans le TextBox txtValeurEntrainee et le caractère dessiné.
        /// </summary>
        /// <param name="sender">L'objet qui à envoyé cet événement.</param>
        /// <param name="e">Les arguments de cet événement.</param>
        private void btnEntrainement_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("test");
            ucDessin.Coordonnees.Reponse = txtValeurEntrainee.Text;
            txtConsole.Text =  _gcpAnalyseEcriture.Entrainement(ucDessin.Coordonnees, ucDessin.Coordonnees.Reponse);
        }

        /// <summary>
        /// Appel le perceptron pour vérifier quel neuronne identifie le mieux le caractère dessiné.
        /// </summary>
        /// <param name="sender">L'objet qui à envoyé cet événement.</param>
        /// <param name="e">Les arguments de cet événement.</param>
        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            
            string caractereIdentifie = _gcpAnalyseEcriture.TesterPerceptron(ucDessin.Coordonnees);

            txtConsole.Text = "Le caractère identifié est : " + caractereIdentifie;
        }
    }
}
