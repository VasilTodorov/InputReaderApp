using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Tests.Helpers
{
    public static class MockConsole
    {
        private static readonly object _consoleLock = new();

        public static void WithInput(string input, Action action)
        {
            lock (_consoleLock)
            {
                var original = Console.In;
                try
                {
                    Console.SetIn(new StringReader(input));
                    action();
                }
                finally
                {
                    Console.SetIn(original);
                }
            }
        }
    }
}
