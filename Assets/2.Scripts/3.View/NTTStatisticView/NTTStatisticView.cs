using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class NTTStatisticView : MonoBehaviour
{
    private NTTBarChartView m_BarChartView;
    private ToggleGroup m_ToggleGroup;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        m_BarChartView = transform.Find("Body/Viewport/Content/PnlChart").GetComponent<NTTBarChartView>();
        m_ToggleGroup = transform.Find("Body/Viewport/Content/PnlChart/TglGr").GetComponent<ToggleGroup>();

        foreach (Transform item in m_ToggleGroup.transform)
        {
            if (item.TryGetComponent(out NTTToggleFilterView tgl))
            {
                tgl.Init();   
            }
        }

        m_BarChartView.Init();
    }
}
