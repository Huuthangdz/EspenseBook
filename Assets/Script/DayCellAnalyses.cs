using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DayCellAnalyses : MonoBehaviour
{
    public List<string> listSpendingCategoryName = new List<string>();
    public Dictionary<string, string> colorCategoriSpending = new Dictionary<string, string>();
    public Dictionary<string, float> categorySpendingOfName = new Dictionary<string, float>();

    public List<string> listRevenueCategoryName = new List<string>();
    public Dictionary<string, string> colorCategoryRevenue = new Dictionary<string, string>();
    public Dictionary<string, float> categoryRevenueOfName = new Dictionary<string, float>();
    public DateTime date;

    private void Start()
    {
        if (DataManagerScript.Instance != null)
        {
            colorCategoriSpending = DataManagerScript.Instance.ColorCategorySpending;
            categorySpendingOfName = DataManagerScript.Instance.CategorySpendingName;

            colorCategoryRevenue = DataManagerScript.Instance.ColorCategoryRevenue;
            categoryRevenueOfName = DataManagerScript.Instance.CategoryRevenueName;
        }
        else
        {
            Debug.LogError("No DataManagerScript found in scene");
        }
    }
    public virtual void SetDate(DateTime time)
    {
        this.date = time;
    }
    public virtual void UpdateReportPerDay()
    {
    }
}
