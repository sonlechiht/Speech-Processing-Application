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

namespace SpeechProcessing.Views
{
    /// <summary>
    /// Interaction logic for UserControl_SplashScreen.xaml
    /// </summary>
    public partial class UserControl_SplashScreen : UserControl
    {
        public UserControl_SplashScreen()
        {
            InitializeComponent();
            System.Windows.Media.Animation.Storyboard s = (System.Windows.Media.Animation.Storyboard)TryFindResource("mystoryboard");
            s.Begin();
        }

        private void stackPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (stackPanel.Visibility == Visibility.Collapsed)
                this.Visibility = Visibility.Collapsed;
        }
    }
}
