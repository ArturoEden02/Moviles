﻿namespace moviles.ViewModels
{
    using System.Collections.Generic;
    using Models;

    class MainViewModel
    {
        #region ViewModels
        public LoginViewModel Login { get; set; }
        public LandsViewModel Lands { get; set; }
        public LandViewModel Land { get; set; }
        #endregion
        #region Constructores
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion

        #region Properties

        public List<Land> LandList
        {
            get;
            set;
        }

        public TokenResponse Token { get; set; }
        #endregion
    }
}