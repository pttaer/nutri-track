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

    private NTTBMIRecordDTO m_CurrentBmiRecord;
    private float m_CurrentDayBMI;
    private NTTDailyCalDTO m_CurrentDayCalRecord;
    private List<NTTCalRecordDTO> m_CurrentCalRecordList = new List<NTTCalRecordDTO>();

    // Non-ref var
    private const string DEFAULT_VALUE = "Unsubmitted";

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
        m_TxtBMIValue.text = DEFAULT_VALUE;
        m_TxtCaloriesValue.text = DEFAULT_VALUE;

        m_PnlChooseRecordType.SetActive(false);
        m_PopupInputBMIRecord.gameObject.SetActive(false);
        m_PopupInputCaloriesRecord.gameObject.SetActive(false);
        m_BGFull.SetActive(false);

        m_PopupInputBMIRecord.Init();
        m_PopupInputCaloriesRecord.Init();

        m_BtnDetailRecord.interactable = false;

        StartCoroutine(NTTApiControl.Api.GetListData<NTTBMIRecordDTO>(string.Format(NTTConstant.BMI_RECORDS_ROUTE_GET_ALL_SORT_FORMAT, NTTConstant.PARAM_DATE), (bmiData, result) =>
        {
            if (result == UnityWebRequest.Result.Success)
            {
                StartCoroutine(NTTApiControl.Api.GetListData<NTTDailyCalDTO>(string.Format(NTTConstant.DAILY_CAL_ROUTE_GET_ALL_SORT_FORMAT, NTTConstant.PARAM_DATE), (dailyCalData, result) =>
                {
                    StartCoroutine(NTTApiControl.Api.GetListData<NTTCalRecordDTO>(string.Format(NTTConstant.CAL_RECORDS_ROUTE, NTTConstant.PARAM_DATE), (calRecordData, result) =>
                    {
                        NTTControl.Api.ShowLoading();
                        if (result == UnityWebRequest.Result.Success)
                        {
                            m_CalendarController.Init(m_TxtDate, null, isDisableAllDayAfterToday: true, bmiRecordsList: bmiData.Results, dailyCalList: dailyCalData.Results, calRecordList: calRecordData.Results, onSelectCallback: (isOn, isBMIExist, isCalRecordExist, date) =>
                            {
                                Debug.Log("Run here " + isOn);
                                m_BtnAddDailyRecord.gameObject.SetActive(isOn);
                                OnClickAddDailyRecord(isForceReset: true);

                                m_BtnDetailRecord.interactable = isBMIExist || isCalRecordExist;

                                if (isBMIExist)
                                {
                                    NTTMyHealthControl.Api.CheckExistItemByDateInListBMI(
                                        date ?? DateTime.MinValue,
                                        bmiData.Results,
                                        callbackExist: (itemData) =>
                                        {
                                            // kg/height^2 (height is in meter)

                                            float bmi = itemData.Weight / (float)Math.Pow(NTTModel.CurrentUser.User.Height / 100, 2);
                                            float roundedBMI = (float)Math.Ceiling(bmi * 10) / 10;
                                            m_TxtBMIValue.text = $"{roundedBMI} {NTTConstant.BMI_UNIT}";
                                            m_CurrentBmiRecord = itemData;
                                            m_CurrentDayBMI = roundedBMI;
                                        }
                                    );
                                }
                                else
                                {
                                    m_TxtBMIValue.text = DEFAULT_VALUE;
                                    m_CurrentBmiRecord = null;
                                    m_CurrentDayBMI = 0;
                                }
                                
                                if(isCalRecordExist)
                                {
                                    NTTMyHealthControl.Api.CheckExistItemByDateInListDailyCal(
                                        date ?? DateTime.MinValue,
                                        dailyCalData.Results,
                                        calRecordData.Results,
                                        callbackExist: (dailyCal, listCalRecord) =>
                                        {
                                            m_TxtCaloriesValue.text = $"{listCalRecord.Sum(item => item.Amount)} {NTTConstant.CALS}";
                                            m_CurrentCalRecordList = listCalRecord;
                                            m_CurrentDayCalRecord = dailyCal;
                                        }
                                    );
                                }
                                else
                                {
                                    m_TxtCaloriesValue.text = DEFAULT_VALUE;
                                    m_CurrentCalRecordList.Clear();
                                    m_CurrentDayCalRecord = null;
                                }
                            });
                        }
                        NTTControl.Api.HideLoading();
                    }));
                }));
            }
        }));
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
        ShowPopupInputCaloriesRecord(true);
    }

    private void OnClickBtnBMIRecord()
    {
        ShowPopupInputBMIRecord(true);
    }

    private void OnClickAddDailyRecord(bool isForceReset = false)
    {
        bool isPnlChooseOn = m_PnlChooseRecordType.activeSelf;

        if (isForceReset)
        {
            ShowPnlChooseRecordType(false);
        }
        else
        {
            ShowPnlChooseRecordType(!isPnlChooseOn);
        }

        m_TxtBtnAddDailyRecord.text = isPnlChooseOn || isForceReset ? "Add daily record" : "Close";
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

    private void ShowPopupInputBMIRecord(bool isOn)
    {
        m_PopupInputBMIRecord.gameObject.SetActive(isOn);
        m_BGFull.SetActive(isOn);
    }

    private void ShowPopupInputCaloriesRecord(bool isOn)
    {
        m_PopupInputCaloriesRecord.gameObject.SetActive(isOn);
        m_BGFull.SetActive(isOn);
    }
}
