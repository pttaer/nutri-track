using System;
using System.Collections.Generic;
using UnityEngine;

public class NTTMyHealthControl
{
    public static NTTMyHealthControl Api;

    public Action OnClosePopupRecordEvent;
    public Action<bool> OnChooseDayEvent;

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

    public void CheckExistItemByDateInListDailyCal(DateTime itemDate, List<NTTDailyCalDTO> dailyCalList, List<NTTCalRecordDTO> calRecordList, Action<NTTDailyCalDTO, List<NTTCalRecordDTO>> callbackExist = null, Action callbackNone = null)
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
                callbackExist?.Invoke(dailyCal, recordsData);
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

    public float CurrentUserBMICalculate(float weight)
    {
        // kg/m^2 (height is in meter)
        return (float)Math.Round(weight / Math.Pow(NTTModel.CurrentUser.User.Height / 100, 2), 1);
    }
}