using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GZipTest
{
    /// <summary>
    /// Сlass for reading source file.
    /// </summary>
    public class FileReader
    {
        private readonly DataProvider _dataProvider;

        public FileReader(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async void ReadAsync() => await Task.Run(() => Read()); //Todo Remove

        public void Read()
        {
            using (var fstream = new FileStream(_dataProvider.SourceFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                byte[] buffer = new byte[1024 * 1024]; //1mb
                long index = 0;
                int readedBytes;
                while ((readedBytes = fstream.Read(buffer, 0, buffer.Length)) > 0) // add try catch
                {
                    if (_dataProvider.SourceQueue.Count() <= 1024) //TODO check memory and gc collect + refactoring if-else
                    {
                        if (readedBytes == buffer.Length)
                        {
                            _dataProvider.SourceQueue.Enqueue(new DataBlock(index, buffer.ToArray()));
                        }
                        else
                        {
                            _dataProvider.SourceQueue.Enqueue(new DataBlock(index, buffer.Take(readedBytes).ToArray()));
                        }
                    }
                    else
                    {
                        Thread.Sleep(500);
                        if (readedBytes == buffer.Length)
                        {
                            _dataProvider.SourceQueue.Enqueue(new DataBlock(index, buffer.ToArray()));
                        }
                        else
                        {
                            _dataProvider.SourceQueue.Enqueue(new DataBlock(index, buffer.Take(readedBytes).ToArray()));
                        }
                    }

                    index++;
                }
            }

            _dataProvider.IsReaderComplete = true;
        }
    }
}
