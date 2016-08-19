using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyForm
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Enter path of JSON description of DyForm");
            string jsonFile = Console.ReadLine();
            Console.WriteLine("Enter HTML form location");
            string htmlFormLocation = Console.ReadLine();

            var myForm = DyForm.CreateFromJson(File.ReadAllText(jsonFile));


            StreamWriter htmlWriter = new StreamWriter(htmlFormLocation);
            htmlWriter.WriteLine(myForm.Render());
            htmlWriter.Close();

            System.Diagnostics.Process.Start(@"C:\personal\temp_form.htm");
        }
    }
}
