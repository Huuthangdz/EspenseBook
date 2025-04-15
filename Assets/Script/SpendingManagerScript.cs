using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpendingManagerScript : InputData
{
    public GameObject warningPressMoney;
    public Button okPressMoney;
    public GameObject plusCategoryPanel;
    public Button PlusCategoryMoreButton;
    public Button CancelPlusCategoryButton;

    public GameObject warningPanelNecessary;
    public Button okPressNecessary;

    public GameObject warningPanelNegativeMoney;
    public Button okPressNegativeMoney;

    public GameObject deleteConfirmationPanel;
    public Button confimDeleteButton;
    public Button cancelDeleteButton;
    private Button buttonToDelete;

    //public GameObject DeletePanelCategories;
    //public Button okPressDeleteCategories;
    //public Button CacelPressDeleteCategories;

    private bool isNecessaryCurrent;
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
            AddLongPressListener(button);
        }
        submitButton.onClick.AddListener(() => SubmitData());
        LoadCategoriesSpending();
        SetCategorySpendingColors();
        SaveColorCategorySpending();
        SaveDataSpending();

        okPressMoney.onClick.AddListener(() => warningPressMoney.SetActive(false));
        PlusCategoryMoreButton.onClick.AddListener(() => plusCategoryPanel.SetActive(true));
        CancelPlusCategoryButton.onClick.AddListener(() => plusCategoryPanel.SetActive(false));
        okPressNecessary.onClick.AddListener(() => warningPanelNecessary.SetActive(false));
        okPressNegativeMoney.onClick.AddListener(() => warningPanelNegativeMoney.SetActive(false));

        cancelDeleteButton.onClick.AddListener(() => deleteConfirmationPanel.SetActive(false));
        confimDeleteButton.onClick.AddListener(ConfirmDelete);
    }
    private void Update()
    {
        float totalSpending = SetTotalAmountSpendingOfMonth(System.DateTime.Now.Month, System.DateTime.Now.Year);
        PlayerPrefs.SetFloat("TotalSpendingText", totalSpending);
    }   

    public override void SetDataCategory(Button button)
    {
        base.SetDataCategory(button);
        NecessaryCategory necessaryCategory = button.GetComponent<NecessaryCategory>(); 
        if (necessaryCategory != null)
        {
            isNecessaryCurrent = necessaryCategory.isNecessary;
        }
    }
    public override void SubmitData()
    {
        base.SubmitData();
        LoadColorCategorySpending();
        int day = dateTime.Day;
        int month = dateTime.Month;
        int year = dateTime.Year;

        if (!float.TryParse(moneyInput.text, out float Amount))
        {
            warningPressMoney.SetActive(true);
            return;
        }

        if(Amount <= 0)
        {
            warningPanelNegativeMoney.SetActive(true);
            return;
        }
        string newCategory = nameCategory.text;

        if (!isNecessaryCurrent)
        {
            warningPanelNecessary.SetActive(true);
        }

        if (!string.IsNullOrEmpty(newCategory) && !ListNameCategorySpending.Contains(newCategory))
        {
            ListNameCategorySpending.Add(newCategory);
            SaveCategoriesSpending();

            if (!colorCategoriSpending.ContainsKey(newCategory))
            {
                Color textColor = nameCategory.color;
                string hexColor = ColorUtility.ToHtmlStringRGB(textColor);
                colorCategoriSpending[newCategory] = hexColor;
                SaveColorCategorySpending();
            }
        }
        String getSpendingCategoryOfMonth = $"{newCategory.Trim()}-{day:D2}-{month:D2}-{year}";
        float currentAmount = PlayerPrefs.GetFloat(getSpendingCategoryOfMonth, 0);

        PlayerPrefs.SetFloat(getSpendingCategoryOfMonth, currentAmount + Amount);
        PlayerPrefs.Save();
        moneyInput.text = String.Empty;
        SetCategorySpendingColors();
        SaveColorCategorySpending();
        SaveDataSpending();
    }

    public void SetCategorySpendingColors()
    {
        foreach (Button button in buttonsCategory)
        {
            string categoryName = button.transform.Find("Name").GetComponent<TMP_Text>().text;
            if (ListNameCategorySpending.Contains(categoryName))
            {
                Color categoryColor = button.transform.Find("Name").GetComponent<TMP_Text>().color;
                String hexColor = ColorUtility.ToHtmlStringRGB(categoryColor);
                colorCategoriSpending[categoryName] = hexColor;
            }
        }
    }

    public Dictionary<string, string> colorsCategoriSpendingDictionary()
    {
        return colorCategoriSpending;
    }

    public Dictionary<string, float> GetCategorySpending()
    {
        Dictionary<string, float> categorySpending = new Dictionary<string, float>();
        foreach (string category in ListNameCategorySpending)
        {
            float spending = PlayerPrefs.GetFloat(category, 0);
            categorySpending[category] = spending;
        }
        return categorySpending;
    }

    public void SaveDataSpending()
    {
        Dictionary<string, float> categorySpending = new Dictionary<string, float>();
        foreach (string category in ListNameCategorySpending)
        {
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;
            string spendingKey = $"{category.Trim()}-{day:D2}-{month:D2}-{year}";
            float spending = PlayerPrefs.GetFloat(spendingKey, 0);
            categorySpending[category] = spending;
        }
        DataManagerScript.Instance.SetCategorySpendingData(colorCategoriSpending, categorySpending);
    }

    public void SaveColorCategorySpending()
    {
        string json = JsonUtility.ToJson(new Serialization<string, string>(colorCategoriSpending));
        PlayerPrefs.SetString("ColorCategoriesSpending", json);
        PlayerPrefs.Save();
    }

    public void LoadColorCategorySpending()
    {
        if (PlayerPrefs.HasKey("ColorCategoriesSpending"))
        {
            string json = PlayerPrefs.GetString("CategoriesSpending");
            colorCategoriSpending = JsonUtility.FromJson<Serialization<string, string>>(json).ToDictionary();
        }
        if (colorCategoriSpending == null)
        {
            colorCategoriSpending = new Dictionary<string, string>();
        }
    }

    private void AddLongPressListener(Button button)
    {
        UnityEngine.EventSystems.EventTrigger eventTrigger = button.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();

        UnityEngine.EventSystems.EventTrigger.Entry pointerDownEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
        pointerDownEntry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((eventData) => { StartCoroutine(LongPressRoutine(button)); });
        eventTrigger.triggers.Add(pointerDownEntry);

        UnityEngine.EventSystems.EventTrigger.Entry pointerUpEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
        pointerUpEntry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((eventData) => { StopAllCoroutines(); });
        eventTrigger.triggers.Add(pointerUpEntry);
    }
    private IEnumerator LongPressRoutine(Button button)
    {
        yield return new WaitForSeconds(5f);
        buttonToDelete = button;
        deleteConfirmationPanel.SetActive(true);
    }
    private void ConfirmDelete()
    {
        if(buttonToDelete != null)
        {
            Destroy(buttonToDelete.gameObject);
            deleteConfirmationPanel.SetActive(false);
            buttonToDelete = null;
        }
    }
}

