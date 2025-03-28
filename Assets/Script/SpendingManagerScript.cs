using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SpendingManagerScript : InputData
{
    private void Start()
    {
        LoadCategoriesSpending();

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
        float totalSpending = SetTotalAmountSpendingOfMonth(System.DateTime.Now.Month, System.DateTime.Now.Year);
        PlayerPrefs.SetFloat("TotalSpendingText", totalSpending); //hàm set tổng chi trong 1 tháng dựa vào tháng năm 
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
        if (!string.IsNullOrEmpty(newCategory) && !ListNameCategorySpending.Contains(newCategory))
        {
            ListNameCategorySpending.Add(newCategory);
            SaveCategoriesSpending();
        }
        String getSpendingCategoryOfMonth = CurrentNameCategory + "-" + month.ToString("D2") + "-" + year;
        float currentAmount = PlayerPrefs.GetFloat(getSpendingCategoryOfMonth, 0);

        PlayerPrefs.SetFloat(getSpendingCategoryOfMonth, currentAmount + Amount);
        PlayerPrefs.Save();
        moneyInput.text = String.Empty;

    }
}
