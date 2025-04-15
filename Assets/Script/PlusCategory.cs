using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlusCategory : MonoBehaviour
{
    public GameObject prefabsCategoryButton; // prefab category button
    public Transform parentCategoryButton; // parent category button
    public Button submitButtonNewCategory;
    public Button IsNecessaryButton;
    public Button IsNotNecessaryButton;
    public TMP_InputField CategoryNameInput;
    public bool IsNecessaryCategory;

    // Start is called before the first frame update

    public virtual void SubmitPlusNewCategory()
    {
       
    }
}
