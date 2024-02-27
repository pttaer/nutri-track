using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class NTTMenu : MonoBehaviour
{
    [SerializeField] Transform m_TfSelected;
    [SerializeField] Transform m_Content;

    [SerializeField] List<Button> m_BtnList = new List<Button>();
    [SerializeField] List<TextMeshProUGUI> m_TxtList = new List<TextMeshProUGUI>();
    [SerializeField] List<Color> m_BtnActiveColor = new List<Color>();
    private const float TWEEN_MOVE_TIME = 0.3f;
    private const int DEFAULT_SELECT_TRANSFORM = 2;

    // Start is called before the first frame update
    void Start()
    {
        m_TfSelected = transform.Find("ChosenImg");
        m_Content = transform.Find("Content");

        InitBtnList();
    }

    private void InitBtnList()
    {
        foreach (Transform tf in m_Content)
        {
            Button btn = tf.Find("Btn").GetComponent<Button>();
            TextMeshProUGUI txt = tf.Find("Btn/Icon").GetComponent<TextMeshProUGUI>();
            btn.onClick.AddListener(() => {
                MoveSelectedTo(btn.transform, m_BtnActiveColor[m_BtnList.IndexOf(btn)], txt);
            });
            m_BtnList.Add(btn);
            m_TxtList.Add(txt);
        }

        // Default
        DOVirtual.DelayedCall(0.03f, () => MoveSelectedTo(m_BtnList[DEFAULT_SELECT_TRANSFORM].transform, Color.black, null, true));
    }

    private void ActiveAllButtonsImage()
    {
        foreach (var btn in m_BtnList)
        {
            if(!btn.gameObject.activeSelf)
            {
                btn.gameObject.SetActive(true);
            }
        }
    }
    
    private void ResetAllButtonsColor()
    {
        foreach (var txt in m_TxtList)
        {
            txt.color = Color.black;
        }
    }

    private void MoveSelectedTo(Transform tf, Color txtColorActive, TextMeshProUGUI txt = null, bool isDefault = false)
    {
        m_TfSelected.DOMoveX(tf.position.x, isDefault ? 0f : TWEEN_MOVE_TIME, true).OnComplete(() =>
        {
            ResetAllButtonsColor();
            txt.color = txtColorActive;
            /*ActiveAllButtonsImage();

            // Set current button off
            tf.gameObject.SetActive(false);*/
        });
    }


}
