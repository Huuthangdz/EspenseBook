using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasePlusCategorySpending : PlusCategory
{
    public SpendingManagerScript spendingManagerScript;
    public GameObject warningPanelNegativeMoney;
    public Button okPressNegativeMoney;
    // Start is called before the first frame update
    void Start()
    {
        if (submitButtonNewCategory != null)
        {
            submitButtonNewCategory.onClick.AddListener(() => SubmitPlusNewCategory());
        }
        IsNotNecessaryButton.onClick.AddListener(() => IsNecessaryCategory = false);
        IsNecessaryButton.onClick.AddListener(() => IsNecessaryCategory = true);
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
            if (newButton != null && spendingManagerScript != null)
            {
                List<Button> buttonList = new List<Button>(spendingManagerScript.buttonsCategory);
                buttonList.Add(newButton);
                spendingManagerScript.buttonsCategory = buttonList.ToArray();

                spendingManagerScript.SaveCategoriesSpending();

                newButton.onClick.AddListener(() => spendingManagerScript.SetDataCategory(newButton));
            }
        }
    }
}
