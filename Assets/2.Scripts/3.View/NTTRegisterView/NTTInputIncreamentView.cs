using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NTTInputIncreamentView : MonoBehaviour
{
    public TMP_InputField Ipf;
    public Button BtnIncrease;
    public Button BtnDecrease;

    public float Value = 0f;

    public void Init()
    {
        Ipf = transform.Find("Ipf").GetComponent<TMP_InputField>();
        BtnIncrease = transform.Find("BtnUp").GetComponent<Button>();
        BtnDecrease = transform.Find("BtnDown").GetComponent<Button>();

        Ipf.onValueChanged.AddListener(OnEdit);
        BtnIncrease.onClick.AddListener(OnClickIncrease);
        BtnDecrease.onClick.AddListener(OnClickDecrease);

    }

    private void OnEdit(string input)
    {
        Value = float.Parse(input);
    }

    private void OnClickIncrease()
    {
        UpdateValue(1f);
    }

    private void OnClickDecrease()
    {
        UpdateValue(-1f);
    }  

    private void UpdateValue(float marginValue)
    {
        Value += marginValue;

        Ipf.textComponent.text = Value.ToString();
    }
}
