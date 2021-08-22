using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    public class FileWriter
    {
        private readonly DataProvider _dataProvider;

        public FileWriter(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async void WriteAsync() => await Task.Run(() => Write());

        public void Write()
        {
            using (var outstream = new FileStream(_dataProvider.DestinationFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                byte[] buffer;
                var index = 0;
                while (!_dataProvider.IsCompressorComplete || _dataProvider.ResultQueue.Count > 0)
                {
                    if (_dataProvider.ResultQueue.TryRemove(index, out buffer) && buffer != null)
                    {
                        outstream.Write(buffer, 0, buffer.Length);
                        index++;
                    }
                }
            }
        }
    }
}
