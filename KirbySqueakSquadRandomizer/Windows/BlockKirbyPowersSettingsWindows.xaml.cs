using KirbySqueakSquadRandomizer.Ram;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
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
    public partial class BlockKirbyPowersSettingsWindows : Window
    {
        string romPath = "";
        private readonly Thread _gameWatchingThread = null;
        private bool _terminateThread = false;

        private readonly object _gameLock = new object();
        public BlockKirbyPowersSettingsWindows()
        {
            InitializeComponent();
            this.Show();
            Thread.Sleep(1000);
            KirbySqueakSquadConnector kssc = new KirbySqueakSquadConnector();
            kssc.DoConnect();
            bool pollSuccessful = false;
            _gameWatchingThread = new Thread(() =>
            {
                while (!_terminateThread)
                {
                    try
                    {
                        pollSuccessful = kssc.DoPoll();
                    }
                    catch (ProcessRamWatcherException e)
                    {
                        Logger.Debug(e.Message);
                    }

                    Thread.Sleep(100);
                }
            });
            _gameWatchingThread.IsBackground = true;
            _gameWatchingThread.Start();

        }

        

    }
}
