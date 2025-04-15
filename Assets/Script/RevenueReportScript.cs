using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RevenueReportScript : PieChartManage
{
    public ReportManageScript reportManageScript;

    public GameObject reportPrefabs;
    public Transform parrentReportPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCreatePieChart();
    }

    public override void CreatePieChart(Dictionary<string, string> colorCategoryRevenue, Dictionary<string, float> categoryRevenue,int month, int year)
    {
        foreach (Transform child in ParrentSlice)
        {
            Destroy(child.gameObject);
        }

        Dictionary<string, float> monthlyRevenue = new Dictionary<string, float>();

        foreach (var category in categoryRevenue)
        {
            string categoryName = category.Key;
            float totalRevenue = 0;
            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                string revenueKey = $"{categoryName.Trim()}-{day:D2}-{month:D2}-{year}";
                totalRevenue += PlayerPrefs.GetFloat(revenueKey, 0);
            }
            if (totalRevenue > 0)
            {
                monthlyRevenue[categoryName] = totalRevenue;
            }
        }

        float totalRevenueSum = 0;

        foreach (var revenue in monthlyRevenue.Values)
        {
            totalRevenueSum += revenue;
        }

        if (totalRevenueSum == 0)
        {
            DisplayWhitePieChart();
            return;
        }

        float zRotation = 0;

        foreach (var category in monthlyRevenue)
        {
            string categoryName = category.Key;
            float revenue = category.Value;
            float revenuePercent = revenue / totalRevenueSum;

            GameObject newPieChart = Instantiate(pieSlicePrefab, ParrentSlice);
            Image sliceImage = newPieChart.GetComponent<Image>();

            sliceImage.fillAmount = revenuePercent;
            newPieChart.transform.localRotation = Quaternion.Euler(0, 0, zRotation);

            if (colorCategoryRevenue.TryGetValue(categoryName, out string hexColor))
            {
                Color color;
                if (ColorUtility.TryParseHtmlString("#" + hexColor, out color))
                {
                    sliceImage.color = color;
                }
                else
                {
                    Debug.LogError("Invalid color" + hexColor);
                }
            }
            zRotation -= 360 * revenuePercent;
        }
    }

    public void UpdateCreatePieChart()
    {
        if (DataManagerScript.Instance != null)
        {
            colorCategoryRevenue = DataManagerScript.Instance.ColorCategoryRevenue;
            categoryRevenueOfName = DataManagerScript.Instance.CategoryRevenueName;
            if (categoryRevenueOfName.Count > 0)
            {
                int currentMonth = reportManageScript.getCurrentMonth();
                int currentYear = reportManageScript.getCurrentYear();
                CreatePieChart(colorCategoryRevenue, categoryRevenueOfName, currentMonth, currentYear);
                CreateReportRevenueText(colorCategoryRevenue, categoryRevenueOfName, currentMonth, currentYear);
            }
        }
    }
    public void CreateReportRevenueText(Dictionary<string, string> coloCategoryRevenue, Dictionary<string, float> categoryRevenue, int month, int year)
    {
        foreach (Transform child in parrentReportPrefabs)
        {
            Destroy(child.gameObject);
        }

        Dictionary<string, float> monthlyRevenue = new Dictionary<string, float>();

        foreach (var category in categoryRevenue)
        {
            string categoryName = category.Key;
            float totalRevenue = 0;
            for ( int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                string revenueKey = $"{categoryName.Trim()}-{day:D2}-{month:D2}-{year}";
                totalRevenue += PlayerPrefs.GetFloat(revenueKey, 0);
            }
            if (totalRevenue > 0)
            {
                monthlyRevenue[categoryName] = totalRevenue;
            }
        }

        float totalReveneSum = 0;

        foreach (var Revenue in monthlyRevenue.Values)
        {
            totalReveneSum += Revenue;
        }

        if (totalReveneSum == 0)
        {
            return;
        }

        foreach (var category in monthlyRevenue)
        {
            string categoryName = category.Key;
            float Revenue = category.Value;
            float RevenuePercent = (Revenue / totalReveneSum) * 100;

            GameObject newReport = Instantiate(reportPrefabs, parrentReportPrefabs);

            Transform categoryNameTransform = newReport.transform.Find("CategoryName");
            Transform revenueTransform = newReport.transform.Find("CategoryRevenue");
            Transform percentTransform = newReport.transform.Find("CategoryPercent");

            if (categoryNameTransform == null || revenueTransform == null || percentTransform == null)
            {
                continue;
            }

            TextMeshProUGUI categoryNameText = categoryNameTransform.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI revenueText = revenueTransform.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI revenuePercentText = percentTransform.GetComponent<TextMeshProUGUI>();

            categoryNameText.text = categoryName;
            revenueText.text = Revenue.ToString();
            revenuePercentText.text = $"{RevenuePercent:F2}%";

            if (coloCategoryRevenue.TryGetValue(categoryName, out string hexColor))
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