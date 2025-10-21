using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Utils
{
    public static class Helpers
    {
        public static bool IsValidName(string name)
        {
            var chars = name.ToCharArray();
            foreach (var c in chars)
            {
                if (!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')))
                    return false;
            }
            return true;
        }
    }
}
