using Application.Readers;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Xunit;

namespace ApplicationTest
{
    /// <summary>
    /// Unit tests for the <see cref="DictionaryReader"/> class.
    /// </summary>
    public class DictionaryReaderTest : IDisposable
    {
        private const string TestPath = @"C:\Temp\test-file.txt";
        private const string FirstLine = "first line";
        private const string SecondLine = "second line";
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
            using (DictionaryReader reader = new DictionaryReader(TestPath, logger))
            {
                Assert.True(reader.Read());
                Assert.Equal(FirstLine, reader.CurrentString);

                Assert.True(reader.Read());
                Assert.Equal(SecondLine, reader.CurrentString);

                Assert.False(reader.Read());
            }
        }

        [Fact]
        public void ResetReaderTest()
        {
            ILogger<DictionaryReader> logger = ServiceHelper.GetService<ILogger<DictionaryReader>>();
            using (DictionaryReader reader = new DictionaryReader(TestPath, logger))
            {
                Assert.True(reader.Read());
                Assert.NotNull(reader.CurrentString);
                Assert.True(reader.Read());
                Assert.NotNull(reader.CurrentString);

                Assert.True(reader.AtEndOfStream());
                reader.ResetReader();
                Assert.Null(reader.CurrentString);
                Assert.False(reader.AtEndOfStream());
            }
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
