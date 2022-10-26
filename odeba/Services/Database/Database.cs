using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using odeba.Services.IServices;
using Plugin.FirebaseAuth;
using Xamarin.Essentials;

namespace odeba.Services.Database
{
    public class Database
    {
        #region Constants

        #endregion
        #region Shared objects

        #endregion
        #region User's Objects

        #endregion
        private readonly IFirebaseAuth _auth;
        private readonly ILocationService _locationService;
        private readonly FirebaseClient _client;
        private string _lastSyncUser = string.Empty;
        public Database(FirebaseClient client,
                        IFirebaseAuth auth,
                        ILocationService locationService)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _auth = auth ?? throw new ArgumentNullException(nameof(auth));


            // Init user's collections
            InitLazyDatabases(auth.Instance);

            auth.Instance.AddAuthStateChangedListener(InitLazyDatabases);
        }
        // save device token into databse
        internal async ValueTask SaveToken()
        {
            var token = "";//Preferences.Get(nameof(Repository.ContactInfoEnvelop.Token), string.Empty);
            if (_auth.Instance.CurrentUser != null && !string.IsNullOrEmpty(token))
            {
                await _client.Child("tokens").Child(_auth.Instance.CurrentUser.Uid)
                    .PutAsync(new { token });
            }
        }

        private void InitLazyDatabases(IAuth instance)
        {
            throw new NotImplementedException();
        }




        private async Task SyncUsersObjects()
        {
            InitLazyDatabases(_auth.Instance);

            //if (_userAddressDb != null)
            //    _ = _userAddressDb.Value.PullAsync();
        }

    }
}

