using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Project.DbContexts;
using Project.Entities;
using Project.Services;
using System.Text;

var AllowedOrigins = "_allowedOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowedOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
}).AddEntityFrameworkStores<CourseStatisticsContext>();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
}); ;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme="oauth2",
                    Name="Bearer",
                    In= ParameterLocation.Header,
                },
                new List<string>()
            }
        });
});

builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<ICourseStatisticsService, CourseStatisticsService>();
builder.Services.AddScoped<ISurvivalAnalysisService, SurvivalAnalysisService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<CourseStatisticsContext>(optionsBuilder =>
{
    //optionsBuilder.UseSqlServer("Server=(local);Database=Test;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
    //optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("CourseStatistics"));
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("CourseStatistics"));
});

builder.Services.AddDbContext<SurvivalAnalysisContext>(optionsBuilder =>
{
    //optionsBuilder.UseSqlServer("Server=(local);Database=Test;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
    //optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("SurvivalAnalysis"));
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("SurvivalAnalysis"));
});

var app = builder.Build();

// for testing
app.MapGet("/", () => "Hello World!");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(AllowedOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CourseStatisticsContext>();
    db.Database.Migrate();
}


app.Run();
