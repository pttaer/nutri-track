using Newtonsoft.Json;
using System;
using TMPro;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class NTTMyHealthView : MonoBehaviour
{
    private CalendarController m_CalendarController;

    private GameObject m_PnlChooseRecordType;
    private GameObject m_BGPopupAddDailyRecord;
    private GameObject m_PnlSummaryBottomBar;

    private NTTMyHealthPopupRecordDetailView m_PopupRecordDetail;
    private NTTMyHealthPopupInputBMIRecordView m_PopupInputBMIRecord;
    private NTTMyHealthPopupInputCalRecordView m_PopupInputCaloriesRecord;

    private Button m_BtnAddDailyRecord;
    private Button m_BtnAddBMI;
    private Button m_BtnAddCal;
    private Button m_BtnRecordDetail;

    private TextMeshProUGUI m_TxtBtnAddDailyRecord;
    private TextMeshProUGUI m_TxtDate;
    private TextMeshProUGUI m_TxtBMIValue;
    private TextMeshProUGUI m_TxtCaloriesValue;

    private float m_CurrentDayBMI;
    private NTTBMIRecordDTO m_CurrentBmiRecord;
    private NTTDailyCalDTO m_CurrentDayCalRecord;
    private List<NTTCalRecordDTO> m_CurrentCalRecordList = new List<NTTCalRecordDTO>();

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        NTTMyHealthControl.Api.OnClosePopupRecordEvent -= ClosePopupBackground;
    }

    private void Init()
    {
        // References
        Transform content = transform.Find("Body/Content");
        Transform popupRecordDetail = transform.Find("PopupRecordDetail");
        Transform pnlCalendar = content.Find("PnlCalendar");
        Transform pnlSummary = content.Find("PnlSummary");

        m_CalendarController = pnlCalendar.Find("CalendarControllerPreb").GetComponent<CalendarController>();

        m_PnlChooseRecordType = pnlSummary.Find("PnlChooseRecordType").gameObject;
        m_BGPopupAddDailyRecord = content.Find("BGFull").gameObject;
        m_PnlSummaryBottomBar = pnlSummary.Find("BottomBar").gameObject;

        m_TxtBtnAddDailyRecord = pnlSummary.Find("TopBar/BtnAddDailyRecord/Text").GetComponent<TextMeshProUGUI>();
        m_TxtDate = pnlSummary.Find("TopBar/TxtDate").GetComponent<TextMeshProUGUI>();
        m_TxtBMIValue = pnlSummary.Find("Record/BMI/Value").GetComponent<TextMeshProUGUI>();
        m_TxtCaloriesValue = pnlSummary.Find("Record/Calories/Value").GetComponent<TextMeshProUGUI>();

        m_BtnAddDailyRecord = pnlSummary.Find("TopBar/BtnAddDailyRecord").GetComponent<Button>();
        m_BtnAddBMI = pnlSummary.Find("PnlChooseRecordType/BMI").GetComponent<Button>();
        m_BtnAddCal = pnlSummary.Find("PnlChooseRecordType/Calories").GetComponent<Button>();
        m_BtnRecordDetail = pnlSummary.Find("Background").GetComponent<Button>();

        m_PopupRecordDetail = popupRecordDetail.GetComponent<NTTMyHealthPopupRecordDetailView>();
        m_PopupInputBMIRecord = content.Find("PopupInputBMIRecord").GetComponent<NTTMyHealthPopupInputBMIRecordView>();
        m_PopupInputCaloriesRecord = content.Find("PopupInputCalRecord").GetComponent<NTTMyHealthPopupInputCalRecordView>();

        // Add listeners
        m_BtnAddDailyRecord.onClick.AddListener(OnClickAddDailyRecord);
        m_BtnAddBMI.onClick.AddListener(OnClickBtnBMIRecord);
        m_BtnAddCal.onClick.AddListener(OnClickBtnCalRecord);
        m_BtnRecordDetail.onClick.AddListener(OnClickDetailRecord);

        // Register events
        NTTMyHealthControl.Api.OnClosePopupRecordEvent += ClosePopupBackground;

        // Default values
        SetRecordValueText(m_TxtBMIValue);
        SetRecordValueText(m_TxtCaloriesValue);
        //m_TxtDate.text = DateTime.Now.ToString(NTTConstant.DATE_FORMAT_FULL_MAIN);

        m_PnlChooseRecordType.SetActive(false);
        m_PopupInputBMIRecord.gameObject.SetActive(false);
        m_PopupInputCaloriesRecord.gameObject.SetActive(false);
        m_BGPopupAddDailyRecord.SetActive(false);

        m_PopupInputBMIRecord.Init(ReloadCalendarWithBMICaloriesData);
        m_PopupInputCaloriesRecord.Init(ReloadCalendarWithBMICaloriesData);

        m_BtnRecordDetail.interactable = false;

        ReloadCalendarWithBMICaloriesData();
    }

    private void ReloadCalendarWithBMICaloriesData()
    {
        GetListBMIRecord((bmiData) =>
        {
            GetListDailyCal((dailyCalData) =>
            {
                GetListCalRecord((calRecordData) =>
                {
                    NTTControl.Api.ShowLoading();

                    m_CalendarController.Init(
                        targetUITxt: m_TxtDate,
                        dateFormat: null,
                        isMyHealth: true,
                        bmiRecordsList: bmiData.Results,
                        dailyCalList: dailyCalData.Results,
                        calRecordsList: calRecordData.Results,
                        onDateSelectCallback: (isOn, isBMIExist, isCalRecordExist, date) =>
                        {
                            m_BtnAddDailyRecord.gameObject.SetActive(isOn);

                            ResetPnlChooseRecord();

                            VerifyBMISetOnUI(bmiData, isBMIExist, date);

                            VerifyCalSetOnUI(dailyCalData, calRecordData, isCalRecordExist, date);

                            m_BtnRecordDetail.interactable = isBMIExist || isCalRecordExist;
                        });

                    NTTControl.Api.HideLoading();
                });
            });
        });
    }

    private void VerifyCalSetOnUI(NTTPageResultDTO<NTTDailyCalDTO> dailyCalData, NTTPageResultDTO<NTTCalRecordDTO> calRecordData, bool isCalRecordExist, DateTime? date)
    {
        if (isCalRecordExist)
        {
            NTTMyHealthControl.Api.CheckExistItemByDateInListDailyCal(
                itemDate: date ?? DateTime.MinValue,
                dailyCalList: dailyCalData.Results,
                calRecordList: calRecordData.Results,
                callbackExist: (dailyCal, listCalRecord) =>
                {
                    SetCurrentDailyCal(dailyCal, listCalRecord);
                }
            );
        }
        else
        {
            SetDefaultCal();
        }
    }

    private void VerifyBMISetOnUI(NTTPageResultDTO<NTTBMIRecordDTO> bmiData, bool isBMIExist, DateTime? date)
    {
        if (isBMIExist)
        {
            NTTMyHealthControl.Api.CheckExistItemByDateInListBMI(
                itemDate: date ?? DateTime.MinValue,
                bmiRecordList: bmiData.Results,
                callbackExist: (itemData) =>
                {
                    SetCurrentBMI(itemData, NTTMyHealthControl.Api.CurrentUserBMICalculate(itemData.Weight));
                }
            );
        }
        else
        {
            SetDefaultBMI();
        }
    }

    private void SetCurrentDailyCal(NTTDailyCalDTO dailyCal, List<NTTCalRecordDTO> listCalRecord)
    {
        SetRecordValueText(m_TxtCaloriesValue, $"{listCalRecord.Sum(item => item.Amount)} {NTTConstant.CALS}");
        m_CurrentCalRecordList = listCalRecord;
        m_CurrentDayCalRecord = dailyCal;
    }

    private void SetCurrentBMI(NTTBMIRecordDTO itemData, float bmi)
    {
        SetRecordValueText(m_TxtBMIValue, $"{bmi} {NTTConstant.BMI_UNIT}");
        m_CurrentBmiRecord = itemData;
        m_CurrentDayBMI = bmi;
    }

    private void SetDefaultCal()
    {
        SetRecordValueText(m_TxtCaloriesValue);
        m_CurrentCalRecordList.Clear();
        m_CurrentDayCalRecord = null;
    }

    private void SetDefaultBMI()
    {
        SetRecordValueText(m_TxtBMIValue);
        m_CurrentBmiRecord = null;
        m_CurrentDayBMI = 0;
    }

    private void OnClickDetailRecord()
    {
        m_PopupRecordDetail.Init(m_CurrentDayBMI, m_CurrentBmiRecord, m_CurrentDayCalRecord, m_CurrentCalRecordList);
    }

    private void ClosePopupBackground()
    {
        m_BGPopupAddDailyRecord.SetActive(false);
    }

    private void OnClickBtnCalRecord()
    {
        if (DateTime.TryParse(m_TxtDate.text, out DateTime result))
        {
            ShowPopupInputCaloriesRecord(result);
        }
    }

    private void OnClickBtnBMIRecord()
    {
        if (DateTime.TryParse(m_TxtDate.text, out DateTime result))
        {
            ShowPopupInputBMIRecord(result);
        }
    }

    private void OnClickAddDailyRecord()
    {
        bool isPnlChooseOn = m_PnlChooseRecordType.activeSelf;

        ShowPnlChooseRecordType(!isPnlChooseOn);

        m_TxtBtnAddDailyRecord.text = isPnlChooseOn ? NTTConstant.ADD_DAILY_RECORD : NTTConstant.CLOSE;
    }

    private void ResetPnlChooseRecord()
    {
        ShowPnlChooseRecordType(false);

        m_TxtBtnAddDailyRecord.text = NTTConstant.ADD_DAILY_RECORD;
    }

    private void ShowPnlChooseRecordType(bool isOn)
    {
        m_PnlChooseRecordType.SetActive(isOn);
    }

    private void ShowPopupInputBMIRecord(DateTime currentDate)
    {
        m_PopupInputBMIRecord.SetCurrentDate(currentDate, m_CurrentBmiRecord);
        m_BGPopupAddDailyRecord.SetActive(true);
    }

    private void ShowPopupInputCaloriesRecord(DateTime currentDate)
    {
        m_PopupInputCaloriesRecord.SetCurrentDate(currentDate, m_CurrentDayCalRecord);
        m_BGPopupAddDailyRecord.SetActive(true);
    }

    private void SetRecordValueText(TextMeshProUGUI txtRecordValue, string valueTxt = null)
    {
        if (valueTxt == null)
        {
            txtRecordValue.text = NTTConstant.UNSUBMITTED;
            txtRecordValue.color = NTTConstant.DISABLED_TEXT_COLOR;
            CheckSetBottomBarSummary();
            return;
        }
        txtRecordValue.text = valueTxt;
        txtRecordValue.color = Color.black;
        CheckSetBottomBarSummary();
    }

    private void CheckSetBottomBarSummary()
    {
        bool isRecordValueAnySubmitted = m_TxtCaloriesValue.text != NTTConstant.UNSUBMITTED || m_TxtBMIValue.text != NTTConstant.UNSUBMITTED;
        m_PnlSummaryBottomBar.SetActive(isRecordValueAnySubmitted);
    }

    public void GetListBMIRecord(Action<NTTPageResultDTO<NTTBMIRecordDTO>> callback = null)
    {
        StartCoroutine(NTTApiControl.Api.GetListData<NTTBMIRecordDTO>(string.Format(NTTConstant.BMI_RECORDS_ROUTE_GET_ALL_SORT_FORMAT, NTTConstant.PARAM_DATE), (bmiData) =>
        {
            callback?.Invoke(bmiData);
        }));
    }

    public void GetListDailyCal(Action<NTTPageResultDTO<NTTDailyCalDTO>> callback = null)
    {
        StartCoroutine(NTTApiControl.Api.GetListData<NTTDailyCalDTO>(string.Format(NTTConstant.DAILY_CAL_ROUTE_GET_ALL_SORT_FORMAT, NTTConstant.PARAM_DATE), (dailyCalData) =>
        {
            callback?.Invoke(dailyCalData);
        }));
    }

    public void GetListCalRecord(Action<NTTPageResultDTO<NTTCalRecordDTO>> callback = null)
    {
        StartCoroutine(NTTApiControl.Api.GetListData<NTTCalRecordDTO>(NTTConstant.CAL_RECORDS_ROUTE, (calRecordData) =>
        {
            callback?.Invoke(calRecordData);
        }));
    }
}
