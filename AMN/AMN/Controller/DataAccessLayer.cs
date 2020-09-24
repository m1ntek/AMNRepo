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
        if(auth != null)
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

    public async Task GetIds()
    {
        idGen = await fb.Child("IDGen").OnceSingleAsync<IDGen>();
    }

    public async Task SaveIds()
    {
        await fb.Child("IDGen").PutAsync(idGen);
    }

    public async Task GetUserData()
    {
        var tempMeals = await fb.Child("Meals").OnceAsync<Meal>();

        //need to re-design firebase data structure, move meals into users.
    }

    public async Task SaveMeal()
    {
        //get mealId from firebase
        //save meal as incremented mealId, save new mealId to firebase

        await GetIds();
        await fb.Child("Meals").Child(idGen.totalMealIds.ToString("0000")).PutAsync(MasterModel.tempMeal.items);
        ++idGen.totalMealIds;
        await SaveIds();
    }

    public async Task SaveMealV2()
    {
        //get mealId from firebase
        //save meal as incremented mealId, save new mealId to firebase

        await GetIds();
        await fb.Child("Users").Child(MasterModel.DAL.GetCurrentLocalId()).Child("Meals").Child(idGen.totalMealIds.ToString("0000")).PutAsync(MasterModel.tempMeal.items);
        ++idGen.totalMealIds;
        await SaveIds();
    }

    public async Task UpdateUser()
    {
        await fb.Child("Users").Child(auth.User.LocalId).PutAsync(MasterModel.currentUser);
    }

    public async Task SignInUser(string _email, string _pw)
    {
        auth = await authProvider.SignInWithEmailAndPasswordAsync(_email, _pw);

        fb = new FirebaseClient(url, new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken)
        });
    }

    public async Task SignUpUser(string _email, string _pw)
    {
        auth = await authProvider.CreateUserWithEmailAndPasswordAsync(_email, _pw, _email, false);
    }
}
