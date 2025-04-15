using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml.Schema;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class BaseDaycellAnalysesSpending : DayCellAnalyses
{
    public CalendarManageScript CalendarManageScript;

    public GameObject DailySpendingPanelPrefabs;
    public Transform ParrentDaily;

    private Dictionary<string, float> categorySpendingName;
    private void Start()
    {
        if (DailySpendingPanelPrefabs == null)
        {
            Debug.LogError("DailySpendingPanelPrefabs is not assigned.");
        }
        if (ParrentDaily == null)
        {
            Debug.LogError("ParrentDaily is not assigned.");
        }
        SetDate(CalendarManageScript.getCurrentDate());

        categorySpendingName = new Dictionary<string, float>();
        LoadCategorySpendingName();
    }
    private void LoadCategorySpendingName()
    {
        if (PlayerPrefs.HasKey("CategoriesSpending"))
        {
            string json = PlayerPrefs.GetString("CategoriesSpending");
            List<string> categories = JsonUtility.FromJson<Serialization<string>>(json).target;
            foreach (var category in categories)
            {
                categorySpendingName[category] = 0; // Khởi tạo với giá trị 0
            }
        }
    }
    public override void UpdateReportPerDay()
    {
        // Xóa các chi tiết chi tiêu cũ
        foreach (Transform child in ParrentDaily)
        {
            Destroy(child.gameObject);  
        }

        // Lặp qua tất cả các ngày trong tháng hiện tại
        int totalDayInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        List<KeyValuePair<DateTime, Dictionary<string, float>>> dailySpendings = new List<KeyValuePair<DateTime,Dictionary<string, float>>>();

        for (int day = 1; day <= totalDayInMonth; day++)
        {
            DateTime currentDate = new DateTime(date.Year, date.Month, day);
            Dictionary<string, float> dailySpending = new Dictionary<string, float>();

            foreach (var category in categorySpendingName)
            {
                string categoryName = category.Key;
                float spendingValue = 0;
                string spendingKey = $"{categoryName.Trim()}-{currentDate:dd-MM-yyyy}";
                spendingValue = PlayerPrefs.GetFloat(spendingKey, 0);

                if (spendingValue > 0)
                {
                    dailySpending[categoryName] = spendingValue;
                }
            }
            if (dailySpending.Count > 0)
            {
                dailySpendings.Add(new KeyValuePair<DateTime, Dictionary<string, float>>(currentDate, dailySpending));
            }
        }

        dailySpendings.Sort((x, y) => y.Key.CompareTo(x.Key));
        foreach (var dailySpending in dailySpendings)
        {
            DateTime currentDate = dailySpending.Key;
            Dictionary<string, float> spendingDetails = dailySpending.Value;

            GameObject dailySpendingPanel = Instantiate(DailySpendingPanelPrefabs, ParrentDaily);

            if (dailySpendingPanel == null)
            {
                Debug.LogError("Failed to instantiate DailySpendingPanelPrefabs.");
                return;
            }

            TextMeshProUGUI dateText = dailySpendingPanel.transform.Find("Date").GetComponent<TextMeshProUGUI>();
            if (dateText == null)
            {
                Debug.LogError("DateText is not assigned.");
                return;
            }
            dateText.text = currentDate.ToString("dd-MM-yyyy");

            float totalSpending = 0;
            foreach (var spending in spendingDetails.Values)
            {
                totalSpending += spending;
            }
            TextMeshProUGUI totalSpendingText = dailySpendingPanel.transform.Find("TotalSpendingOfDay").GetComponent<TextMeshProUGUI>();
            totalSpendingText.text = "-" + totalSpending.ToString() + " VND";
        }

    }
}
[Serializable]
public class Serilization<T>
{
    public List<T> target;
}
