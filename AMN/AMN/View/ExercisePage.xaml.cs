using AMN.Controller;
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
    public partial class ExercisePage : ContentPage
    {
        ExercisePageController thisPageController;
        public ExercisePage()
        {
            InitializeComponent();

            thisPageController = new ExercisePageController();
            BindingContext = thisPageController;
        }

        //Pass OnAppearing method to viewmodel, viewmodel doesn't appear
        //to be able to do this.
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await thisPageController.OnAppearing();
        }
    }
}