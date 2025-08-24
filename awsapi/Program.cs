using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using awsapi.Repository;
using awsapi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace awsapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            //builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
            builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions("AWS"));
            builder.Services.AddAWSService<IAmazonS3>();
            builder.Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
            builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
            builder.Services.AddSingleton<ICustomerService, CustomerService>();
            //builder.Services.AddSingleton<IAmazonS3>(sp =>
            //{
            //    var config = new AmazonS3Config
            //    {
            //        RegionEndpoint = RegionEndpoint.USEast1 // Choose your region
            //    };
            //    var credentials = new BasicAWSCredentials("YOUR_ACCESS_KEY", "YOUR_SECRET_KEY");

            //    return new AmazonS3Client(credentials, config);
            //});

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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
