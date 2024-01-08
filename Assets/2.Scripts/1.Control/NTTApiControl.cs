using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NTTApiControl
{
    public static NTTApiControl Api;

    public static UnityWebRequest WebRequestWithAuthorizationHeader(string url, string method)
    {
        UnityWebRequest request = new UnityWebRequest(url, method.ToUpper());
        //Debug.Log("Token " + FAMConstant.BEARER_TOKEN);
#if UNITY_ANDROID
        Debug.Log("Calling api with token: " + PlayerPrefs.GetString(NTTConstant.BEARER_TOKEN_CACHE));
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString(NTTConstant.BEARER_TOKEN_CACHE));
#endif

#if UNITY_EDITOR
        Debug.Log("Calling with editor token: " + NTTConstant.BEARER_TOKEN_EDITOR);
        request.SetRequestHeader("Authorization", NTTConstant.BEARER_TOKEN_EDITOR);
#endif
        return request;
    }

    public IEnumerator GetListData<T>(string url, Dictionary<string, string> param = null, Action<T[]> renderPage = null, bool isNotShowSorry = false)
    {
        NTTControl.Api.ShowLoading();

        if (param != null)
        {
            url += "?";

            foreach (var item in param)
            {
                url = url + item.Key + "=" + item.Value + "&";
            }
        }

        Debug.Log("CALL " + url);

        UnityWebRequest request = WebRequestWithAuthorizationHeader(url, NTTConstant.METHOD_GET);

        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        Debug.Log("request result= " + request.result);

        // Check that downloadHandler is not null
        if (request.downloadHandler != null)
        {
            Debug.Log("request data= " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error: downloadHandler is null");
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            NTTControl.Api.HideLoading();

            string response = request.downloadHandler.text;

            if (JsonConvert.DeserializeObject(response).GetType() == typeof(JArray))
            {
                T[] dataArray = JsonConvert.DeserializeObject<T[]>(response);

                renderPage?.Invoke(dataArray);
            }
            else
            {
                //NTTListDTO<T> data = JsonConvert.DeserializeObject<NTTListDTO<T>>(response);

                //renderPage?.Invoke(data.results);
            }
        }
        else
        {
            NTTControl.Api.HideLoading();
            // Show popup error

            Debug.LogError("test error: " + request.error);
        }
    }

    public IEnumerator GetData<T>(string url, Dictionary<string, string> param = null, Action<T> renderPage = null)
    {
        NTTControl.Api.ShowLoading();

        if (param != null)
        {
            url += "?";

            foreach (var item in param)
            {
                url = url + item.Key + "=" + item.Value + "&";
            }
        }

        Debug.Log("CALL " + url);

        UnityWebRequest request = WebRequestWithAuthorizationHeader(url, NTTConstant.METHOD_GET);


        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        Debug.Log("request result= " + request.result);

        // Check that downloadHandler is not null
        if (request.downloadHandler != null)
        {
            Debug.Log("request data= " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error: downloadHandler is null");
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            NTTControl.Api.HideLoading();
            string response = request.downloadHandler.text;

            T data = JsonConvert.DeserializeObject<T>(response);
            renderPage?.Invoke(data);
        }
        else
        {
            // Show popup error
            Debug.LogError("test error: " + request.error);
        }
        NTTControl.Api.HideLoading();
    }

    public IEnumerator SetRequestMemberAccess(string url, int memberId, int requestAccess)
    {
        UnityWebRequest request = WebRequestWithAuthorizationHeader(url, NTTConstant.METHOD_PUT);
        request.SetRequestHeader("Content-Type", "application/json");

        JObject json = new JObject
         {
             { "id", memberId },
             { "requestStatus", requestAccess }
         };

        byte[] rawJsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(json));

        request.uploadHandler = new UploadHandlerRaw(rawJsonData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        string response = request.downloadHandler.text;

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Handled request access successfully: " + response);
        }
        else
        {
            Debug.Log("error: " + request.error);
        }
    }

    public IEnumerator PatchData(string uri, Action callback = null)
    {
        NTTControl.Api.ShowLoading();

        UnityWebRequest request = WebRequestWithAuthorizationHeader(uri, NTTConstant.METHOD_PATCH);
        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log("request result= " + request.result);

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.LogError("PATCH OK: ");
            NTTControl.Api.HideLoading();
            callback?.Invoke();
        }
        else
        {
            Debug.LogError("Error sending data: " + request.error);
        }
        NTTControl.Api.HideLoading();
    }

    public IEnumerator EditData<T>(string uri, T formData, Action callback = null)
    {
        NTTControl.Api.ShowLoading();

        Debug.Log("formData " + formData);

        string jsonData = JsonConvert.SerializeObject(formData, Formatting.Indented);

        Debug.Log("jsonData " + jsonData);

        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = WebRequestWithAuthorizationHeader(uri, NTTConstant.METHOD_PUT);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        Debug.Log("request result= " + request.result);

        if (request.uploadHandler != null)
        {
            byte[] uploadedData = request.uploadHandler.data;
            string uploadedDataString = System.Text.Encoding.UTF8.GetString(uploadedData);
            Debug.Log("Uploaded data: " + uploadedDataString);
        }
        else
        {
            Debug.Log("Error: uploadHandler is null");
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            NTTControl.Api.HideLoading();

            Debug.LogError("SENT OK: ");
            callback?.Invoke();
        }
        else
        {
            Debug.LogError("Error sending data: " + request.error);
        }
        NTTControl.Api.HideLoading();
    }

    public IEnumerator PostData<T>(string uri, T formData, Action<JObject> callback = null)
    {
        NTTControl.Api.ShowLoading();

        string jsonData = JsonConvert.SerializeObject(formData, Formatting.Indented);
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = WebRequestWithAuthorizationHeader(uri, NTTConstant.METHOD_POST);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.uploadHandler != null)
        {
            byte[] uploadedData = request.uploadHandler.data;
            string uploadedDataString = System.Text.Encoding.UTF8.GetString(uploadedData);
            Debug.Log("Uploaded data: " + uploadedDataString);
        }
        else
        {
            Debug.Log("Error: uploadHandler is null");
        }

        string response = request.downloadHandler.text;

        if (request.result == UnityWebRequest.Result.Success)
        {
            NTTControl.Api.HideLoading();

            Debug.LogError("SENT OK: ");
            JObject data = JsonConvert.DeserializeObject<JObject>(response);
            callback?.Invoke(data);
        }
        else
        {
            Debug.LogError("Error sending data: " + request.error);
            JObject data = JsonConvert.DeserializeObject<JObject>(response);
            callback?.Invoke(data);
        }
        NTTControl.Api.HideLoading();
    }

    public IEnumerator PurchasePack<T>(string uri, T formData, Action callback = null)
    {
        NTTControl.Api.ShowLoading();

        string jsonData = JsonConvert.SerializeObject(formData, Formatting.Indented);
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = WebRequestWithAuthorizationHeader(uri, NTTConstant.METHOD_POST);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();

        callback?.Invoke();

        yield return request.SendWebRequest();
    }

    public IEnumerator DelItem(string uri, Action callback = null)
    {
        NTTControl.Api.ShowLoading();

        UnityWebRequest request = NTTApiControl.WebRequestWithAuthorizationHeader(uri, NTTConstant.METHOD_DELETE);

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        string response = request.downloadHandler.text;

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Handled request access successfully: " + response);
            callback?.Invoke();
        }
        else
        {
            Debug.Log("error: " + request.error);
            callback?.Invoke();
        }
        NTTControl.Api.HideLoading();
    }

    public void Logout()
    {

    }

    public IEnumerator Login(string email, string password, bool isRememberMe = false, Action callback = null)
    {
        NTTControl.Api.ShowLoading();
        UnityWebRequest request = new UnityWebRequest(NTTConstant.LOGIN, NTTConstant.METHOD_POST.ToUpper());

        JObject data = new JObject()
        {
            {"email", email },
            {"password", password}
        };

        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        // Check that downloadHandler is not null
        if (request.downloadHandler != null)
        {
            Debug.Log("request data= " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error: downloadHandler is null");
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;

//            NTTUserResponseDTO userData = JsonConvert.DeserializeObject<NTTUserResponseDTO>(response);
//            string token = userData.token;

//            Debug.Log($"Response from {NTTConstant.LOGIN}: " + response);

//            NTTModel.Api.CurrentUser = JsonConvert.DeserializeObject<NTTUserDTO>(response);

//            PlayerPrefs.SetString(NTTConstant.BEARER_TOKEN_CACHE, NTTModel.Api.CurrentUser.Token);
//            PlayerPrefs.SetInt(NTTConstant.USER_ID, userData.id);

//            if (isRememberMe)
//            {
//                SetCacheAccount(password, userData);
//            }
//            else
//            {
//                ClearCacheLoginRemember();
//            }

//            PlayerPrefs.SetString(NTTConstant.USER_EMAIL_CACHE, userData.email);
//            PlayerPrefs.SetString(NTTConstant.USER_FULLNAME_CACHE, userData.fullName);

//            Debug.Log("Set token: " + PlayerPrefs.GetString(NTTConstant.BEARER_TOKEN_CACHE));

//#if UNITY_EDITOR
//            NTTConstant.BEARER_TOKEN_EDITOR = "Bearer " + NTTModel.Api.CurrentUser.Token;
//#endif
            callback?.Invoke();
        }
        else
        {
            Debug.LogError("test error: " + request.error);
            if (request.responseCode == 404 || request.responseCode == 500)
            {
                NTTControl.Api.FailLogin(false);
            }
            else
            {
                NTTControl.Api.FailLogin(true);
            }
        }
        NTTControl.Api.HideLoading();
    }
}