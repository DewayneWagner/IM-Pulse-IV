using System.Threading.Tasks;
using Xamarin.Forms;
using Application = Xamarin.Forms.Application;

namespace TheManXS.ViewModel.Services
{
    class PageService : IPageService
    {
        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, ok, cancel);
        }
        public async Task DisplayAlert(string message)
        {
            await Application.Current.MainPage.DisplayAlert("The Man", message, "OK");
        }

        public async Task DisplayAlert(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }

        public async Task PushAsync(Page page)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        public async Task PopAsync()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
