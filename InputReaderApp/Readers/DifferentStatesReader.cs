using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Readers
{
    public record Employee(string Name, int Age, decimal Salary);
    public record Location(string City, string Description);
    public record Meeting(Location Location, float DurationInHours, List<Employee> Guests);
    public struct DataBase
    {
        public List<Employee> Employees { get; set; }
        public List<Location> Locations { get; set; }
        public List<Meeting> Meetings { get; set; }

        public DataBase()
        {
            Employees = new List<Employee>();
            Locations = new List<Location>();
            Meetings = new List<Meeting>();
        }
    }
    public class DifferentStatesReader : ReaderBase<DataBase>
    {
        enum Format { None, Employee, Location, Meeting }
        public DifferentStatesReader(TextReader? input = null) : base(input) { }

        public override Result<DataBase> Read()
        {
            var dataBase = new DataBase();

            Format currentFormat = Format.None;

            string? line;

            List<string> EmployeesRawData = new List<string>();
            List<string> LocationsRawData = new List<string>();
            List<string> MeetingsRawData = new List<string>();

            Dictionary<Format, bool> hasRead = new Dictionary<Format, bool>();
            hasRead[Format.Employee] = false;
            hasRead[Format.Location] = false;
            hasRead[Format.Meeting] = false;

            while ((line = Input.ReadLine()) is not null)
            {
                line = line.Trim();

                Format tempFormat = GetFormat(line);
                if (tempFormat != Format.None)
                {
                    if (hasRead[tempFormat])
                        return Result<DataBase>
                                .Fail(ErrorCode.InvalidFormat, $"Error: {tempFormat.ToString()} table is read a second time");
                    hasRead[tempFormat] = true;
                    currentFormat = tempFormat;
                }
                else
                {
                    switch (currentFormat)
                    {
                        case Format.Employee:
                            EmployeesRawData.Add(line);                            
                            break;
                        case Format.Location:
                            LocationsRawData.Add(line);
                            break;
                        case Format.Meeting:
                            MeetingsRawData.Add(line);
                            break;
                        case Format.None:
                            if (line != string.Empty)
                                return Result<DataBase>.Fail(ErrorCode.InvalidFormat, "Error: data in no table");
                            break;
                    }
                }
            }

            if(!hasRead[Format.Employee] || !hasRead[Format.Location] || !hasRead[Format.Meeting])
                return Result<DataBase>.Fail(ErrorCode.InvalidFormat, $"Error: a table hasn't been read -" +
                    $"Employees:{hasRead[Format.Employee]}," + 
                    $"Locations:{hasRead[Format.Location]}," +
                    $"Meetings:{hasRead[Format.Meeting]}");

            var employeesResults = EmployeesRawData.Where(e=>e.Trim() != string.Empty).Select(x => ReadEmployee(x));
            var locationsResults = LocationsRawData.Where(l => l.Trim() != string.Empty).Select(x => ReadLocation(x));

            var firstEmployeeFailer = employeesResults.FirstOrDefault(r => r.IsFailure);
            var firstLoctionFailer = locationsResults.FirstOrDefault(r => r.IsFailure);

            if(firstEmployeeFailer is not null)
                return Result<DataBase>.Fail(ErrorCode.InvalidFormat, firstEmployeeFailer.Message);
            if (firstLoctionFailer is not null)
                return Result<DataBase>.Fail(ErrorCode.InvalidFormat, firstLoctionFailer.Message);

            dataBase.Employees = employeesResults.Select(r => r.Data!).ToList();
            dataBase.Locations = locationsResults.Select(r => r.Data!).ToList();

            var meetingsResults = MeetingsRawData.Where(m => m.Trim() != string.Empty).Select(x => ReadMeeting(x, dataBase));
            var firstMeetingFailer = meetingsResults.FirstOrDefault(r => r.IsFailure);
            if (firstMeetingFailer is not null)
                return Result<DataBase>.Fail(ErrorCode.InvalidFormat, firstMeetingFailer.Message);
            dataBase.Meetings = meetingsResults.Select(r => r.Data!).ToList();

            return Result<DataBase>.Success(dataBase);
        }

        private Format GetFormat(string line)
        {
            line = line.Trim();
            return line switch
            {
                "<Employees>" => Format.Employee,
                "<Locations>" => Format.Location,
                "<Meetings>" => Format.Meeting,
                _ => Format.None
            };
        }
        internal Result<Employee> ReadEmployee(string line)
        {
            line = line.Trim();
            var parts = line.Split(' ',StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3)
                return Result<Employee>.Fail(ErrorCode.InvalidFormat, $"Error: Employee needs only 3 arguments : {line}");

            if (int.TryParse(parts[1], out int age) && decimal.TryParse(parts[2], out decimal salary))
                return Result<Employee>.Success(new Employee(parts[0], age, salary));
            else
                return Result<Employee>.Fail(ErrorCode.InvalidFormat, "Error: Arguments types are invalid");
        }
        internal Result<Location> ReadLocation(string line)
        {
            line = line.Trim();

            int beginDesIndex = line.IndexOf('"');

            if (beginDesIndex == -1)
                return Result<Location>.Fail(ErrorCode.InvalidFormat, "Error: No Location description");

            string City = line.Substring(0, beginDesIndex).Trim();
            if (City.Contains(' '))
                return Result<Location>.Fail(ErrorCode.InvalidFormat, "Error: City must be one word");

            line = line.Substring(++beginDesIndex).Trim();
            int endDesIndex = line.IndexOf('"');

            if (beginDesIndex == -1)
                return Result<Location>.Fail(ErrorCode.InvalidFormat, "Error: Location description must be in \"\"");

            if(endDesIndex != line.Length-1)
                return Result<Location>.Fail(ErrorCode.InvalidFormat, "Error: Too many arguments \"\"");            

            line = line.Substring(0, line.Length - 1).Trim();

            return Result<Location>.Success(new Location(City, line));

        }
        internal Result<Meeting> ReadMeeting(string line, DataBase dataBase)
        {
            line = line.Trim();

            Location? location;
            float duration;
            List<Employee> guests = new List<Employee>();

            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 3)
                return Result<Meeting>.Fail(ErrorCode.InvalidFormat, "Error: not enough arguments");

            //finding the location
            string city = parts[0].Trim();
            location = dataBase.Locations.Find(l => l.City == city);
            if (location is null)
                return Result<Meeting>.Fail(ErrorCode.InvalidFormat, $"Error: there is no city:<{city}> in locations");

            //finding the duration
            string strDuration = parts[1].Trim();
            if (!strDuration.EndsWith('h'))
                return Result<Meeting>.Fail(ErrorCode.InvalidFormat, $"Error: duration dorsn't end with 'h'");
           
            if (float.TryParse(strDuration.Substring(0, strDuration.Length - 1), out float d))
                duration = d;
            else
                return Result<Meeting>.Fail(ErrorCode.InvalidFormat, $"Error: duration can't parse to float");

            //finding Guests

            for (int i = 2; i < parts.Length; i++)
            {
                Employee? guest = dataBase.Employees.Find(e => e.Name == parts[i]);
                if (guest is null)
                    return Result<Meeting>.Fail(ErrorCode.InvalidFormat, $"Error: there is no employee with name:<{parts[i]}>");

                guests.Add(guest);
            }

            return Result<Meeting>.Success(new Meeting(location, duration, guests));
        }
    }
}
