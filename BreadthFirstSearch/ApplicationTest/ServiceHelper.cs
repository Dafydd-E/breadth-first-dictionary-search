using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApplicationTest
{
    /// <summary>
    /// Helper class to provide unit tests with dependency inected services.
    /// </summary>
    public class ServiceHelper
    {
        /// <summary>
        /// Gets the service provider.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; } =
            new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

        /// <summary>
        /// Gets the service from the service provider.
        /// </summary>
        /// <typeparam name="T">The type of the service to get.</typeparam>
        public static T GetService<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}
