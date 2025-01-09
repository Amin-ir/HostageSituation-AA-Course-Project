using UnityEngine;

public static class ConfigRetriever
{
    private static string GetConfigLocation<TConfig>()
    {
        //if (typeof(TConfig) == typeof(HealthComponentConfiguration))
        //    return ConfigsLocations.PlayerHealthConfiguration;
        return "";
    }

    public static TConfig Get<TConfig>() where TConfig : IComponentConfiguration
    {
        string location = GetConfigLocation<TConfig>();

        return JsonUtility.FromJson<TConfig>(location);
    }
}
