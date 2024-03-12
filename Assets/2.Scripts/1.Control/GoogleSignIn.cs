using Assets.SimpleGoogleSignIn;
using Assets.SimpleGoogleSignIn.Scripts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSignIn : MonoBehaviour
{
    //TODO: hide googleIdToken with api
    private string googleClientIdAndroid = "529134634305-u3unoed5gpmfksstg82oi2jasj7rsdh8.apps.googleusercontent.com";

    private string googleClientId = "529134634305-oin934ukcggp0qspi786jqgvsoqq52o1.apps.googleusercontent.com";
    private string googleClientSecret = "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";

    public GoogleAuth GoogleAuth;

    public void Start()
    {
        GoogleAuth = new GoogleAuth();
    }

    public void SignIn()
    {
        GoogleAuth.SignIn(OnSignIn, caching: true);
    }

    public void SignOut()
    {
        GoogleAuth.SignOut(revokeAccessToken: true);
        Debug.LogError("signed out");
    }

    public void GetAccessToken()
    {
        GoogleAuth.GetAccessToken(OnGetAccessToken);
    }

    private void OnSignIn(bool success, string error, UserInfo userInfo)
    {
        // When done sign in
        //SignOut();
    }

    private void OnGetAccessToken(bool success, string error, TokenResponse tokenResponse)
    {
        if (!success) return;

        var jwt = new JWT(tokenResponse.IdToken);

        Debug.Log($"JSON Web Token (JWT) Payload: {jwt.Payload}");

        //jwt.ValidateSignature(GoogleAuth.ClientId, (success, error) => OnValidateSignature(success, error, tokenResponse.IdToken));
    }

    private void OnValidateSignature(bool success, string error, string idToken)
    {
        if (success)
        {
            // NOTE: login and set bearer token first and then call the other api
            StartCoroutine(ApiLogin(idToken));
        }
        else
        {
            Debug.Log("error= " + error);
        }
    }

    private IEnumerator ApiLogin(string idToken)
    {
        NTTControl.Api.ShowLoading();

        JObject json = new JObject
        {
            { "token", idToken }
        };

        using (UnityWebRequest request = new UnityWebRequest(NTTConstant.LOGIN_ROUTE, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");

            byte[] rawJsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(json));

            request.uploadHandler = new UploadHandlerRaw(rawJsonData);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();
            try
            {
                var response = request.downloadHandler.text;

                if (request.result == UnityWebRequest.Result.Success)
                {
                    NTTControl.Api.HideLoading();
                    Debug.Log("Login succeeded!");
                    NTTConstant.BEARER_TOKEN = "Bearer " + response;
                    Debug.Log("Login succeeded!" + "Bearer " + response);
                    PlayerPrefs.SetString(NTTConstant.BEARER_TOKEN_CACHE, NTTConstant.BEARER_TOKEN);
                }
                else
                {
                    //NTTControl.Api.ShowNTTPopup("Oops!", "Something was wrong, please try again.", "Continue", "NotShow");
                    NTTControl.Api.HideLoading();
                    Debug.LogError("Request error: " + request.error);
                }
            }
            catch (Exception err)
            {
                //NTTControl.Api.ShowNTTPopup("Oops!", "Something was wrong, please try again.", "Continue", "NotShow");
                NTTControl.Api.HideLoading();
                Debug.Log(err);
            }
        }
    }

    public void Navigate(string url)
    {
        Application.OpenURL(url);
    }
}