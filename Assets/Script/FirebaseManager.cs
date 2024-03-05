using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager FBM;
    public DataUser DU;
    public SceneScript SS;
    

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text conFirmLoginText;

    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVarifyField;
    public TMP_Text warningRegisterText;

    [Header("UserData[ForAdmin]")]
    public TMP_InputField usernameField;
    public TMP_InputField monneyField;
    public TMP_InputField levelField;
    public TMP_InputField tsuField;

    [Header("UserProfile")]
    public TMP_Text ProfileusernameText;
    public TMP_Text ProfilemonneyText;
    public TMP_Text ProfilelevelText;
    public TMP_Text ProfiletsuText;
    public TMP_Text UpgrademonneyText;
    public TMP_Text MainusernameText;
    public Text Test;




  
    private void Update()
    {
       
        UserProfile();
       

    }
    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);

            }
        });




        DU = GameObject.Find("UserData").GetComponent<DataUser>();
        SS = GameObject.Find("SceneScript").GetComponent<SceneScript>();
        if (DU.LoggedIn == true)
        {
            SS.CloseOtherWindow();
            UpdatePlayerFirebase();
        }





    }




    private void InitializeFirebase()
    {
        Debug.Log("setting up Firebase Auth");

        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != User)
        {
            bool signedIn = User != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && User != null)
            {
                Debug.Log("Signed out " + User.UserId);
            }
            User = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + User.UserId);
            }
        }
    }
    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }
    public void ClearRegisterField()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVarifyField.text = "";
    }

    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
        

    }
   
    public void RegisterButton()
    {
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));

        
    }
    public void SignOutButton()
    {
        DU.LoggedIn = false;
        auth.SignOut();
        SceneScript.instance.Login();
        ClearLoginFields();
        ClearRegisterField();
    }

    public void SaveDataButton()
    {
        StartCoroutine(UpdateUsernameAuth(usernameField.text));
        StartCoroutine(UpdateUsernameDatabase(usernameField.text));

        StartCoroutine(UpdateMonney(int.Parse(monneyField.text)));
        StartCoroutine(UpdateLevel(int.Parse(levelField.text)));
        StartCoroutine(UpdateTsu(int.Parse(tsuField.text)));
        
    }
    public void UpdatePlayerFirebase()
    {

        StartCoroutine(PlayerFirebaseDataUpdate());


    }
    public void UserProfile()
    {
        
        MainusernameText.text = DU.Username;
        ProfileusernameText.text = DU.Username;
        ProfilemonneyText.text = DU.Monney.ToString();
        ProfilelevelText.text = DU.Level.ToString();
        ProfiletsuText.text = DU.Tsu.ToString();
        UpgrademonneyText.text = DU.Monney.ToString();
       
    }
    public void TransferDataUser()
    {
        // DU.Username = usernameField.text;
        // DU.Monney = int.Parse(monneyField.text);
        // DU.Level = int.Parse(levelField.text);
        //DU.Tsu = int.Parse(tsuField.text);

        DU.Username = usernameField.text;

        int monney;
        if (int.TryParse(monneyField.text, out monney))
        {
            DU.Monney = monney;
        }
        else
        {

            //Debug.LogError("MonneyField text is not in a valid format.");
        }

        int level;
        if (int.TryParse(levelField.text, out level))
        {
            DU.Level = level;
        }
        else
        {

            //  Debug.LogError("LevelField text is not in a valid format.");
        }

        int tsu;
        if (int.TryParse(tsuField.text, out tsu))
        {
            DU.Tsu = tsu;
        }
        else
        {

            // Debug.LogError("TsuField text is not in a valid format.");
        }
    }

    private IEnumerator PlayerFirebaseDataUpdate()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(UpdateUsernameAuth(ProfileusernameText.text));
        StartCoroutine(UpdateUsernameDatabase(ProfileusernameText.text));
        StartCoroutine(UpdateMonney(int.Parse(ProfilemonneyText.text)));
        StartCoroutine(UpdateLevel(int.Parse(ProfilelevelText.text)));
        StartCoroutine(UpdateTsu(int.Parse(ProfiletsuText.text)));
    }






    private IEnumerator Login(string _email, string _password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failled";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "WrongPassword";
                    break;
                case AuthError.InvalidEmail:
                    message = "InvalidEmail";
                    break;
                case AuthError.UserNotFound:
                    message = "UserNotFound";
                    break;
            }
            warningLoginText.text = message;


        }
        else
        {
            User = LoginTask.Result;
            Debug.LogFormat("User signed in succesfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            conFirmLoginText.text = "Logged In";
            StartCoroutine(LoadUserData());
            usernameField.text = User.DisplayName;
            /*usernameText.text = User.DisplayName;*/
            MainusernameText.text = User.DisplayName;
            DU.LoggedIn = true;

            yield return new WaitForSeconds(2);
            TransferDataUser();

           




            SceneScript.instance.CloseOtherWindow();
            conFirmLoginText.text = "";
            ClearLoginFields();
            ClearRegisterField();
            


        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterField.text)
        {
            warningRegisterText.text = "Password Does not Match!";
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failled to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                User = RegisterTask.Result;
                if (User != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    var ProfileTask = User.UpdateUserProfileAsync(profile);

                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed";

                    }
                    else
                    {

                        SceneScript.instance.Login();
                        warningRegisterText.text = "";
                        ClearLoginFields();
                        ClearRegisterField();
                    }

                }
            }
        }

    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        UserProfile profile = new UserProfile { DisplayName = _username };

        var ProfileTask = User.UpdateUserProfileAsync(profile);

        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth Username is now update
        }

    }

    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if(DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failled to registrer task with {DBTask.Exception}");

        }
        else
        {
            //Database username is now updated
        }
    }

    private IEnumerator UpdateMonney(int _monney)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("monney").SetValueAsync(_monney);
    


        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failled to registrer task with {DBTask.Exception}");
        }
        else
        {
            //Xp are now updated
        }
      
    }

    

    private IEnumerator UpdateLevel(int _level)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("level").SetValueAsync(_level);
      

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failled to registrer task with {DBTask.Exception}");
        }
        else
        {
            //kills are now updated
        }
    }
    private IEnumerator UpdateTsu(int _tsu)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("tsu").SetValueAsync(_tsu);
     

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failled to registrer task with {DBTask.Exception}");
        }
        else
        {
            //kills are now updated
        }
    }
    private IEnumerator LoadUserData()
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
  



        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if(DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failled to registrer task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            monneyField.text = "0";
            levelField.text = "1";
            tsuField.text = "1";
           
            
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            monneyField.text = snapshot.Child("monney").Value.ToString();
            levelField.text = snapshot.Child("level").Value.ToString();
            tsuField.text = snapshot.Child("tsu").Value.ToString();
           

            

        }
    }
  

}
