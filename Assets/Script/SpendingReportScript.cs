using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpendingReportScript : PieChartManage
{
    public ReportManageScript reportManageScript;

    public GameObject reportPrefabs;
    public Transform parrentReportPrefabs;

    void Start()
    {
        UpdateCreatePieChart();
    }
    private void OnEnable()
    { 
        if (DataManagerScript.Instance == null)
        {
            Debug.LogError("DataManagerScript instance is null");
            return;
        }
        DataManagerScript.Instance.LoadData();
        UpdateCreatePieChart();
    }
    public override void CreatePieChart(Dictionary<string, string> colorCategorySpending, Dictionary<string, float> categorySpending, int month, int year)
    {
        // Clear existing pie chart slices
        foreach (Transform child in ParrentSlice)
        {
            Destroy(child.gameObject);
        }

        Dictionary<string, float> monthlySpending = new Dictionary<string, float>();

        foreach (var category in categorySpending)
        {
            string categoryName = category.Key;
            float totalSpending = 0;
            for(int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                string spendingKey = $"{categoryName.Trim()}-{day:D2}-{month:D2}-{year}";
                totalSpending += PlayerPrefs.GetFloat(spendingKey, 0);
            }
            if (totalSpending > 0)
            {
                monthlySpending[categoryName] = totalSpending;
            }
        }

        float totalSpendingSum = 0;
         
        foreach (var spending in monthlySpending.Values)
        {
            totalSpendingSum += spending;
        }

        if (totalSpendingSum == 0)
        {
            DisplayWhitePieChart();
            return;
        }

        float zRotation = 0;

        foreach (var category in monthlySpending)
        {
            string categoryName = category.Key;
            float spending = category.Value;
            float spendingPercent = spending / totalSpendingSum;

            GameObject newPieChart = Instantiate(pieSlicePrefab, ParrentSlice);
            Image sliceImage = newPieChart.GetComponent<Image>();

            sliceImage.fillAmount = spendingPercent;
            newPieChart.transform.localRotation = Quaternion.Euler(0,0, zRotation);

            if (colorCategorySpending.TryGetValue(categoryName, out string hexColor))
            {
                Color color;
                if (ColorUtility.TryParseHtmlString("#" + hexColor, out color))
                {
                    sliceImage.color = color;
                }
                else
                {
                    Debug.LogError("Invalid color " + hexColor);
                }
            }
            zRotation -= spendingPercent * 360f;
        }
    }


    public void UpdateCreatePieChart()
    {
        if (DataManagerScript.Instance != null)
        {
            colorCategoriSpending = DataManagerScript.Instance.ColorCategorySpending;
            categorySpendingOfName = DataManagerScript.Instance.CategorySpendingName;

            if (colorCategoriSpending.Count == 0)
            {
                Debug.Log("colorCategorySpending is empty");
            }

            if (categorySpendingOfName.Count == 0)
            {
                Debug.Log("categorySpendingOfName is empty");
            }

            if (categorySpendingOfName.Count > 0)
            {
                int currentMonth = reportManageScript.getCurrentMonth();
                int curenttYear = reportManageScript.getCurrentYear();
                CreatePieChart(colorCategoriSpending, categorySpendingOfName, currentMonth, curenttYear);
                CreateReportSpendingText(colorCategoriSpending, categorySpendingOfName, currentMonth, curenttYear);
            }
            else
            {
                Debug.Log("No spending data found");
            }
        }
        else
        {
            Debug.LogError("No DataManagerScript found in scene");
        }
    }

    public void CreateReportSpendingText(Dictionary<string, string> coloCategorySpending, Dictionary<string, float> categorySpending, int month, int year)
    {
        foreach (Transform child in parrentReportPrefabs)
        {
            Destroy(child.gameObject);
        }

        Dictionary<string, float> monthlySpending = new Dictionary<string, float>();

        foreach (var category in categorySpending)
        {
            string categoryName = category.Key;
            float totalSpending = 0;
            for (int day = 1;day <= DateTime.DaysInMonth(year, month); day++)
            {
                string spendingKey = $"{categoryName.Trim()}-{day:D2}-{month:D2}-{year}";
                totalSpending += PlayerPrefs.GetFloat(spendingKey, 0);
            }
            if (totalSpending > 0)
            {
                monthlySpending[categoryName] = totalSpending;
            }
        }

        float totalSpendingSum = 0;

        foreach (var spending in monthlySpending.Values)
        {
            totalSpendingSum += spending;
        }

        if (totalSpendingSum == 0)
        {
            return;
        }

        foreach(var category in monthlySpending)
        {
            string categoryName = category.Key;
            float spending = category.Value;
            float spendingPercent = (spending / totalSpendingSum) * 100;
            
            GameObject newReport = Instantiate(reportPrefabs, parrentReportPrefabs);

            Transform categoryNameTransform = newReport.transform.Find("CategoryName");
            Transform spendingTransform = newReport.transform.Find("CategorySpending");
            Transform percentTransform = newReport.transform.Find("CategoryPercent");

            if (categoryNameTransform == null || spendingTransform == null || percentTransform == null)
            {
                continue;
            }

            TextMeshProUGUI categoryNameText = categoryNameTransform.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI spendingText = spendingTransform.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI spendingPercentText = percentTransform.GetComponent<TextMeshProUGUI>();

            categoryNameText.text = categoryName;
            spendingText.text = spending.ToString();
            spendingPercentText.text = $"{spendingPercent:F2}%";

            if ( coloCategorySpending.TryGetValue(categoryName, out string hexColor))
            {
                Color color;
                if (ColorUtility.TryParseHtmlString("#" + hexColor, out color))
                {
                    newReport.GetComponent<Image>().color = color;
                }
                else
                {
                    Debug.LogError("Invalid color " + hexColor);
                }
            }
        }
    }
}