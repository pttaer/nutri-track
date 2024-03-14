using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NTTLoginView : MonoBehaviour
{
    private InputField m_IpfEmail;
    private InputField m_IpfPassword;

    private Button m_BtnLogin;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        m_IpfEmail = transform.Find("Body/LoginPnl/IpfEmailPnl/IpfEmail").GetComponent<InputField>();
        m_IpfPassword = transform.Find("Body/LoginPnl/IpfPasswordPnl/IpfPassword").GetComponent<InputField>();

        m_BtnLogin = transform.Find("Body/LoginPnl/BtnLogin").GetComponent<Button>();

        m_BtnLogin.onClick.AddListener(OnLogin);
    }

    private void OnLogin()
    {
        NTTLoginDTO m_LoginDTO = new(m_IpfEmail.text, m_IpfPassword.text);

        StartCoroutine(NTTApiControl.Api.PostData(NTTConstant.LOGIN_ROUTE, m_LoginDTO, (response, status) =>
        {
            if (status == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                NTTModel.CurrentUser = response.ToObject<NTTUserDTO>();
                NTTConstant.BEARER_TOKEN_EDITOR = NTTModel.CurrentUser.Tokens.Access.Token;

                NTTSceneLoaderControl.Api.LoadSingularScene(NTTConstant.SCENE_MENU);
                NTTSceneLoaderControl.Api.LoadSingularScene(NTTConstant.SCENE_HOME);
            }
        }));
    }

#if UNITY_EDITOR
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            NTTLoginDTO m_LoginDTO = new("user1@gmail.com", "password1");

            StartCoroutine(NTTApiControl.Api.PostData(NTTConstant.LOGIN_ROUTE, m_LoginDTO, (response, status) =>
            {
                if (status == UnityEngine.Networking.UnityWebRequest.Result.Success)
                {
                    NTTModel.CurrentUser = response.ToObject<NTTUserDTO>();
                    NTTConstant.BEARER_TOKEN_EDITOR = NTTModel.CurrentUser.Tokens.Access.Token;

                    NTTSceneLoaderControl.Api.LoadSingularScene(NTTConstant.SCENE_MENU);
                    NTTSceneLoaderControl.Api.LoadSingularScene(NTTConstant.SCENE_HOME);
                }
            }));
        }
    }
#endif
}
