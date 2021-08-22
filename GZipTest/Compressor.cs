using System;
using System.IO;
using System.IO.Compression;

namespace GZipTest
{
    /// <summary>
    /// Сlass for compress source data.
    /// </summary>
    public class Compressor : IGZiper
    {
        private readonly DataProvider _dataProvider;

        public Compressor(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void Transform()
        {
            while (!_dataProvider.IsReaderComplete || _dataProvider.SourceQueue.Count > 0)
            {
                if (_dataProvider.SourceQueue.TryDequeue(out var buffer) && buffer != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        using (GZipStream compressionStream = new GZipStream(ms, CompressionMode.Compress))
                        {
                            compressionStream.Write(buffer.Data, 0, buffer.Data.Length);
                        }

                        _dataProvider.ResultQueue.TryAdd(buffer.Index, SetHeaderDataLength(ms.ToArray()));
                    }
                }
            }
        }

        /// <summary>
        /// Method for writing block length to header.
        /// </summary>
        private static byte[] SetHeaderDataLength(byte[] data)
        {
            var sizeData = BitConverter.GetBytes(data.Length);
            Array.Copy(sizeData, 0, data, 3, sizeData.Length);
            return data;
        }
    }
}
