using IM_Pulse_IV.Models.Main;
using IM_Pulse_IV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IM_Pulse_IV.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuView : ContentPage
    {
        public MainMenuView()
        {            
            InitializeComponent();
        }
        public void OnClicked(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedCommandParameter = (CommandParameter)e.SelectedItem;
            MainMenuViewModel mmvm = (MainMenuViewModel)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.MainMenuViewModel)];
            mmvm.DeleteCommand(selectedCommandParameter);
        }
    }
}