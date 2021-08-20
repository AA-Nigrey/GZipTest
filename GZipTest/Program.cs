using System;
using System.Linq;

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


        }
    }
}
