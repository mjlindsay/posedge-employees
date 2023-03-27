using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSEdge.Employee.Application.Models
{
    public record class CreateEmployeeDto(
        string FirstName,
        string LastName,
        string Role
        );
}
