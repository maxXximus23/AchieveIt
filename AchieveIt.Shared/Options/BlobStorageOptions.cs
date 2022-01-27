
namespace AchieveIt.Shared.Options
{
    public class BlobStorageOptions
    {
        public const string SectionName = nameof(BlobStorageOptions);
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string EndpointSuffix { get; set; }

        public string SasHost => $"{AccountName}.blob.{EndpointSuffix}";

        public string ConnectionString => "DefaultEndpointsProtocol=https;" +
                                          $"AccountName={AccountName};" +
                                          $"AccountKey={AccountKey};" +
                                          $"EndpointSuffix={EndpointSuffix};";
    }
}