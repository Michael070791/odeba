using System;
using odeba.Services.IServices;

namespace odeba.iOS.Settings
{
    public class ProjectSettings : IProjectSettings
    {
        public ProjectSettings(Firebase.Core.Options options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            FirebaseClientBaseUrl = options.DatabaseUrl;
            FirebaseStorageBucket = options.StorageBucket;
        }

        public string FirebaseStorageBucket { get; }

        public string FirebaseClientBaseUrl { get; }
    }
}

