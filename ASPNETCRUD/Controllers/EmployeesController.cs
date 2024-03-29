﻿using ASPNETCRUD.Data;
using ASPNETCRUD.Models;
using ASPNETCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;

        public EmployeesController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees =  await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        //Asynchonous method
        //Method for Adding
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Department = addEmployeeRequest.Department,
                //Gender = addEmployeeRequest.Gender,
            };

            await mvcDemoDbContext.Employees.AddAsync(employee); 
            await mvcDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //Method for viewing/read
        [HttpGet]
        public async Task<IActionResult> View(Guid id) 
        {
            var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if(employee !=null) 
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department,
                    //Gender = employee.Gender,
                };

                return await Task.Run(() => View("View" ,viewModel));
            }
            return RedirectToAction("Index");
        }

        //Method for updating 
        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);

            if(employee != null) 
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;
                //employee.Gender = model.Gender;

                await mvcDemoDbContext.SaveChangesAsync();

                     return RedirectToAction("Index");
            }
                 return RedirectToAction("Index");
        }

        //Method for Deleting
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee =  await mvcDemoDbContext.Employees.FindAsync(model.Id);

            if(employee != null) 
            {
                mvcDemoDbContext.Employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
