using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trivia;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            for (var run = 0; run < 100; ++run)
            {
                var originalMessageChannel = new MemoryMessageChannel();

                GameRunner.Run(originalMessageChannel, new Random(run));

                var refactoredMessageChannel = new MemoryMessageChannel();

                Trivia.Refactored.GameRunner.Run(refactoredMessageChannel, new Random(run));

                CollectionAssert.AreEqual(originalMessageChannel.Message, refactoredMessageChannel.Message);
            }
        }

        class MemoryMessageChannel : IMessageChannel
        {
            private readonly List<string> _message = new List<string>();

            public void WriteMessage(string message) => _message.Add(message);

            public ICollection Message => _message;
        }
    }
}
