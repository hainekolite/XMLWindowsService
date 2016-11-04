using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsServiceForXML
{
    public partial class XMLWindowsService : ServiceBase
    {
        System.Timers.Timer timer;

        Thread thread;
        bool isTimerRunning = false;
        bool isThreadRunning = false;

        public XMLWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += TimerReady;
            timer.Interval = 60 * 1000; //60 * (1 seg = 1000 miliseg)
            timer.Start();

            isTimerRunning = true;
            //thread = new Thread(DoStuff);
            //thread.Start();
            //isThreadRunning = true;
        }

        protected override void OnStop()
        {
            isTimerRunning = false;
            timer = null;
            //thread = null;
            //isThreadRunning = false;
        }


        public void TimerReady(object sender, EventArgs e)
        {
            File.AppendAllText(@"C:\hola.txt", "MUEVA LINEA\n\r");
        }

        private void DoStuff()
        {
            while (isThreadRunning)
            {
                File.AppendAllText(@"C:\hola.txt","MUEVA LINEA\n\r");
            }
        }
    }
}
