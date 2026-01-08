namespace FiapCloudGames.Infrastructure.Logs
{
    public class CorrelationIdGenerator : ICorrelationIdGenerator
    {
        private string _correlationId;

        public string Get() => _correlationId;

        public void Set(string correlationId) => _correlationId = correlationId;
    }
}
