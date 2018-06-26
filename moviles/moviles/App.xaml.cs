namespace moviles
{
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using Views;
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }
        #region  Methods
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Cuando se minimiza la aplicacion
        }

        protected override void OnResume()
        {
            // Cuando se vuelve a abrir la aplicacion
        }
        #endregion
    }
}