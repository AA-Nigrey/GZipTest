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

        public long Index { get; private set; }

        public byte[] Data { get; private set; }
    }
}
