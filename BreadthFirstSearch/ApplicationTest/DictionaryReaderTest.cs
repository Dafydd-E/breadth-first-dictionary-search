using Application.Readers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace ApplicationTest
{
    /// <summary>
    /// Unit tests for the <see cref="DictionaryReader"/> class.
    /// </summary>
    public class DictionaryReaderTest : IDisposable
    {
        private const string TestPath = @"C:\Temp\test-file.txt";
        private const string FirstLine = "four";
        private const string SecondLine = "five";
        private const string ThirdLine = "My very long word";
        private readonly string[] TestContent = new string[] { FirstLine, SecondLine };

        /// <summary>
        /// Initialises a new instance of the <see cref="DictionaryReaderTest"/> class.
        /// </summary>
        public DictionaryReaderTest()
        {
            File.WriteAllLines(TestPath, TestContent);
        }

        [Fact]
        public void ReadTest()
        {
            ILogger<DictionaryReader> logger = ServiceHelper.GetService<ILogger<DictionaryReader>>();
            DictionaryReader reader = new DictionaryReader(TestPath, logger);

            var strings = new List<string> { FirstLine, SecondLine };

            Assert.Equal(strings.AsEnumerable<string>(), reader.Read());
        }

        [Fact]
        public void ReadOnlyFourLetterWordsTest()
        {
            ILogger<DictionaryReader> logger = ServiceHelper.GetService<ILogger<DictionaryReader>>();
            DictionaryReader reader = new DictionaryReader(TestPath, logger);

            var strings = new List<string> { FirstLine, SecondLine };

            Assert.Equal(strings.AsEnumerable<string>(), reader.Read());
        }

        /// <summary>
        /// Deletes the temporary test TXT file.
        /// </summary>
        public void Dispose()
        {
            File.Delete(TestPath);
        }
    }
}
