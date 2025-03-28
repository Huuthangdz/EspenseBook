using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CalendarManageScript : MonoBehaviour
{
    public TextMeshProUGUI textMonth;
    public GameObject dayCellPrefabs;
    public Transform gridCalendar;
    public Button previousMonth;
    public Button nextMonth;
    public TextMeshProUGUI spendingTotalMonthText;
    public TextMeshProUGUI revenueTotalMonthText;
    public TextMeshProUGUI totalExpenseText;

    private DateTime currentDate; //ngày hiển thị 
    private float totalSpendingOfMonth = 0;
    private float totalRevenueOfMonth = 0;
    private float totalExpense = 0;
    private String CurrentMonth;
    private List<String> ListCategorySpendingName = new List<String>();
    private List<String> ListCategoryRevenueName = new List<String>();


    // Start is called before the first frame update
    void Start()
    {
        currentDate = System.DateTime.Now;
        GenerateCalendar();

        previousMonth.onClick.AddListener(PreviousMonth);
        nextMonth.onClick.AddListener(NextMonth);
        
        CurrentMonth = currentDate.ToString("MM-yyyy");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("CurrentMonth", currentDate.Month);
        PlayerPrefs.SetFloat("CurrentYear", currentDate.Year);

        if (PlayerPrefs.HasKey("CategoriesSpending"))
        {
            string json = PlayerPrefs.GetString("CategoriesSpending");
            ListCategorySpendingName = JsonUtility.FromJson<Serialization<string>>(json).target;
        }

        if (PlayerPrefs.HasKey("CategoriesRevenue"))
        {
            String json = PlayerPrefs.GetString("CategoriesRevenue");
            ListCategoryRevenueName = JsonUtility.FromJson<Serialization<string>>(json).target;
        }

        UpdateTextExpenseOfMonth(currentDate.Month, currentDate.Year);
    }
    private void GenerateCalendar()
    {

        // set text month
        int totalDayInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
        textMonth.text = currentDate.ToString("MM yyyy") + " (01/" + currentDate.ToString("MM") + " - " + totalDayInMonth + "/" + currentDate.ToString("MM") + ")";

        // get information of month
        DateTime firstDay = new DateTime(currentDate.Year, currentDate.Month, 1);
        int startDayOfWeek = (int)firstDay.DayOfWeek;

        // get previous month 
        DateTime previousMonth = firstDay.AddDays(-startDayOfWeek);
        for (int i = 0; i < startDayOfWeek; i++)
        {
            CreateDayCell(previousMonth.Day, false);
            previousMonth = previousMonth.AddDays(1);
        }


        //get day in month
        DateTime today = System.DateTime.Now;
        for (int day = 1; day <= totalDayInMonth; day++)
        {
            GameObject obj = CreateDayCell(day, true);

            int weekendDay = (startDayOfWeek + day - 1) % 7;
            TMP_Text dayText = obj.transform.Find("DayNumber").GetComponent<TMP_Text>();
            if (weekendDay == 6)
            {
                obj.transform.Find("DayNumber").GetComponent<TextMeshProUGUI>().color = Color.blue;
            }
            if (weekendDay == 0)
            {
                obj.transform.Find("DayNumber").GetComponent<TextMeshProUGUI>().color = Color.red;
            }
            if (today.Year == currentDate.Year && today.Month == currentDate.Month && today.Day == day)
            {
                Image dayBackground = obj.transform.Find("DayBackground").GetComponent<Image>();
                dayBackground.color = Color.green;
            }

        }
        //get next month 
        int totalCell = 42;
        DateTime nextMonth = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1);
        for (int i = 0; i < totalCell - totalDayInMonth - startDayOfWeek; i++)
        {
            CreateDayCell(nextMonth.Day, false);
            nextMonth = nextMonth.AddDays(1);
        }
    }

    private GameObject CreateDayCell(int day, bool isCurrentDay)
    {
        GameObject dayCell = Instantiate(dayCellPrefabs, gridCalendar);
        TextMeshProUGUI dayText = dayCell.transform.Find("DayNumber").GetComponent<TextMeshProUGUI>();
        dayText.text = day.ToString();
        Image dayBackground = dayCell.transform.Find("DayBackground").GetComponent<Image>();

        if (!isCurrentDay)
        {
            dayText.color = Color.black;
            dayBackground.color = Color.gray;
        }
        return dayCell;
    }

    public void NextMonth()
    {
        updateMonth();
        currentDate = currentDate.AddMonths(1);
        GenerateCalendar();
        UpdateTextExpenseOfMonth(currentDate.Month, currentDate.Year);

    }

    public void PreviousMonth()
    {
        updateMonth();
        currentDate = currentDate.AddMonths(-1);
        GenerateCalendar();
        UpdateTextExpenseOfMonth(currentDate.Month, currentDate.Year);

    }

    public void updateMonth()
    {
        foreach (Transform child in gridCalendar)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateTextExpenseOfMonth(int Month, int Year)
    {
        totalSpendingOfMonth = GetTotalSpending(Month, Year);
        totalRevenueOfMonth = GetTotalRevenue(Month, Year);
        totalExpense = totalRevenueOfMonth - totalSpendingOfMonth;
        spendingTotalMonthText.text = totalSpendingOfMonth.ToString("N0") + " VND";
        revenueTotalMonthText.text = totalRevenueOfMonth.ToString("N0") + " VND";
        totalExpenseText.text = totalExpense.ToString("N0") + " VND";
    }

    private float GetTotalSpending(int month, int Year)
    {

        float totalAmount = 0;

        if (ListCategorySpendingName != null)
        {
            foreach (string tag in ListCategorySpendingName)
            {
                String getSpendingCategoryOfMonth = tag + "-" + month.ToString("D2") + "-" + Year;
                totalAmount += PlayerPrefs.GetFloat(getSpendingCategoryOfMonth, 0);
            }
        }
        return totalAmount;
    }

    private float GetTotalRevenue(int month, int Year)
    {

        float totalAmount = 0;

        if (ListCategoryRevenueName != null)
        {
            foreach (string tag in ListCategoryRevenueName)
            {
                String getRevenueCategoryOfMonth = tag + "-" + month.ToString("D2") + "-" + Year;
                totalAmount += PlayerPrefs.GetFloat(getRevenueCategoryOfMonth, 0);
            }
        }
        return totalAmount;
    }
}
