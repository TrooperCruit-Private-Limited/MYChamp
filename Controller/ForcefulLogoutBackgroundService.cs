using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MYChamp.Controller;
using MYChamp.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ForcefulLogoutBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IServiceProvider _serviceProvider;

    public ForcefulLogoutBackgroundService(IServiceScopeFactory scopeFactory, IServiceProvider serviceProvider)
    {
        _scopeFactory = scopeFactory;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("something print");
            using (var scope = _scopeFactory.CreateScope())
            {
                // Resolve the scoped services from the service provider
                var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<AppUser>>();
                var sessionHandlerController = scope.ServiceProvider.GetRequiredService<SessionHandlerController>();

                // Check if there is an authenticated user
                if (signInManager.Context.User.Identity.IsAuthenticated)
                {
                    // Your logic here
                    if (sessionHandlerController.checkForcefulLogout())
                    {
                        Console.WriteLine("outside force logout");
                        await signInManager.SignOutAsync();
                        // Clear all session data
                    }
                }
            }

            // Wait for 5 seconds before the next execution
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
