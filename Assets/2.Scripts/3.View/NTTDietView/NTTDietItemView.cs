using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NTTDietItemView : MonoBehaviour
{
    public Button m_BtnDetail;

    public Button m_BtnActive;
    public Button m_BtnInActive;

    public TextMeshProUGUI m_TxtName;
    public TextMeshProUGUI m_TxtDescription;
    public TextMeshProUGUI m_TxtExpertName;

    public TextMeshProUGUI m_TxtInitialWeight;
    public TextMeshProUGUI m_TxtTargetWeight;
    public TextMeshProUGUI m_TxtActualWeight;

    public TextMeshProUGUI m_TxtStartDate;
    public TextMeshProUGUI m_TxtEndDate;

    public bool m_IsActive;

    public void Init(NTTDietDTO data)
    {
        gameObject.SetActive(true);

        m_BtnDetail = GetComponent<Button>();

        Transform info = transform.Find("Info");
        Transform weight = transform.Find("Weight");
        Transform date = transform.Find("Date");
        Transform topBar = transform.Find("TopBar");

        m_BtnActive = topBar.Find("BtnActive").GetComponent<Button>();
        m_BtnInActive = topBar.Find("BtnInActive").GetComponent<Button>();

        m_BtnActive.gameObject.SetActive(data.IsActive);
        m_BtnInActive.gameObject.SetActive(!data.IsActive);

        m_TxtName = info.Find("DietName").GetComponent<TextMeshProUGUI>();
        m_TxtDescription = info.Find("DietDescription").GetComponent<TextMeshProUGUI>();
        m_TxtExpertName = info.Find("DietExpert").GetComponent<TextMeshProUGUI>();

        m_TxtInitialWeight = weight.Find("InitialWeight/Value").GetComponent<TextMeshProUGUI>();
        m_TxtTargetWeight = weight.Find("ActualWeight/Value").GetComponent<TextMeshProUGUI>();
        m_TxtActualWeight = weight.Find("TargetWeight/Value").GetComponent<TextMeshProUGUI>();

        m_TxtStartDate = date.Find("StartDate/Value").GetComponent<TextMeshProUGUI>();
        m_TxtEndDate = date.Find("EndDate/Value").GetComponent<TextMeshProUGUI>();

        m_BtnDetail.onClick.AddListener(OnClickViewDetail);

        m_TxtName.text = $"Diet name: {data.Name}"; 
        m_TxtDescription.text = $"Description: {data.Description}";
        m_TxtExpertName.text = $"Expert: --";

        // Get expert name
        StartCoroutine(NTTApiControl.Api.GetData<NTTUserDTO>(string.Format(NTTConstant.USER_ROUTE, data.ExpertId), callback: (data) =>
        {
            m_TxtExpertName.text = $"Expert: {data.User.Name ?? NTTConstant.NONE}";
        }));

        SetWeightTxt(m_TxtInitialWeight, data.InitialWeight);
        SetWeightTxt(m_TxtTargetWeight, data.TargetWeight);
        SetWeightTxt(m_TxtActualWeight, data.ActualWeight);

        SetDateFormat(m_TxtStartDate, data.StartDate);
        SetDateFormat(m_TxtEndDate, data.EndDate);
    }

    private void OnClickViewDetail()
    {
        throw new NotImplementedException();
    }

    private void SetWeightTxt(TextMeshProUGUI txt, float value)
    {
        txt.text = $"{value}{NTTConstant.WEIGHT_UNIT_KG}";
    }
    
    private void SetDateFormat(TextMeshProUGUI txt, DateTime date)
    {
        txt.text = $"{date.ToString(NTTConstant.DATE_FORMAT_FULL_MAIN)}";
    }
}