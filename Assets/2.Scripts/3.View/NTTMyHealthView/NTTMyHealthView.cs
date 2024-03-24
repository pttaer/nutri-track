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

    private GameObject m_PnlSummary;
    private GameObject m_PnlChooseRecordType;

    private NTTMyHealthPopupRecordDetailView m_PopupRecordDetail;
    private NTTMyHealthPopupInputBMIRecordView m_PopupInputBMIRecord;
    private NTTMyHealthPopupInputCalRecordView m_PopupInputCaloriesRecord;
    private GameObject m_BGFull;

    private Button m_BtnSummary;
    private Button m_BtnAddDailyRecord;
    private Button m_BtnBMIRecord;
    private Button m_BtnCalRecord;
    private Button m_BtnDetailRecord;

    private TextMeshProUGUI m_TxtBtnAddDailyRecord;
    private TextMeshProUGUI m_TxtDate;
    private TextMeshProUGUI m_TxtBMIValue;
    private TextMeshProUGUI m_TxtCaloriesValue;

    private float m_CurrentDayBMI;
    private NTTBMIRecordDTO m_CurrentBmiRecord;
    private NTTDailyCalDTO m_CurrentDayCalRecord;
    private List<NTTCalRecordDTO> m_CurrentCalRecordList = new List<NTTCalRecordDTO>();

    // Non-ref var
    private const string UNSUBMITTED = "Unsubmitted";

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        //NTTMyHealthControl.Api.OnDateClickShowBMICaloriesValueEvent -= SetBMICaloriesValue;
        NTTMyHealthControl.Api.OnClosePopupRecordEvent -= ClosePopupBackground;
    }

    private void Init()
    {
        // References
        Transform topBar = transform.Find("TopBar");
        Transform content = transform.Find("Body/Content");
        Transform popupRecordDetail = transform.Find("PopupRecordDetail");
        Transform pnlCalendar = content.Find("PnlCalendar");
        Transform pnlSummary = content.Find("PnlSummary");

        m_CalendarController = pnlCalendar.Find("CalendarControllerPreb").GetComponent<CalendarController>();

        m_PnlSummary = pnlSummary.gameObject;

        m_TxtBtnAddDailyRecord = pnlSummary.Find("TopBar/BtnAddDailyRecord/Text").GetComponent<TextMeshProUGUI>();
        m_TxtDate = pnlSummary.Find("TopBar/TxtDate").GetComponent<TextMeshProUGUI>();
        m_TxtBMIValue = pnlSummary.Find("BMI/Value").GetComponent<TextMeshProUGUI>();
        m_TxtCaloriesValue = pnlSummary.Find("Calories/Value").GetComponent<TextMeshProUGUI>();

        m_BtnAddDailyRecord = pnlSummary.Find("TopBar/BtnAddDailyRecord").GetComponent<Button>();
        m_BtnBMIRecord = content.Find("PnlChooseRecordType/BMI").GetComponent<Button>();
        m_BtnCalRecord = content.Find("PnlChooseRecordType/Calories").GetComponent<Button>();
        m_BtnDetailRecord = pnlSummary.Find("Background").GetComponent<Button>();

        m_PnlChooseRecordType = content.Find("PnlChooseRecordType").gameObject;
        m_PopupRecordDetail = popupRecordDetail.GetComponent<NTTMyHealthPopupRecordDetailView>();
        m_PopupInputBMIRecord = content.Find("PopupInputBMIRecord").GetComponent<NTTMyHealthPopupInputBMIRecordView>();
        m_PopupInputCaloriesRecord = content.Find("PopupInputCalRecord").GetComponent<NTTMyHealthPopupInputCalRecordView>();
        m_BGFull = content.Find("BGFull").gameObject;

        m_BtnSummary = pnlSummary.GetComponent<Button>();

        // Add listeners
        m_BtnSummary.onClick.AddListener(OnSummaryClick);
        m_BtnAddDailyRecord.onClick.AddListener(() => OnClickAddDailyRecord());
        m_BtnBMIRecord.onClick.AddListener(OnClickBtnBMIRecord);
        m_BtnCalRecord.onClick.AddListener(OnClickBtnCalRecord);
        m_BtnDetailRecord.onClick.AddListener(OnClickDetailRecord);

        // Register events
        //NTTMyHealthControl.Api.OnDateClickShowBMICaloriesValueEvent += SetBMICaloriesValue;
        NTTMyHealthControl.Api.OnClosePopupRecordEvent += ClosePopupBackground;

        // Default values
        m_TxtDate.text = DateTime.Now.ToString("dd MMM yyyy");
        m_TxtBMIValue.text = UNSUBMITTED;
        m_TxtCaloriesValue.text = UNSUBMITTED;

        m_PnlChooseRecordType.SetActive(false);
        m_PopupInputBMIRecord.gameObject.SetActive(false);
        m_PopupInputCaloriesRecord.gameObject.SetActive(false);
        m_BGFull.SetActive(false);

        m_PopupInputBMIRecord.Init(ReloadCalendarWithBMICaloriesData);
        m_PopupInputCaloriesRecord.Init(ReloadCalendarWithBMICaloriesData);

        m_BtnDetailRecord.interactable = false;

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
                        calRecordList: calRecordData.Results,
                        onDateSelectCallback: (isOn, isBMIExist, isCalRecordExist, date) =>
                        {
                            m_BtnAddDailyRecord.gameObject.SetActive(isOn);

                            ResetPnlChooseRecord();

                            m_BtnDetailRecord.interactable = isBMIExist || isCalRecordExist;

                            VerifyBMISetOnUI(bmiData, isBMIExist, date);

                            VerifyCalSetOnUI(dailyCalData, calRecordData, isCalRecordExist, date);
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
                date ?? DateTime.MinValue,
                dailyCalData.Results,
                calRecordData.Results,
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
                date ?? DateTime.MinValue,
                bmiData.Results,
                callbackExist: (itemData) =>
                {
                    float bmi = NTTMyHealthControl.Api.CurrentUserBMICalculate(itemData.Weight);
                    SetCurrentBMI(itemData, bmi);
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
        m_TxtCaloriesValue.text = $"{listCalRecord.Sum(item => item.Amount)} {NTTConstant.CALS}";
        m_CurrentCalRecordList = listCalRecord;
        m_CurrentDayCalRecord = dailyCal;
    }

    private void SetCurrentBMI(NTTBMIRecordDTO itemData, float bmi)
    {
        m_TxtBMIValue.text = $"{bmi} {NTTConstant.BMI_UNIT}";
        m_CurrentBmiRecord = itemData;
        m_CurrentDayBMI = bmi;
    }

    private void SetDefaultCal()
    {
        m_TxtCaloriesValue.text = UNSUBMITTED;
        m_CurrentCalRecordList.Clear();
        m_CurrentDayCalRecord = null;
    }

    private void SetDefaultBMI()
    {
        m_TxtBMIValue.text = UNSUBMITTED;
        m_CurrentBmiRecord = null;
        m_CurrentDayBMI = 0;
    }

    private void OnClickDetailRecord()
    {
        m_PopupRecordDetail.Init(m_CurrentDayBMI, m_CurrentBmiRecord, m_CurrentDayCalRecord, m_CurrentCalRecordList);
    }

    private void ClosePopupBackground()
    {
        m_BGFull.SetActive(false);
    }

    private void OnClickBtnCalRecord()
    {
        if (DateTime.TryParse(m_TxtDate.text, out DateTime result))
        {
            ShowPopupInputCaloriesRecord(true, result);
        }
    }

    private void OnClickBtnBMIRecord()
    {
        if (DateTime.TryParse(m_TxtDate.text, out DateTime result))
        {
            ShowPopupInputBMIRecord(true, result);
        }
    }

    private void OnClickAddDailyRecord()
    {
        bool isPnlChooseOn = m_PnlChooseRecordType.activeSelf;

        ShowPnlChooseRecordType(!isPnlChooseOn);

        m_TxtBtnAddDailyRecord.text = isPnlChooseOn ? "Add daily record" : "Close";
    }

    private void ResetPnlChooseRecord()
    {
        ShowPnlChooseRecordType(false);
        m_TxtBtnAddDailyRecord.text = "Add daily record";
    }

    private void SetBMICaloriesValue(float bmi, float calo)
    {
        return;

        m_TxtBMIValue.text = bmi.ToString();
        m_TxtCaloriesValue.text = calo.ToString();

        // NOTE: Take user data in here to init for the popup summary here
    }

    private void OnSummaryClick()
    {
        // TODO: Show popup summary
    }

    private void ShowPnlChooseRecordType(bool isOn)
    {
        m_PnlChooseRecordType.SetActive(isOn);
    }

    private void ShowPopupInputBMIRecord(bool isOn, DateTime currentDate)
    {
        m_PopupInputBMIRecord.gameObject.SetActive(isOn);
        m_PopupInputBMIRecord.SetCurrentDate(currentDate, m_CurrentBmiRecord);
        m_BGFull.SetActive(isOn);
    }

    private void ShowPopupInputCaloriesRecord(bool isOn, DateTime currentDate)
    {
        m_PopupInputCaloriesRecord.gameObject.SetActive(isOn);
        m_PopupInputCaloriesRecord.SetCurrentDate(currentDate);
        m_BGFull.SetActive(isOn);
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
        StartCoroutine(NTTApiControl.Api.GetListData<NTTCalRecordDTO>(string.Format(NTTConstant.CAL_RECORDS_ROUTE, NTTConstant.PARAM_DATE), (calRecordData) =>
        {
            callback?.Invoke(calRecordData);
        }));
    }
}
