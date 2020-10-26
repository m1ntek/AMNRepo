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
    //Experimenting with MVVM design pattern
    class ExercisePageController: INotifyPropertyChanged
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

            //Temp initial data and push to db
            //Workout testWorkout = new Workout();
            //testWorkout.Weight = 100;
            //testWorkout.Reps = 10;

            //Exercise ex1 = new Exercise();
            //ex1.Name = "Flat bench";
            //ex1.WorkoutTypes = new List<Workout>();
            //ex1.WorkoutTypes.Add(testWorkout);

            //Exercise ex2 = new Exercise();
            //ex2.Name = "Standing military press";
            //ex2.WorkoutTypes = new List<Workout>();
            //ex2.WorkoutTypes.Add(testWorkout);

            //ExerciseLoadout CurrentExerciseLoadout = new ExerciseLoadout();
            //CurrentExerciseLoadout.Exercises = new List<Exercise>();
            //CurrentExerciseLoadout.Exercises.Add(ex1);
            //CurrentExerciseLoadout.Exercises.Add(ex2);

            //try
            //{
            //    await MasterModel.DAL.SaveSelectedExerciseLoadoutAsync(CurrentExerciseLoadout);
            //}
            //catch (Exception ex)
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            //}
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
