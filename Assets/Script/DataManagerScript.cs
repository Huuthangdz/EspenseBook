using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManagerScript : MonoBehaviour
{
    public static DataManagerScript Instance { get; private set; }

    public Dictionary<string, string> ColorCategorySpending { get; private set; } = new Dictionary<string, string>();
    public Dictionary<string, float> CategorySpendingName { get; private set; } = new Dictionary<string, float>();
    
    public Dictionary<string, string> ColorCategoryRevenue { get; private set; } = new Dictionary<string, string>();
    public Dictionary<string, float> CategoryRevenueName { get; private set; } = new Dictionary<string, float>();

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadData();
    }
    public void LoadData()
    {
        ColorCategorySpending = LoadDictionaryFromPlayerPrefs<string>("ColorCategorySpending");
        CategorySpendingName = LoadDictionaryFromPlayerPrefs<float>("CategorySpendingName");

        ColorCategoryRevenue = LoadDictionaryFromPlayerPrefs<string>("ColorCategoryRevenue");
        CategoryRevenueName = LoadDictionaryFromPlayerPrefs<float>("CategoryRevenueName");
    }
    public void SetCategorySpendingData(Dictionary<string, string> colorCategorySpending, Dictionary<string, float> categorySpending)
    {
        ColorCategorySpending = colorCategorySpending;
        CategorySpendingName = categorySpending;

        //lưu dữ liệu vào playerprefs 
        SaveDictionaryToPlayerPrefs("ColorCategorySpending", colorCategorySpending);
        SaveDictionaryToPlayerPrefs("CategorySpendingName", categorySpending);
    }

    public void SetCategoryRevenueData(Dictionary<string, string> colorCategoryRevenue, Dictionary<string, float> categoryRevenue)
    {
        ColorCategoryRevenue = colorCategoryRevenue;
        CategoryRevenueName = categoryRevenue;

        //lưu dữ liệu vào playerprefs 
        SaveDictionaryToPlayerPrefs("ColorCategoryRevenue", colorCategoryRevenue);
        SaveDictionaryToPlayerPrefs("CategoryRevenueName", categoryRevenue);
    }

    private void SaveDictionaryToPlayerPrefs<T>(string key, Dictionary<string, T> dictionary)
    {
        var keys = string.Join(",", dictionary.Keys);
        PlayerPrefs.SetString($"{key}_keys", keys);

        foreach (var item in dictionary)
        {
            PlayerPrefs.SetString($"{key}_{item.Key}", item.Value.ToString());
        }
        PlayerPrefs.Save();
        
    }

    private Dictionary<string, T> LoadDictionaryFromPlayerPrefs<T>(string key)
    {
        var dictionary = new Dictionary<string, T>();
        var keys = PlayerPrefs.GetString($"{key}_keys");

        if (!string.IsNullOrEmpty(keys))
        {
            foreach (var item in keys.Split(','))
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var value = PlayerPrefs.GetString($"{key}_{item}");
                    dictionary[item] = (T)System.Convert.ChangeType(value, typeof(T));
                }
            }
        }
        return dictionary;
    }


    public float GetMonthlySpending(string categoryName, int month, int year)
    {
        float totalSpending = 0;
        for(int day = 1;day <= DateTime.DaysInMonth(year, month); day++)
        {
            string key = $"{categoryName.Trim()}-{day:D2}-{month:D2}-{year}";
            totalSpending += PlayerPrefs.GetFloat(key, 0);
        }
        return totalSpending;
        
    }
    public float GetMonthlyRevenue(string categoryName, int month, int year)
    {
        float totalRevenue = 0;
        for(int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
        {
            string key = $"{categoryName.Trim()}-{day:D2}-{month:D2}-{year}";
            totalRevenue += PlayerPrefs.GetFloat(key, 0);
        }
        return totalRevenue;
    }
}