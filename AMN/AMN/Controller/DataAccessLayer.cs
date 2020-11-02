using System.Collections;
using System.Collections.Generic;
using System;
using Firebase.Database.Query;
using Firebase.Database;
using Firebase.Auth;
using AMN.Model;
using System.Threading.Tasks;
using AMN;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AMN.Controller;
using Xamarin.Essentials;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

public class DataAccessLayer
{
    public Person user;
    public string error;
    public IDGen idGen;

    private readonly string apiKey;
    private string url;
    private FirebaseClient fb;
    private readonly FirebaseAuthProvider authProvider;
    private FirebaseAuthLink auth;

    //constructor
    public DataAccessLayer()
    {
        apiKey = "AIzaSyCo4orwpS8lGQJsBLISIYjPiY5eOaEs5lA";
        authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
        error = null;
        url = "https://project-amn.firebaseio.com";
    }

    public bool UserLoggedIn()
    {
        if (auth != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetCurrentLocalId()
    {
        return auth.User.LocalId;
    }

    public string SimplifyException(string exception)
    {
        string[] reason = exception.Split('\n');
        return reason[reason.Length - 1];
    }

    public async Task GetIdsAsync()
    {
        idGen = await fb.Child("IDGen").OnceSingleAsync<IDGen>();
    }

    public async Task SaveIdsAsync()
    {
        await fb.Child("IDGen").PutAsync(idGen);
    }

    public async Task<Person> GetUserDataAsync()
    {
        return await fb.Child("Users").Child(auth.User.LocalId).OnceSingleAsync<Person>();
    }

    public async Task SaveUserDataAsync(Person _user)
    {
        await fb.Child("Users").Child(auth.User.LocalId).PutAsync<Person>(_user);
    }
    public async Task SaveGoalAsync()
    {
        await fb.Child("Users").Child(GetCurrentLocalId()).Child("DailyGoal").PutAsync(MasterModel.dailyGoal);
    }

    public async Task GetGoal()
    {
        MasterModel.dailyGoal = await fb.Child("Users").Child(GetCurrentLocalId()).Child("DailyGoal").OnceSingleAsync<MacroNutrients>();
    }

    public async Task<MacroNutrients> GetGoalV2Async()
    {
        return await fb.Child("Users").Child(GetCurrentLocalId()).Child("DailyGoal").OnceSingleAsync<MacroNutrients>();
    }

    public async Task SaveMeal()
    {
        //get mealId from firebase
        //save meal as incremented mealId, save new mealId to firebase

        await GetIdsAsync();
        await fb.Child("Meals").Child(idGen.totalMealIds.ToString("0000")).PutAsync(MasterModel.tempMeal.items);
        ++idGen.totalMealIds;
        await SaveIdsAsync();
    }

    public async Task SaveMealV2()
    {
        //get mealId from firebase
        //save meal as incremented mealId, save new mealId to firebase

        await GetIdsAsync();
        await fb.Child("Users").Child(GetCurrentLocalId()).Child("Meals").PutAsync(MasterModel.currentUser.Meals);
        ++idGen.totalMealIds;
        await SaveIdsAsync();
    }

    public async Task SaveMealV3Async(Meal _tempMeal) //testing post async
    {
        //get mealId from firebase
        //save meal as incremented mealId, save new mealId to firebase

        await GetIdsAsync();
        await fb.Child("Users").Child(auth.User.LocalId).Child("Meals").PostAsync(new Meal()
        {
            index = _tempMeal.index,
            items = _tempMeal.items,
            mealId = _tempMeal.mealId,
            mealName = _tempMeal.mealName,
            userLocalId = _tempMeal.userLocalId
        });
        ++idGen.totalMealIds;
        await SaveIdsAsync();
    }

    public async Task UpdateUserAsync(string _email)
    {
        await fb.Child("Users").Child(auth.User.LocalId).PutAsync(new Person() { email = _email });
    }

    public async Task SignInUserAsync(string _email, string _pw)
    {
        auth = await authProvider.SignInWithEmailAndPasswordAsync(_email, _pw);

        fb = new FirebaseClient(url, new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken)
        });
    }

    public async Task SignUpUserAsync(string _email, string _pw)
    {
        auth = await authProvider.CreateUserWithEmailAndPasswordAsync(_email, _pw, _email, false);
    }

    public async Task<List<Meal>> GetSavedMealsAsync()
    {
        return (await fb.Child("Users").Child(auth.User.LocalId).Child("Meals").OnceAsync<Meal>()).Select(item => new Meal
        {
            index = item.Object.index,
            mealId = item.Object.mealId,
            items = item.Object.items,
            mealName = item.Object.mealName,
            userLocalId = item.Object.userLocalId
        }).ToList();
    }

    //This one works, other similar methods currently does not retrieve data successfully.
    public async Task<List<Exercise>> GetSavedExercisesAsync()
    {
        return (await fb.Child("Users")
            .Child(auth.User.LocalId)
            .Child("Exercises")
            .OnceAsync<Exercise>())
            .Select(item => new Exercise()
            {
                Key = item.Key,
                Name = item.Object.Name,
                Types = item.Object.Types
            }).ToList();
    }

    public async Task SaveExercisesAsync(List<Exercise> exercises)
    {
        foreach (var item in exercises)
        {
            await fb.Child("Users").Child(auth.User.LocalId).Child("Exercises").PostAsync(item);
        }
    }

    public Task<Exercise> GetSelectedExerciseAsync(string key)
    {
        return fb.Child("Users").Child(auth.User.LocalId).Child("Exercises").Child(key).OnceSingleAsync<Exercise>();
    }

    public async Task SaveSelectedExerciseAsync(Exercise exercise)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("Exercises").Child(exercise.Key).PutAsync(exercise);
    }

    public async Task DeleteSelectedExerciseAsync(Exercise exercise)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("Exercises").Child(exercise.Key).DeleteAsync();
    }

    public async Task DeleteSelectedExerciseTypeAsync(Exercise exercise, int index)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("Exercises").Child(exercise.Key).Child("Types").Child(index.ToString()).DeleteAsync();
    }

    public async Task<List<ExerciseLoadout>> GetExerciseLoadoutsAsync()
    {
        return (await fb.Child("Users")
            .Child(auth.User.LocalId)
            .Child("ExerciseLoadouts")
            .OnceAsync<ExerciseLoadout>())
            .Select(item => new ExerciseLoadout()).ToList();
    }

    public Task<ExerciseLoadout> GetSelectedExerciseLoadoutAsync()
    {
        return (fb.Child("Users")
            .Child(auth.User.LocalId)
            .Child("SelectedExerciseLoadout")
            .OnceSingleAsync<ExerciseLoadout>());
    }

    public async Task SaveSelectedExerciseLoadoutAsync(ExerciseLoadout eLoadout)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("SelectedExerciseLoadout").PutAsync(eLoadout);
    }

    public async Task SaveNewExerciseAsync(Exercise exercise)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("Exercises").PostAsync(exercise);
    }
}
