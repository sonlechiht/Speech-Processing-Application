using SpeechProcessing.Models;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpeechProcessing.Views
{
    /// <summary>
    /// Interaction logic for ActiveLicense.xaml
    /// </summary>
    public partial class ActiveLicense : Window
    {
        private string License = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "LicenseKey.txt");
        public bool Active = false;
        public ActiveLicense()
        {
            InitializeComponent();
        }
        public bool GetActive()
        {
            return Active;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SpeechProcessing.Models.LicenseKey.ListKey.IndexOf(licenseKey.Text) != -1)
                {
                    
                    try 
                    {
                        
                        File.WriteAllText(License, licenseKey.Text);
                        Active = true;
                        System.Windows.Forms.MessageBox.Show("Active successfully!");
                        this.DialogResult = true;
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("Run application with administrator.");
                        this.Close();
                    }
                    
                }
                else 
                {
                    System.Windows.Forms.MessageBox.Show("Fail!");
                }
                //this.DialogResult = true;
            }
            catch (Exception)
            {
                this.Close();
            }
        }
    }
}
