namespace moviles.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;
    using Services;
    using Helpers;

    public class LoginViewModel : BaseViewModel
    {
        #region services
        private ApiService apiService;
        #endregion 

        #region atributo
        private string email;
        private string password;
        private bool isrunning;
        private bool isenabled;
        #endregion

        #region propiedades
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }
        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }
        public bool IsRunning
        {
            get { return this.isrunning; }
            set { SetValue(ref this.isrunning, value); }
        }
        public bool IsRemember { get; set; }
        public bool IsEnabled
        {
            get { return this.isenabled; }
            set { SetValue(ref this.isenabled, value); }
        }
        #endregion

        #region Constructores
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.IsRemember = true;
            this.IsEnabled = true;
            this.Email = "arturo.aragonsanchez@hotmail.com";
            this.Password = "123456";
        }
        #endregion

        #region comandos
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error
                    , Languages.EmailValidation
                    , Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error
                    , Languages.PasswordValidation
                    , Languages.Accept);
                return;
            }
            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error
                    , connection.Message
                    , Languages.Accept);
                return;
            }

            var token = await this.apiService.GetToken("http://movilesapi.azurewebsites.net/", this.Email, this.Password);

            if (token == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error
                    , Languages.SomethingWrong
                    , Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(token.AccessToken))
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error
                    , token.ErrorDescription
                    , Languages.Accept);
                this.Password = string.Empty;
                return;
            }
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;
            this.IsRunning = false;
            this.IsEnabled = true;
            this.Email = string.Empty;
            this.Password = string.Empty;
            mainViewModel.Lands = new LandsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new LandsPage());
        }
        #endregion
    }
}