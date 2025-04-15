using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasePlusRevenueScript : PlusCategory
{
    public RevenueManageScript revenueManagerScript;
    public GameObject warningPanelNegativeMoney;
    public Button okPressNegativeMoney;
    // Start is called before the first frame update
    void Start()
    {
        if (submitButtonNewCategory != null)
        {
            submitButtonNewCategory.onClick.AddListener(() => SubmitPlusNewCategory());
        }
        okPressNegativeMoney.onClick.AddListener(() => warningPanelNegativeMoney.SetActive(false));
    }

    public override void SubmitPlusNewCategory()
    {
        base.SubmitPlusNewCategory();

        if (string.IsNullOrEmpty(CategoryNameInput.text))
        {
            warningPanelNegativeMoney.SetActive(true);
            return;
        }
        else
        {
            GameObject newItem = Instantiate(prefabsCategoryButton, parentCategoryButton);

            TMP_Text categoryNameText = newItem.GetComponentInChildren<TMP_Text>();
            categoryNameText.text = CategoryNameInput.text;

            categoryNameText.color = new Color(Random.value, Random.value, Random.value);

            NecessaryCategory necessaryCategory = newItem.GetComponent<NecessaryCategory>();
            necessaryCategory.isNecessary = IsNecessaryCategory;

            int siblingIndex = parentCategoryButton.childCount - 2;
            newItem.transform.SetSiblingIndex(siblingIndex);

            CategoryNameInput.text = string.Empty;

            Button newButton = newItem.GetComponent<Button>();

            base.SubmitPlusNewCategory();
            if (newButton != null && revenueManagerScript != null)
            {
                List<Button> buttonList = new List<Button>(revenueManagerScript.buttonsCategory);
                buttonList.Add(newButton);
                revenueManagerScript.buttonsCategory = buttonList.ToArray();

                revenueManagerScript.SaveCategoryRevenue();

                newButton.onClick.AddListener(() => revenueManagerScript.SetDataCategory(newButton));
            }
        }
    }
}
