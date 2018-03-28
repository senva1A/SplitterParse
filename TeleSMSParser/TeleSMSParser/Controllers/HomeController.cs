using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TeleSMSParser.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //Hosted web API REST Service base url          
        string Baseurl = ConfigurationManager.AppSettings["BaseURL"];

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(View = "Error")]
        public async Task<ActionResult> ParseTextApi(FormCollection form)
        {
            string textToParse = form["parseTextArea"].ToString();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource  using HttpClient  
                HttpResponseMessage Res = await client.PostAsJsonAsync("api/Values/", textToParse);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var resultObj = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the string list  
                    var ParsedText = JsonConvert.DeserializeObject<List<string>>(resultObj);
                    return View(ParsedText);
                }
                else
                {
                    ViewBag.CustomError = Res.ToString();
                    return View("CustomError");
                }
            }
        }
    }
}
