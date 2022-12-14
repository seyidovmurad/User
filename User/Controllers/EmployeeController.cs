using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using User.Data;
using User.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace User.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly WorkDbContext _context;

        public EmployeeController(WorkDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var employees = _context.Employees;
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var employee = new Employee();
            return View(employee);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var found = _context.Employees.Find(id);

            if(found == null)
                return Redirect("/");

            _context.Employees.Remove(found);
            _context.SaveChanges();
            return Redirect("/");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var found = _context.Employees.Find(id);

            if (found == null)
                return Redirect("/");

            return View(found);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                
                _context.Employees.Update(employee);
                _context.SaveChanges();
                return Redirect("/");
            }
            else
            {
                return View(employee);
            }
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            if(ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return Redirect("/");
            }
            else
            {
                return View(employee);
            }
        }
    }

    public class JsonHandle
    {
        public static List<Employee> GetEmployee()
        {
            if (!File.Exists("data.json"))
                File.Create("data.json").Close();
            List<Employee> employees;
            using (StreamReader r = new StreamReader("data.json", Encoding.UTF8))
            {

                string json = r.ReadToEnd();
                try
                {

                    employees = JsonSerializer.Deserialize<List<Employee>>(json);
                }
                catch
                {
                    employees = new List<Employee>();
                }
            }
            return employees;
        }


        public static void AddEmployee(Employee employee)
        {
            var employees = GetEmployee();

            int id = employees.LastOrDefault()?.Id ?? 0;

            employee.Id = ++id;
            employees.Add(employee);

            string jsonString = JsonSerializer.Serialize(employees);
            using (StreamWriter outputFile = new StreamWriter("data.json"))
            {
                outputFile.Write(jsonString);
            }
        }
    }


}

