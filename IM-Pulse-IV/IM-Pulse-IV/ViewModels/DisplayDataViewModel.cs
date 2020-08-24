using IM_Pulse_IV.Models.Main;
using IM_Pulse_IV.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace IM_Pulse_IV.ViewModels
{
    class DisplayDataViewModel : BaseViewModel
    {
        PageService _pageServices;
        public DisplayDataViewModel()
        {
            _pageServices = new PageService();
            VerifyReadData = new Command(OnVerifyReadData);
            MultiParameters = new Command(OnMultiParameters);
            ReadAndProcessData = new Command(OnReadAndProcessData);
            InverseButtonEnabled = true;
        }
        
        public ICommand ReadAndProcessData { get; set; }
        private void OnReadAndProcessData(object obj)
        {
            new ProcessData();
            ButtonEnabled = true;
        }

        public ICommand VerifyReadData { get; set; }
        private async void OnVerifyReadData(object obj)
        {
            await _pageServices.PushAsync(new IM_Pulse_IV.Views.VerifyDataView());
        }

        public ICommand MultiParameters { get; set; }
        private void OnMultiParameters(object obj)
        {
            throw new NotImplementedException();
        }

        private bool _buttonEnabled;
        public bool ButtonEnabled
        {
            get => _buttonEnabled;
            set
            {
                _buttonEnabled = value;
                InverseButtonEnabled = false;
                SetValue(ref _buttonEnabled, value);
            }
        }

        private bool _inverseButtonEnabled;
        public bool InverseButtonEnabled
        {
            get => _inverseButtonEnabled;
            set
            {
                _inverseButtonEnabled = value;
                SetValue(ref _inverseButtonEnabled, value);
                
            }
        }
    }
}
