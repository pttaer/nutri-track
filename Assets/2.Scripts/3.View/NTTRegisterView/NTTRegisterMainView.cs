using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class NTTRegisterMainView : MonoBehaviour
{
    private Transform m_MainPnl;
    private Transform m_EmailPnl;
    private Transform m_InfoPnl;
    private Transform m_BmiPnl;
    private Transform m_ConditionPnl;
    private Transform m_GooglePnl;
    private Transform m_CalendarPnl;
    private NTTInputIncreamentView m_HeightPnl;
    private NTTInputIncreamentView m_WeightPnl;
    private TextMeshProUGUI m_TxtBmi;

    private InputField m_IpfEmail;
    private InputField m_IpfPassword;
    private InputField m_IpfName;

    private CalendarController m_CalendarController;
    private AutoCompleteComboBox m_ComboBoxMedical;

    private Button m_BtnDob;
    private Button m_BtnNext;
    private Button m_BtnBack;

    private int m_CurrentPnlIndex = 0;
    private NTTUserDTO NTTUserDTO = new();

    private enum PnlEnum
    {
        Email,
        Info,
        Bmi,
        Conditional
    }

    private Dictionary<PnlEnum, Transform> m_DictPnl;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        m_MainPnl = transform.Find("Body/MainPnl");
        m_CalendarController = transform.Find("Body/CalendarPnl/CalendarControllerPreb").GetComponent<CalendarController>();

        m_EmailPnl = m_MainPnl.Find("EmailPnl");
        m_InfoPnl = m_MainPnl.Find("InfoPnl");
        m_BmiPnl = m_MainPnl.Find("BmiPnl");
        m_ConditionPnl = m_MainPnl.Find("ConditionPnl");

        // EMAIL
        m_IpfEmail = m_EmailPnl.Find("EmailPnl/IpfEmail").GetComponent<InputField>();
        m_IpfPassword = m_EmailPnl.Find("PasswordPnl/IpfPassword").GetComponent<InputField>();

        // INFO
        m_IpfName = m_InfoPnl.Find("NamePnl/IpfName").GetComponent<InputField>();
        m_BtnDob = m_InfoPnl.Find("DobPnl/BtnDob").GetComponent<Button>();

        // BMI
        m_HeightPnl = m_BmiPnl.Find("HWPnl/HeightPnl").GetComponent<NTTInputIncreamentView>();
        m_WeightPnl = m_BmiPnl.Find("HWPnl/WeightPnl").GetComponent<NTTInputIncreamentView>();
        m_TxtBmi = m_BmiPnl.Find("BmiPnl/ValuePnl/TxtValue").GetComponent<TextMeshProUGUI>();

        // CONDITION
        m_ComboBoxMedical = m_ConditionPnl.Find("MedicalPnl/ComboBoxMedical").GetComponent<AutoCompleteComboBox>();

        m_BtnNext = m_MainPnl.Find("BtnGr/BtnNext").GetComponent<Button>();
        m_BtnBack = m_MainPnl.Find("BtnGr/BtnBack").GetComponent<Button>();

        m_BtnNext.onClick.AddListener(NextPnl);
        m_BtnBack.onClick.AddListener(BackPnl);

        NTTRegisterControl.Api.OnUpdateBmiValueEvent += UpdateBmiView;

        InitView();
    }

    private void InitView()
    {
        m_BtnBack.gameObject.SetActive(!m_EmailPnl.gameObject.activeSelf);

        m_DictPnl = new Dictionary<PnlEnum, Transform>()
        {
            { PnlEnum.Email, m_EmailPnl },
            { PnlEnum.Info, m_InfoPnl },
            { PnlEnum.Bmi, m_BmiPnl },
            { PnlEnum.Conditional, m_ConditionPnl }
        };

        m_HeightPnl.Init();
        m_WeightPnl.Init();
        m_TxtBmi.text = "0";
        //m_CalendarController.Init();

        GetMedicalConditions();
    }

    private void NextPnl()
    {
        m_CurrentPnlIndex++;
        if (m_CurrentPnlIndex > 3)
        {
            NTTApiControl.Api.PostData(NTTConstant.REGISTER_ROUTE, NTTUserDTO, (response) =>
            {

            });
        }
        else
        {
            UpdateView(m_CurrentPnlIndex);
        }
    }

    private void BackPnl()
    {
        if (m_EmailPnl.gameObject.activeSelf || m_CurrentPnlIndex == 0)
        {
            return;
        }

        m_CurrentPnlIndex--;
        UpdateView(m_CurrentPnlIndex);
    }

    private void UpdateView(int pnlIndex)
    {
        foreach (var item in m_DictPnl)
        {
            item.Value.gameObject.SetActive(false);
        }

        m_DictPnl.TryGetValue((PnlEnum)pnlIndex, out Transform tf);
        tf.gameObject.SetActive(true);

        m_BtnBack.gameObject.SetActive(!m_EmailPnl.gameObject.activeSelf);
    }

    private void UpdateBmiView()
    {
        m_TxtBmi.text = Utils.GetBMI(m_HeightPnl.Value, m_WeightPnl.Value).ToString();
    }

    private void GetMedicalConditions()
    {
        StartCoroutine(NTTApiControl.Api.GetListData<NTTMedicalConditionDTO>(NTTConstant.MEDICAL_ROUTE, (response) =>
        {
            List<string> ComboBoxOptions = new List<string>();

            foreach (NTTMedicalConditionDTO item in response.Results)
            {
                ComboBoxOptions.Add(item.Name);
            }

            m_ComboBoxMedical.AvailableOptions = ComboBoxOptions;

        }, new Dictionary<string, string>()
        {
            {"limit", "22" },
        }));

    }

    public void OnSelectMedical(string selected)
    {
        Debug.LogError("Name: " + m_ComboBoxMedical.SelectedItem.Caption);
        Debug.LogError("selected: " + selected);

    }
}
