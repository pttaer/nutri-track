using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using uPalette.Generated;

[SerializeField]
class CalendarController : MonoBehaviour
{
    [SerializeField] GameObject m_CalendarPanel;

    // Date buttons:
    Button m_BtnMonthPrev;
    Button m_BtnMonthNext;
    Button m_BtnYearPrev;
    Button m_BtnYearNext;

    TextMeshProUGUI m_YearNumText;
    TextMeshProUGUI m_MonthNumText;

    Button m_BtnExit;
    Button m_BtnFinish;

    CalendarDateItem m_Item;

    [SerializeField] List<CalendarDateItem> m_DateItems = new List<CalendarDateItem>();

    const int _totalDateNum = 42;
    [SerializeField] float m_Width = 78.5f;
    [SerializeField] float m_Height = 48f;
    [SerializeField] TextMeshProUGUI m_Target;

    [SerializeField] float m_TweenDuration = 0.3f;
    [SerializeField] DateTime m_DateTime;

    private string m_DateFormat;
    private bool m_IsMyHealth;
    private Action<bool, bool, bool, DateTime?> m_OnSelectCallback;

    List<NTTBMIRecordDTO> m_BmiRecordsList = new List<NTTBMIRecordDTO>();
    List<NTTDailyCalDTO> m_DailyCalList = new List<NTTDailyCalDTO>();
    List<NTTCalRecordDTO> m_CalRecordList = new List<NTTCalRecordDTO>();

    public static CalendarController Api;

    private bool m_IsInited = false;

    private const string MONTH_FORMAT = "MMM";

    public void Init(TextMeshProUGUI targetUITxt, string dateFormat = null, bool isMyHealth = false, List<NTTBMIRecordDTO> bmiRecordsList = null, List<NTTDailyCalDTO> dailyCalList = null, List<NTTCalRecordDTO> calRecordsList = null, Action<bool, bool, bool, DateTime?> onDateSelectCallback = null)
    {
        m_BmiRecordsList = bmiRecordsList;
        m_DailyCalList = dailyCalList;
        m_CalRecordList = calRecordsList;
        m_IsMyHealth = isMyHealth;
        m_OnSelectCallback = onDateSelectCallback;

        if (!m_IsInited)
        {
            Api = this;

            Transform calendarBG = transform.Find("CalendarBG");
            Transform topbar = calendarBG.Find("TopBar");

            m_BtnMonthPrev = topbar.Find("BtnMonthPrev").GetComponent<Button>();
            m_BtnMonthNext = topbar.Find("BtnMonthNext").GetComponent<Button>();
            m_BtnYearPrev = topbar.Find("BtnYearPrev").GetComponent<Button>();
            m_BtnYearNext = topbar.Find("BtnYearNext").GetComponent<Button>();

            m_YearNumText = topbar.Find("Year").GetComponent<TextMeshProUGUI>();
            m_MonthNumText = topbar.Find("Month").GetComponent<TextMeshProUGUI>();

            m_Item = calendarBG.Find("Item").GetComponent<CalendarDateItem>();

            m_BtnExit = calendarBG.Find("BtnExit").GetComponent<Button>();
            m_BtnFinish = calendarBG.Find("BottomBar/BtnFinish").GetComponent<Button>();

            m_BtnMonthPrev.onClick.AddListener(MonthPrev);
            m_BtnMonthNext.onClick.AddListener(MonthNext);
            m_BtnYearPrev.onClick.AddListener(YearPrev);
            m_BtnYearNext.onClick.AddListener(YearNext);

            m_BtnExit.onClick.AddListener(ClosePopup);
            m_BtnFinish.onClick.AddListener(FinishPopup);

            m_DateFormat = dateFormat;
            m_DateItems.Clear();
            m_DateItems.Add(m_Item);

            m_BtnExit.gameObject.SetActive(false);
            m_BtnFinish.gameObject.SetActive(false);

            m_DateTime = DateTime.Now;

            GenerateCalendarItems(m_Item.transform.localPosition);

            m_IsInited = true;
        }

        CreateCalendar();
        m_Target = targetUITxt;
    }

