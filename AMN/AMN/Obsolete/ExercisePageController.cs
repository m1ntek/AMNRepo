using AMN.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AMN.Controller
{
    //obsolete, did not have enough time to experiment with MVVM.
    //Experimenting with MVVM design pattern
    public class ExercisePageController: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //Properties
        public ObservableCollection<Exercise> SelectedExerciseLoadout { get; set; }
        public List<Exercise> SelectedExerciseLoadoutList { get; set; }

        protected async Task OnPropertyChanged(string propertyName)
        {

        }
        public ExercisePageController()
        {
            SelectedExerciseLoadoutList = new List<Exercise>();
        }

        //My own OnAppearing method for this controller/viewmodel
        public async Task OnAppearing()
        {
            try
            {
                await GetSelectedExerciseLoadout();
                
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task GetSelectedExerciseLoadout()
        {
            try
            {
                var loadout = await MasterModel.DAL.GetSelectedExerciseLoadoutAsync();
                SelectedExerciseLoadoutList = loadout.Exercises;
                SelectedExerciseLoadout = new ObservableCollection<Exercise>(SelectedExerciseLoadoutList);

            }
            catch (Exception ex)
            {
                //This violates a MVVM rule (ViewModel shouldn't talk to View),
                //but due to time constraint, cannot implement the correct
                //solution in time.
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
