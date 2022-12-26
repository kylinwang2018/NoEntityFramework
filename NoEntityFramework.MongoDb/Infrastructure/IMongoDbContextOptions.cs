namespace NoEntityFramework.MongoDb
{
    public interface IMongoDbContextOptions : IDbContextOptions
    {
        string DatabaseName { get; set; }
    }
}