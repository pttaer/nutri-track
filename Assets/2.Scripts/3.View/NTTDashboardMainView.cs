using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XCharts.Runtime;

public class NTTDashboardMainView : MonoBehaviour
{
    private Transform m_tfMenu;
    private Transform m_tfView;

    private BarChart m_barChart;

    private int m_maxXAxisValue;
    private XAxis m_xAxis;
    private List<double> mockdata = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        m_tfMenu = transform.Find("Body/Main/Menu");
        m_tfView = transform.Find("Body/Main/View");

        m_barChart = m_tfView.Find("Body/Viewport/Content/BarChart").GetComponent<BarChart>();
        m_xAxis = m_barChart.EnsureChartComponent<XAxis>();

        BarChartConfig();
    }

    private void BarChartConfig()
    {
        // Series data
        AddNewSerieAndData("Months", mockdata);

        // Axis theme
        m_barChart.theme.axis.fontSize = 12;
        m_barChart.theme.axis.lineWidth = 1;

        // X axis data
        m_xAxis.splitNumber = m_maxXAxisValue;

        for (int i = 0; i < m_maxXAxisValue; i++)
        {
            m_xAxis.AddData($"{i + 1}");
        }
    }

    /// <summary>
    /// Add a new serie and populate it with data
    /// </summary>
    /// <param name="name">The name of the serie</param>
    /// <param name="maxRange">The length of the data</param>
    /// <param name="data"></param>
    private void AddNewSerieAndData(string name, List<double> data = null)
    {
        Serie serie = m_barChart.AddSerie<Bar>(name, true, false);

        //for (int i = 0; i < maxRange; i++)
        //{
        //    SerieData serieData = new();
        //    serieData.data.Add(Random.Range(1, 30));
        //    m_barChart.AddData(serie.index, serieData.data);
        //}

        data.ForEach(x =>
        {
            SerieData serieData = new();
            serieData.data.Add(x);
            m_barChart.AddData(serie.index, serieData.data);
        });

        m_maxXAxisValue = data.Count;
    }
}
