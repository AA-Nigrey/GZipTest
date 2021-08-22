using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.IO.Compression;
using System.Linq;

namespace GZipTest
{
    public class DataProvider
    {
        public DataProvider(string compressionMode, string sourceFile, string destinationFile)
        {
            SourceFile = sourceFile;
            DestinationFile = destinationFile;

            switch (compressionMode.ToLower())
            {
                case "compress":
                    CompressionMode = CompressionMode.Compress;
                    break;
                case "decompress":
                    CompressionMode = CompressionMode.Decompress;
                    break;
            }
        }

        public string SourceFile { get; private set; }

        public string DestinationFile { get; private set; }

        public CompressionMode CompressionMode { get; private set; }

        public ConcurrentQueue<DataBlock> SourceQueue { get; set; } = new ConcurrentQueue<DataBlock>();

        public ConcurrentDictionary<long, byte[]> ResultQueue { get; set; } = new ConcurrentDictionary<long, byte[]>(); // for multithreadCompress/Decompress

        public bool IsReaderComplete { get; set; } //may be remove

        public bool IsCompressorComplete { get; set; } //may be remove

    }
}
