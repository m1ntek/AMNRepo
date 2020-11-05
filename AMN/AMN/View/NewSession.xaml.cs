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
    public partial class NewSession : ContentPage
    {
        public ExerciseLoadout SelectedLoadout { get; set; }
        public NewSession()
        {
            InitializeComponent();
            SelectedLoadout = new ExerciseLoadout();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            SelectedLoadout = await MasterModel.DAL.GetSelectedExerciseLoadoutAsync();
            Refresh();
        }

        private void Refresh()
        {
            BindingContext = null;
            BindingContext = this;
        }

        private void Add_Clicked(object sender, EventArgs e)
        {
            //Add a new rep for the exercise type.
            var button = (Button)sender;
            var exerciseType = (ExerciseType)button.CommandParameter;
            var exerciseKey = exerciseType.ExerciseKey;
        }

        private void DelType_Clicked(object sender, EventArgs e)
        {

        }

        private void DelRep_Clicked(object sender, EventArgs e)
        {

        }
    }
}