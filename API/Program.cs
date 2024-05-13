
using API.Models;
using API.Reposatories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors();

            builder.Services.AddDbContext<NewsContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Production")));

            builder.Services.AddIdentityApiEndpoints<IdentityUser>()
                .AddEntityFrameworkStores<NewsContext>();

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddTransient<IReposatory<Article>, Reposatory<Article>>();
            builder.Services.AddTransient<IReposatory<Author>, Reposatory<Author>>();

            builder.Services.AddScoped<ISieveProcessor, SieveProcessor>();
            builder.Services.AddScoped<UserManager<IdentityUser>>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseCors(options => options
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
                );

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.MapGroup("/api/v1/account").MapIdentityApi<IdentityUser>();

            app.Run();
        }
    }
}
