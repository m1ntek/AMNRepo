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

    public async Task<List<Meal>> GetSelectedLoadoutMeals()
    {
        return (await fb.Child("Users")
        .Child(auth.User.LocalId)
        .Child("SelectedLoadout")
        .Child("Meals")
        .OnceAsync<Meal>())
        .Select(item => new Meal()
        {
            index = Convert.ToInt32(item.Key),
            isEaten = item.Object.isEaten,
            items = item.Object.items,
            mealId = item.Object.mealId,
            mealName = item.Object.mealName,
            totalCarbs = item.Object.totalCarbs,
            totalEnergy = item.Object.totalEnergy,
            totalFat = item.Object.totalFat,
            totalProtein = item.Object.totalProtein,
            userLocalId = item.Object.userLocalId
        }).ToList();
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
    public async Task SaveGoalAsync(MacroNutrients dailyGoal)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("DailyGoal").PutAsync(dailyGoal);
    }
    public async Task SaveGoalProgressAsync(MacroNutrients goalProgress)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("GoalProgress").PutAsync(goalProgress);
    }

    public async Task<string> GetEmail()
    {
        return await fb.Child("Users")
            .Child(auth.User.LocalId)
            .Child("email")
            .OnceSingleAsync<string>();
    }

    public async Task<MacroNutrients> GetGoalAsync()
    {
        return await fb.Child("Users").Child(auth.User.LocalId).Child("DailyGoal").OnceSingleAsync<MacroNutrients>();
    }

    //public async Task<MacroNutrients> GetGoalV2Async()
    //{
    //    return await fb.Child("Users").Child(GetCurrentLocalId()).Child("DailyGoal").OnceSingleAsync<MacroNutrients>();
    //}

    public async Task<MacroNutrients> GetGoalProgressAsync()
    {
        return await fb.Child("Users").Child(GetCurrentLocalId()).Child("GoalProgress").OnceSingleAsync<MacroNutrients>();
    }

    public async Task<string> GetLoadoutNameAsync()
    {
        return (await fb.Child("Users")
        .Child(auth.User.LocalId)
        .Child("SelectedLoadout")
        .Child("LoadoutName")
        .OnceSingleAsync<string>());
    }

    public async Task<List<Loadout>> GetLoadoutsAsync()
    {
        return (await fb.Child("Users")
        .Child(auth.User.LocalId)
        .Child("Loadouts")
        .OnceAsync<Loadout>())
        .Select(item => new Loadout()
        {
            LoadoutId = item.Object.LoadoutId,
            LoadoutName = item.Object.LoadoutName,
            Meals = item.Object.Meals
        }).ToList();
    }

    public async Task ResetTempLoadoutMealsAsync()
    {
        await fb.Child("Users").
            Child(auth.User.LocalId).
            Child("TempLoadoutMeals").
            PutAsync(new List<Meal>());
    }
    //public async Task SaveTempLoadoutAsync(Loadout loadout)
    //{
    //    await fb.Child("Users").
    //        Child(auth.User.LocalId).
    //        Child("TempLoadout").
    //        PutAsync(loadout);
    //}

    public async Task SaveNewTempLoadoutMealAsync(Meal loadoutMeal)
    {
        await fb.Child("Users").
            Child(auth.User.LocalId).
            Child("TempLoadoutMeals").
            PostAsync(loadoutMeal);
    }

    public async Task SaveTempLoadoutMealAsync(Meal loadoutMeal, string key)
    {
        await fb.Child("Users").
        Child(auth.User.LocalId).
        Child("TempLoadoutMeals").
        Child(key).
        PutAsync(loadoutMeal);
    }

    public async Task SaveSelectedLoadoutMealsAsync(List<Meal> loadoutMeals)
    {
        await fb.Child("Users").
            Child(auth.User.LocalId).
            Child("SelectedLoadout").
            Child("Meals").
            PutAsync(loadoutMeals);
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
    public async Task SaveNewMealAsync(Meal meal) //testing post async
    {
        await GetIdsAsync();
        await fb.Child("Users").Child(auth.User.LocalId).Child("Meals").PostAsync(meal);
        ++idGen.totalMealIds;
        await SaveIdsAsync();
    }

    public async Task SaveMealV3Async(Meal meal, string key) //testing post async
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("Meals").Child(key).PutAsync(meal);
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
            userLocalId = item.Object.userLocalId,
            isEaten = item.Object.isEaten,
            key = item.Key,
            totalCarbs = item.Object.totalCarbs,
            totalEnergy = item.Object.totalEnergy,
            totalFat = item.Object.totalFat,
            totalProtein = item.Object.totalProtein
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

    public async Task<List<Exercise>> GetExerciseLoadoutExercisesAsync(string key)
    {
        return (await fb.Child("Users")
        .Child(auth.User.LocalId)
        .Child("ExerciseLoadouts")
        .Child(key)
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

    public async Task SaveSelectedLoadout(Loadout loadout)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("SelectedLoadout").PutAsync(loadout);
    }

    public async Task SaveNewLoadout(Loadout loadout)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("Loadouts").PostAsync(loadout);
    }

    public async Task SaveLoadoutAsync(Loadout loadout, string key)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("Loadouts").Child(key).PutAsync(loadout);
    }

    public Task<Loadout> GetSelectedLoadoutAsync()
    {
        return fb.Child("Users").
            Child(auth.User.LocalId).
            Child("SelectedLoadout").
            OnceSingleAsync<Loadout>();
    }

    public Task<Exercise> GetSelectedExerciseAsync(string key)
    {
        return fb.Child("Users").Child(auth.User.LocalId).Child("Exercises").Child(key).OnceSingleAsync<Exercise>();
    }

    public async Task SaveSelectedExerciseAsync(Exercise exercise)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("Exercises").Child(exercise.Key).PutAsync(exercise);
    }

    public async Task DeleteMealAsync(string key)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("Meals").Child(key).DeleteAsync();
    }
    public async Task DeleteSelectedLoadoutMealAsync(string key)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("SelectedLoadout").Child(key).DeleteAsync();
    }

    public async Task DeleteTempLoadoutMealAsync(string key)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("TempLoadoutMeals").Child(key).DeleteAsync();
    }

    public async Task DeleteLoadoutAsync(string key)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("Loadouts").Child(key).DeleteAsync();
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
            .Select(item => new ExerciseLoadout()
            {
                Exercises = item.Object.Exercises,
                Key = item.Key,
                Name = item.Object.Name,
                Sets = item.Object.Sets
            }).ToList();
    }

    public async Task<List<ExerciseLoadout>> GetExerciseSessionsAsync()
    {
        return (await fb.Child("Users")
            .Child(auth.User.LocalId)
            .Child("ExerciseSessions")
            .OrderByKey()
            .LimitToLast(7)
            .OnceAsync<ExerciseLoadout>())
            .Reverse()

            .Select(item => new ExerciseLoadout()
            {
                Exercises = item.Object.Exercises,
                Key = item.Key,
                Name = item.Object.Name,
                Sets = item.Object.Sets,
                EndTime = item.Object.EndTime,
                StartTime = item.Object.StartTime,
                DateString = item.Object.DateString,
                StartToEnd = item.Object.StartToEnd
            }).ToList();
    }

    public async Task<ExerciseLoadout> GetExerciseLoadoutAsync(string key)
    {
        return await fb.Child("Users")
            .Child(auth.User.LocalId)
            .Child("ExerciseLoadouts")
            .Child(key)
            .OnceSingleAsync<ExerciseLoadout>();
    }

    public Task<ExerciseLoadout> GetSelectedExerciseLoadoutAsync()
    {
        return (fb.Child("Users")
            .Child(auth.User.LocalId)
            .Child("SelectedExerciseLoadout")
            .OnceSingleAsync<ExerciseLoadout>());
    }

    public Task<ExerciseLoadout> GetLastWeeksExerciseLoadoutAsync(string previousWeekDate)
    {
        return (fb.Child("Users")
            .Child(auth.User.LocalId)
            .Child("ExerciseSessions")
            .OrderByKey()
            .StartAt(previousWeekDate)
            .LimitToFirst(1)
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
    public async Task<List<Meal>> GetTempLoadoutMealsAsync()
    {
        return (await fb.Child("Users")
            .Child(auth.User.LocalId)
            .Child("TempLoadoutMeals")
            .OnceAsync<Meal>())
            .Select(item => new Meal()
            {
                index = item.Object.index,
                mealId = item.Object.mealId,
                items = item.Object.items,
                mealName = item.Object.mealName,
                userLocalId = item.Object.userLocalId,
                isEaten = item.Object.isEaten,
                key = item.Key,
                totalCarbs = item.Object.totalCarbs,
                totalEnergy = item.Object.totalEnergy,
                totalFat = item.Object.totalFat,
                totalProtein = item.Object.totalProtein
            }).ToList();
    }

    public Task<ExerciseLoadout> GetTempExerciseLoadoutAsync()
    {
        return (fb.Child("Users")
            .Child(auth.User.LocalId)
            .Child("TempExerciseLoadout")
            .OnceSingleAsync<ExerciseLoadout>());
    }

    public Task<ExerciseLoadout> GetExerciseLoadoutExerciseAsync(string key)
    {
        return fb.Child("Users").
            Child(auth.User.LocalId).
            Child("ExerciseLoadouts").
            Child(key)
            .OnceSingleAsync<ExerciseLoadout>();
    }

    public async Task SaveNewTempLoadoutExerciseAsync(ExerciseLoadout exLoadout)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("TempExerciseLoadout").PutAsync(exLoadout);
    }

    public async Task ResetTempLoadoutExerciseAsync()
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("TempExerciseLoadout").PutAsync(new ExerciseLoadout());
    }

    public async Task SaveNewExerciseLoadoutAsync(ExerciseLoadout exLoadout)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("ExerciseLoadouts").PostAsync(exLoadout);
    }

    public async Task SaveExerciseLoadoutAsync(ExerciseLoadout exLoadout, string key)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("ExerciseLoadouts").Child(key).PutAsync(exLoadout);
    }

    public async Task DeleteExerciseLoadoutAsync(string key)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("ExerciseLoadouts").Child(key).DeleteAsync();
    }

    public async Task SaveNewExerciseSessionLogAsync(ExerciseLoadout loadout)
    {
        await fb.Child("Users").Child(auth.User.LocalId).Child("ExerciseSessions").PostAsync(loadout);
    }
}
