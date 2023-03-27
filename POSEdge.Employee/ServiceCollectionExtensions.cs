using FluentValidation;
using POSEdge.Employee.Application.Models;
using POSEdge.Employee.Application.Models.Validation;
using System.Runtime.CompilerServices;

namespace POSEdge.Employee
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddValidators(this IServiceCollection services) {

            services.AddScoped<IValidator<CreateEmployeeDto>, CreateEmployeeDtoValidator>();
            services.AddScoped<IValidator<UpdateEmployeeDto>, UpdateEmployeeDtoValidator>();
            return services;
        }
    }
}
