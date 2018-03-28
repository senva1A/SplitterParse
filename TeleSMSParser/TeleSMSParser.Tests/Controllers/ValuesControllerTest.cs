using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleSMSParser;
using TeleSMSParser.Controllers;

namespace TeleSMSParser.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void GetParsedData()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            //Mock string text
            string mockParseText="Dear, Seeni write a program that breaks this text into small chucks. Each chunk should have a maximum length of 25 characters. The program should try to break on complete sentences or punctuation marks if possible.  If a comma or sentence break occurs within 5 characters of the max line length, the line should be broken early.  The exception to this rule is when the next line will only contain 5 characters.  Redundant whitespace should not be counted between lines, and any duplicate   spaces should be removed.  Does this make sense? If not please ask for further clarification, an array containing the desired outcome has been provided below. Any text beyond this point is not part of the instructions, it's only here to ensure test converge. Finish line. Aaa asdf asdfjk las, asa.eru";
            // Act
            IEnumerable<string> result = controller.Post(mockParseText);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(39, result.Count());
            Assert.AreNotEqual(25, result.Count());
            Assert.AreEqual("Dear", result.ElementAt(0));
            Assert.AreNotEqual("Dear, Seeni write a progr", result.ElementAt(0));
            Assert.AreEqual(" asa.eru", result.ElementAt(38));
        }

    }
}
