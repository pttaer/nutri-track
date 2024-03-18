using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using uPalette.Generated;
using uPalette.Runtime.Core;
using static UnityEngine.GraphicsBuffer;

[SerializeField]
class CalendarController : MonoBehaviour
{
    [SerializeField] GameObject m_CalendarPanel;
    [SerializeField] TextMeshProUGUI m_YearNumText;
    [SerializeField] TextMeshProUGUI m_MonthNumText;

    [SerializeField] Button BtnExit;
    [SerializeField] Button m_BtnFinish;

    [SerializeField] GameObject m_Item;

    [SerializeField] List<CalendarDateItem> m_DateItems = new List<CalendarDateItem>();
    const int _totalDateNum = 42;
    [SerializeField] float m_Width = 78.5f;
    [SerializeField] float m_Height = 48f;
    [SerializeField] TextMeshProUGUI m_Target;

    [SerializeField] float TWEEN_DURATION = 0.3f;
    [SerializeField] DateTime m_DateTime;
    private string m_DateFormat;
    private Action<bool> m_OnSelectCallback;

    public static CalendarController Api;

    private const string DEFAULT_DATE_FORMAT = "dd MMM yyyy";
    private const string DATE_DEFAULT = "Date";

    public void Init(TextMeshProUGUI txt, string dateFormat = null, bool isDisableAllDayAfterToday = false, Action<bool> onSelectCallback = null)
    {
        Api = this;
        m_OnSelectCallback = onSelectCallback;
        Vector3 startPos = m_Item.transform.localPosition;
        m_DateFormat = dateFormat;
        m_DateItems.Clear();
        m_DateItems.Add(m_Item.GetComponent<CalendarDateItem>());

        BtnExit = transform.Find("CalendarBG/BtnExit").GetComponent<Button>();
        m_BtnFinish = transform.Find("CalendarBG/BottomBar/BtnFinish").GetComponent<Button>();

        BtnExit.onClick.AddListener(ClosePopup);
        m_BtnFinish.onClick.AddListener(FinishPopup);

        BtnExit.gameObject.SetActive(false);
        m_BtnFinish.gameObject.SetActive(false);
        m_DateTime = DateTime.Now;
        GenerateCalendarItems(startPos);
        CreateCalendar();
        SetTargetTxt(txt);
    }

    private void GenerateCalendarItems(Vector3 startPos)
    {
        for (int i = 1; i < _totalDateNum; i++)
        {
            GameObject item = Instantiate(m_Item);
            item.name = "Item" + (i + 1).ToString();
            item.transform.SetParent(m_Item.transform.parent);
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = new Vector3((i % 7 * m_Width) + startPos.x, startPos.y - (i / 7 * m_Height), startPos.z);

            CalendarDateItem itemView = item.GetComponent<CalendarDateItem>();
            m_DateItems.Add(itemView);

            if ((i + 1) % 7 == 0)
            {
                itemView.m_IsSunday = true;
            }
        }
    }

    private void FinishPopup()
    {
        if (IsAtLeastOneChoosen())
        {
            SetAllItemsOff();
            ClosePopup();
        }
    }

    public bool IsAtLeastOneChoosen()
    {
        foreach (var item in m_DateItems)
        {
            if (item.m_IsChoosen)
            {
                return true;
            }
        }
        return false;
    }

    private void ClosePopup()
    {
        NTTControl.Api.ShowCalendarPopup(null, isClosePopup: true);
        gameObject.SetActive(false);
    }

    public void SetTargetTxt(TextMeshProUGUI txt, bool isShowPopup = false)
    {
        m_Target = txt;
    }

    public void ShowExitFinishPopup(bool isShowPopup = false)
    {
        // For calling popup
        if (isShowPopup)
        {
            BtnExit.gameObject.SetActive(true);
            m_BtnFinish.gameObject.SetActive(true);
        }
    }

    private void CreateCalendar()
    {
        SetAllItemsOff();
        ClearTargetText();

        DateTime firstDay = m_DateTime.AddDays(-(m_DateTime.Day));
        int index = GetDays(firstDay.DayOfWeek);

        var palette = PaletteStore.Instance.ColorPalette;
        var color = palette.GetActiveValue(ColorEntry.DateSelected.ToEntryId()).Value;

        int date = 0;
        firstDay = firstDay.AddDays(1);
        for (int i = 0; i < _totalDateNum; i++)
        {
            Text label = m_DateItems[i].GetComponentInChildren<Text>();
            m_DateItems[i].gameObject.SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    m_DateItems[i].gameObject.SetActive(true);

                    label.text = (date + 1).ToString();

                    m_DateItems[i].gameObject.name = $"{label.text} {thatDay.Month} {thatDay.Year}";

                    m_DateItems[i].EnableButton(thatDay < DateTime.Today.AddDays(1));

                    if (m_DateItems[i].m_IsSunday)
                    {
                        label.color = color;
                    }
                }
                date++;
            }
        }

        TweenUtils.TypingAnimation(m_YearNumText, m_DateTime.Year.ToString(), TWEEN_DURATION);
        TweenUtils.TypingAnimation(m_MonthNumText, m_DateTime.ToString("MMM"), TWEEN_DURATION);
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
        m_CalendarPanel.SetActive(true);
        m_CalendarPanel.transform.localScale = Vector3.zero;
        m_CalendarPanel.transform.DOScale(1, 0.3f).SetEase(Ease.OutExpo);
        m_Target = target;
        Debug.Log("TARGET IS: " + m_Target.name);
    }

    public void OnDateItemClick(string day)
    {
        SetAllItemsOff();
        Debug.Log("CLICK" + day);
        if (int.TryParse(day, out int dayInt) && int.TryParse(m_YearNumText.text, out int year))
        {
            DateTime date = new DateTime(year, MonthAbbreviationToNumber(m_MonthNumText.text), dayInt);
            TweenUtils.TypingAnimation(m_Target, date.ToString(m_DateFormat != null ? m_DateFormat : DEFAULT_DATE_FORMAT), TWEEN_DURATION);
            m_OnSelectCallback?.Invoke(true);

            //NTTMyHealthControl.Api.DateClickShowBMICaloriesValue(dayInt, month, year);
        }
        else
        {
            Debug.LogError("Invalid input for date.");
        }
        //_calendarPanel.SetActive(false);
    }

    public void ClearTargetText()
    {
        if (m_Target != null)
        {
            m_Target.text = DATE_DEFAULT;
            m_OnSelectCallback?.Invoke(false);
        }

        // TODO: May clear the others info here too
    }

    private void SetAllItemsOff()
    {
        int count = m_DateItems.Count;
        for (int i = 0; i < count; i++)
        {
            m_DateItems[i].SetItemOff();
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
