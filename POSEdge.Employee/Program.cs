
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using POSEdge.Employee.Application.Configuration;
using POSEdge.Employee.Application.Models;
using POSEdge.Employee.Application.Services;
using POSEdge.Employee.Configuration;
using POSEdge.Employee.Domain.Models;
using POSEdge.Employee.Domain.Repositories;
using POSEdge.Employee.Domain.Services;
using POSEdge.Employee.Infrastructure.Repositories;

namespace POSEdge.Employee
{
    public class Program
    {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add custom validators (built using FLuentValidation)
            builder.Services.AddValidators();

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddAutoMapper(autoMapperCfg => {
                autoMapperCfg.AddProfile<DtoMappingProfile>();
            });

            #region Mongo
            builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection(ConfigConstants.MongoDbSectionName));

            builder.Services.AddSingleton<IMongoClient>(mongoClient => {
                var mongoOptions = builder.Configuration.GetSection(ConfigConstants.MongoDbSectionName).Get<MongoDbOptions>();
                return new MongoClient(mongoOptions.ConnectionString);
            });

            builder.Services.AddSingleton<IMongoDatabase>(svc => {
                var mongoOptions = svc.GetRequiredService<IOptions<MongoDbOptions>>();
                return svc.GetRequiredService<IMongoClient>().GetDatabase(mongoOptions.Value.DatabaseName);
            });

            builder.Services.AddSingleton<IMongoCollection<EmployeeInfo>>(svc => {
                var mongoOptions = svc.GetRequiredService<IOptions<MongoDbOptions>>();
                return svc.GetRequiredService<IMongoDatabase>().GetCollection<EmployeeInfo>(mongoOptions.Value.EmployeeCollectionName);
            });
            #endregion

            builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddSingleton<IEmployeeService, EmployeeService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}