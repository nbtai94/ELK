namespace DemoElasticKibana.Configs
{
    public class AppSettings
    {
        public AppSettings() { }
        ElasticSearchOptions ElasticSearch { get; set; }
        SerilogOptions Serilog { get; set; }
    }

    public class ElasticSearchOptions
    {
        public string Url { get; set; }
    }

    public class SerilogOptions
    {
        public string MinimumLevel { get; set; }
        public List<WriteToOption> WriteTo { get; set; }
        public List<string> Enrich { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }

    public class WriteToOption
    {
        public string Name { get; set; }
        public WriteToArgs Args { get; set; }
    }

    public class WriteToArgs
    {
        public string nodeUris { get; set; }
        public string indexFormat { get; set; }
        public bool autoRegisterTemplate { get; set; }
        public string templateName { get; set; }
        public int numberOfShards { get; set; }
        public int numberOfReplicas { get; set; }
    }
}
