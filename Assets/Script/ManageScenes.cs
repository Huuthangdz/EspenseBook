using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScenes : MonoBehaviour
{
public void ButtonSpending()
    {
        SceneManager.LoadScene("SpendingScenes");
    }

    public void ButtonRevenue()
    {
        SceneManager.LoadScene("RevenueScenes");
    }
    public void ButtonReport()
    {
        SceneManager.LoadScene("ReportScenes");
    }
    public void ButtonCalender()
    {
        SceneManager.LoadScene("CalenderScenes");
    }
}

