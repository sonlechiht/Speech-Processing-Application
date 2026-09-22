using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SpeechProcessing.Styles
{
    /// <summary>
    /// Interaction logic for ProcessBarDotStyle.xaml
    /// </summary>
    public partial class ProcessBarDotStyle : UserControl
    {
        private BackgroundWorker worker = null;
        private int maxPercent = 0;
        private int currentPercent = 0;
        private string UIMessage = "";
        private int waitingTime = 300;


        public ProcessBarDotStyle()
        {
            InitializeComponent();

            System.Windows.Media.Animation.Storyboard s = (System.Windows.Media.Animation.Storyboard)TryFindResource("mystoryboard");
            s.Begin();
        }

        public void SetMessage(string message)
        {
            this.textBlockMessage.Text = message;
            UIMessage = message;
        }


        public void StartRenderPercent()
        {
            if (worker == null)
            {
                worker = new BackgroundWorker();
            }

            maxPercent = 0;
            currentPercent = 0;

            //worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = 0;
            while (true)
            {
                if (maxPercent > 100)
                    maxPercent = 100;
                if (currentPercent > 100)
                    currentPercent = 100;

                if (maxPercent == 100 && currentPercent == 100)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        textBlockMessage.Text = UIMessage + currentPercent + "%";
                    }),
                    System.Windows.Threading.DispatcherPriority.Background, null);
                    break;
                }

                if (currentPercent == maxPercent)
                {
                    count++;

                    if (count == 100)
                    {
                        maxPercent++;
                        count = 0;
                    }

                    System.Threading.Thread.Sleep(100);
                }
                else
                {
                    currentPercent++;
                    if (currentPercent > 100)
                        currentPercent = 100;

                    if (maxPercent < 100 && currentPercent >= 100)
                        currentPercent = 99;

                    count = 0;

                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        textBlockMessage.Text = UIMessage + " " + currentPercent + "%";
                    }),
                    System.Windows.Threading.DispatcherPriority.Background, null);
                    System.Threading.Thread.Sleep(waitingTime);
                }
            }
        }

        public void StopRenderPercent()
        {
            if (worker != null)
            {
                worker.Dispose();
                worker = null;
            }
        }

        public void SetPercent(int percent)
        {
            currentPercent = percent;
            textBlockMessage.Text = UIMessage + currentPercent + "%";
        }

        public void UpdatePercent(int percent)
        {
            maxPercent = percent;

            if (maxPercent > currentPercent)
                waitingTime = (int)(1000 * 1.0 / (maxPercent - currentPercent)) + 1;
            else
                waitingTime = 1000;

        }
    }
}
