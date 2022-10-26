using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using odeba.Models;
using Plugin.FirebaseAuth;

namespace odeba.Services.Auth
{
    public interface IAuthService
    {
        Task Signout();
        Task<string> VerifyPhoneNumber(string phoneNumber);
        Task<User> SigninWithPhoneNumber(string verifcationId, string code);
    }
    public class AuthService : IAuthService
    {
        private const string GoogleProviderId = "google.com";
        private IAuth _auth;
        private readonly IFirebaseAuth _firebaseAuth;
        //private readonly IGoogleSignInService googleSignInService;

        public AuthService(IAuth auth, IFirebaseAuth firebaseAuth)
        {
            _auth = auth ?? throw new ArgumentNullException(nameof(auth));
            _firebaseAuth = firebaseAuth ?? throw new ArgumentNullException(nameof(_firebaseAuth));
        }
        public async Task<User> SigninWithPhoneNumber(string verifcationId, string code)
        {
            try
            {
                var credential = _firebaseAuth.PhoneAuthProvider
                                .GetCredential(verifcationId, code);
                var authResult = await _firebaseAuth.Instance
                    .SignInWithCredentialAsync(credential);
                var info = authResult.User;
                if (info == null)
                    return null;
                var token = await info.GetIdTokenAsync(false);
                return new User
                {
                    DisplayName = info.DisplayName,
                    PhoneNumber = info.PhoneNumber,
                    AvatarUrl = info.PhotoUrl?.ToString(),
                    IdToken = token,
                    Email = info.Email
                };
            }
            catch (Exception ex)
            {
                TrackError(ex);
                return null;
            }
        }

        public Task Signout()
        {
            //if (_auth.CurrentUser.ProviderData.Any(f => f.ProviderId == GoogleProviderId))
            //    googleSignInService.SignOut();
            _auth.SignOut();
            return Task.CompletedTask;
        }

        public async Task<string> VerifyPhoneNumber(string phoneNumber)
        {
            var result = await _firebaseAuth.PhoneAuthProvider
                 .VerifyPhoneNumberAsync(phoneNumber);
            return result.VerificationId;
        }

        //error tracking
        private async void TrackError(Exception e)
        {
            if (await Crashes.IsEnabledAsync())
                Crashes.TrackError(e);
            System.Diagnostics.Debug.WriteLine(e);
        }
    }
}

