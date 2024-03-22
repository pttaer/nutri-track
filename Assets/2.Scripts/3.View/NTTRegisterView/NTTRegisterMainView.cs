using Assets.SimpleGoogleSignIn.Scripts;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using static NativeGallery;
using XCharts.Runtime;

public class NTTRegisterMainView : MonoBehaviour
{
    private Transform m_MainPnl;
    private Transform m_EmailPnl;
    private Transform m_InfoPnl;
    private Transform m_BmiPnl;
    private Transform m_ConditionPnl;
    private Transform m_ExpertApplicationPnl;
    private Transform m_GooglePnl;
    private Transform m_CalendarPnl;
    private Transform m_SelectRolePnl;
    private Transform m_ImagePnl;
    private Transform m_ApplicationDescriptionPnl;
    private NTTInputIncreamentView m_HeightPnl;
    private NTTInputIncreamentView m_WeightPnl;
    private TextMeshProUGUI m_TxtBmi;

    private InputField m_IpfEmail;
    private InputField m_IpfPassword;
    private InputField m_IpfName;

    private TMP_Dropdown m_DrdwGender;

    private CalendarController m_CalendarController;
    private AutoCompleteComboBox m_ComboBoxMedical;
    private NTTTagListView m_MedicalTaglist;
    private NTTImageItemView m_ImagePrefItem;

    private Button m_BtnDob;
    private Button m_BtnExpert;
    private Button m_BtnCustomer;
    private Button m_BtnNext;
    private Button m_BtnBack;
    private Button m_BtnAddImages;

    private Dictionary<int, string> m_ComboBoxOptions = new Dictionary<int, string>();
    private UserInfo GoogleUserInfo;
    private List<Texture2D> m_ApplicationImageTextures = new List<Texture2D>();

    private int m_CurrentPnlIndex = 0;
    private bool m_IsExpert;

    private enum PnlEnum
    {
        Email,
        Info,
        Bmi,
        Conditional,
        ExpertApplication = 2
    }

    private Dictionary<PnlEnum, Transform> m_DictPnl;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        m_MainPnl = transform.Find("Body/MainPnl");
        m_SelectRolePnl = transform.Find("Body/SelectRolePnl");
        m_CalendarController = transform.Find("Body/CalendarPnl/CalendarControllerPreb").GetComponent<CalendarController>();
        m_GooglePnl = m_MainPnl.Find("GooglePnl").transform;

        m_EmailPnl = m_MainPnl.Find("EmailPnl");
        m_InfoPnl = m_MainPnl.Find("InfoPnl");
        m_BmiPnl = m_MainPnl.Find("BmiPnl");
        m_ConditionPnl = m_MainPnl.Find("ConditionPnl");
        m_ExpertApplicationPnl = m_MainPnl.Find("ExpertApplicationPnl");
        m_ImagePnl = m_ExpertApplicationPnl.Find("ImagesPnl");
        m_ApplicationDescriptionPnl = m_ExpertApplicationPnl.Find("DescriptionPnl");

        // EMAIL
        m_IpfEmail = m_EmailPnl.Find("EmailPnl/IpfEmail").GetComponent<InputField>();
        m_IpfPassword = m_EmailPnl.Find("PasswordPnl/IpfPassword").GetComponent<InputField>();

        // INFO
        m_IpfName = m_InfoPnl.Find("NamePnl/IpfName").GetComponent<InputField>();
        m_BtnDob = m_InfoPnl.Find("DobPnl/BtnDob").GetComponent<Button>();
        m_DrdwGender = m_InfoPnl.Find("GenderPnl/DropdownGender").GetComponent<TMP_Dropdown>();

        // BMI
        m_HeightPnl = m_BmiPnl.Find("HWPnl/HeightPnl").GetComponent<NTTInputIncreamentView>();
        m_WeightPnl = m_BmiPnl.Find("HWPnl/WeightPnl").GetComponent<NTTInputIncreamentView>();
        m_TxtBmi = m_BmiPnl.Find("BmiPnl/ValuePnl/TxtValue").GetComponent<TextMeshProUGUI>();

