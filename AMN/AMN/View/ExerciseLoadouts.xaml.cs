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
            Refresh();
        }

        private async void NewLoadout_Clicked(object sender, EventArgs e)
        {
            await MasterModel.DAL.ResetTempLoadoutExerciseAsync();
            await Navigation.PushAsync(new NewExerciseLoadout());
        }

        private async void Loadout_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new EditExerciseLoadout(ELoadouts[e.ItemIndex].Key));
        }

        private void Refresh()
        {
            BindingContext = null;
            BindingContext = this;
        }

        private async void Del_Clicked(object sender, EventArgs e)
        {
            //Find index of click event
            var button = (Button)sender;
            var exerciseLoadout = (ExerciseLoadout)button.CommandParameter;
            var index = ELoadouts.IndexOf(exerciseLoadout);

            await MasterModel.DAL.DeleteExerciseLoadoutAsync(ELoadouts[index].Key);
            ELoadouts.RemoveAt(index);


            Refresh();
        }
    }
}