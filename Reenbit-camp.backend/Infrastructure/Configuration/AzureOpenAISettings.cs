namespace Infrastructure.Configuration;

public class AzureOpenAISettings
{
    public string ApiKey { get; set; }
    
    public string Endpoint { get; set; }
    
    public string DeploymentChatName { get; set; }
    
    public string DeploymentEmbeddingName { get; set; }
}