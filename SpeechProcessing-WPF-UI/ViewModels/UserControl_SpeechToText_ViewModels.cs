using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;
using SpeechProcessing.Models;

namespace SpeechProcessing.ViewModels
{
    internal class UserControl_SpeechToText_ViewModels : BaseViewModel
    {
        public event EventHandler ProcessCompleted;
        private BackgroundWorker worker;

        private Thread runningThread = null;
        private bool isRunning = false;
        private string inputFolder = "Folder";
        public string InputFolder { get => inputFolder; set => SetProperty(ref inputFolder, value); }
        private string processButton = "Process";
        public string ProcessButton { get => processButton; set => SetProperty(ref processButton, value); }
        private bool processFlag = false;
        public bool ProcessFlag { get => processFlag; set => SetProperty(ref processFlag, value); }
        private bool buttonFlag = true;
        public bool ButtonFlag { get => buttonFlag; set => SetProperty(ref buttonFlag, value); }
        private string exceptionLoading;
        public string ExceptionLoading { get => exceptionLoading; set => SetProperty(ref exceptionLoading, value); }
        private Brush colorLoading = Brushes.Red;
        public Brush ColorLoading { get => colorLoading; set => SetProperty(ref colorLoading, value); }
        private string textPredict;
        public string TextPredict { get => textPredict; set => SetProperty(ref textPredict, value); }
        public MediaPlayer Player = new MediaPlayer();
        private string DataInput;
        public void LoadData(string selectedPath)
        {
            try
            {
                WorkingBase.WorkingFolder = Path.GetDirectoryName(selectedPath);
                InputFolder = selectedPath;

                Player = new MediaPlayer();
                Player.Open(new Uri(selectedPath));
                //Player.Play();
                ExceptionLoading = $"Load input successful.";
                ColorLoading = Brushes.Green;
                ProcessFlag = true;
                TextPredict = "";
            }
            catch (Exception ex)
            {
                ExceptionLoading = $"Load input false.\nException Information: {ex.Message}";
                ColorLoading = Brushes.Red;
                ProcessFlag = false;
            }
        }
        internal void SpeechToText()
        {
            try
            {
                if (worker != null)
                {
                    worker.Dispose();
                }
                //Player = new MediaPlayer();
                worker = new BackgroundWorker();
                worker.DoWork += Worker_ImportNewData_DoWork;
                worker.RunWorkerCompleted += Worker_ImportNewData_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }
            catch (Exception)
            {
            }
        }
        private void Worker_ImportNewData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.DoWork -= Worker_ImportNewData_DoWork;
            worker.RunWorkerCompleted -= Worker_ImportNewData_RunWorkerCompleted;

            if (ProcessCompleted != null)
                ProcessCompleted(null, null);

        }
        private void Worker_ImportNewData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                SpeechProcessingModel speechProcessingModel = new SpeechProcessingModel();
                speechProcessingModel.WorkingFolder = WorkingBase.WorkingFolder;
                speechProcessingModel.File = InputFolder;
                speechProcessingModel.Task = "STT";

                string fileLocation = Path.Combine(WorkingBase.WorkingFolder, WorkingBase.STTData);
                using (StreamWriter file = File.CreateText(fileLocation))
                {

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, speechProcessingModel);
                }
                ProcessButton = "Processing...";
                ProcessFlag = false;
                ButtonFlag = false;
                DataInput = fileLocation;
                isRunning = true;
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), WorkingBase.CMDSpeechProcessingFoldername, WorkingBase.CMDSpeechProcessingFilename);
                start.Arguments = $"--input {DataInput}";
                start.WorkingDirectory = Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), WorkingBase.CMDSpeechProcessingFoldername);
                start.UseShellExecute = false;
                start.RedirectStandardOutput = false;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                start.CreateNoWindow = true;
                Process process = Process.Start(start);
                try
                {
                    while (isRunning)
                    {
                        if (process.HasExited)
                        {
                            TextPredict = File.ReadAllText(InputFolder.Substring(0, InputFolder.Length - 3) + "txt");
                            ProcessButton = "Process";
                            ProcessFlag = true;
                            ButtonFlag = true;
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            catch (Exception ex)
            {

            }
        }
        public void SpeechToTextRealTime()
        {
            try
            {
                SpeechProcessingModel speechProcessingModel = new SpeechProcessingModel();
                speechProcessingModel.WorkingFolder = WorkingBase.WorkingFolder;
                speechProcessingModel.File = InputFolder;
                speechProcessingModel.Task = "STT";

                string fileLocation = Path.Combine(WorkingBase.WorkingFolder, WorkingBase.STTData);
                using (StreamWriter file = File.CreateText(fileLocation))
                {

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, speechProcessingModel);
                }
                ProcessButton = "Processing...";
                ProcessFlag = false;
                ButtonFlag = false;
                DataInput = fileLocation;

                if (runningThread != null)
                {
                    isRunning = false;
                    Thread.Sleep(300);

                    if (runningThread.IsAlive)
                    {
                        try
                        {
                            runningThread.Abort();
                        }
                        catch (Exception)
                        {
                        }
                    }

                    runningThread = null;
                }

                runningThread = new Thread(DoSpeechToText);
                isRunning = true;
                runningThread.Start();


            }
            catch (Exception ex) { }
            

        }
        public void DoSpeechToText()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), WorkingBase.CMDSpeechProcessingFoldername, WorkingBase.CMDSpeechProcessingFilename);
            start.Arguments = $"--input {DataInput}";
            start.WorkingDirectory = Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), WorkingBase.CMDSpeechProcessingFoldername);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = false;
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            Process process = Process.Start(start);
            try
            {
                while (isRunning)
                {
                    if (process.HasExited)
                    {
                        TextPredict = File.ReadAllText(InputFolder.Substring(0, InputFolder.Length - 3) + "txt");
                        ProcessButton = "Process";
                        ProcessFlag = true;
                        ButtonFlag = true;
                        break;
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
