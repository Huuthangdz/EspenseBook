using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReportManageScript : MonoBehaviour
{
    public TextMeshProUGUI totalExpenseText;    
    public TextMeshProUGUI DateText;
    public Button buttonNextMonth;
    public Button buttonPrevMonth;

    private List<String> listSpendingCategoryName = new List<String>();
    private List<String> listRevenueCategoryName = new List<String>();
    private DateTime dateTime;
    private float totalSpendingOfMonth = 0;
    private float totalRevenueOfMonth = 0;
    private float totalExpense = 0;

    public SpendingReportScript spendingReport;
    public RevenueReportScript revenueReport;

    // Start is called before the first frame update
    void Start()
    {
        dateTime = System.DateTime.Now;
        DateText.text = dateTime.ToString("MM") + " / " + dateTime.ToString("yyyy");


        buttonNextMonth.onClick.AddListener(NextMonth);
        buttonPrevMonth.onClick.AddListener(PrevMonth);


        if (DataManagerScript.Instance != null)
        {
            var colorCategoriSpending = DataManagerScript.Instance.ColorCategorySpending;
            var categorySpendingOfName = DataManagerScript.Instance.CategorySpendingName;
            spendingReport.CreatePieChart(colorCategoriSpending, categorySpendingOfName, dateTime.Month, dateTime.Year);
            spendingReport.CreateReportSpendingText(colorCategoriSpending, categorySpendingOfName, dateTime.Month, dateTime.Year);

            var colorCategoryRevenue = DataManagerScript.Instance.ColorCategoryRevenue;
            var categoryRevenueOfName = DataManagerScript.Instance.CategoryRevenueName;
            revenueReport.CreatePieChart(colorCategoryRevenue, categoryRevenueOfName, dateTime.Month, dateTime.Year);
            revenueReport.CreateReportRevenueText(colorCategoryRevenue, categoryRevenueOfName, dateTime.Month, dateTime.Year);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.HasKey("CategoriesSpending"))
        {
            string json = PlayerPrefs.GetString("CategoriesSpending");
            listSpendingCategoryName = JsonUtility.FromJson<Serialization<string>>(json).target;
        }

        if (PlayerPrefs.HasKey("CategoriesRevenue"))
        {
            String json = PlayerPrefs.GetString("CategoriesRevenue");
            listRevenueCategoryName = JsonUtility.FromJson<Serialization<string>>(json).target;
        }
        UpdateTextExpenseOfMonth(dateTime.Month, dateTime.Year);

    }

    public void NextMonth()
    {
        dateTime = dateTime.AddMonths(1);
        UpdateDateText();
        UpdateTextExpenseOfMonth(dateTime.Month, dateTime.Year);
        spendingReport.UpdateCreatePieChart();
        revenueReport.UpdateCreatePieChart();
    }
    public void PrevMonth()
    {
        dateTime = dateTime.AddMonths(-1);
        UpdateDateText();
        UpdateTextExpenseOfMonth(dateTime.Month, dateTime.Year);
        spendingReport.UpdateCreatePieChart();
        revenueReport.UpdateCreatePieChart();
    }
    public void UpdateTextExpenseOfMonth(int Month, int Year)
    {
        totalSpendingOfMonth = GetTotalSpending(Month, Year);
        totalRevenueOfMonth = GetTotalRevenue(Month, Year);
        totalExpense = totalRevenueOfMonth - totalSpendingOfMonth;
        totalExpenseText.text = totalExpense.ToString("N0") + " VND";
    }

    public float GetTotalSpending(int month, int Year)
    {

        float totalAmount = 0;

        if (listSpendingCategoryName != null)
        {
            foreach (string tag in listSpendingCategoryName)
            {
                for(int day = 1;day <= DateTime.DaysInMonth(Year, month); day++)
                {
                    string key = $"{tag.Trim()}-{day:D2}-{month:D2}-{Year}";
                    totalAmount += PlayerPrefs.GetFloat(key, 0);
                }
            }
        }
        return totalAmount;
    }

    private float GetTotalRevenue(int month, int Year)
    {

        float totalAmount = 0;

        if (listRevenueCategoryName != null)
        {
            foreach (string tag in listRevenueCategoryName)
            {
                for ( int day = 1; day <= DateTime.DaysInMonth(Year, month); day++)
                {
                    string key = $"{tag.Trim()}-{day:D2}-{month:D2}-{Year}";
                    totalAmount += PlayerPrefs.GetFloat(key, 0);
                }
            }
        }
        return totalAmount;
    }
    
    public int getCurrentDay()
    {
        return dateTime.Day;
    }
    public int getCurrentMonth()
    {
        return dateTime.Month;
    }

    public int getCurrentYear()
    {
        return dateTime.Year;
    }

    private void UpdateDateText()
    {
        DateText.text = dateTime.ToString("MM - yyyy");
    }
}
