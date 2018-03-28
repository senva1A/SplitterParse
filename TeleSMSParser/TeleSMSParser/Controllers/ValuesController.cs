using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace TeleSMSParser.Controllers
{
    public class ValuesController : ApiController
    {

        public static List<string> teleSms;

        // POST api/values
        public IEnumerable<string> Post([FromBody]string value)
        {
            try
            {
                teleSms = new List<string>();
                // string id = "Please write a program that breaks this text into small chucks. Each chunk should have a maximum length of 25 characters. The program should try to break on complete sentences or punctuation marks if possible.  If a comma or sentence break occurs within 5 characters of the max line length, the line should be broken early.  The exception to this rule is when the next line will only contain 5 characters.  Redundant whitespace should not be counted between lines, and any duplicate   spaces should be removed.  Does this make sense? If not please ask for further clarification, an array containing the desired outcome has been provided below. Any text beyond this point is not part of the instructions, it's only here to ensure test converge. Finish line. Aaa asdf asdfjk las, asa.eru";
                string input = Regex.Replace(value, @"\s+", " ");
                var SplittedData = SplitDataBy25Lines(input, 25);

                //Add remaining string to SplittedData
                int remStartPos = ((input.Length / 25) * 25);
                int lastEndPos = input.Length % 25;
                SplittedData.Add(input.Substring(remStartPos, lastEndPos));

                foreach (var parsedstr in SplittedData)
                {
                    if (parsedstr.Length >= 5)
                    {
                        var splitted5Chars = parsedstr.Substring(0, 5).Where(x => x.ToString().Contains(',') || x.ToString().Contains('.'));
                        if (splitted5Chars.Any())
                            SplitDataByCommasDot(parsedstr);
                        else
                            teleSms.Add(parsedstr);
                    }
                    else
                    {
                        teleSms.Add(parsedstr);
                    }
                }

                return teleSms;
            }
            catch (Exception ex)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(string.Format("Internal Server Error"), Encoding.UTF8, "text/html")
                };
                throw new HttpResponseException(resp);

            }
        }

        private void SplitDataByCommasDot(string parsedstr)
        {
            var splittedByCommasDot = new List<string>();
            splittedByCommasDot = parsedstr.Split(',').ToList<string>();
            if (!(splittedByCommasDot.Count > 1))
                splittedByCommasDot = parsedstr.Split('.').ToList<string>();
            if (splittedByCommasDot.Count > 1 && splittedByCommasDot[1].Length > 5)
            {
                teleSms.Add(splittedByCommasDot[0]);
                SplitDataByCommasDot(splittedByCommasDot[1]);
            }
            else
                teleSms.Add(parsedstr);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        private List<string> SplitDataBy25Lines(string str, int maxSize)
        {
            return Enumerable.Range(0, str.Length / maxSize).Select(i => str.Substring(i * maxSize, maxSize)).ToList<string>();
        }
    }
}