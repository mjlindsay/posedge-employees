namespace POSEdge.Employee.Application.Configuration
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string EmployeeCollectionName { get; set; }
    }
}
