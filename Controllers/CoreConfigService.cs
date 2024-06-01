using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

public class CoreConfig
{
    [JsonPropertyName("serverName")]
    public string ServerName { get; set; }

    public Features Features { get; set; }
}

public class Features
{
    public ChatbotFeatures ChatbotFeatures { get; set; }
}

public class ChatbotFeatures
{
    public bool SptFriendEnabled { get; set; }
    public bool CommandoEnabled { get; set; }
}


public class CoreConfigService
{
    private readonly string coreConfigPath;

    public string CoreConfigPath => coreConfigPath;


    public CoreConfigService(IConfiguration configuration)
    {
        var serverPath = configuration["ServerPath"];
        coreConfigPath = Path.Combine(serverPath, "Aki_Data", "Server", "configs", "core.json");
    }

    public void UpdateField<T>(string fieldPath, T value)
    {
        if (!File.Exists(coreConfigPath))
        {
            throw new FileNotFoundException("Configuration file not found.");
        }

        var jsonString = File.ReadAllText(coreConfigPath);
        var jsonNode = JsonNode.Parse(jsonString);

        if (jsonNode == null)
        {
            throw new InvalidOperationException("Failed to parse the configuration file.");
        }

        var segments = fieldPath.Split('.');
        var current = jsonNode;

        for (int i = 0; i < segments.Length - 1; i++)
        {
            current = current[segments[i]] ??= new JsonObject();
        }

        current[segments[^1]] = JsonValue.Create(value);

        File.WriteAllText(coreConfigPath, jsonNode.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
    }

    public CoreConfig LoadCoreConfig()
    {
        if (!File.Exists(coreConfigPath))
        {
            throw new FileNotFoundException("Configuration file not found.");
        }

        var jsonString = File.ReadAllText(coreConfigPath);
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(jsonString);

        var coreConfig = new CoreConfig();

        // Deserialize 'serverName' property
        if (jsonElement.TryGetProperty("serverName", out var serverNameElement) && serverNameElement.ValueKind == JsonValueKind.String)
        {
            coreConfig.ServerName = serverNameElement.GetString();
            Console.WriteLine($"Server Name: {coreConfig.ServerName}");
        }

        // Deserialize 'Features' property
        if (jsonElement.TryGetProperty("features", out var featuresElement) && featuresElement.ValueKind == JsonValueKind.Object)
        {
            coreConfig.Features = new Features();

            // Deserialize 'ChatbotFeatures' property
            if (featuresElement.TryGetProperty("chatbotFeatures", out var chatbotFeaturesElement) && chatbotFeaturesElement.ValueKind == JsonValueKind.Object)
            {
                coreConfig.Features.ChatbotFeatures = new ChatbotFeatures();

                // Deserialize 'SptFriendEnabled' property
                if (chatbotFeaturesElement.TryGetProperty("sptFriendEnabled", out var sptFriendEnabledElement) && sptFriendEnabledElement.ValueKind == JsonValueKind.True || sptFriendEnabledElement.ValueKind == JsonValueKind.False)
                {
                    coreConfig.Features.ChatbotFeatures.SptFriendEnabled = sptFriendEnabledElement.GetBoolean();
                    Console.WriteLine($"Spt Friend Enabled: {coreConfig.Features.ChatbotFeatures.SptFriendEnabled}");
                }

                // Deserialize 'CommandoEnabled' property
                if (chatbotFeaturesElement.TryGetProperty("commandoEnabled", out var commandoEnabledElement) && commandoEnabledElement.ValueKind == JsonValueKind.True || commandoEnabledElement.ValueKind == JsonValueKind.False)
                {
                    coreConfig.Features.ChatbotFeatures.CommandoEnabled = commandoEnabledElement.GetBoolean();
                    Console.WriteLine($"Commando Enabled: {coreConfig.Features.ChatbotFeatures.CommandoEnabled}");
                }
            }
        }

        return coreConfig;
    }
}