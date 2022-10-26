using System;
using odeba.Models;
using System.Threading.Tasks;
using Akavache;
using Firebase.Storage;
using odeba.Services.IServices;
using Plugin.FirebaseAuth;
using System.Reactive.Subjects;

namespace odeba.Services.Auth
{
    public interface IUserService
    {
        Task<string> VerifyPhoneNumber(string phoneNumber);
        Task<bool> SignInByPhone(string verificationId, string code);

        IObservable<bool> IsLoggedIn();
        IObservable<User> GetUser();
        Task UpdateUser(string name, string surname);
        Task<SimpleImage> UpdateUserPicture(string pictureBase64);
        Task SaveUser(User user);
        Task Signout();
    }
    public class UserService : IUserService
    {
        private const string IdTokenKey = nameof(IdTokenKey);

        private readonly IBlobCache _cache;
        private readonly IAuthService _client;
        private readonly ILocationService _location;
        //private readonly Database _database;
        private readonly IFirebaseAuth _firebaseAuth;
        private readonly INavigationService _navigation;
        private readonly FirebaseStorage _storage;
        private readonly Subject<bool> _isLoggedIn = new Subject<bool>();
        private IUser _user;

        public UserService(IAuthService client,
                            IBlobCache cache,
                            ILocationService location,
                            IFirebaseAuth firebaseAuth,
                            INavigationService navigation,
                            FirebaseStorage firebaseStorage)
        {
            _client = client;
            _cache = cache;
            _location = location;
            _firebaseAuth = firebaseAuth;
            _navigation = navigation;
            _storage = firebaseStorage;

            firebaseAuth.Instance.AddAuthStateChangedListener(auth =>
            {
                _user = auth.CurrentUser;
                _isLoggedIn.OnNext(_user != null);
            });


        }
        public IObservable<User> GetUser()
        {
            throw new NotImplementedException();
        }

        public IObservable<bool> IsLoggedIn()
        {
            throw new NotImplementedException();
        }

        public Task SaveUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SignInByPhone(string verificationId, string code)
        {
            var user = await _client.SigninWithPhoneNumber(verificationId, code);
            return user != null;
        }

        public Task Signout()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(string name, string surname)
        {
            throw new NotImplementedException();
        }

        public Task<SimpleImage> UpdateUserPicture(string pictureBase64)
        {
            throw new NotImplementedException();
        }

        public Task<string> VerifyPhoneNumber(string phoneNumber)
        {
            return _client.VerifyPhoneNumber(phoneNumber);
        }
    }
}

