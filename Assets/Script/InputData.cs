using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;


public class InputData : MonoBehaviour
{
    public TextMeshProUGUI nameCategory;
    public TMP_InputField moneyInput;

    public Button[] buttonsCategory;
    public Button submitButton;

    public DateTime dateTime = System.DateTime.Now;
    public List<string> ListNameCategorySpending = new List<string>();
    public Dictionary<string, string> colorCategoriSpending = new Dictionary<string, string>();

    public List<string> ListNameCategoryRevenue = new List<string>();
    public Dictionary<string, string> colorCategoryRevenue = new Dictionary<string, string>();

    void Start()
    {
        foreach (Button button in buttonsCategory)
        {
            button.onClick.AddListener(() => SetDataCategory(button));
        }

        SetDataCategory(buttonsCategory[0]); // set default category

        submitButton.onClick.AddListener(SubmitData);

    }
    public virtual void SetDataCategory(Button button)
    {
        nameCategory.text = button.transform.Find("Name").GetComponent<TMP_Text>().text;
        PlayerPrefs.SetString("NameCategory", nameCategory.text); // save current category
    }

    public virtual void SubmitData()
    {

    }
    public float SetTotalAmountSpendingOfMonth(int month, int year)
    {
        float totalAmount = 0;

        if (ListNameCategorySpending != null)
        {
            foreach (string tag in ListNameCategorySpending)
            {
                for ( int day = 1;day <= DateTime.DaysInMonth(year,month);day++)
                {
                    string key = $"{tag.Trim()}-{day:D2}-{month:D2}-{year}";
                    totalAmount += PlayerPrefs.GetFloat(key, 0);
                }
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
                for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
                {
                    string key = $"{tag.Trim()}-{day:D2}-{month:D2}-{year}";
                    totalAmount += PlayerPrefs.GetFloat(key, 0);
                }
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