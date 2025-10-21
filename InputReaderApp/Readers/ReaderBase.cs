using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Readers
{
    namespace InputReaderApp.Readers
    {
        public abstract class ReaderBase<T> : IInputReader<T>
        {
            protected TextReader Input { get; }

            protected ReaderBase(TextReader? input = null)
            {
                Input = input ?? Console.In;
            }

            public abstract Result<T> Read();
        }
    }
}
