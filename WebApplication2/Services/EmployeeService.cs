using WebApplication2.Entry;
using WebApplication2.Helpers;

namespace WebApplication2.Services
{
    public class EmployeeService
    {
        private readonly EmployeeDB _empDB; 
        private readonly CacheHelper _cacheHelper;

        public EmployeeService(EmployeeDB empDB, CacheHelper cacheHelper)
        {
            _empDB = empDB;
            _cacheHelper = cacheHelper;
        }

        public int CalculateWorkingDays(DateTime startDate, DateTime endDate)
        {


            // Retrieve holidays from cache or database
            var publicHolidays = _cacheHelper.CachedLong("PublicHolidays", _empDB.GetPublicHolidays);

            // Calculate working days logic remains the same...
            while (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday)
            {
                startDate = startDate.AddDays(1);
            } 

            int workingDays = 0;

            // Loop through the dates from start to end
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Check if the current date is a weekday and not a holiday
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday &&
                    !publicHolidays.Contains(date.Date))
                {
                    workingDays++;
                }
            }

            return workingDays;
        }


    }
}
