using Application.Readers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace ApplicationTest
{
    public class DictionaryReaderTest
    {
        [Fact]
        public void DictionaryReaderGetNeighboursTest()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            ILoggerFactory factory = serviceProvider.GetService<ILoggerFactory>();
            ILogger<DictionaryReader> logger = factory.CreateLogger<DictionaryReader>();

            DictionaryReader reader = new DictionaryReader(@"C:\Temp\", logger);
        }
    }
}
