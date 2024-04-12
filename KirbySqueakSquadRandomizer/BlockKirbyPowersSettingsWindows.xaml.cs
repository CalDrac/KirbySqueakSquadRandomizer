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
            try
            {
                kssc.DoConnect();
                Game_status_value.Content = "Active";
            }
            catch
            {
                Game_status_value.Content = "Inactive";
            }
            bool pollSuccessful = false;
            _gameWatchingThread = new Thread(() =>
            {
                while (!_terminateThread)
                {
                    try
                    {
                        pollSuccessful = kssc.DoPoll();
                        if (pollSuccessful)
                        {
                            string[] tab = new string[1];
                            tab[0] = "Active";
                            Dispatcher.BeginInvoke(UpdateStatus, tab);
                        }
                    }
                    catch (ProcessRamWatcherException e)
                    {
                        Logger.Debug(e.Message);
                        try
                        {
                            kssc.DoConnect();
                            pollSuccessful = kssc.DoPoll();
                            if (pollSuccessful)
                            {
                                string[] tab = new string[1];
                                tab[0] = "Active";
                                Dispatcher.BeginInvoke(UpdateStatus, tab);
                            }
                        }
                        catch (Exception e2)
                        {
                            string[] tab = new string[1];
                            tab[0] = "Inactive";
                            Dispatcher.BeginInvoke(UpdateStatus, tab);
                        }
                    }

                    Thread.Sleep(100);
                }
            });
            _gameWatchingThread.IsBackground = true;
            _gameWatchingThread.Start();

        }
        private void UpdateStatus(string msg)
        {
            Game_status_value.Content = msg;
        }
    }
}
