namespace FiapCloudGames.Infrastructure.Logs
{
    public interface ICorrelationIdGenerator
    {
        string Get();
        void Set(string correlationId);
    }
}
