using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NTTImageItemView : MonoBehaviour
{
    private RawImage m_Image;
    private Button m_BtnRemove;

    private Action OnClickRemoveEvent;

    public void Init(Texture2D texture, Action callback = null)
    {
        m_Image = transform.Find("RawImage").GetComponent<RawImage>();
        m_BtnRemove = transform.Find("BtnRemove").GetComponent<Button>();

        m_BtnRemove.onClick.AddListener(OnClickRemove);

        if (texture != null)
        {
            m_Image.texture = texture;
        }
    }

    private void OnClickRemove()
    {
        Destroy(gameObject);
        OnClickRemoveEvent?.Invoke();
    }
}
