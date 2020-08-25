namespace GridBeyond.Domain.Interfaces.Repository
{
    public interface ICacheRepository
    {
        bool GetCache<T>(string name, out T cacheEntry);

        void SetOrUpdate<T>(T data, string name);
    }
}
