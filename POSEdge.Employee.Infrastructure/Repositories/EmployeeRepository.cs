using MongoDB.Driver;
using POSEdge.Employee.Domain.Models;
using POSEdge.Employee.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSEdge.Employee.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IMongoCollection<EmployeeInfo> _employeeCollection;

        public EmployeeRepository(
            IMongoCollection<EmployeeInfo> employeeCollection) {
            _employeeCollection = employeeCollection;
        }

        public async Task CreateEmployee(EmployeeInfo employee) {
            await _employeeCollection.InsertOneAsync(employee);
        }

        public async Task DeleteEmployeeById(Guid id) {
            await _employeeCollection.DeleteOneAsync(GetIdFilter(id));
        }

        public async Task DeleteEmployeeByPunchCode(string punchCode) {
            await _employeeCollection.DeleteOneAsync(GetPunchCodeFilter(punchCode));
        }

        public async Task<IEnumerable<EmployeeInfo>> GetEmployees()
            => await GetEmployees(new EmployeeSpecification());

        public async Task<IEnumerable<EmployeeInfo>> GetEmployees(EmployeeSpecification specification) {
            var employees = await _employeeCollection.FindAsync(Builders<EmployeeInfo>.Filter.FromSpecification(specification));
            
            return employees.ToEnumerable();
        }

        public async Task<EmployeeInfo?> GetEmployeeById(Guid id) {
            var employees = await _employeeCollection.FindAsync(GetIdFilter(id));
            return employees.First();
        }

        public async Task<EmployeeInfo?> GetEmployeeByPunchCode(string punchCode) {
            var employees = await _employeeCollection.FindAsync(GetPunchCodeFilter(punchCode));
            return employees.First();
        }

        public async Task UpdateEmployee(EmployeeInfo employee) {
            await _employeeCollection.ReplaceOneAsync(GetIdFilter(employee.Id), employee);
        }

        private FilterDefinition<EmployeeInfo> GetIdFilter(Guid id)
            => GetFilter("Id", id.ToString());

        private FilterDefinition<EmployeeInfo> GetPunchCodeFilter(string punchCode)
            => GetFilter("PunchCode", punchCode);

        private FilterDefinition<EmployeeInfo> GetFilter(string fieldName, string punchCode) {
            return Builders<EmployeeInfo>.Filter.Eq(fieldName, punchCode);
        }
    }
}
