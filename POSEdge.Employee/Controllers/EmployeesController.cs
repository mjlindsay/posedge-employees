using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using POSEdge.Employee.Application.Models;
using POSEdge.Employee.Application.Models.Validation;
using POSEdge.Employee.Application.Services;
using POSEdge.Employee.Domain.Models;
using POSEdge.Employee.Domain.Repositories;
using POSEdge.Employee.Domain.Services;

namespace POSEdge.Employee.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {

        private readonly ILogger<EmployeesController> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateEmployeeDto> _createEmployeeDtoValidator;
        private readonly IValidator<UpdateEmployeeDto> _updateEmployeeDtoValidator;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(
            ILogger<EmployeesController> logger,
            IMapper mapper,
            IValidator<CreateEmployeeDto> createEmployeeDtoValidator,
            IValidator<UpdateEmployeeDto> updateEmployeeDtoValidator,
            IEmployeeService employeeService,
            IEmployeeRepository employeeRepository) {
            _logger = logger;
            _mapper = mapper;
            _createEmployeeDtoValidator = createEmployeeDtoValidator;
            _updateEmployeeDtoValidator = updateEmployeeDtoValidator;
            _employeeService = employeeService;
            _employeeRepository = employeeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(
            [FromBody] CreateEmployeeDto createEmployeeDto) {

            var createEmployeeValidationResults = _createEmployeeDtoValidator.Validate(createEmployeeDto);

            if(!createEmployeeValidationResults.IsValid)
                return BadRequest(createEmployeeValidationResults.Errors);

            var employee = _mapper.Map<EmployeeInfo>(createEmployeeDto) with {
                PunchCode = await _employeeService.GetUniquePunchCode()
            };
            
            await _employeeRepository.CreateEmployee(employee);

            return new OkObjectResult(employee);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployeeById(
            [FromRoute] Guid id) {
            return new OkObjectResult(await _employeeRepository.GetEmployeeById(id));
        }

        [HttpGet]
        public async Task<IActionResult> SearchEmployees(
            [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] EmployeeSpecification? specification) {
            return specification is null ?
                new OkObjectResult(await _employeeRepository.GetEmployees())
                : new OkObjectResult(await _employeeRepository.GetEmployees(specification));
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee(
            [FromRoute] Guid id,
            [FromBody] UpdateEmployeeDto updateEmployeeDto) {

            var updateEmployeeDtoValidationResults = _updateEmployeeDtoValidator.Validate(updateEmployeeDto);

            if (!updateEmployeeDtoValidationResults.IsValid)
                return BadRequest(updateEmployeeDtoValidationResults.Errors);

            try {
                await _employeeRepository.UpdateEmployee(_mapper.Map<EmployeeInfo>(updateEmployeeDto) with {
                    Id = id
                });
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return new OkResult();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeById(
            [FromRoute] Guid id) {
            await _employeeRepository.DeleteEmployeeById(id);
            return new OkResult();
        }
    }
}