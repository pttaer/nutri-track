using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTMyHealthControl : MonoBehaviour
{
    public static NTTMyHealthControl Api;

    public Action<float, float> OnDateClickShowBMICaloriesValueEvent;

    public void DateClickShowBMICaloriesValue(int day, int month, int year)
    {
        // TODO: Call api show here then listen in MyHealthView
        //OnDateClickShowBMICaloriesValueEvent?.Invoke(bmi, calo);
    }
}