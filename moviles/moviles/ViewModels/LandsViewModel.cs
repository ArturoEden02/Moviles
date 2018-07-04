namespace moviles.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using Xamarin.Forms;

    public class LandsViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<LandItemViewModel> lands;
        private bool isRefreshing;
        private string filter;
        #endregion

        #region Properties
        public ObservableCollection<LandItemViewModel> Lands
        {
            get { return this.lands; }
            set { SetValue(ref this.lands, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public string Filter
        {
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                this.Search();
            }
        }
        #endregion

        #region Constructors
        public LandsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadLands();
        }

        #endregion

        #region Methods

        private async void LoadLands()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            var response = await this.apiService.GetList<Land>("http://restcountries.eu",
                "/rest",
                "/v2/all");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            MainViewModel.GetInstance().LandList = (List<Land>)response.Result;
            this.Lands = new ObservableCollection<LandItemViewModel>(this.ToLanItemViewModel());
            this.IsRefreshing = false;
        }

        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadLands);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Lands = new ObservableCollection<LandItemViewModel>(this.ToLanItemViewModel());
            }
            else
            {
                this.Lands = new ObservableCollection<LandItemViewModel>(this.ToLanItemViewModel().Where(
                    l => l.Name.ToLower().Contains(this.Filter.ToLower()) ||
                    l.Capital.ToLower().Contains(this.Filter.ToLower())));
            }
        }
        #endregion

        #region Methods
        private IEnumerable<LandItemViewModel> ToLanItemViewModel()
        {
            return MainViewModel.GetInstance().LandList.Select(l => new LandItemViewModel
            {
                Name = l.Name,
                TopLevelDomain = l.TopLevelDomain,
                Alpha2Code = l.Alpha2Code,
                Alpha3Code = l.Alpha3Code,
                CallingCodes = l.CallingCodes,
                Capital = l.Capital,
                AltSpellings = l.AltSpellings,
                Region = l.Region,
                Subregion = l.Subregion,
                Population = l.Population,
                Latlng = l.Latlng,
                Demonym = l.Demonym,
                Area = l.Area,
                Gini = l.Gini,
                Timezones = l.Timezones,
                Borders = l.Borders,
                NativeName = l.NativeName,
                NumericCode = l.NumericCode,
                Currencies = l.Currencies,
                Languages = l.Languages,
                Translations = l.Translations,
                Flag = l.Flag,
                RegionalBlocs = l.RegionalBlocs,
                Cioc = l.Cioc,
            });
        }
        #endregion
    }
}