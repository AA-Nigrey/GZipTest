using System.Collections.Concurrent;

namespace GZipTest
{
    public class DataProvider
    {
        public DataProvider(string compressionMode, string sourceFile, string destinationFile)
        {
            SourceFile = sourceFile;
            DestinationFile = destinationFile;
            CompressionMode = compressionMode.ToLower();
        }

        public int BlockLength { get; } = 1024 * 1024; //1 Mb

        public string SourceFile { get; }

        public string DestinationFile { get; }

        public string CompressionMode { get; }

        public ConcurrentQueue<DataBlock> SourceQueue { get; set; } = new ConcurrentQueue<DataBlock>();

        public ConcurrentDictionary<long, byte[]> ResultQueue { get; set; } = new ConcurrentDictionary<long, byte[]>();

        public bool IsReaderComplete { get; set; }

        public bool IsGZiperComplete { get; set; }
    }
}