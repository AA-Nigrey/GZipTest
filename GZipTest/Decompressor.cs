using System.IO;
using System.IO.Compression;

namespace GZipTest
{
    /// <summary>
    /// Сlass for decompress source data.
    /// </summary>
    public class Decompressor : IGZiper
    {
        private readonly DataProvider _dataProvider;

        public Decompressor(DataProvider dataProvider)
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
                        using (GZipStream compressionStream = new GZipStream(new MemoryStream(buffer.Data), CompressionMode.Decompress))
                        {
                            compressionStream.CopyTo(ms);
                        }
                        _dataProvider.ResultQueue.TryAdd(buffer.Index, ms.ToArray());
                    }
                }
            }
        }
    }
}