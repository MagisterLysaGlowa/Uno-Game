
using api.RealTime;
using System.Text.Json.Serialization;

namespace api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //APPLICATION CONTROLLERS
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSignalR()
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.PayloadSerializerOptions.MaxDepth = 64; // Optional: Increase max depth if needed
                });

            //APPLICATION CORS CONFIGURATION
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowNextApp", corsBuilder =>
                {
                    corsBuilder
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            var app = builder.Build();

            //CONFIGURE HTTP SWAGGER ENVIRONMENT
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //ADD APPLICATION SERVICES
            app.UseWebSockets();
            app.UseCors("AllowNextApp");
            app.MapHub<ChatHub>("/chatHub").RequireCors("AllowNextApp");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
