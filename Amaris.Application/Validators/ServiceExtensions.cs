using Amaris.Application.Interfaces;
using Amaris.Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Amaris.Application.Validators
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITurnService, TurnService>();
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateTurnValidator>();
            return services;
        }
    }
}
