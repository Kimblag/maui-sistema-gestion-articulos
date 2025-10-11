using CatalogoApp.Core.Interfaces;
using CatalogoApp.Data.Repositorios;
using CatalogoApp.UI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace CatalogoApp.UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            //Buscamos el app settings que contiene la cadena de conexión de la base de datos.
            //var a = Assembly.GetExecutingAssembly();
            //using var stream = a.GetManifestResourceStream("CatalogoApp.UI.appsettings.json");

            ////Comprobar si no es null antes de ejecutar
            //if (stream != null)
            //{
            //    var config = new ConfigurationBuilder()
            //                .AddJsonStream(stream)
            //                .Build();

            //    builder.Configuration.AddConfiguration(config);
            //}

            //// inyección de dependencias.
            //// utilizamos singleton porque crea un solo objeto Repositorio la primera vez que se necesite, y luego reutiliza ese mismo objeto siempre
            //// como es ligero, Singleton es una opción eficiente y común.
            //builder.Services.AddSingleton<IRepositorioMarca, RepositorioMarcaSQL>();
            //builder.Services.AddSingleton<IRepositorioCategoria, RepositorioCategoriaSQL>();
            //builder.Services.AddSingleton<IRepositorioArticulo, RepositorioArticuloSQL>();

            //// En este caso se utiliza Transient, a diferencia de Singleton, cada vez que se necesita un VM se trae uno nuevo.
            //builder.Services.AddTransient<MarcaViewModel>();
            //builder.Services.AddTransient<CategoriaViewModel>();
            //builder.Services.AddTransient<ArticuloViewModel>();

            //builder.Services.AddTransient<MainPage>();
            //builder.Services.AddTransient<ArticuloPage>();
            //builder.Services.AddTransient<MarcaPage>();
            //builder.Services.AddTransient<CategoriaPage>();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
