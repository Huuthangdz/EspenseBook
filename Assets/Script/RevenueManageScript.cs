using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.UI;

public class RevenueManageScript : InputData
{
    public GameObject warningPressMoney;
    public Button okPressMoney;

    public GameObject plusCategoryPanel;
    public Button PlusCategoryMoreButton;
    public Button CancelPlusCategoryButton;

    public GameObject warningPanelNegativeMoney;
    public Button okPressNegativeMoney;

    public GameObject deleteConfirmPanel;
    public Button confirmDelete;
    public Button cancelDelete;
    private Button buttonToDelete;

    private void Start()
    {

        if (buttonsCategory == null || buttonsCategory.Length == 0)
        {
            Debug.LogError("Please assign buttons ");
            return;
        }

        SetDataCategory(buttonsCategory[0]); // set default category

        foreach (Button button in buttonsCategory)
        {
            button.onClick.AddListener(() => SetDataCategory(button));
            AddLongPressDeleteButton(button);
        }

        if (submitButton != null)
        {
            submitButton.onClick.AddListener(() => SubmitData());
        }
        LoadCategoryRevenue();
        SetCategoryRevenueColors();
        SaveColorCategoryRevenue();
        SaveDataRevenue();

        okPressMoney.onClick.AddListener(() => warningPressMoney.SetActive(false));
        PlusCategoryMoreButton.onClick.AddListener(() => plusCategoryPanel.SetActive(true));
        CancelPlusCategoryButton.onClick.AddListener(() => plusCategoryPanel.SetActive(false));
        okPressNegativeMoney.onClick.AddListener(() => warningPanelNegativeMoney.SetActive(false));

        cancelDelete.onClick.AddListener(() => deleteConfirmPanel.SetActive(false));
        confirmDelete.onClick.AddListener(() => ConfirmDelete());
    }
    private void Update()
    {
        float totalRevenue = SetTotalAmountRevenueOfMonth(System.DateTime.Now.Month, System.DateTime.Now.Year);
        PlayerPrefs.SetFloat("TotalRevenueText", totalRevenue); //hàm set tổng thu trong 1 tháng dựa vào tháng năm 
    }

    public override void SetDataCategory(Button button)
    {
        base.SetDataCategory(button);
    }
    public override void SubmitData()
    {
        base.SubmitData();
        LoadColorCategoryRevenue();
        int day = dateTime.Day;
        int month = dateTime.Month;
        int year = dateTime.Year;

        if (!float.TryParse(moneyInput.text, out float Amount))
        {
            warningPressMoney.SetActive(true);
            return;
        }
        if (Amount < 0)
        {
            warningPressMoney.SetActive(true);
            return;
        }
        string newCategory = nameCategory.text;

        if (!string.IsNullOrEmpty(newCategory) && !ListNameCategoryRevenue.Contains(newCategory))
        {
            ListNameCategoryRevenue.Add(newCategory);
            SaveCategoryRevenue();

            if (!colorCategoryRevenue.ContainsKey(newCategory))
            {
                Color textColor = nameCategory.color;
                string hexColor = ColorUtility.ToHtmlStringRGB(textColor);
                colorCategoryRevenue.Add(newCategory, hexColor);
                SaveCategoryRevenue();  
            }
        }
        String getRevenueCategoryOfMonth = $"{newCategory.Trim()}-{day:D2}-{month:D2}-{year}";
        float currentAmount = PlayerPrefs.GetFloat(getRevenueCategoryOfMonth, 0); // lấy số tiền hiện tại của danh mục

        PlayerPrefs.SetFloat(getRevenueCategoryOfMonth, currentAmount + Amount);
        PlayerPrefs.Save();
        moneyInput.text = String.Empty;
        SetCategoryRevenueColors();
        SaveColorCategoryRevenue();
        SaveDataRevenue();  
    }
    public void SetCategoryRevenueColors()
    {
        foreach (Button button in buttonsCategory)
        {
            string categoryName = button.transform.Find("Name").GetComponent<TMP_Text>().text;
            if (ListNameCategoryRevenue.Contains(categoryName))
            {
                Color categoryColor = button.transform.Find("Name").GetComponent<TMP_Text>().color;
                String hexColor = ColorUtility.ToHtmlStringRGB(categoryColor);
                colorCategoryRevenue[categoryName] = hexColor;
            }
        }
    }
    public Dictionary<string, string> colorsCategoriRevenueDictionary()
    {
        return colorCategoryRevenue;
    }
    public Dictionary<string, float> GetCategoryRevenue()
    {
        Dictionary<string, float> categoryRevenue = new Dictionary<string, float>();
        foreach (string category in ListNameCategoryRevenue)
        {
            float revenue = PlayerPrefs.GetFloat(category, 0);
            categoryRevenue[category] = revenue;
        }
        return categoryRevenue;
    }

    public void SaveDataRevenue()
    {
        Dictionary<string, float> categoryRevenue = new Dictionary<string, float>();
        foreach (string category in ListNameCategoryRevenue)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;
            string revenueKey = $"{category.Trim()}-{day:D2}-{month:D2}-{year}";
            float revenue = PlayerPrefs.GetFloat(revenueKey, 0);
            categoryRevenue[category] = revenue;
        }
        DataManagerScript.Instance.SetCategoryRevenueData(colorCategoryRevenue, categoryRevenue);
    }
    public void SaveColorCategoryRevenue()
    {
        string json = JsonUtility.ToJson(new Serialization<string, string>(colorCategoryRevenue));
        PlayerPrefs.SetString("ColorCategoriesRevenue", json);
        PlayerPrefs.Save();
    }

    public void LoadColorCategoryRevenue()
    {
        if (PlayerPrefs.HasKey("ColorCategoriesRevenue"))
        {
            string json = PlayerPrefs.GetString("CategoriesRevenue");
            colorCategoryRevenue = JsonUtility.FromJson<Serialization<string,string>>(json).ToDictionary();
        }
        if (colorCategoryRevenue == null)
        {
            colorCategoryRevenue = new Dictionary<string, string>();
        }
    }
    private void AddLongPressDeleteButton(Button button)
    {
        UnityEngine.EventSystems.EventTrigger eventTrigger = button.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
        UnityEngine.EventSystems.EventTrigger.Entry entry = new UnityEngine.EventSystems.EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { StartCoroutine(LongPressRoutine(button)); });
        eventTrigger.triggers.Add(entry);
    }
    private IEnumerator LongPressRoutine(Button button)
    {
        yield return new WaitForSeconds(5f);
        buttonToDelete = button;
        deleteConfirmPanel.SetActive(true);
    }
    private void ConfirmDelete()
    {
        if ( buttonToDelete != null)
        {
            Destroy(buttonToDelete.gameObject);
            deleteConfirmPanel.SetActive(false);
            buttonToDelete = null;
        }   
    }
}


