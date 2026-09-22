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
using System.Windows.Threading;

namespace SpeechProcessing.Views
{
    /// <summary>
    /// Interaction logic for UserControl_SpeechToText.xaml
    /// </summary>
    public partial class UserControl_SpeechToText : UserControl
    {
        private bool IsPlaying = false;
        private bool IsUserDraggingSlider = false;

        private readonly DispatcherTimer Timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.1) };
        public UserControl_SpeechToText()
        {
            InitializeComponent();
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }
        private void ShowProcessBar(string message)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                Application.Current.MainWindow.IsEnabled = false;
                Mouse.OverrideCursor = Cursors.Wait;

                this.borderProcessBarDotStyle.Visibility = Visibility.Visible;
                this.processBarDotStyle.SetMessage(message);

                this.processBarDotStyle.StartRenderPercent();

                //this.gridMain.Visibility = Visibility.Hidden;
            }),
            System.Windows.Threading.DispatcherPriority.Background, null);

        }
        private void HideProcessBar()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                Application.Current.MainWindow.IsEnabled = true;
                Mouse.OverrideCursor = Cursors.Arrow;

                this.borderProcessBarDotStyle.Visibility = Visibility.Collapsed;
                this.processBarDotStyle.SetMessage("");

                this.processBarDotStyle.StopRenderPercent();

                this.gridMain.Visibility = Visibility.Visible;

            }),
            System.Windows.Threading.DispatcherPriority.Background, null);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (viewModel.Player.Source != null && viewModel.Player.NaturalDuration.HasTimeSpan && !IsUserDraggingSlider)
            {
                ProgressSlider.Maximum = viewModel.Player.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressSlider.Value = viewModel.Player.Position.TotalSeconds;
                if (ProgressSlider.Maximum == ProgressSlider.Value)
                {
                    btnPause.Visibility = Visibility.Collapsed;
                    btnPlay.Visibility = Visibility.Visible;
                    viewModel.Player.Pause();
                    viewModel.Player.Position = TimeSpan.Zero;
                }
            }
        }

        private void Button_LoadData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var grid = button.Parent as Grid;

                StackPanel stackPanel = null;

                for (int i = 0; i < grid.Children.Count; i++)
                {
                    if ((grid.Children[i] is StackPanel) == false)
                        continue;
                    if ((grid.Children[i] as StackPanel).Tag.ToString() != button.Tag.ToString())
                        continue;

                    stackPanel = grid.Children[i] as StackPanel;
                    break;
                }

                TextBlock textBlock0 = (stackPanel.Children[0] as TextBlock);
                TextBlock textBlock1 = (stackPanel.Children[1] as TextBlock);

                var dlg = new System.Windows.Forms.OpenFileDialog();
                dlg.Title = "Select Speech Sound File";
                dlg.Filter = "Speech Sound File (MP3, WAV)|*.mp3;*.wav";

                if (string.IsNullOrEmpty(textBlock0.Text) == false)
                {
                    var folder = System.IO.Path.GetDirectoryName(textBlock0.Text);
                    if (System.IO.Directory.Exists(folder))
                        dlg.InitialDirectory = folder;
                }

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //textBlock0.Text = dlg.FileName;
                    //textBlock0.ToolTip = dlg.FileName;
                    viewModel.LoadData(dlg.FileName);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ProgressSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            IsUserDraggingSlider = true;
        }

        private void ProgressSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            IsUserDraggingSlider = false;
            viewModel.Player.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            StatusLbl.Text = TimeSpan.FromSeconds(ProgressSlider.Value).ToString(@"hh\:mm\:ss");
        }

        private void Button_Processing_Click(object sender, RoutedEventArgs e)
        {
            if (IsPlaying)
            {
                btnPause.Visibility = Visibility.Collapsed;
                btnPlay.Visibility = Visibility.Visible;
                viewModel.Player.Pause();
            }
            ShowProcessBar("Processing...");
            viewModel.ProcessCompleted += ViewModel_ImportNewData_ProcessCompleted;
            viewModel.SpeechToText();

        }
        private void ViewModel_ImportNewData_ProcessCompleted(object sender, EventArgs e)
        {
            viewModel.ProcessCompleted -= ViewModel_ImportNewData_ProcessCompleted;
            HideProcessBar();
        }
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            
            if (IsPlaying)
            {
                btnPause.Visibility = Visibility.Collapsed;
                btnPlay.Visibility = Visibility.Visible;
                viewModel.Player.Pause();
            }
                
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Player?.Source != null)
            {
                btnPause.Visibility = Visibility.Visible;
                btnPlay.Visibility = Visibility.Collapsed; 
                viewModel.Player.Play();
                IsPlaying = true;
            }
        }

        private void btnGuide_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Step 1: Load file waveform (.mp3, .wav ).\nStep 2: Press Process (wait until processing is complete)";
            string caption = "Speech To Text information";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }
    }
}
