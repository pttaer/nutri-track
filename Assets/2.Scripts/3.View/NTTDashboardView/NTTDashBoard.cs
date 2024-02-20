using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTDashBoard : MonoBehaviour
{
    private NTTDashboardMenu m_Menu;
    private NTTDashboardMain m_Main;
    
    void Start()
    {
        NTTDashboardControl.Api = new NTTDashboardControl();
        Init();
    }

    public void Init()
    {
        m_Main = transform.Find("Body/Main").GetComponent<NTTDashboardMain>();
        m_Menu = transform.Find("Body/Menu").GetComponent<NTTDashboardMenu>();
        
        m_Main.Init();
        m_Menu.Init();
    }
}
