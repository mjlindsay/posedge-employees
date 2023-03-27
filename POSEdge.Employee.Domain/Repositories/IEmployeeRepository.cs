using POSEdge.Employee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSEdge.Employee.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeInfo>> GetEmployees();

        Task<IEnumerable<EmployeeInfo>> GetEmployees(EmployeeSpecification specification);

        Task<EmployeeInfo?> GetEmployeeById(Guid id);

        Task<EmployeeInfo?> GetEmployeeByPunchCode(string punchCode);

        Task CreateEmployee(EmployeeInfo employee);

        Task UpdateEmployee(EmployeeInfo employee);

        Task DeleteEmployeeById(Guid id);

        Task DeleteEmployeeByPunchCode(string punchCode);
    }
}
