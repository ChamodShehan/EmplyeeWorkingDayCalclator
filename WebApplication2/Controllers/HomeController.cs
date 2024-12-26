using Microsoft.AspNetCore.Mvc;
using WebApplication2.Entry;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeDB _empDB; 
        public HomeController(EmployeeDB empDB, EmployeeService employeeService)
        {
            _empDB = empDB; 
        }

        // GET: Home/Index
        public IActionResult Index()
        {
            return View();
        }
         
        public JsonResult List()
        { 
            var employeeList = _empDB.ListAll();
             
            Console.WriteLine("Employee List Retrieved:");
            foreach (var emp in employeeList)
            {
                Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Email: {emp.Email}, Position: {emp.JobPosition}");
            } 
            return Json(employeeList);
        }

  
        [HttpPost]
        public JsonResult Add([FromBody] Employee emp)
        {
            if (emp == null)
            {
                return Json(new { success = false, message = "Invalid employee data." });
            }

            var result = _empDB.Add(emp);
            return Json(result);
        }

 
        public JsonResult GetById(int ID)
        { 
            var employee = _empDB.GetById(ID);
             
            return Json(employee);
        }
 
        [HttpPost]
        public JsonResult Update([FromBody] Employee emp)
        {
            if (emp == null || emp.Id <= 0)
            {
                return Json(new { success = false, message = "Invalid employee data." });
            }

            try
            { 
                int result = _empDB.Update(emp);  
                if (result > 0)  
                {
                    return Json(new { success = true, message = "Employee updated successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to update employee." });
                }
            }
            catch (Exception ex)
            { 
                return Json(new { success = false, message = $"Error during update: {ex.Message}" });
            }
        }

        public JsonResult Delete(int ID)
        { 
            var result = _empDB.Delete(ID); 
            return Json(result);
        } 
    }
}