        // CONDITION
        m_ComboBoxMedical = m_ConditionPnl.Find("MedicalPnl/ComboBoxMedical").GetComponent<AutoCompleteComboBox>();
        m_MedicalTaglist = m_ConditionPnl.Find("MedicalPnl/MedicalTagList").GetComponent<NTTTagListView>();

        // EXPERT
        m_BtnCustomer = m_SelectRolePnl.Find("BtnGr/CustomerPnl/BtnCustomer").GetComponent<Button>();
        m_BtnExpert = m_SelectRolePnl.Find("BtnGr/ExpertPnl/BtnExpert").GetComponent<Button>();
        m_BtnAddImages = m_ExpertApplicationPnl.Find("ImagesPnl/Add/BtnAdd").GetComponent<Button>();
        m_ImagePrefItem = m_ExpertApplicationPnl.Find("ImagesPnl/Item").GetComponent<NTTImageItemView>();

        m_BtnNext = m_MainPnl.Find("BtnGr/BtnNext").GetComponent<Button>();
        m_BtnBack = m_MainPnl.Find("BtnGr/BtnBack").GetComponent<Button>();

        m_BtnCustomer.onClick.AddListener(delegate { OnSelectRole(false); });
        m_BtnExpert.onClick.AddListener(delegate { OnSelectRole(true); });
        m_BtnAddImages.onClick.AddListener(OnSelectApplicationImages);
        m_BtnNext.onClick.AddListener(NextPnl);
        m_BtnBack.onClick.AddListener(BackPnl);

