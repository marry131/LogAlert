using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace LogAlert
{
   public class AlertController : ApiController
    {
        public string GetTest1(int id)
        {
            return "This is a test Controller invoke";

        }

        public Alert GetConfiguration()
        {

            return new Alert();
        }
      
    }
}
