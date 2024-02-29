using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTStatisticView : MonoBehaviour
{
    private NTTBarChartView m_BarChartView;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        m_BarChartView = transform.Find("Body/Viewport/Content/PnlChart").GetComponent<NTTBarChartView>();

        m_BarChartView.Init();
    }
}
