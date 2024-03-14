using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NTTMyHealthView : MonoBehaviour
{
    private CalendarController m_CalendarController;

    private GameObject m_PnlSummary;

    private Button m_BtnSummary;

    private TextMeshProUGUI m_TxtDate;
    private TextMeshProUGUI m_TxtBMIValue;
    private TextMeshProUGUI m_TxtCaloriesValue;

    // Non-ref var
    private const string DEFAULT_VALUE = "Unsubmitted";

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        //NTTMyHealthControl.Api.OnDateClickShowBMICaloriesValueEvent -= SetBMICaloriesValue;
    }

    private void Init()
    {
        // References
        Transform topBar = transform.Find("TopBar");
        Transform pnlCalendar = transform.Find("Body/Content/PnlCalendar");
        Transform pnlSummary = transform.Find("Body/Content/PnlSummary");

        m_CalendarController = pnlCalendar.Find("CalendarControllerPreb").GetComponent<CalendarController>();

        m_PnlSummary = pnlSummary.gameObject;
        m_TxtDate = pnlSummary.Find("TxtDate").GetComponent<TextMeshProUGUI>();
        m_TxtBMIValue = pnlSummary.Find("BMI/Value").GetComponent<TextMeshProUGUI>();
        m_TxtCaloriesValue = pnlSummary.Find("Calories/Value").GetComponent<TextMeshProUGUI>();

        m_BtnSummary = pnlSummary.GetComponent<Button>();

        // Add listeners
        m_BtnSummary.onClick.AddListener(OnSummaryClick);

        // Register events
        //NTTMyHealthControl.Api.OnDateClickShowBMICaloriesValueEvent += SetBMICaloriesValue;

        // Default values
        m_TxtDate.text = DateTime.Now.ToString("dd MMM yyyy");
        m_TxtBMIValue.text = DEFAULT_VALUE;
        m_TxtCaloriesValue.text = DEFAULT_VALUE;

        m_CalendarController.Init(m_TxtDate);
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
}
