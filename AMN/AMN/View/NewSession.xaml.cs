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
        bool loadoutReceived;
        public NewSession()
        {
            InitializeComponent();
            SelectedLoadout = new ExerciseLoadout();
            loadoutReceived = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //Just grab the loadout once.
            if(loadoutReceived==false)
            {
                SelectedLoadout = await MasterModel.DAL.GetSelectedExerciseLoadoutAsync();
                SelectedLoadout.StartTime = DateTime.Now;
                SelectedLoadout.DateString = SelectedLoadout.StartTime.ToShortDateString();
                loadoutReceived = true;
            }

            //Set correct indices once.
            //Ended up having to do it here as it isn't
            //set up correctly in the whole app/code structure.
            UpdateIndices();


            Refresh();
        }

        private void UpdateIndices()
        {
            //Update indices
            for (int i = 0; i < SelectedLoadout.Exercises.Count; i++)
            {
                for (int j = 0; j < SelectedLoadout.Exercises[i].Types.Count; j++)
                {
                    //Set each exercise type index.
                    SelectedLoadout.Exercises[i].Types[j].Index = j;
                    for (int k = 0; k < SelectedLoadout.Exercises[i].Types[j].Reps.Count; k++)
                    {
                        //Easier to conceptualise with named variables.
                        string relativeExKey = SelectedLoadout.Exercises[i].Key;
                        int relativeTypeIndex = j;

                        //Set each rep references within each exercise type
                        SelectedLoadout.Exercises[i].Types[j].Reps[k].Index = k;
                        SelectedLoadout.Exercises[i].Types[j].Reps[k].ExerciseKey = relativeExKey;
                        SelectedLoadout.Exercises[i].Types[j].Reps[k].ExerciseTypeIndex = relativeTypeIndex;
                    }
                }
            }
        }

        private void Refresh()
        {
            BindingContext = null;
            BindingContext = this;
        }

        private void Add_Clicked(object sender, EventArgs e)
        {
            //Add a new rep for the exercise type.
            //Find the exercise type in relation to the button clicked.
            var button = (Button)sender;
            var exerciseType = (ExerciseType)button.CommandParameter;

            //Add a new rep to it.
            exerciseType.Reps.Add(new Rep());

            //Save it to the binded loadout data.
            foreach (var exercise in SelectedLoadout.Exercises)
            {
                //find the correct exercise, save it to the correct index.
                if (exercise.Key == exerciseType.ExerciseKey)
                    exercise.Types[exerciseType.Index] = exerciseType;
            }

            UpdateIndices();

            //Refresh binding
            Refresh();
        }

        private void DelType_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var exerciseType = (ExerciseType)button.CommandParameter;

            for (int i = 0; i < SelectedLoadout.Exercises.Count; ++i)
            {
                if(SelectedLoadout.Exercises[i].Key == exerciseType.ExerciseKey)
                {
                    for (int j = 0; j < SelectedLoadout.Exercises[i].Types.Count; ++j)
                    {
                        if (j == exerciseType.Index)
                            SelectedLoadout.Exercises[i].Types.RemoveAt(j);
                    }
                }
            }

            UpdateIndices();

            //Refresh binding
            Refresh();
        }

        private void DelRep_Clicked(object sender, EventArgs e)
        {
            //Delete a rep for the exercise type.
            //Find the rep in relation to the button clicked.
            var button = (Button)sender;
            var rep = (Rep)button.CommandParameter;

            //Find the rep from the binded loadout data.
            for (int i = 0; i < SelectedLoadout.Exercises.Count; ++i)
            {
                //Find correct exercise.
                if (SelectedLoadout.Exercises[i].Key == rep.ExerciseKey)
                {
                    for (int j = 0; j < SelectedLoadout.Exercises[i].Types.Count; ++j)
                    {
                        int exerciseTypeIndex = j;
                        
                        //Find correct exercise type and delete the rep from it
                        if (rep.ExerciseTypeIndex == exerciseTypeIndex)
                        {
                            SelectedLoadout.Exercises[i].Types[j].Reps.RemoveAt(rep.Index);
                        }
                    }
                }

            }

            UpdateIndices();

            //Refresh binding
            Refresh();
        }

        private async void LogSession_Clicked(object sender, EventArgs e)
        {
            SelectedLoadout.EndTime = DateTime.Now;
            SelectedLoadout.StartToEnd = $"{SelectedLoadout.StartTime.ToShortTimeString()} - {SelectedLoadout.EndTime.ToShortTimeString()}";
            await MasterModel.DAL.SaveNewExerciseSessionLogAsync(SelectedLoadout);
            await Navigation.PopAsync();
        }
    }
}