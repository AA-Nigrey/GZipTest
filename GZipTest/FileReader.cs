using System;
using System.IO;
using System.Linq;
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

        public void Read()
        {
            if (_dataProvider.CompressionMode == "compress")
            {
                ReadFile();
            }
            else
            {
                ReadCompressedFile();
            }

            _dataProvider.IsReaderComplete = true;
        }

        private void ReadFile()
        {
            using (var fstream = new FileStream(_dataProvider.SourceFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                byte[] buffer = new byte[_dataProvider.BlockLength];
                long index = 0;
                int readedBytes;
                while ((readedBytes = fstream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    _dataProvider.SourceQueue.Enqueue(new DataBlock(index, buffer.Take(readedBytes).ToArray()));

                    CollectGarbage();

                    index++;
                }
            }
        }

        private void ReadCompressedFile()
        {
            using (var fstream = new FileStream(_dataProvider.SourceFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                byte[] gZipHeader = new byte[10];
                long index = 0;
                byte[] gZipHeaderDefault = new byte[10] { 0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0a };

                while (fstream.Read(gZipHeader, 0, gZipHeaderDefault.Length) > 0)
                {
                    if (gZipHeader[0] == gZipHeaderDefault[0] && gZipHeader[1] == gZipHeaderDefault[1] && gZipHeader[2] == gZipHeaderDefault[2]) //check block header
                    {
                        var blockLength = BitConverter.ToInt32(gZipHeader.Skip(3).Take(4).ToArray()); //read length of compressed data block from header
                        var buffer = new byte[blockLength];
                        fstream.Read(buffer, 0, blockLength - gZipHeader.Length);

                        _dataProvider.SourceQueue.Enqueue(new DataBlock(index, gZipHeaderDefault.Concat(buffer).ToArray()));

                        CollectGarbage();

                        index++;
                    }
                    else
                    {
                        throw new ArgumentException(Resources.Messages.IncorrectGzipHeader);
                    }
                }
            }
        }

        private void CollectGarbage()
        {
            if (_dataProvider.SourceQueue.Count > GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / (2 * _dataProvider.BlockLength)) //Use half of Available Memory  
            {
                Thread.Sleep(500);
                GC.Collect();
            }
        }
    }
}