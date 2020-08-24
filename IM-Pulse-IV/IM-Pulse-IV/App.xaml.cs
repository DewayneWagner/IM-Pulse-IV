using IM_Pulse_IV.IO;
using IM_Pulse_IV.Models;
using IM_Pulse_IV.Models.RandomDataGenerator;
using IM_Pulse_IV.Services;
using IM_Pulse_IV.ViewModels;
using IM_Pulse_IV.Views;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IM_Pulse_IV
{
    public partial class App : Application
    {
        public enum FileNames { DB, RandomDataParameters, RandomTextData, Total }
        public enum ObjectsInPropertyDictionary { MainMenuViewModel, ScreenWidth, ScreenHeight, RandomDataParameters, RandomDataStats }
        public static ListOfAllPaths ListOfAllPaths;

        public App(ListOfAllPaths listOfAllPaths)
        {
            ListOfAllPaths = listOfAllPaths;
            InitializeComponent();
            InitPropertyDictionary();

            //MainPage = new NavigationPage(new MainMenuView());
            MainPage = new NavigationPage(new MainPage());
        }

        private void InitPropertyDictionary()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            Properties[Convert.ToString(ObjectsInPropertyDictionary.MainMenuViewModel)] = new MainMenuViewModel(true);
            Properties[Convert.ToString(ObjectsInPropertyDictionary.ScreenWidth)] = mainDisplayInfo.Width;
            Properties[Convert.ToString(ObjectsInPropertyDictionary.ScreenHeight)] = mainDisplayInfo.Height;
            Properties[Convert.ToString(ObjectsInPropertyDictionary.RandomDataParameters)] = SavedRandomDataParameters_JSON.GetSavedData();
            Properties[Convert.ToString(ObjectsInPropertyDictionary.RandomDataStats)] = new List<RandomDataStats>();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            SavedRandomDataParameters_JSON.WriteRandomDataParametersToFile();
        }

        

        protected override void OnResume()
        {
        }
    }
}
