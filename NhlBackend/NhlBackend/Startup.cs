using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NhlBackend.Contracts.Services;
using NhlBackend.Models.Converters;
using NhlBackend.Models.ModelBinderProviders;
using NhlBackend.Services;
using Prometheus;

namespace NhlBackend;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IMatchesService, MatchesService>();
        services.AddScoped<IPlayersService, PlayersService>();
        services.AddScoped<ITeamsService, TeamsService>();
        services.AddScoped<IUsersService, UsersService>();

        services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateOnlyModelBinderProvider());
                options.ModelBinderProviders.Insert(1, new YearOnlyModelBinderProvider());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new YearOnlyJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();    
        }
        
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "NHL API v1");
            options.RoutePrefix = string.Empty;
        });
        
        app.UseMetricServer(); 
        app.UseHttpMetrics(opts =>
        {
            opts.AddCustomLabel("route", ctx => ctx.Request.Path.Value!.ToLowerInvariant());
        });
        
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}