using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PollsApp;
using PollsApp.Interfaces;
using PollsApp.Repositories;
using PollsApp.Services;
using PollsApp.Hubs;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ����������� � SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����������� ������������
builder.Services.AddScoped<IPollRepository, PollRepository>();
builder.Services.AddScoped<IVoteRepository, VoteRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ����������� ��������
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPollService, PollService>();
builder.Services.AddScoped<IVoteService, VoteService>();

// ���������� CORS-��������, ����������� ��� �������
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// JWT-��������������
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// ��������� ����������� � ���������� JSON-������������ ��� ������������� ������
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddSignalR();

// ���������� Swagger � ����������� ����� �����������
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\n Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// �������� Swagger � ������ ����������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ��������� CORS-�������� "AllowAll"
app.UseCors("AllowSpecificOrigin");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<VoteHub>("/voteHub");

app.Run();
