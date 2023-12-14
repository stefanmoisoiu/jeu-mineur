using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;

public static class CloudSave
{
    public delegate void CloudSaveCallback();
    
    public static async  void Save(string key, object value, CloudSaveCallback callback = null)
    {
        Dictionary<string,object> data = new() { { key, value } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        callback?.Invoke();
    }
    
    public static async Task<T> Load<T>(string key, CloudSaveCallback callback = null)
    {
        
        HashSet<string> keys = new() { key };
        Dictionary<string,Item> data = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);
        
        if (!data.TryGetValue(key, out Item item))
        {
            callback?.Invoke();
            return default;
        }

        callback?.Invoke();
        
        return item.Value.GetAs<T>();
    }

    public static async Task<T[]> LoadMultiple<T>(string[] keys, CloudSaveCallback callback = null)
    {
        HashSet<string> keySet = new();
        foreach (string key in keys) keySet.Add(key);
        
        Dictionary<string,Item> data = await CloudSaveService.Instance.Data.Player.LoadAsync(keySet);
        
        List<T> values = new();

        foreach (string key in keys)
        {
            if (!data.TryGetValue(key, out Item item))
            {
                Debug.LogError($"Key {key} not found in cloud save data");
                callback?.Invoke();
                return values.ToArray();
            }
            values.Add(item.Value.GetAs<T>());
        }

        callback?.Invoke();
        return values.ToArray();
    }
    
    public static async Task<T> SaveOrLoad<T> (string key, T value, CloudSaveCallback callback = null)
    {
        HashSet<string> keys = new() { key };
        
        Dictionary<string,Item> data = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

        if (data.TryGetValue(key, out Item item))
        {
            // Load
            
            callback?.Invoke();
            return item.Value.GetAs<T>();
        }
        else
        {
            // Save
            
            Dictionary<string, object> newData = new() { { key, value } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(newData);
            callback?.Invoke();
            return value;
        }
    }
    public static async Task<T[]> SaveOrLoadMultiple<T> (string[] keys, T[] values, CloudSaveCallback callback = null)
    {
        HashSet<string> keySet = new();
        foreach (string key in keys) keySet.Add(key);
        
        Dictionary<string,Item> data = await CloudSaveService.Instance.Data.Player.LoadAsync(keySet);

        List<T> returnValues = new();

        for (int i = 0; i < keys.Length; i++)
        {
            string key = keys[i];
            T value = values[i];
            
            if (data.TryGetValue(key, out Item item))
            {
                // Load
                returnValues.Add(item.Value.GetAs<T>());
            }
            else
            {
                // Save
                Dictionary<string, object> newData = new() { { key, value } };
                await CloudSaveService.Instance.Data.Player.SaveAsync(newData);
                returnValues.Add(value);
            }
        }
        
        callback?.Invoke();
        return returnValues.ToArray();
    }
    
    public static async void Delete(string key, CloudSaveCallback callback = null)
    {
        await CloudSaveService.Instance.Data.Player.DeleteAsync(key);
        callback?.Invoke();
    }
    
    public static async void DeleteMultiple(string[] keys, CloudSaveCallback callback = null)
    {
        foreach (string key in keys)
        {
            await CloudSaveService.Instance.Data.Player.DeleteAsync(key);
        }
        callback?.Invoke();
    }
    public static async void ResetData(CloudSaveCallback callback = null)
    {
        await CloudSaveService.Instance.Data.Player.DeleteAllAsync();
        callback?.Invoke();
    }
}