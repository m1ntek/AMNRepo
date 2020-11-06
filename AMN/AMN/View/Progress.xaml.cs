using AMN.Controller;
using AMN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMN.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Progress : ContentPage
    {
        public List<ExerciseLoadout> ExSessions { get; set; }
        public Progress()
        {
            InitializeComponent();
            ExSessions = new List<ExerciseLoadout>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ExSessions = await MasterModel.DAL.GetExerciseSessionsAsync();
            Refresh();
        }

        private void Refresh()
        {
            BindingContext = null;
            BindingContext = this;
        }

        private async void NewSession_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewSession());
        }
    }
}