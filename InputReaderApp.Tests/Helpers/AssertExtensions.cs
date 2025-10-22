using InputReaderApp.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Tests.Helpers
{
    public static class AssertExtensions
    {
        /// <summary>
        /// Deeply compares the contents of two 2D arrays for equality.
        /// Fails if dimensions differ or any element is not equal.
        /// </summary>
        public static void Equal2D<T>(T[,] expected, T[,] actual)
        {
            if (expected is null)
                throw new ArgumentNullException(nameof(expected));
            if (actual is null)
                throw new ArgumentNullException(nameof(actual));

            int expectedRows = expected.GetLength(0);
            int expectedCols = expected.GetLength(1);
            int actualRows = actual.GetLength(0);
            int actualCols = actual.GetLength(1);

            Assert.Equal(expectedRows, actualRows);
            Assert.Equal(expectedCols, actualCols);

            for (int i = 0; i < expectedRows; i++)
            {
                for (int j = 0; j < expectedCols; j++)
                {
                    Assert.Equal(expected[i, j], actual[i, j]);
                }
            }
        }
        public static void EqualLists<T>(List<T> expected, List<T> actual)
        {
            if (expected is null)
                throw new ArgumentNullException(nameof(expected));
            if (actual is null)
                throw new ArgumentNullException(nameof(actual));

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i], actual[i]);                
            }
        }
        public static void EqualNestedLists<T>(List<List<T>> expected, List<List<T>> actual)
        {
            if (expected is null)
                throw new ArgumentNullException(nameof(expected));
            if (actual is null)
                throw new ArgumentNullException(nameof(actual));

            Assert.Equal(expected.Count, actual.Count);            

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Count, actual[i].Count);
                for (int j = 0; j < expected[i].Count; j++)
                {
                    Assert.Equal(expected[i][j], actual[i][j]);
                }
            }


        }
        public static void EqualDataBase(DataBase expected,  DataBase actual)
        {            
            EqualLists(expected.Employees, actual.Employees);
            EqualLists(expected.Locations, actual.Locations);
            
            Assert.Equal(expected.Meetings.Count, actual.Meetings.Count);
            
            for(int i = 0; i < expected.Meetings.Count; i++)
            {               
                EqualMeetings(expected.Meetings[i], actual.Meetings[i]);
            }
        }
        public static void EqualMeetings(Meeting expected, Meeting actual)
        {
            if (expected is null)
                throw new ArgumentNullException(nameof(expected));
            if (actual is null)
                throw new ArgumentNullException(nameof(actual));

            Assert.Equal(expected.Location, actual.Location);
            Assert.Equal(expected.DurationInHours, actual.DurationInHours);

            Assert.Equal(expected.Guests.Count, expected.Guests.Count);
            for (int i = 0; i < expected.Guests.Count; i++)
            {
                Assert.Equal(expected.Guests[i], expected.Guests[i]);
            }
        }
    }
}
