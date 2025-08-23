using Amazon.S3;
using Microsoft.Extensions.Configuration;

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
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
