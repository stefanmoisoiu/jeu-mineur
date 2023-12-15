using System.Collections.Generic;
using Unity.Services.CloudSave.Models;

public static class CachedCloudData
{
    private static readonly string[] KeysToCache =
    {
        "masterVolume",
        "musicVolume",
        "sfxVolume"
    };
    
    private static Dictionary<string,Item> _cachedValues = new();

    public static async void UpdateCachedValues() =>
        _cachedValues = await CloudSave.LoadMultiple(KeysToCache);
    
    public static async void UpdateCachedValue(string key) =>
        _cachedValues[key] = await CloudSave.Load(key);
    
    public static bool CachedValueExists(string key) => _cachedValues.ContainsKey(key);
    
    public static T GetCachedValue<T>(string key) => _cachedValues[key].Value.GetAs<T>();
}