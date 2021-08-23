using System;
using System.Threading.Tasks;

namespace GZipTest
{
    /// <summary>
    /// Сlass for multiTask compress-decompress files.
    /// </summary>
    public class GZipProcessor
    {
        private readonly DataProvider _dataProvider;

        public GZipProcessor(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public bool Process()
        {
            FileReader fileReader = new FileReader(_dataProvider);
            IGZiper gZiper = _dataProvider.CompressionMode == "compress" ? new Compressor(_dataProvider) : new Decompressor(_dataProvider);
            FileWriter fileWriter = new FileWriter(_dataProvider);

            var taskReader = new Task(() => fileReader.Read());
            taskReader.Start();

            Task[] taskGZipers = new Task[Environment.ProcessorCount - 3];
            for (int i = 0; i < taskGZipers.Length; i++)
            {
                taskGZipers[i] = new Task(() => gZiper.Transform());
                taskGZipers[i].Start();
            }

            Task.WhenAll(taskGZipers).ContinueWith(t => _dataProvider.IsGZiperComplete = true);

            var taskWriter = new Task(() => fileWriter.Write());
            taskWriter.Start();

            try
            {
                taskReader.Wait();
                Task.WaitAll(taskGZipers);
                taskWriter.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }

            return true;
        }
    }
}