
using AutoMapper;
using Managment.Common;
using Managment.Common.Middlewares;
using Managment.Common.Models;
using Managment.Employees.Data;
using Managment.Employees.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions( x => x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Auth Service", Version = "v1" });
        c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
            In = ParameterLocation.Header,
            Name = "Authorization",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme."
        });
    });

builder.ConfigureAutharization();
builder.ConfigureAuthentication();
//builder.ConfigureJwt();
builder.Services.AddDbContext<EmployeesDbContext>(options => options.UseInMemoryDatabase("EmployeesInMemory"));

builder.Services.AddTransient<IRepository<EmployeeBase>, EmployeeBaseRepository>();
builder.Services.AddTransient<IRepository<EmployeeSubordinate>, SubordinateRepository>();

builder.Services.AddTransient<IRepository<EmployeeManager>, ManagerRepository>();
// builder.Services.AddTransient(provider => new MapperConfiguration(cfg =>
//     {
//         cfg.AddProfile(new AppProfile(provider.GetService<IRepository<EmployeeBase>>()));
//     }).CreateMapper());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<InMemoryDbSeeder>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
if (app.Environment.IsDevelopment())
{
    await app.Services.CreateScope().ServiceProvider.GetService<InMemoryDbSeeder>().SeedDb();
}
app.Run();
