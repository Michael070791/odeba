using System;
namespace odeba.Services.IServices
{
    public interface IProjectSettings
    {
        string FirebaseStorageBucket { get; }
        string FirebaseClientBaseUrl { get; }
    }
}

