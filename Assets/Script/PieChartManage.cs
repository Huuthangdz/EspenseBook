using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieChartManage : MonoBehaviour
{
    public List<string> listSpendingCategoryName = new List<string>();
    public Dictionary<string, string> colorCategoriSpending = new Dictionary<string, string>();
    public Dictionary<string, float> categorySpendingOfName = new Dictionary<string, float>();

    public List<string> listRevenueCategoryName = new List<string>();
    public Dictionary<String, String> colorCategoryRevenue = new Dictionary<string, String>();
    public Dictionary<string, float> categoryRevenueOfName = new Dictionary<string, float>();

    public Transform ParrentSlice;
    public GameObject pieSlicePrefab;
    void Start()
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

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void CreatePieChart(Dictionary<string,string> colorCategorySpending, Dictionary<string,float> categorySpending, int month, int year)
    {
        
    }

    public void DisplayWhitePieChart()
    {
        foreach (Transform child in ParrentSlice)
        {
            Destroy(child.gameObject);
        }

        GameObject newPieChart = Instantiate(pieSlicePrefab, ParrentSlice);
        Image image = newPieChart.GetComponent<Image>();
        image.fillAmount = 1f;
        image.color = Color.white;
    }
}
