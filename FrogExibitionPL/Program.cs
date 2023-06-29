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
using FrogExhibitionBLL.Interfaces.IHelper;
using FrogExhibitionBLL.Constants;
using FrogExhibitionBLL.Interfaces.IProvider;
using FrogExhibitionBLL.Providers;
using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;

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
            builder.Services.AddScoped<IFrogOnExhibitionService, FrogOnExhibitionService>();
            builder.Services.AddScoped<IVoteService, VoteService>();
            builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
            builder.Services.AddScoped<IFrogPhotoService, FrogPhotoService>();
            builder.Services.AddScoped<IFrogStarRatingService, FrogStarRatingService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<IUserProvider, UserProvider>();

            builder.Services.AddSingleton<IFileHelper, FileHelper>();
            builder.Services.AddSingleton<ISortHelper<Frog>, SortHelper<Frog>>();
            builder.Services.AddSingleton<ISortHelper<Exhibition>, SortHelper<Exhibition>>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
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
            builder.Services.AddSwaggerGen(option =>
            {
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
                    var roles = new[] { 
                        RoleConstants.AdminRole,
                        RoleConstants.UserRole,
                        RoleConstants.UserAdminRole
                    };
                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }

                    var adminUser = new ApplicationUserDtoForLogin() { Email = "Admin@mail.com", Password = "P@ssw0rd" };
                    var userAdminUser = new ApplicationUserDtoForLogin() { Email = "UserAdmin@mail.com", Password = "P@ssw0rd" };
                    var userUser = new ApplicationUserDtoForLogin() { Email = "User@mail.com", Password = "P@ssw0rd" };
                    await authService.RegisterUserAsync(adminUser);
                    await authService.RegisterUserAsync(userAdminUser);
                    await authService.RegisterUserAsync(userUser);


                    await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(adminUser.Email), RoleConstants.AdminRole);
                    await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(adminUser.Email), RoleConstants.UserAdminRole);
                    await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(adminUser.Email), RoleConstants.UserRole);
                    await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(userAdminUser.Email), RoleConstants.UserAdminRole);
                    await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(userAdminUser.Email), RoleConstants.UserRole);
                    await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(userUser.Email), RoleConstants.UserRole);
                    var service = scope.ServiceProvider.GetService<Seed>();
                    service.SeedApplicationContextAsync();

                    var frogsOnExhibitions = await unitOfWork.FrogOnExhibitions.GetAllAsync();
                    var frogs = await unitOfWork.Frogs.GetAllAsync();
                    var rand = new Random();
                    var users = userManager.Users.ToList();
                    var votes = new List<Vote>();
                    users
                        .ForEach(u => frogsOnExhibitions.Take(3).ToList()
                        .ForEach(frgOnEx => votes.Add(new Vote()
                        {
                            ApplicationUserId = u.Id,
                            FrogOnExhibitionId = frgOnEx.Id
                        })));
                    await unitOfWork.Votes.CreateRangeAsync(votes);
                   

                    var comments = new List<Comment>();
                    users
                        .ForEach(u => frogsOnExhibitions.ToList()
                        .ForEach(frgOnEx => comments.Add(new Comment()
                        {
                            ApplicationUserId = u.Id,
                            FrogOnExhibitionId = frgOnEx.Id,
                            Text = $"imho this frog is top{rand.Next(10)} in the world",
                            CreationDate = DateTime.Now
                        })));

                    await unitOfWork.Comments.CreateRangeAsync(comments);

                    var frogStarRatings = new List<FrogStarRating>();
                    users
                        .ForEach(u => frogs.ToList()
                        .ForEach(f => frogStarRatings.Add(new FrogStarRating()
                        {
                            ApplicationUserId = u.Id,
                            FrogId = f.Id,
                            Rating = rand.Next(5),
                            Comment = rand.Next(1000).ToString()
                        })));
                    await unitOfWork.FrogStarRatings.CreateRangeAsync(frogStarRatings);
                    await unitOfWork.SaveAsync();
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