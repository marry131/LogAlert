using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LogAlert
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new SeviceAlert()
            //};
            //ServiceBase.Run(ServicesToRun);


            SeviceAlert service = new SeviceAlert();

            service.StartTest();
            service.Check();


            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
        }
    }
}
