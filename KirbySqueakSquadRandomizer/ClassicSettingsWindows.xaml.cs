using Microsoft.Win32;
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

namespace KirbySqueakSquadRandomizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ClassicSettingsWindow : Window
    {
        string romPath = "";
        public ClassicSettingsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "NDS rom (*.nds)|*.nds";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (!openFileDialog1.ShowDialog().Value)
            {
                return;
            }

            string selectedFileName = openFileDialog1.FileName;
            labelFilename.Content = selectedFileName;
            romPath = selectedFileName;
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            var opt = new Options();
            if (string.IsNullOrEmpty(romPath))
            {
                MessageBox.Show("Select a ROM");
                return;
            }
            SetOptions(opt);
            if (Generator.generateNewRomClassic(opt))
            {
                
                this.Close();
                return;
            }

        }

        private void SetOptions(Options opt)
        {
            opt.path = romPath;
            opt.isBossW1_6Randomized = bossRandomizerW1_6Check.IsChecked.Value;
        }

 
    }
}
