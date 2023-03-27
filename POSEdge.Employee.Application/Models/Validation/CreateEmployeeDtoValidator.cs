using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSEdge.Employee.Application.Models.Validation
{
    public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeDtoValidator() {
            RuleFor(createEmployee => createEmployee.FirstName)
                .NotEmpty();
            RuleFor(createEmployee => createEmployee.LastName)
                .NotEmpty();
            RuleFor(createEmployee => createEmployee.Role)
                .NotEmpty();
        }
    }
}
