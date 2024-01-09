using IDZ_Digital.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NTTSaveFileControl
{
    public static NTTSaveFileControl Api;

    public static string saveFileLocation = "Assets/1.Assets/Save/";

    public static string PASSWORD;
    public static string SALT;
    public static byte[] SALT_BYTE;

    public void Init(string password, string salt)
    {
        StorageManager.SetLocation(saveFileLocation);

        PASSWORD = password;
        SALT = salt;

        byte[] SALT_BYTE = Encoding.ASCII.GetBytes(SALT);
    }

    /// <summary>
    /// Load json data from encrypted savefile, savefile can initially be a json to which can be return normally.
    /// But if json data is encrypted (not a valid json), then we decrypt data and return it as a json. 
    /// </summary>
    /// <typeparam name="T">Data schema</typeparam>
    /// <param name="filename">The name of the file in which you want to load data from</param>
    /// <param name="callback">Invoke callback on returning data</param>
    public void LoadDataFromSaveFile<T>(string filename, Action<T> callback)
    {
        if (!string.IsNullOrEmpty(PASSWORD) && !string.IsNullOrEmpty(SALT))
        {
            string data = StorageManager.ReadNow(filename);

            if (!CheckValidJson(data))
            {
                data = StorageManager.ReadEncryptedNow(filename, null, PASSWORD, SALT_BYTE);
            }

            callback?.Invoke(JsonConvert.DeserializeObject<T>(data));
        }
    }

    /// <summary>
    /// Saving the json string to a file
    /// </summary>
    /// <param name="filename">The name of the file in which you want to save to</param>
    /// <param name="data">Json string</param>
    /// <param name="callback"></param>
    /// <returns>Successfully saving data or not</returns>
    public bool SaveDataToSaveFile(string filename, string data, Action callback)
    {
        if (CheckValidJson(data))
        {
            try
            {
                StorageManager.EncryptAndWrite(filename, data, PASSWORD, SALT_BYTE);
                callback?.Invoke();

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        return false;
    }

    public bool CheckValidJson(string data)
    {
        try
        {
            JsonUtility.FromJson<JObject>(data); return true;

        }
        catch (Exception e)
        {
            return false;
        }
    }
}
