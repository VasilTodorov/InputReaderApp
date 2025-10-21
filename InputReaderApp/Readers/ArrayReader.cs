using InputReaderApp.Readers.InputReaderApp.Readers;
using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Readers
{
    public class ArrayReader : ReaderBase<int[,]>
    {
        public ArrayReader(TextReader? input=null) : base(input) { }
        
        public override Result<int[,]> Read()
        {
            try
            {
                int[] data = Input.ReadLine()!
                            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x=>int.Parse(x))
                            .ToArray();

                if (data.Length != 2 || data[0] <= 0 || data[1] <= 0) 
                    return Result<int[,]>.Fail(ErrorCode.InvalidDimension);

                int[,] array = new int[data[0], data[1]] ;

                for(int i = 0; i < array.GetLength(0); i++)
                {
                    int[] row = Input.ReadLine()!
                                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => int.Parse(x))
                                .ToArray();

                    if(row.Length != data[1]) return Result<int[,]>.Fail(ErrorCode.RowColMismatch);
                    for (int j = 0; j < row.Length; j++)
                        array[i,j] = row[j];
                }
           
                return Result<int[,]>.Success(array);
            }
            catch
            {
                return Result<int[,]>.Fail(ErrorCode.InvalidFormat);
            }
            
        }
        public static string ToString(int[,] arr)
        {
            string toPrint = string.Empty;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    toPrint += arr[i, j].ToString() + " ";
                }
                toPrint += "\n";
            }

            return toPrint;
        }
    }
}
