using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Readers
{
    public record Employee(string Name, int Age, decimal Salary);
    public record Location(string City, string Description);
    public record Meeting(Location Location, float DurationInHours, ImmutableList<Employee> Guests);
    public struct DataBase
    {
        public List<Employee> Employees { get; set; }
        public List<Employee> Locations { get; set; }
        public List<Employee> Meetings { get; set; }
    }
    public class DifferentStatesReader : ReaderBase<DataBase>
    {
        public override Result<DataBase> Read()
        {
            throw new NotImplementedException();
        }
    }
}
