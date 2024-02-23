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
    public partial class ArchipelagoSettingsWindow : Window
    {
        string romPath;
        
        public ArchipelagoSettingsWindow()
        {
            romPath = "";
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Aller chercher la rom
        }

        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            //Modifier la rom
            Generator.generateNewRomArchipelago(ServerName.Text, Username.Text, Password.Text,romPath);

        }

    }
}
