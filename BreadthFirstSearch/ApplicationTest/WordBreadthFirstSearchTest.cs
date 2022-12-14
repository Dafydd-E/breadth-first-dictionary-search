using Application.Models;
using Application.Queues;
using Application.Readers;
using Application.PathFinders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace ApplicationTest
{
    /// <summary>
    /// Unit tests for the <see cref="WordBreadthFirstSearch"/> class.
    /// </summary>
    public class WordBreadthFirstSearchTest : IDisposable
    {
        private const string TestPath = @"C:\Temp\test-dictionary.txt";

        /// <summary>
        /// Initialises a new instance of the <see cref="WordBreadthFirstSearch"/>
        /// class.
        /// </summary>
        public WordBreadthFirstSearchTest()
        {
            using (StreamWriter writer = new StreamWriter(File.Create(TestPath)))
            {
                //    new string[] { "Ross", "Rost", "Rosh", "Rosk", "Roth" }) ;
                writer.WriteLine("Ross");
                writer.WriteLine("Rost");
                writer.WriteLine("Rosh");
                writer.WriteLine("Rosk");
                writer.WriteLine("Roth");
            }
        }

        /// <summary>
        /// Deletes the temporary test file.
        /// </summary>
        public void Dispose()
        {
            File.Delete(TestPath);
        }

        [Fact]
        public void FindNeighboursTest()
        {
            var search = new WordBreadthFirstSearch(
                new DictionaryReader(TestPath, ServiceHelper.GetService<ILogger<DictionaryReader>>()),
                new DistinctQueue<Node>(ServiceHelper.GetService<ILogger<DistinctQueue<Node>>>()),
                ServiceHelper.GetService<ILogger<WordBreadthFirstSearch>>());

            IEnumerator<Node> neighbours = search
                .FindNeighbours(new Node("Rosh"), new List<string> { "Ross", "Rost", "Rosh", "Rosk", "Roth" })
                .GetEnumerator();

            neighbours.MoveNext();
            Assert.Equal("Ross", neighbours.Current.Word);
            neighbours.MoveNext();
            Assert.Equal("Rost", neighbours.Current.Word);
            neighbours.MoveNext();
            Assert.Equal("Rosk", neighbours.Current.Word);
            neighbours.MoveNext();
            Assert.Equal("Roth", neighbours.Current.Word);
            Assert.False(neighbours.MoveNext());
        }

        [Fact]
        public void SearchTest()
        {
            var search = new WordBreadthFirstSearch(
                new DictionaryReader(TestPath, ServiceHelper.GetService<ILogger<DictionaryReader>>()),
                new DistinctQueue<Node>(ServiceHelper.GetService<ILogger<DistinctQueue<Node>>>()),
                ServiceHelper.GetService<ILogger<WordBreadthFirstSearch>>());

            Node node = search.FindPath(new Node("Ross"), new Node("Roth"));
            Assert.Equal("Roth", node.Word);
            node = node.Parent;
            Assert.Equal("Rosh", node.Word);
            node = node.Parent;
            Assert.Equal("Ross", node.Word);
            node = node.Parent;
            Assert.Null(node);
        }
    }
}
