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

public class DataAccessLayer
{
    public Person user;
    public string error;

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

    public string SimplifyException(string exception)
    {
        string[] reason = exception.Split('\n');
        return reason[reason.Length - 1];
    }

    public async Task UpdateUser()
    {
        await fb.Child(auth.User.LocalId).PutAsync(MasterModel.currentUser);
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
