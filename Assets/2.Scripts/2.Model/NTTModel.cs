using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTTModel
{
    public static NTTModel m_api;

    public static NTTModel Api
    {
        get
        {
            if (m_api == null)
                m_api = new NTTModel();
            return m_api;
        }
    }

    public static NTTUserDTO CurrentUser;
}
