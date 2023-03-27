using MongoDB.Driver;
using POSEdge.Employee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSEdge.Employee.Infrastructure
{
    internal static class MongoBuilderExtensions
    {

        internal static FilterDefinition<EmployeeInfo> FromSpecification(
            this FilterDefinitionBuilder<EmployeeInfo> filterBuilder,
            EmployeeSpecification specification) {

            var filter = filterBuilder.Empty;
            if(!string.IsNullOrWhiteSpace(specification.FirstName)) {
                filter &= filterBuilder.Eq("FirstName", specification.FirstName);
            }

            if(!string.IsNullOrWhiteSpace(specification.LastName)) {
                filter &= filterBuilder.Eq("LastName", specification.LastName);
            }

            if(!string.IsNullOrWhiteSpace(specification.Role)) {
                filter &= filterBuilder.Eq("Role", specification.Role);
            }

            if(!string.IsNullOrWhiteSpace(specification.PunchCode)) {
                filter &= filterBuilder.Eq("PunchCode", specification.PunchCode);
            }

            return filter;
        }
    }
}
