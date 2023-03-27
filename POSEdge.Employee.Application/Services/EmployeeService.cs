using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using POSEdge.Employee.Application.Configuration;
using POSEdge.Employee.Application.Exceptions;
using POSEdge.Employee.Domain.Models;
using POSEdge.Employee.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSEdge.Employee.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMongoCollection<EmployeeInfo> _employeeCollection;

        public EmployeeService(
            ILogger<EmployeeService> logger,
            IMongoCollection<EmployeeInfo> employeeCollection) {
            _logger = logger;
            _employeeCollection = employeeCollection;
        }

        public async Task<string> GetUniquePunchCode() {
            var filter = Builders<EmployeeInfo>.Filter.Empty;
            var projection = Builders<EmployeeInfo>.Projection.Include("PunchCode");

            var queryResults = (await _employeeCollection.FindAsync(filter)).ToList();

            var usedPunchCodes = queryResults.Select(emp => emp.PunchCode);

            var random = new Random();
            string? newPunchCode = null;
            int attemptLimit = 50; // TODO: Move to configuration
            for(int i = 0; i < attemptLimit; i++) {
                string potentialPunchCode = random.Next(9999, 999999).ToString(); // TODO: Move to configuration

                if (!usedPunchCodes.Contains(potentialPunchCode))
                    newPunchCode = potentialPunchCode;
            }

            if (newPunchCode is null)
                throw new PunchCodeGenerationException();

            return (string) newPunchCode;
        }


    }
}
