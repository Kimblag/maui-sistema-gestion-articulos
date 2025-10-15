using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogoApp.UI.Helpers
{
    public static class ServiceHelper
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static T GetService<T>() where T : notnull
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}

