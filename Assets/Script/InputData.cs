using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class InputData : MonoBehaviour
{
    public TextMeshProUGUI nameCategory;
    public TMP_InputField moneyInput;
    public String CurrentMonthText;

    public Button[] buttonsCategory;
    public Button submitButton;
    public DateTime dateTime = System.DateTime.Now;
    public List<String> ListNameCategorySpending = new List<string>();
    public List<String> ListNameCategoryRevenue = new List<string>();

    public String CurrentNameCategory;

    void Start()
    {
        foreach (Button button in buttonsCategory)
        {
            button.onClick.AddListener(() => SetDataCategory(button));
        }

        SetDataCategory(buttonsCategory[0]); // set default category

        submitButton.onClick.AddListener(SubmitData);

        CurrentMonthText = System.DateTime.Now.ToString("MM-yyyy");

    }
    
    public virtual void SetDataCategory(Button button)
    {
        nameCategory.text = button.transform.Find("Name").GetComponent<TMP_Text>().text;
        PlayerPrefs.SetString("NameCategory", nameCategory.text); // save current category
        CurrentNameCategory = PlayerPrefs.GetString("NameCategory", nameCategory.text);
    }

    public virtual void SubmitData()
    {

        // thêm điều kiện nhập số tiền là 0
        if (moneyInput.text == "")
        {
            Debug.Log("Please enter the money");
            return;
        }
    }
    public float SetTotalAmountSpendingOfMonth(int month, int year)
    {
        float totalAmount = 0;

        if (ListNameCategorySpending != null)
        {
            foreach (string tag in ListNameCategorySpending)
            {
                String getSpendingCategoryOfMonth = tag + "-" + month.ToString("D2") + "-" + year;
                totalAmount += PlayerPrefs.GetFloat(getSpendingCategoryOfMonth, 0);
            }
        }
        return totalAmount;
    }

    public float SetTotalAmountRevenueOfMonth(int month, int year)
    {
        float totalAmount = 0;

        if (ListNameCategoryRevenue != null)
        {
            foreach (string tag in ListNameCategoryRevenue)
            {
                String getRevenueCategoryOfMonth = tag + "-" + month.ToString("D2") + "-" + year;
                totalAmount += PlayerPrefs.GetFloat(getRevenueCategoryOfMonth, 0);
            }
        }
        return totalAmount;
    }
    public void SaveCategoriesSpending()
    {
        string json = JsonUtility.ToJson(new Serialization<string>(ListNameCategorySpending));
        PlayerPrefs.SetString("CategoriesSpending", json);
        PlayerPrefs.Save();
    }

    public void LoadCategoriesSpending()
    {
        if (PlayerPrefs.HasKey("CategoriesSpending"))
        {
            string json = PlayerPrefs.GetString("CategoriesSpending");
            ListNameCategorySpending = JsonUtility.FromJson<Serialization<string>>(json).target;
        }
        if (ListNameCategorySpending == null)
        {
            ListNameCategorySpending = new List<string>();
        }
    }

    public void SaveCategoryRevenue()
    {
        string jason = JsonUtility.ToJson(new Serialization<string>(ListNameCategoryRevenue));
        PlayerPrefs.SetString("CategoriesRevenue", jason);
        PlayerPrefs.Save();
    }

    public void LoadCategoryRevenue()
    {
        if (PlayerPrefs.HasKey("CategoriesRevenue"))
        {
            string json = PlayerPrefs.GetString("CategoriesRevenue");
            ListNameCategoryRevenue = JsonUtility.FromJson<Serialization<string>>(json).target;
        }
        if (ListNameCategoryRevenue == null)
        {
            ListNameCategoryRevenue = new List<string>();
        }
    }
}

[System.Serializable]
public class Serialization<T>
{
    public List<T> target;
    public Serialization(List<T> target)
    {
        this.target = target;
    }
}