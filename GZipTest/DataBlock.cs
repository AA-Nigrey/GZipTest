using System.Linq;

namespace GZipTest
{
    public class DataBlock
    {
        public DataBlock(long index, byte[] data)
        {
            Index = index;
            Data = data.ToArray();
        }

        public long Index { get; }

        public byte[] Data { get; }
    }
}