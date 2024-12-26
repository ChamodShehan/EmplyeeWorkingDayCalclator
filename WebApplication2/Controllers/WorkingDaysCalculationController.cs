using Microsoft.AspNetCore.Mvc;
using WebApplication2.Entry;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    public class WorkingDaysCalculationController : Controller
    { 
        private readonly EmployeeService _employeeService;
 
        public WorkingDaysCalculationController(EmployeeDB empDB, EmployeeService employeeService)
        {
  
            _employeeService = employeeService;
        }
      
        public IActionResult CalculateWorkingDays()
        {
            return View();
        }


        [HttpPost]
        public JsonResult CalculateWorkingDays([FromBody] DateRange dateRange)
        {

            if (dateRange == null || dateRange.StartDate == null || dateRange.EndDate == null)
            {
                return Json(new { success = false, message = "Invalid date range." });
            }

            Console.WriteLine($"Received date range: StartDate = {dateRange.StartDate}, EndDate = {dateRange.EndDate}");

            var workingDays = _employeeService.CalculateWorkingDays(dateRange.StartDate, dateRange.EndDate);
            return Json(new { success = true, workingDays });
        }
    }


}
