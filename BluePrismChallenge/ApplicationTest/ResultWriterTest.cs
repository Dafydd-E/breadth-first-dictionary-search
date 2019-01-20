using Application.Models;
using Application.Writers;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Xunit;

namespace ApplicationTest
{
    /// <summary>
    /// Unit tests for the <see cref="ResultWriter"/> class.
    /// </summary>
    public class ResultWriterTest : IDisposable
    {
        private const string TestPath = @"C:\Temp\output-test.txt";
        private const string TestNodeValue = "Test";
        private const string FirstParent = "parent #1";
        private const string SecondParent = "parent #2";

        /// <summary>
        /// Initialises a new instance of the <see cref="ResultWriterTest"/> class.
        /// </summary>
        public ResultWriterTest()
        {
            File.Create(TestPath).Close();
        }

        /// <summary>
        /// Deletes the temporary test file.
        /// </summary>
        public void Dispose()
        {
            File.Delete(TestPath);
        }

        [Fact]
        public void WriteTest()
        {
            ResultWriter writer = new ResultWriter(
                TestPath,
                ServiceHelper.GetService<ILogger<ResultWriter>>());

            writer.Write(new Node(TestNodeValue, new Node(FirstParent, new Node(SecondParent))));

            using (StreamReader reader = new StreamReader(TestPath))
            {
                Assert.Equal(SecondParent, reader.ReadLine());
                Assert.Equal(FirstParent, reader.ReadLine());
                Assert.Equal(TestNodeValue, reader.ReadLine());
            }
        }
    }
}
