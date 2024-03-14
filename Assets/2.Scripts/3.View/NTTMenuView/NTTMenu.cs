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
            NTTMenuBtnView btnView = tf.GetComponent<NTTMenuBtnView>();
            btnView.Init();

            btnView.Btn.onClick.AddListener(() => {
                MoveSelectedTo(btnView.Btn.transform, m_BtnActiveColor[m_BtnList.IndexOf(btnView.Btn)], btnView.Text);
                LoadScene(btnView.ButtonScene);
            });
            
            m_BtnList.Add(btnView.Btn);
            m_TxtList.Add(btnView.Text);
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

    private void LoadScene(NTTMenuControl.MenuScene menuScene)
    {
        switch (menuScene)
        {
            case NTTMenuControl.MenuScene.MyHealth:
                NTTSceneLoaderControl.Api.LoadSingularScene(NTTConstant.SCENE_MY_HEALTH);
                break;
            case NTTMenuControl.MenuScene.Statistic:
                NTTSceneLoaderControl.Api.LoadSingularScene(NTTConstant.SCENE_STATISTIC);
                break;
            case NTTMenuControl.MenuScene.Home:
                NTTSceneLoaderControl.Api.LoadSingularScene(NTTConstant.SCENE_HOME);
                break;
            case NTTMenuControl.MenuScene.Store:
                NTTSceneLoaderControl.Api.LoadSingularScene(NTTConstant.SCENE_SHOP);
                break;
            case NTTMenuControl.MenuScene.Profile:
                NTTSceneLoaderControl.Api.LoadSingularScene(NTTConstant.SCENE_PROFILE);
                break;
        }
    }


}
