using CatalogoApp.UI.ViewModels;

namespace CatalogoApp.UI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel(ContentArea);
        }

    }
}
