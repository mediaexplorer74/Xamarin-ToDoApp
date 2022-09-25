//using Android.Gms.Extensions;
//using Firebase.Auth;
using FirebaseAdmin.Auth;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDoApp.Auth;
using ToDoApp.Models;

namespace ToDoApp.UWP.Auth
{
    public class FirebaseAuthentication : IFirebaseAuthentication
    {
        public async Task<UserModel> LoginWithEmailAndPassword(string email, string password)
        {
            
            try
            {
                //TODO
                /*
                var firebaseUser = await FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email,password);//.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = await firebaseUser.User.GetIdToken(false).AsAsync<GetTokenResult>();
                var user = new UserModel()
                {
                    DisplayName = firebaseUser.User.DisplayName,
                    Email = firebaseUser.User.Email,
                    Token = token.Token
                };
                return user;
                */
                
                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> RegisterWithEmailAndPassword(string username, string email, string password)
        {
            try
            {
                //TODO
                /*
                var result = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var userProfileBuilder = new UserProfileChangeRequest.Builder();
                userProfileBuilder.SetDisplayName(username);
                await result.User.UpdateProfileAsync(userProfileBuilder.Build());
                return result.User != null;
                */
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task ForgetPassword(string email)
        {
            try
            {
                //TODO
                //await FirebaseAuth.Instance.SendPasswordResetEmailAsync(email);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public string GetUsername()
        {
            //var user = FirebaseAuth.Instance.CurrentUser;
            return "UserName";//user?.DisplayName;
        }

        public string GetUserId()
        {
            //var user = FirebaseAuth.Instance.CurrentUser;
            return "Id";//user?.Uid;
        }


        public bool IsLoggedIn()
        {
            var user = "yes";//FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public bool LogOut()
        {
            try
            {
                //TODO
                //FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }
    }
}