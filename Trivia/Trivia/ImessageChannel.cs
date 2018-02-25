using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trivia
{
    public interface IMessageChannel
    {
        void WriteMessage(string message);
    }

    public class ConsoleMessageCannel : IMessageChannel
    {
        public void WriteMessage(string message) => Console.WriteLine(message);
    }

}
