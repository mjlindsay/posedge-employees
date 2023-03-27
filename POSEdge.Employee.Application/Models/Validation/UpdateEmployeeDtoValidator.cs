using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSEdge.Employee.Application.Models.Validation
{
    public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
    {

        public UpdateEmployeeDtoValidator() {
            RuleFor(dto => dto.FirstName)
                .NotEmpty();
            RuleFor(dto => dto.LastName)
                .NotEmpty();
            RuleFor(dto => dto.Role)
                .NotEmpty();
            RuleFor(dto => dto.PunchCode)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(6)
                .Must(punchCode => Int32.TryParse(punchCode, out int parsedPunchCode))
                .Must(punchCode => Int32.Parse(punchCode) >= 9999)
                .Must(punchCode => Int32.Parse(punchCode) <= 999999);
        }
    }
}
