using System.Collections.Generic;
using System.IO;

namespace GZipTest
{
    /// <summary>
    /// Сlass for validating the input string.
    /// </summary>
    static class Validator
    {
        private static string _сompress = "compress";
        private static string _decompress = "decompress";

        private static List<string> errors = new List<string>();

        public static List<string> Validate(string[] input)
        {
            if (input.Length != 3)
            {
                errors.Add(Resources.Messages.IncorrectCommandFormat);
            }
            else
            {
                var compressionMode = input[0].ToLower();
                var sourceFile = input[1];
                var destinationFile = input[2];

                if (compressionMode != _сompress && compressionMode != _decompress)
                {
                    errors.Add(Resources.Messages.IncorrectCommandFormat);
                }

                if (!File.Exists(sourceFile))
                {
                    errors.Add(string.Format(Resources.Messages.IncorrectFilePath, sourceFile));
                }

                if (Path.GetFullPath(sourceFile) == Path.GetFullPath(destinationFile))
                {
                    errors.Add(Resources.Messages.EqualFilePaths);
                }
            }

            return errors;
        }
    }
}
