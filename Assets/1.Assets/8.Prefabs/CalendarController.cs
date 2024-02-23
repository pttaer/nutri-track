using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarController : MonoBehaviour
{
    public GameObject _calendarPanel;
    public Text _yearNumText;
    public Text _monthNumText;

    public GameObject _item;

    public List<CalendarDateItem> _dateItems = new List<CalendarDateItem>();
    const int _totalDateNum = 42;
    public float m_Width = 78.5f;
    public float m_Height = 48f;
    public TextMeshProUGUI _target;

    private DateTime m_DateTime;
    public static CalendarController Api;

    void Start()
    {
        Api = this;
        Vector3 startPos = _item.transform.localPosition;
        _dateItems.Clear();
        _dateItems.Add(_item.GetComponent<CalendarDateItem>());

        for (int i = 1; i < _totalDateNum; i++)
        {
            GameObject item = Instantiate(_item);
            item.name = "Item" + (i + 1).ToString();
            item.transform.SetParent(_item.transform.parent);
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = new Vector3((i % 7 * m_Width) + startPos.x, startPos.y - (i / 7 * m_Height), startPos.z);

            _dateItems.Add(item.GetComponent<CalendarDateItem>());
        }

        m_DateTime = DateTime.Now;

        CreateCalendar();

        //_calendarPanel.SetActive(false);
    }

    void CreateCalendar()
    {
        SetAllItemsOff();

        DateTime firstDay = m_DateTime.AddDays(-(m_DateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;
        for (int i = 0; i < _totalDateNum; i++)
        {
            Text label = _dateItems[i].GetComponentInChildren<Text>();
            _dateItems[i].gameObject.SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].gameObject.SetActive(true);

                    label.text = (date + 1).ToString();
                    date++;
                }
            }
        }
        _yearNumText.text = m_DateTime.Year.ToString();
        _monthNumText.text = m_DateTime.ToString("MMM");
    }

    int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }
    public void YearPrev()
    {
        m_DateTime = m_DateTime.AddYears(-1);
        CreateCalendar();
    }

    public void YearNext()
    {
        m_DateTime = m_DateTime.AddYears(1);
        CreateCalendar();
    }

    public void MonthPrev()
    {
        m_DateTime = m_DateTime.AddMonths(-1);
        CreateCalendar();
    }

    public void MonthNext()
    {
        m_DateTime = m_DateTime.AddMonths(1);
        CreateCalendar();
    }

    public void ShowCalendar(TextMeshProUGUI target)
    {
        _calendarPanel.SetActive(true);
        _calendarPanel.transform.localScale = Vector3.zero;
        _calendarPanel.transform.DOScale(1, 0.3f).SetEase(Ease.OutExpo);
        _target = target;
        Debug.Log("TARGET IS: " + _target.name);
    }

    public void OnDateItemClick(string day)
    {
        SetAllItemsOff();
        Debug.Log("CLICK" + day);
        if (int.TryParse(day, out int dayInt) && 
            int.TryParse(_yearNumText.text, out int year))
        {
            DateTime date = new DateTime(year, MonthAbbreviationToNumber(_monthNumText.text), dayInt);
            _target.text = date.ToString("dd MMM yyyy");

            //NTTMyHealthControl.Api.DateClickShowBMICaloriesValue(dayInt, month, year);

        }
        else
        {
            Debug.LogError("Invalid input for date.");
        }
        //_calendarPanel.SetActive(false);
    }

    private void SetAllItemsOff()
    {
        int count = _dateItems.Count;
        for (int i = 0; i < count; i++)
        {
            _dateItems[i].SetItemOff();
        }
    }

    public int MonthAbbreviationToNumber(string monthAbbreviation)
    {
        switch (monthAbbreviation.ToUpper())
        {
            case "JAN": return 1;
            case "FEB": return 2;
            case "MAR": return 3;
            case "APR": return 4;
            case "MAY": return 5;
            case "JUN": return 6;
            case "JUL": return 7;
            case "AUG": return 8;
            case "SEP": return 9;
            case "OCT": return 10;
            case "NOV": return 11;
            case "DEC": return 12;
            default:
                Debug.LogError("Invalid month abbreviation: " + monthAbbreviation);
                return -1; // or throw an exception, depending on your requirements
        }
    }
}
