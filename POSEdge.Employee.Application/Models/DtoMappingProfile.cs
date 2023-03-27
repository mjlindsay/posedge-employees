using AutoMapper;
using POSEdge.Employee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSEdge.Employee.Application.Models
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile() {
            CreateMap<CreateEmployeeDto, EmployeeInfo>();
            CreateMap<UpdateEmployeeDto, EmployeeInfo>();
        }
    }
}
