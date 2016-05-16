namespace CounterApp
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Media;
    using System.Net;
    using System.Threading;

    using Models;

    using Newtonsoft.Json;

    public partial class Form1 : Form
    {
        private const string NumberFormat = "N0";

        private readonly Stopwatch watch = new Stopwatch();

        private static int counter = 0;

        private int elapsedSeconds;

        private int sleepMilliseconds;

        private Uri endpoint;

        public Form1()
        {
            this.InitializeComponent();
            this.InitializeAppSettings();
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.label1.Visible = true;
            this.ResetCounter();
        }

        private void InitializeAppSettings()
        {
            if (bool.Parse(ConfigurationManager.AppSettings["TestSound"]))
            {
                Debug.WriteLine("Testing sound");
                PlaySound();
            }
            this.elapsedSeconds = int.Parse(ConfigurationManager.AppSettings["ElapsedSeconds"]);
            this.sleepMilliseconds = int.Parse(ConfigurationManager.AppSettings["SleepMilliseconds"]);
            this.endpoint = new Uri(ConfigurationManager.AppSettings["Endpoint"]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ResetCounter();
            if (this.backgroundWorker1.IsBusy != true)
            {
                this.backgroundWorker1.RunWorkerAsync();
            }
            else if (this.backgroundWorker1.WorkerSupportsCancellation)
            {
                this.backgroundWorker1.CancelAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            this.watch.Start();
            while (true)
            {
                if (worker == null)
                {
                    break;
                }

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                Thread.Sleep(this.sleepMilliseconds);

                counter = this.GetCounterValue();
                worker.ReportProgress(counter);

                if (this.watch.Elapsed.Seconds >= this.elapsedSeconds)
                {
                    this.watch.Stop();
                    break;
                }
            }
        }

        private int GetCounterValue()
        {
            var total = 0;
            using (var wc = new WebClient())
            {
                var interactions = JsonConvert.DeserializeObject<InteractionsModel[]>(wc.DownloadString(this.endpoint));
                total += interactions.Sum(i => i.Sent);
            }

            return total;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.label1.Text = e.ProgressPercentage.ToString(NumberFormat);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.Assert(e.Error == null);
            if (e.Cancelled)
            {
                this.ResetCounter();
                return;
            }

            this.label1.ForeColor = Color.Red;
            PlaySound();
        }

        private void ResetCounter()
        {
            this.label1.Text = counter.ToString(NumberFormat);
            this.label1.ForeColor = Color.Black;
            this.watch.Reset();
        }

        private static void PlaySound()
        {
            SystemSounds.Exclamation.Play();
        }
    }
}