    private void GenerateCalendarItems(Vector3 startPos)
    {
        for (int i = 1; i < _totalDateNum; i++)
        {
            CalendarDateItem itemView = Instantiate(m_Item.gameObject).GetComponent<CalendarDateItem>();
            itemView.transform.SetParent(m_Item.transform.parent);
            itemView.transform.localScale = Vector3.one;
            itemView.transform.localRotation = Quaternion.identity;
            itemView.transform.localPosition = new Vector3((i % 7 * m_Width) + startPos.x, startPos.y - (i / 7 * m_Height), startPos.z);

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

    public void ShowExitFinishPopup(bool isShowPopup = false)
    {
        if (isShowPopup)
        {
            m_BtnExit.gameObject.SetActive(true);
            m_BtnFinish.gameObject.SetActive(true);
        }
    }

    private void CreateCalendar()
    {
        SetAllItemsOff();
        ClearTargetText();

        DateTime firstDay = m_DateTime.AddDays(-m_DateTime.Day);
        DateTime tomorow = DateTime.Today.AddDays(1);

        int index = GetDays(firstDay.DayOfWeek);

        var sundayColor = NTTControl.Api.GetPaletteColorValue(ColorEntry.Primary);

        int date = 0;

        firstDay = firstDay.AddDays(1);

        for (int i = 0; i < _totalDateNum; i++)
        {
            CalendarDateItem itemView = m_DateItems[i];

            Text label = itemView.GetComponentInChildren<Text>();

            GameObject goItem = itemView.gameObject;

            goItem.SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                //Debug.Log("Run here thatDay: " + thatDay);

                if (thatDay.Month == firstDay.Month)
                {
                    goItem.SetActive(true);

                    goItem.name = $"{label.text} {thatDay.Month} {thatDay.Year}";

                    label.text = (date + 1).ToString();

                    if (itemView.m_IsSunday)
                    {
                        label.color = sundayColor;
                    }

                    if (m_IsMyHealth)
                    {
                        itemView.EnableButton(thatDay < tomorow);

                        itemView.EnableBMI(thatDay, m_BmiRecordsList?.FindAll(item => item.Date.Month == thatDay.Month));

                        itemView.EnableDailyCal(thatDay, m_DailyCalList?.FindAll(item => item.Date.Month == thatDay.Month), m_CalRecordList);
                    }
                }
                date++;
            }
        }

        TweenUtils.TypingAnimation(m_YearNumText, m_DateTime.Year.ToString(), m_TweenDuration);
        TweenUtils.TypingAnimation(m_MonthNumText, m_DateTime.ToString(MONTH_FORMAT), m_TweenDuration);
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

    public void OnDateItemClick(string day, bool isBmiExist, bool isCalRecordExist)
    {
        SetAllItemsOff();

        if (int.TryParse(day, out int dayInt) && int.TryParse(m_YearNumText.text, out int year))
        {
            DateTime date = new DateTime(year, MonthAbbreviationToNumber(m_MonthNumText.text), dayInt);

            DateTime isoStringFormatDate = DateTime.Parse(date.ToString(NTTConstant.DATE_FORMAT_ISO_STRING));

            TweenUtils.TypingAnimation(m_Target, date.ToString(m_DateFormat ?? NTTConstant.DATE_FORMAT_FULL_MAIN), m_TweenDuration);

            m_OnSelectCallback?.Invoke(true, isBmiExist, isCalRecordExist, isoStringFormatDate);
        }
        else
        {
            Debug.LogError("Invalid input for date.");
        }
    }

    public void ClearTargetText()
    {
        if (m_Target != null)
        {
            m_Target.text = NTTConstant.DATE;

            // DateTime.MinValue for default
            m_OnSelectCallback?.Invoke(false, false, false, null);
        }
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

    private int GetDays(DayOfWeek day)
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
}
