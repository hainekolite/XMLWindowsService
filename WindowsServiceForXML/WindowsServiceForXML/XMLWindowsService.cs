using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsServiceForXML
{
    public partial class XMLWindowsService : ServiceBase
    {
        private System.Timers.Timer timer;
        private double hours;
        private double minutes;
        private double seconds;

        public XMLWindowsService()
        {
            InitializeComponent();
            timer = new System.Timers.Timer();
            timer.Elapsed += TimerReady;
            hours = Convert.ToDouble(ConfigurationManager.AppSettings["HoursForRunTimeService"]);
            minutes = Convert.ToDouble(ConfigurationManager.AppSettings["MinutesForRunTimeService"]);
            seconds = Convert.ToDouble(ConfigurationManager.AppSettings["SecondsForRunTimeService"]);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                timer.Interval = (hours*60.0*60.0*1000) + (minutes*60.0*1000) +  (seconds * 1000); //60 * (1 seg = 1000 miliseg)
            }
            catch (FormatException)
            {
                timer.Interval = 60 * 1000; //Something got wrong in the format? set default to 1 min 
            }
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
            timer.Dispose();
            timer = null;
        }

        protected override void OnPause() => timer.Stop();

        protected override void OnContinue() =>  timer.Start();

        public void TimerReady(object sender, EventArgs e)
        {
            File.AppendAllText(@"C:\hola.txt", "MUEVA LINEA\n\r");
        }

        private void GetMaterialsFromDB()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MaterialManagementDbContext"].ConnectionString;
            string query = string.Empty;
            //string query = "SELECT SerialNumber FROM MaterialInstances m WHERE m.SerialNumber =" + "'" + Regex.Replace(CartridgeId, " ", string.Empty) + "'";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                //Read the specified fields to fill the custom model for the generation XML file
                /*string CartridgeName = (reader["SerialNumber"].ToString().ToUpper());
                if (Regex.Replace(CartridgeId, " ", string.Empty).ToUpper().Equals(Regex.Replace(CartridgeName, " ", string.Empty)))
                {
                    connection.Close();
                    return (true);
                }*/
            }
            connection.Close();
        }


    }
}
