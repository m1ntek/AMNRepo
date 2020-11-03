using AMN.Controller;
using AMN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMN.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExerciseLoadouts : ContentPage
    {
        public List<ExerciseLoadout> ELoadouts { get; set; }
        public ExerciseLoadouts()
        {
            InitializeComponent();
            ELoadouts = new List<ExerciseLoadout>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            ELoadouts = await MasterModel.DAL.GetExerciseLoadoutsAsync();
        }

        private async void NewLoadout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewExerciseLoadout());
        }
    }
}