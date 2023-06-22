using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FrogExhibitionPL.Swashbuckle;
using Microsoft.OpenApi.Models;
using FrogExhibitionDAL.Models;
using FrogExhibitionBLL.Services;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.UoW;
using FrogExhibitionDAL.Database;
using FrogExhibitionBLL.Helpers;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.ExhibitionViewModels;
using FrogExhibitionBLL.DTO.VoteDtos;
using FrogExhibitionBLL.Interfaces.IHelper;

namespace FrogExhibitionPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            

            builder.Services.AddDbContext<ApplicationContext>(
                o => o.UseLazyLoadingProxies()
                    .UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 5;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IFrogService, FrogService>();
            builder.Services.AddScoped<IExhibitionService, ExhibitionService>();
            builder.Services.AddScoped<IFrogOnExhibitionService,FrogOnExhibitionService>();
            builder.Services.AddScoped<IVoteService, VoteService>();
            builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
            builder.Services.AddScoped<IFrogPhotoService, FrogPhotoService>();
            builder.Services.AddScoped<IUserProvider, UserProvider>();

            builder.Services.AddSingleton<IFileHelper, FileHelper>();
            builder.Services.AddSingleton<ISortHelper<Frog>, SortHelper<Frog>>();
            builder.Services.AddSingleton<ISortHelper<ExhibitionDetailViewModel>, SortHelper<ExhibitionDetailViewModel>>();



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
                    ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
                };
            }
            );

            builder.Services.AddTransient<IAuthService, AuthService>();



            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddTransient<Seed>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option => {
                // ...
                option.SchemaFilter<SchemaFilter>();
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });

            

            var app = builder.Build();

            if (args.Length == 1 && args[0].ToLower() == "seeddata")
                SeedData(app);

            async void SeedData(IHost app)
            {
                var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

                using (var scope = scopedFactory.CreateScope())
                {
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                    var voteService = scope.ServiceProvider.GetRequiredService<IVoteService>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roles = new[] { "Admin", "User" };
                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }

                    var adminUser = new LoginUser() { Email = "Admin@mail.com", Password = "P@ssw0rd" };
                    var regularUser = new LoginUser() { Email = "User@mail.com", Password = "P@ssw0rd" };
                    await authService.RegisterUserAsync(adminUser);
                    await authService.RegisterUserAsync(regularUser);


                    await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(adminUser.Email), "Admin");
                    await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(regularUser.Email), "User");
                    var service = scope.ServiceProvider.GetService<Seed>();
                    service.SeedApplicationContextAsync();

                    var frogsOnExhibitions = unitOfWork.FrogOnExhibitions.GetAll();
                    var users = userManager.Users.ToList();
                    foreach (var user in users)
                    {
                        var voteCount = 0;
                        foreach (var frogOnExhibition in frogsOnExhibitions)
                        { 
                            if (voteCount<3)
                            {
                                var temp = new VoteDtoForCreate()
                                {
                                    ApplicationUserId = user.Id,
                                    FrogOnExhibitionId = frogOnExhibition.Id
                                };
                                try
                                {
                                    await voteService.CreateVoteAsync(temp);
                                    voteCount++;
                                }
                                catch (Exception)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();


            app.MapControllers();

            app.Run();
        }
    }
}