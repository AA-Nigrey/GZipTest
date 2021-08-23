using System.IO;

namespace GZipTest
{
    /// <summary>
    /// Сlass for writing source file.
    /// </summary>
    public class FileWriter
    {
        private readonly DataProvider _dataProvider;

        public FileWriter(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void Write()
        {
            using (var outstream = new FileStream(_dataProvider.DestinationFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                var index = 0;
                while (!_dataProvider.IsGZiperComplete || !_dataProvider.ResultQueue.IsEmpty)
                {
                    if (_dataProvider.ResultQueue.TryRemove(index, out byte[] buffer) && buffer != null)
                    {
                        outstream.Write(buffer, 0, buffer.Length);
                        index++;
                    }
                }
            }
        }
    }
}