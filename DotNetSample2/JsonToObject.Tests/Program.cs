using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToObject.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = "{ \"name\":\"runoob\", \"alexa\":10000, \"site\":null }";

            string classCode = JsonClassGenerator.GenerateString(json, CodeLanguage.CSharp);

            Console.WriteLine(classCode);
            Console.ReadLine();



        }
    }
}
