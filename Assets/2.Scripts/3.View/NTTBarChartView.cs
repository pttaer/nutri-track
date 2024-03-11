using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class NTTBarChartView : MonoBehaviour
{
    private BarChart m_barChart;
    private XAxis m_xAxis;

    private Dropdown m_DrdwSelectChart;

    private int m_maxXAxisValue;
    private List<float> m_Data = new();
    private List<float> mockdata = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

    private enum ChartType
    {
        Bar,
        CandleStick,
        EffectScatter,
        HeatMap,
        Line,
        Parallel,
        Pie,
        Radar,
        Ring,
        Scatter
    }

    public void Init()
    {
        m_barChart = transform.Find("BarChart").GetComponent<BarChart>();
        m_xAxis = m_barChart.EnsureChartComponent<XAxis>();

        BarChartConfig();
    }

    private void BarChartConfig()
    {
        // Series data
        AddNewSerieAndData("Months", mockdata, ChartType.Bar);
        AddNewSerieAndData("Months", mockdata, ChartType.Line);

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
    private void AddNewSerieAndData(string name, List<float> data, ChartType chartType)
    {
        Serie serie = InitSerie(name, chartType);

        data.ForEach(x =>
        {
            SerieData serieData = new();
            serieData.data.Add(x);
            m_barChart.AddData(serie.index, serieData.data);
        });

        m_maxXAxisValue = data.Count;
    }

    private Serie InitSerie(string name, ChartType chartType)
    {
        Serie serie = new Serie();

        switch (chartType)
        {
            case ChartType.Bar:
                serie = m_barChart.AddSerie<Bar>(name, true, false);
                break;
            case ChartType.CandleStick:
                serie = m_barChart.AddSerie<Candlestick>(name, true, false);
                break;
            case ChartType.EffectScatter:
                serie = m_barChart.AddSerie<EffectScatter>(name, true, false);
                break;
            case ChartType.HeatMap:
                serie = m_barChart.AddSerie<Heatmap>(name, true, false);
                break;
            case ChartType.Line:
                serie = m_barChart.AddSerie<Line>(name, true, false);
                break;
            case ChartType.Parallel:
                serie = m_barChart.AddSerie<Parallel>(name, true, false);
                break;
            case ChartType.Pie:
                serie = m_barChart.AddSerie<Pie>(name, true, false);
                break;
            case ChartType.Radar:
                serie = m_barChart.AddSerie<Radar>(name, true, false);
                break;
            case ChartType.Ring:
                serie = m_barChart.AddSerie<Ring>(name, true, false);
                break;
            case ChartType.Scatter:
                serie = m_barChart.AddSerie<Scatter>(name, true, false);
                break;
        }
        
        return serie;
    }
}
