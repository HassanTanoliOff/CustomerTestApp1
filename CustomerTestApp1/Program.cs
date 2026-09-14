using System.Text;
using CustomerTestApp1;
using CustomerTestApp1.Auths;
using CustomerTestApp1.Data;
using CustomerTestApp1.Models;
using CustomerTestApp1.Services;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//builder.Services.AddSwaggerGen();
//builder.Services.AddOpenApi();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IPasswordHasher<User>, PassWordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var jwtKey = builder.Configuration["Jwt:Key"]!;

builder
  .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,

      ValidIssuer = builder.Configuration["Jwt:Issuer"],
      ValidAudience = builder.Configuration["Jwt:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
      ClockSkew = TimeSpan.Zero,
    };
    options.Events = new JwtBearerEvents
    {
      /// no token or invalid expired token
      OnChallenge = async context =>
      {
        context.HandleResponse();
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(
          new { Error = " Failed to open.", message = "Login Required to access this page." }
        );
      },

      OnForbidden = async context =>
      {
        context.Response.StatusCode = 403;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(
          new { Error = " Failed to open.", message = "Only Admins can use this service." }
        );
      },
    };
  });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
  c.AddSecurityDefinition(
    "Bearer",
    new OpenApiSecurityScheme
    {
      Name = "Authorization",
      Type = SecuritySchemeType.Http,
      Scheme = "Bearer",
      BearerFormat = "JWT",
      In = ParameterLocation.Header,
      Description = "Enter Your jwt Token.",
    }
  );

  c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
  {
    [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
  });
});

builder.Services.AddCors(options =>
{
  options.AddPolicy(
    "AllowBlazorUI",
    policy => policy.WithOrigins("http://localhost:5266").AllowAnyHeader().AllowAnyMethod()
  );
});

var app = builder.Build();

await DbSeeder.SeedAdminAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowBlazorUi");
app.MapControllers();

app.Run();
