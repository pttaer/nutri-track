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

    public void CheckExistItemByDateInListBMI(DateTime itemDate, List<NTTBMIRecordDTO> bmiRecordList, Action<NTTBMIRecordDTO> callbackExist = null, Action callbackNone = null)
    {
        foreach (NTTBMIRecordDTO bmiRecord in bmiRecordList)
        {
            if (bmiRecord.Date.Date == itemDate.Date)
            {
                callbackExist?.Invoke(bmiRecord);
                return;
            }
        }
        callbackNone?.Invoke();
    }

    public void CheckExistItemByDateInListDailyCal(DateTime itemDate, List<NTTDailyCalDTO> dailyCalList, List<NTTCalRecordDTO> calRecordList, Action<List<NTTCalRecordDTO>> callbackExist = null, Action callbackNone = null)
    {
        bool isRecordExistThatDay = false;
        List<NTTCalRecordDTO> recordsData = new List<NTTCalRecordDTO>();

        foreach (NTTDailyCalDTO dailyCal in dailyCalList)
        {
            if (dailyCal.Date.Date == itemDate.Date)
            {
                foreach (NTTCalRecordDTO calRecord in calRecordList)
                {
                    if (calRecord.DayCalId == dailyCal.Id)
                    {
                        isRecordExistThatDay = true;
                        recordsData.Add(calRecord);
                    }
                }
                callbackExist?.Invoke(recordsData);
            }
        }

        if (isRecordExistThatDay)
        {
            return;
        }

        callbackNone?.Invoke();
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