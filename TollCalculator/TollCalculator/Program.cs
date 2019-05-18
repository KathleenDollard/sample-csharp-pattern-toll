using TollingData;

namespace TollRunner

{
    class Program
    {

        private static readonly ITollDataSource[] tollDataSources = new ITollDataSource[]
        {
            new TollBoothA()
        };

        static void Main(string[] args)
        {
            // DisplayTolls();

            try
            {
                var tollCalculator = new TollCalculator();

                foreach (ITollDataSource tollSource in tollDataSources)
                {
                    foreach (TollEvent tollEvent in tollSource.GetTollData())
                    {

                    }
                }
            }
            catch
            {

            }

        }

    }
}