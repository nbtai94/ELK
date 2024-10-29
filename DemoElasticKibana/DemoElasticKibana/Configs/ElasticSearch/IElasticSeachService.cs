namespace DemoElasticKibana.Configs.ElasticSearch
{
    public interface IElasticSeachService<T> where T : class
    {
        Task IndexDocumentAsync(string index, T document);
    }
}
