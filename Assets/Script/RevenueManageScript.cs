using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevenueManageScript : InputData
{
    private float totalSpending = 0;
    private void Start()
    {
        LoadCategoryRevenue();

        if (buttonsCategory == null || buttonsCategory.Length == 0)
        {
            Debug.LogError("Please assign buttons ");
            return;
        }

        SetDataCategory(buttonsCategory[0]); // set default category

        foreach (Button button in buttonsCategory)
        {
            button.onClick.AddListener(() => SetDataCategory(button));
        }

        if (submitButton != null)
        {
            submitButton.onClick.AddListener(() => SubmitData());
        }
    }
    private void Update()
    {
        float totalRevenue = SetTotalAmountRevenueOfMonth(System.DateTime.Now.Month, System.DateTime.Now.Year);
        PlayerPrefs.SetFloat("TotalRevenueText", totalRevenue); //hàm set tổng chi trong 1 tháng dựa vào tháng năm 
    }

    public override void SetDataCategory(Button button)
    {
        base.SetDataCategory(button);
    }

    public override void SubmitData()
    {
        base.SubmitData();

        int month = dateTime.Month;
        int year = dateTime.Year;

        if (!float.TryParse(moneyInput.text, out float Amount))
        {
            Debug.Log("Please enter a valid amount of money");
            return;
        }

        string newCategory = nameCategory.text;
        if (!string.IsNullOrEmpty(newCategory) && !ListNameCategoryRevenue.Contains(newCategory))
        {
            ListNameCategoryRevenue.Add(newCategory);
            SaveCategoryRevenue();
        }
        String getRevenueCategoryOfMonth = CurrentNameCategory + "-" + month.ToString("D2") + "-" + year;
        float currentAmount = PlayerPrefs.GetFloat(getRevenueCategoryOfMonth, 0);

        PlayerPrefs.SetFloat(getRevenueCategoryOfMonth, currentAmount + Amount);
        PlayerPrefs.Save();
        moneyInput.text = String.Empty;

    }
}
