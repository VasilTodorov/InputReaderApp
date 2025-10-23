using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Utils
{
    public interface IOutputWriter<T>
    {
        Result Write(string outputFilePath,T value);
    }
}
