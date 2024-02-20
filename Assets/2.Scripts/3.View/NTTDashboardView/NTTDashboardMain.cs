using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class NTTDashboardMain : MonoBehaviour
{
    private BarChart m_barChart;
    private XAxis m_xAxis;

    private Dropdown m_DrdwSelectChart;

    private int m_maxXAxisValue;
    private List<double> mockdata = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

    public void Init()
    {
        m_barChart = transform.Find("Body/Viewport/Content/ChartPnl/BarChart").GetComponent<BarChart>();
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

        data.ForEach(x =>
        {
            SerieData serieData = new();
            serieData.data.Add(x);
            m_barChart.AddData(serie.index, serieData.data);
        });

        m_maxXAxisValue = data.Count;
    }
}
