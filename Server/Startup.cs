using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc.Server;
using SetValues.Contracts.Services;
using SetValues.Server.Contexts;
using SetValues.Server.Repositories;
using SetValues.Server.Services;

namespace SetValues.Server;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) => _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddCodeFirstGrpc();

        services
            .AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>))
            .AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        
        services.AddTransient<ICustomerService, CustomerService>();
        
        services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseRouting();
            
        app.UseGrpcWeb(new GrpcWebOptions
        {
            DefaultEnabled = true
        });
        app.UseCors();

        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapGrpcService<CustomerService>();
                endpoints.MapFallbackToFile("index.html");
            });
    }
}