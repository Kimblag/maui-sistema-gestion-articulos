using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CatalogoApp.UI.ViewModels
{
    /// <summary>
    /// Esta clase es la base para todos los MV, ya que permite simplificar la lógica que permite notificar cuando un 
    /// objeto cambia sus propiedades, en lugar de crearlo en cada model view, evitamos repetir la lógica.
    /// </summary>
    public partial class BaseViewModel : INotifyPropertyChanged
    {
        // public event: Declara un "evento". imagino que un evento es como un canal de radio.
        // Otras partes del código (la UI) pueden sintonizar este canal.
        // PropertyChanged: Es el nombre del evento que la interfaz INotifyPropertyChanged nos obliga a tener.
        //PropertyChangedEventHandler?: Es el tipo de "delegado", define qué tipo de método puede suscribirse a
        //este evento.El ? indica que es posible que nadie esté escuchando el canal(que el evento sea nulo).
        public event PropertyChangedEventHandler? PropertyChanged; // el contrato exige tener esta propiedad


        // Método de ayuda para emitir ese "canal de radio".
        // protected: Significa que este método solo puede ser llamado desde la propia BaseViewModel o
        // desde las clases que hereden de ella(como MarcaViewModel).
        // OnPropertyChanged: Es un nombre de convención para un método que dispara un evento.
        // [CallerMemberName]: Esto es "magia" del compilador. Cuando se llama a OnPropertyChanged() desde una propiedad,
        // este atributo inserta automáticamente el nombre de esa propiedad en el parámetro propertyName.
        // Esto ahorra escribir el nombre a mano y previene errores de tipeo.
        protected void OnPropertyChanged([CallerMemberName] string? nombrePropiedad = null)
        {

            // Esta es la línea que finalmente hace sonar la radio.
            // PropertyChanged?: El ? comprueba si alguien está escuchando el canal de
            // radio. Si nadie está sintonizado(es nulo), no hace nada y evita un error.
            // .Invoke(...): Si hay oyentes, .Invoke "dispara" el evento, enviando dos piezas de información:
            // this: Quién está enviando la notificación(el propio ViewModel).
            // new PropertyChangedEventArgs(propertyName): Un objeto que contiene el nombre de
            // la propiedad que ha cambiado, para que la UI sepa exactamente qué parte de la pantalla debe actualizar.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombrePropiedad));
        }
    }
}
