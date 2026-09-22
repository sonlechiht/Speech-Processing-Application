using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;

namespace SpeechProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ConfigFileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "appconfig.xml");
        private string License = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "LicenseKey.txt");
        private bool Active = false;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                if (File.Exists(License))
                {
                    string key = File.ReadAllText(License);
                    if (SpeechProcessing.Models.LicenseKey.ListKey.IndexOf(key) != -1)
                    {
                        Active = true;
                        activeLicense.Visibility = Visibility.Collapsed;
                    }
                }
                if (!Active)
                    if (IsExpired())
                    {
                        main.Visibility = Visibility.Collapsed;
                        expiration.Visibility = Visibility.Visible;
                        //MessageBox.Show("Your trial period has expired.", "Trial Ended", MessageBoxButton.OK, MessageBoxImage.Warning);
                        //Application.Current.Shutdown();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Run application with administrator.");
                Application.Current.Shutdown();
            }
            
        }
        private bool IsExpired()
        {
            try
            {
                XDocument doc = XDocument.Load(ConfigFileName);
                DateTime installDate = DateTime.Parse(doc.Root.Element("InstallDate").Value);
                DateTime expirationDate = installDate.AddDays(3);

                var remaining = expirationDate - DateTime.Now;
                activeLicense.Text = String.Format("Not activated - Only {0} days {1} hours", remaining.Days, remaining.Hours);
                return DateTime.Now > expirationDate;
            }
            catch (FileNotFoundException)
            {
                // Handle first-time installation
                CreateConfigFile();
                XDocument doc = XDocument.Load(ConfigFileName);
                DateTime installDate = DateTime.Parse(doc.Root.Element("InstallDate").Value);
                DateTime expirationDate = installDate.AddDays(3);

                var remaining = expirationDate - DateTime.Now;
                activeLicense.Text = String.Format("Not activated - Only {0} days {1} hours", remaining.Days, remaining.Hours);
                return false;
            }
        }

        private void CreateConfigFile()
        {
            XDocument doc = new XDocument(
                new XElement("AppConfiguration",
                    new XElement("InstallDate", DateTime.Now.ToString())
                )
            );
            doc.Save(ConfigFileName);
        }
        public void SwitchTab(string syntax)
        { 
            userControl_Introduction.Visibility = Visibility.Collapsed;
            switch (syntax)
            {
                case "STT":
                    {
                        userControl_TextToSpeech.Visibility = Visibility.Collapsed;
                        userControl_SpeechToText.Visibility = Visibility.Visible;
                        break;
                    }
                case "TTS":
                    {
                        userControl_SpeechToText.Visibility = Visibility.Collapsed;
                        userControl_TextToSpeech.Visibility = Visibility.Visible;
                        break;
                    }
                default:
                    break;
            }
        }

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState==WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                btnMaximize.Visibility = Visibility.Collapsed;
                btnRestore.Visibility = Visibility.Visible;
            }
        }
        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                btnMaximize.Visibility = Visibility.Visible;
                btnRestore.Visibility = Visibility.Collapsed;
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnSTT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SwitchTab("STT");
            }
            catch (Exception)
            {
            }
        }

        private void btnTTS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SwitchTab("TTS");
            }
            catch (Exception)
            {
            }
        }

        private void Active_License(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var window = new Views.ActiveLicense();
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
                //viewModel.DefectClasses = window.GetDefectNames();
                if (window.GetActive())
                {
                    activeLicense.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnActive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new Views.ActiveLicense();
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
                //viewModel.DefectClasses = window.GetDefectNames();
                if (window.GetActive())
                {
                    activeLicense.Visibility = Visibility.Collapsed;
                    expiration.Visibility = Visibility.Collapsed;
                    main.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
