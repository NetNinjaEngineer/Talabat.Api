﻿using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IGenericRepository<Employee> _employeeRepo;

    public EmployeesController(IGenericRepository<Employee> employeeRepo)
    {
        _employeeRepo = employeeRepo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployeesAsync()
    {
        var spec = new EmployeeWithDepartmentSpecification();
        var employees = await _employeeRepo.GetAllWithSpecificationAsync(spec);
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeeAsync(int id)
    {
        var spec = new EmployeeWithDepartmentSpecification(id);
        var employee = await _employeeRepo.GetByIdWithSpecification(spec);
        return Ok(employee);
    }

}