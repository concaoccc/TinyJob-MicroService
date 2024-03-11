using System.Diagnostics;

namespace JobSDK
{
    public interface IJobLogger
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogDebug(string message);
    }
}
