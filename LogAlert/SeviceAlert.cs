using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.IO;
using System.Timers;

namespace LogAlert
{
    public partial class SeviceAlert : ServiceBase
    {
        Alert alert;
        private Timer timer;
        public SeviceAlert()
        {
            InitializeComponent();
        }
       
        protected override void OnStart(string[] args)
        {

            
            StartTest();
            timer = new Timer();
            this.timer.Interval = alert.checkTime;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.tick);
            timer.Enabled = true;

        }
        public  void StartTest()
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8080");

            config.Routes.MapHttpRoute(
               name: "API",
               routeTemplate: "{controller}/{action}/{id}",
               // "{controller}/{action}/{id}",
               //  "api/{controller}/{id}"
               defaults: new { id = RouteParameter.Optional }
           );
            HttpSelfHostServer server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8080/");

            //HttpResponseMessage response = client.GetAsync("Test/GetTest1/1").Result;

      
            HttpResponseMessage response = client.GetAsync("Alert/GetConfiguration").Result;
            if (response.IsSuccessStatusCode)
            {
                alert = response.Content.ReadAsAsync<Alert>().Result;
                
            }
          
        }

        public void Check()
        {
            alert.CheckSpace();
        }

        public void TimerStart()
        {

            StartTest();
        }

        private void tick(object sender, ElapsedEventArgs e)
        {
            alert.CheckSpace();
        }

        protected override void OnStop()
        {
        }
    }
}
