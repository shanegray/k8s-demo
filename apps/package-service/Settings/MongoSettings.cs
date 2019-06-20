namespace PackageService.Settings
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string EventStoreCollectionName { get; set; }
        public string ReadModelCollectionName { get; set; }
    }
}
