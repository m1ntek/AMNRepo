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
    public partial class SelectedExercise : ContentPage
    {
        public string Key { get; set; }
        public Exercise Exercise { get; set; }
        public SelectedExercise(string key)
        {
            InitializeComponent();
            Exercise = new Exercise();
            Key = key;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Exercise = await GetExercise();
            BindingContext = this;
        }

        private void entryName_Unfocused(object sender, FocusEventArgs e)
        {
            entryReps.Focus();
        }

        private Task<Exercise> GetExercise()
        {
            return MasterModel.DAL.GetSelectedExerciseAsync(Key);
        }

        private void FillForm()
        {

        }

        private void AddRep_Clicked(object sender, EventArgs e)
        {

        }

        private void EditRep_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}