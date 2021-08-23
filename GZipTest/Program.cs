using System;
using System.Linq;

namespace GZipTest
{
    class Program
    {
        static int Main(string[] args)
        {
            var errors = Validator.Validate(args);
            if (errors.Any())
            {
                foreach (var error in errors)
                    Console.WriteLine(error);
                return 1;
            }
            Console.WriteLine(Resources.Messages.Process);

            DataProvider dataProvider = new DataProvider(args[0], args[1], args[2]);
            GZipProcessor gZipProcessor = new GZipProcessor(dataProvider);
            var success = gZipProcessor.Process();

            if(success)
            {
                Console.WriteLine(Resources.Messages.Success);
                return 0;
            }
            else
            {
                Console.WriteLine(Resources.Messages.Fail);
                return 1;
            }
        }
    }
}