using System;
using System.Linq;
using System.Threading.Tasks;

namespace GZipTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var errors = Validator.Validate(args);
            if (errors.Any())
            {
                foreach (var error in errors)
                    Console.WriteLine(error);
                return;
            }
            DataProvider dataProvider = new DataProvider(args[0], args[1], args[2]);

            FileReader fileReader = new FileReader(dataProvider);
            FileWriter fileWriter = new FileWriter(dataProvider);

            var taskReader = new Task(() => fileReader.Read());
            taskReader.Start();

            var taskWriter = new Task(() => fileWriter.Write());
            taskWriter.Start();


        }
    }
}
