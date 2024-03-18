using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTMyHealthControl
{
    public static NTTMyHealthControl Api;

    public Action<float, float> OnDateClickShowBMICaloriesValueEvent;
    public Action OnClosePopupRecordEvent;
    public Action<bool> OnChooseDayEvent;

    public void DateClickShowBMICaloriesValue(int day, int month, int year)
    {
        // TODO: Call api show here then listen in MyHealthView
        //OnDateClickShowBMICaloriesValueEvent?.Invoke(bmi, calo);
    }
    
    public void ClosePopupRecord()
    {
        OnClosePopupRecordEvent?.Invoke();
    }
    
    public void ChooseDay(bool isOn)
    {
        OnChooseDayEvent?.Invoke(isOn);
    }
}