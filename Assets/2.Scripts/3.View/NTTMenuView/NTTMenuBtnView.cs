using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NTTMenuBtnView : MonoBehaviour
{
    public Button Btn;
    public TextMeshProUGUI Text;
    [SerializeField]
    public NTTMenuControl.MenuScene ButtonScene;

    public void Init()
    {
        Btn = transform.Find("Btn").GetComponent<Button>();
        Text = transform.Find("Btn/Icon").GetComponent<TextMeshProUGUI>();
    }
}