        NTTRegisterControl.Api.OnUpdateBmiValueEvent += UpdateBmiView;
        NTTRegisterControl.Api.OnRegisterWithGoogleEvent += OnRegisterWithGoogleHandler;
        m_ComboBoxMedical.OnItemSelected.AddListener(OnSelectMedical);
    }

    private void OnDestroy()
    {
        NTTRegisterControl.Api.OnUpdateBmiValueEvent -= UpdateBmiView;
        NTTRegisterControl.Api.OnRegisterWithGoogleEvent -= OnRegisterWithGoogleHandler;
        m_ComboBoxMedical.OnItemSelected.RemoveListener(OnSelectMedical);
    }


    private void InitView()
    {
        m_SelectRolePnl.gameObject.SetActive(false);
        m_MainPnl.gameObject.SetActive(true);
        m_BtnBack.gameObject.SetActive(!m_EmailPnl.gameObject.activeSelf);
        m_GooglePnl.gameObject.SetActive(m_EmailPnl.gameObject.activeSelf);

        if (m_IsExpert)
        {
            m_DictPnl = new Dictionary<PnlEnum, Transform>()
            {
                { PnlEnum.Email, m_EmailPnl },
                { PnlEnum.Info, m_InfoPnl },
                { PnlEnum.ExpertApplication, m_ExpertApplicationPnl }
            };
        }
        else
        {
            m_DictPnl = new Dictionary<PnlEnum, Transform>()
            {
                { PnlEnum.Email, m_EmailPnl },
                { PnlEnum.Info, m_InfoPnl },
                { PnlEnum.Bmi, m_BmiPnl },
                { PnlEnum.Conditional, m_ConditionPnl }
            };

            m_HeightPnl.Init();
            m_WeightPnl.Init();
            m_MedicalTaglist.Init();
            m_TxtBmi.text = "0";
            //m_CalendarController.Init();

            GetMedicalConditions();
        }
    }

    private void OnSelectRole(bool isExpert)
    {
        m_IsExpert = isExpert;
        InitView();
    }

    private void NextPnl()
    {
        m_CurrentPnlIndex++;
        if (m_CurrentPnlIndex >= m_DictPnl.Count)
        {
            //NTTApiControl.Api.PostData(NTTConstant.REGISTER_ROUTE, RegisterDTO, (response) =>
            //{

            //});
            PopulateRegisterInfo();
        }
        else
        {
            UpdateView(m_CurrentPnlIndex);
        }
    }

    private void BackPnl()
    {
        if (m_EmailPnl.gameObject.activeSelf || m_CurrentPnlIndex == 0)
        {
            return;
        }

        m_CurrentPnlIndex--;
        UpdateView(m_CurrentPnlIndex);
    }

    private void UpdateView(int pnlIndex)
    {
        foreach (var item in m_DictPnl)
        {
            item.Value.gameObject.SetActive(false);
        }

        m_DictPnl.TryGetValue((PnlEnum)pnlIndex, out Transform tf);
        tf.gameObject.SetActive(true);

        m_BtnBack.gameObject.SetActive(!m_EmailPnl.gameObject.activeSelf);
        m_GooglePnl.gameObject.SetActive(m_EmailPnl.gameObject.activeSelf);
    }

    private void UpdateBmiView()
    {
        m_TxtBmi.text = Utils.GetBMI(m_HeightPnl.Value, m_WeightPnl.Value).ToString();
    }

    private void GetMedicalConditions()
    {
        StartCoroutine(NTTApiControl.Api.GetListData<NTTMedicalConditionDTO>(NTTConstant.MEDICAL_ROUTE, (response) =>
        {
            foreach (NTTMedicalConditionDTO item in response.Results)
            {
                m_ComboBoxOptions.Add(int.Parse(item.Id), item.Name.ToLower());
            }

            m_ComboBoxMedical.AvailableOptions = m_ComboBoxOptions.Values.ToList();

        }, new Dictionary<string, string>()
        {
            {"limit", "22" },
        }));

    }

    public void OnSelectMedical(string selected)
    {
        if (m_ComboBoxOptions.Values.Contains(selected))
        {
            m_MedicalTaglist.AddItem(selected);
        }
    }

    private void OnRegisterWithGoogleHandler(UserInfo userInfo)
    {
        m_IpfEmail.text = userInfo.email;
        m_IpfName.text = userInfo.name;

        GoogleUserInfo = userInfo;

        m_IpfPassword.interactable = false;
        m_IpfEmail.interactable = false;
        m_IpfName.interactable = false;
    }

    private void PopulateRegisterInfo()
    {
        List<int> medicalKeys = new List<int>();

        foreach (var kvp in m_ComboBoxOptions)
        {
            if (m_MedicalTaglist.Data.Contains(kvp.Value, StringComparer.OrdinalIgnoreCase))
            {
                medicalKeys.Add(kvp.Key);
            }
        }

        NTTRegisterDTO RegisterDTO = new NTTRegisterDTO
        {
            User = new NTTUserDTO.UserDTO
            {
                Email = GoogleUserInfo.email,
                Name = GoogleUserInfo.name,
                Avatar = GoogleUserInfo.picture,
                Height = float.Parse(m_HeightPnl.Ipf.text),
                Gender = m_DrdwGender.options[m_DrdwGender.value].text.ToUpper()
            },
            BmiRecord = new NTTBMIRecordPostDTO
            {
                Weight = float.Parse(m_WeightPnl.Ipf.text)
            },
            MedicalConditionIds = medicalKeys

        };

        Debug.LogError(JsonConvert.SerializeObject(RegisterDTO));
    }

    private void OnSelectApplicationImages()
    {

        Permission permission = GetImagesFromGallery((paths) =>
        {
            if (paths != null)
            {
                foreach (var path in paths)
                {
                    Debug.Log("Image path: " + path);

                    // Create Texture from selected image
                    Texture2D texture = LoadImageAtPath(path, 512);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }

                    m_ApplicationImageTextures.Add(texture);

                    // Assign texture to a temporary quad and destroy it after 5 seconds
                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                    quad.transform.forward = Camera.main.transform.forward;
                    quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                    Material material = quad.GetComponent<Renderer>().material;
                    if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                        material.shader = Shader.Find("Legacy Shaders/Diffuse");

                    material.mainTexture = texture;

                    Destroy(quad, 5f);

                    // If a procedural texture is not destroyed manually, 
                    // it will only be freed after a scene change
                    Destroy(texture, 5f);
                }
            }
        });

        if (m_ApplicationImageTextures.Count > 0)
        {
            foreach (var texture in m_ApplicationImageTextures)
            {
                NTTImageItemView image = Instantiate(m_ImagePrefItem, m_ImagePnl);
                image.gameObject.SetActive(true);
                image.Init(texture, () =>
                {
                    m_ApplicationImageTextures.Remove(texture);
                });
            }

            m_BtnAddImages.transform.SetAsLastSibling();
        }

        Debug.Log("Permission result: " + permission);
    }
}
