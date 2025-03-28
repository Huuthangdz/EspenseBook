using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using System;

public class DateManageSpendingMoney : MonoBehaviour
{
    public TextMeshProUGUI date;

    private int day;
    private int month;
    private int year;

    private int maxDay = 30;
    private int maxMonth = 12;

    // Start is called before the first frame update
    void Start()
    {
        day = System.DateTime.Now.Day;
        month = System.DateTime.Now.Month;
        year = System.DateTime.Now.Year;
    }

    // Update is called once per frame
    void Update()
    {
        if (day > maxDay)
        {
            day = 1;
            month++;
            if (month > maxMonth)
            {
                month = 1;
                year++;
            }
        }
        SetTimeNow();
    }
    void SetTimeNow()
    {
        date.text = day + "/" + month + "/" + year; 
    }
}
