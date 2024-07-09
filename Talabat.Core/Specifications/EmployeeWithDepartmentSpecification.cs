using Talabat.Core.Entities;

namespace Talabat.Core.Specifications;
public class EmployeeWithDepartmentSpecification : BaseSpecifications<Employee>
{
    public EmployeeWithDepartmentSpecification()
    {
        Includes.Add(e => e.Department);
    }

    public EmployeeWithDepartmentSpecification(int id) : base(e => e.Id == id)
    {
        Includes.Add(e => e.Department);
    }
}
