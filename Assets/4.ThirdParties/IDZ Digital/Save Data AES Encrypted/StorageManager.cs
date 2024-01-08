using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using Object = System.Object;

namespace IDZ_Digital.Extensions
{
    public static class StorageManager
    {
        public static string SelectedLocation { get; private set; }
        /// <summary>
        /// Path: Application.persistentDataPath + "/Storage"
        /// </summary>
        public static readonly string ExternalLocation = Application.persistentDataPath + "/Storage/";
        /// <summary>
        /// Path: Application.streamingAssetsPath + "/Storage"
        /// </summary>
        public static readonly string StreamingAssetsLocation = Application.streamingAssetsPath + "/Storage/";
        /// <summary>
        /// Path: "Storage" folder which is along with Asset's folder
        /// </summary>
        public const string ProjectLocation = "Storage/";


        /// <summary>
        /// Set path for StorageManager where further operations will be done
        /// </summary>
        /// <example>
        /// StorageManager.ExternalLocation
        /// StorageManager.StreamingAssetsLocation
        /// StorageManager.ProjectLocation
        /// </example>
        
        public static void SetLocation(string storageLocation)
        {
            if (!storageLocation.EndsWith("/"))
                storageLocation += "/";
            SelectedLocation = storageLocation;
            if (!Directory.Exists(SelectedLocation))
                Directory.CreateDirectory(SelectedLocation);
        }

        private static void TouchPath(string storageLocation)
        {
            storageLocation = storageLocation.Substring(0, storageLocation.LastIndexOf('/'));
            Directory.CreateDirectory(storageLocation);
        }

        public static string GetNewFileName(string prepend, string append)
        {
            string path;

            do
            {
                var lastGeneratedIndex = PlayerPrefs.GetInt("lastGeneratedIndex", 0);
                lastGeneratedIndex++;
                PlayerPrefs.SetInt("lastGeneratedIndex", lastGeneratedIndex);
                PlayerPrefs.Save();

                path = prepend + lastGeneratedIndex + append;
            } while (CheckFileExist(path));

            return path;
        }

        public static bool CheckFileExist(string path)
        {
            return File.Exists(path) || File.Exists(StreamingAssetsLocation + path) ||
                   File.Exists(SelectedLocation + path);
        }
        
        /// <summary>
        /// Save data to file in initialized location
        /// </summary>
        /// <param name="file">Name of file</param>
        /// <param name="data">Data to be stored</param>
        public static void Write(string file, string data)
        {
            TouchPath(SelectedLocation + file);
            var writer = new StreamWriter(SelectedLocation + file, false);
            writer.Write(data);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Encrypt and Save data to file in initialized location
        /// </summary>
        /// <param name="file">Name of the file to be stored</param>
        /// <param name="data">Data to be stored</param>
        /// <param name="password">Password to be used to encrypt the data</param>
        /// <param name="salt">Salt to be used while encrypting the data</param>
        public static void EncryptAndWrite(string file, string data, string password = null,
            byte[] salt = null)
        {
            TouchPath(SelectedLocation + file);
            var writer = new StreamWriter(SelectedLocation + file, false);

            var encryptedData = AESCryptographicSystem.Encrypt(data, password, salt);

            writer.Write(encryptedData);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Save data to file in initialized location
        /// </summary>
        /// <param name="file">Name of file</param>
        /// <param name="data">Data to be stored</param>
        public static void Write(string file, byte[] data)
        {
            TouchPath(SelectedLocation + file);
            var writer = new StreamWriter(SelectedLocation + file, false);
            writer.BaseStream.Write(data, 0, data.Length);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Encrypt and Save data to file in initialized location
        /// </summary>
        /// <param name="file">Name of the file to be stored</param>
        /// <param name="data">Data to be stored</param>
        /// <param name="password">Password to be used to encrypt the data</param>
        /// <param name="salt">Salt to be used while encrypting the data</param>
        public static void EncryptAndWrite(string file, byte[] data, string password = null,
            byte[] salt = null)
        {
            TouchPath(SelectedLocation + file);

            var encryptedData = AESCryptographicSystem.Encrypt(data, password, salt);

            var writer = new StreamWriter(SelectedLocation + file, false);
            writer.BaseStream.Write(encryptedData, 0, encryptedData.Length);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Read saved file
        /// </summary>
        /// <param name="file">File name to be read</param>
        /// <param name="defaultData">Default data if file not present</param>
        /// <returns>Data stored in the file</returns>
        public static string ReadNow(string file, string defaultData = null)
        {
            var path = SelectedLocation + file;
            if (!CheckFileExist(path))
            {
                path = ProjectLocation + "/" + file;

                // Remove extension if present
                int dot;
                if ((dot = path.LastIndexOf('.')) > -1)
                    path = path.Substring(0, dot);

                var textAsset = Resources.Load<TextAsset>(path);
                if (textAsset == null) return defaultData;

                return textAsset.text;
            }

            var reader = new StreamReader(path);
            try
            {
                var data = reader.ReadToEnd();
                reader.Close();
                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return defaultData;
            }
            finally
            {
                reader.Close();
            }
        }
/// <summary>
/// Read encrypted data from the file
/// </summary>
/// <param name="file">Name of the file</param>
/// <param name="defaultData">Default data if file not present</param>
/// <param name="password">Password used for encryption of the data</param>
/// <param name="salt">Salt used while encrypting data</param>
/// <returns>Decrypted data from the file</returns>
        public static string ReadEncryptedNow(string file, string defaultData = null, string password = null,
            byte[] salt = null)
        {
            var path = SelectedLocation + file;
            if (!CheckFileExist(path))
            {
                path = ProjectLocation + "/" + file;

                // Remove extension if present
                int dot;
                if ((dot = path.LastIndexOf('.')) > -1)
                    path = path.Substring(0, dot);

                var textAsset = Resources.Load<TextAsset>(path);
                if (textAsset == null) return defaultData;

                return textAsset.text;
            }

            var reader = new StreamReader(path);
            try
            {
                var data = reader.ReadToEnd();
                reader.Close();

                var decryptedData = AESCryptographicSystem.Decrypt(data, password, salt);

                return decryptedData;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return defaultData;
            }
            finally
            {
                reader.Close();
            }
        }
/// <summary>
/// Read saved bytes data from file
/// </summary>
/// <param name="file">File name to be read</param>
/// <returns>Bytes data stored in the file</returns>
        public static byte[] ReadBytesNow(string file)
        {
            var path = SelectedLocation + file;

            if (!File.Exists(path))
            {
                return null;
            }

            var reader = new StreamReader(path);
            try
            {
                using (var memstream = new MemoryStream())
                {
                    reader.BaseStream.CopyTo(memstream);
                    return memstream.ToArray();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
            finally
            {
                reader.Close();
            }
        }
/// <summary>
/// Read encrypted bytes data from the file
/// </summary>
/// <param name="file">Name of the file</param>
/// <param name="password">Password used for encryption of the data</param>
/// <param name="salt">Salt used while encrypting data</param>
/// <returns>Decrypted bytes data from the file</returns>
        public static byte[] ReadEncryptedBytesNow(string file, string password = null, byte[] salt = null)
        {
            var path = SelectedLocation + file;

            if (!File.Exists(path))
            {
                return null;
            }

            var reader = new StreamReader(path);
            try
            {
                using var memstream = new MemoryStream();
                reader.BaseStream.CopyTo(memstream);

                var decryptedBytes = AESCryptographicSystem.Decrypt(memstream.ToArray(), password, salt);
                
                return decryptedBytes;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
            finally
            {
                reader.Close();
            }
        }

        public static ReadOperation Read(string file, string defaultData = null)
        {
            return new ReadOperation(file, defaultData);
        }

        public static string GetFullPath(string file)
        {
            var path = SelectedLocation + file;
            if (CheckFileExist(path)) return path;

            path = StreamingAssetsLocation + file;
            if (CheckFileExist(path)) return path;

            Debug.Log(path + " Doesn't exist");
            return null;
        }
/// <summary>
/// Clear all saved data inside currently selected location
/// </summary>
        public static void EraseEverything()
        {
            if (Directory.Exists(SelectedLocation))
            {
                foreach (var file in Directory.GetFiles(SelectedLocation))
                {
                    if (Directory.Exists(file)) continue;
                    File.Delete(file);
                }
            }
        }

        public static bool IsStorageEmpty()
        {
            var Files = Directory.GetFiles(SelectedLocation);
            if (Files.Length > 1)
                return false;
            else
                return true;
        }
/// <summary>
/// Deletes file at specified location if it exists
/// </summary>
/// <returns></returns>
        public static bool Delete(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            else if (File.Exists(SelectedLocation + filePath))
                File.Delete(SelectedLocation + filePath);
            else
                return false;

            return true;
        }
    
/// <summary>
/// Convert Object to Byte array
/// </summary>
/// <param name="obj">Instance of Object class to be converted to Byte array</param>
/// <returns>Object in byte array form</returns>
public static byte[] ObjectToByteArray(Object obj)
{
    if (obj == null)
        return null;

    BinaryFormatter bf = new BinaryFormatter();
    MemoryStream ms = new MemoryStream();
    bf.Serialize(ms, obj);

    return ms.ToArray();
}

/// <summary>
/// Convert Byte array to Object Instance
/// </summary>
/// <param name="arrBytes">Array bytes to be converted to Object class's Instance</param>
/// <returns>Array bytes in Object instance form</returns>
public static Object ByteArrayToObject(byte[] arrBytes)
{
    MemoryStream memStream = new MemoryStream();
    BinaryFormatter binForm = new BinaryFormatter();
    memStream.Write(arrBytes, 0, arrBytes.Length);
    memStream.Seek(0, SeekOrigin.Begin);
    Object obj = binForm.Deserialize(memStream);

    return obj;
}

    }
    

    public class ReadOperation
    {
        public string file;
        public string data;
        public bool isComplete { get; private set; }
        private Action<string> _action;

        public ReadOperation(string file, string defaultData)
        {
            this.file = file;
            data = defaultData;
            isComplete = false;
        }

        public ReadOperation OnComplete(Action<string> onCompleteAction)
        {
            _action = onCompleteAction;
            return this;
        }

        public ReadOperation Start(MonoBehaviour monoBehaviour)
        {
            monoBehaviour.StartCoroutine(Read());
            return this;
        }

        public IEnumerator Read()
        {
            var _data = StorageManager.ReadNow(file);
            if (_data != null)
            {
                data = _data;
                isComplete = true;
                if (_action != null)
                    _action(data);
                yield break;
            }

            var path = StorageManager.SelectedLocation + file;
            if (!StorageManager.CheckFileExist(path))
                path = StorageManager.StreamingAssetsLocation + file;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS
            if (!StorageManager.CheckFileExist(path))
            {
                isComplete = true;
                if (_action != null)
                    _action(data);
                yield break;
            }
#endif

            if (path.Contains("://"))
            {
                var wwwReader = UnityWebRequest.Get(path);
                while (!isComplete)
                {
                    yield return wwwReader.SendWebRequest();
                    isComplete = wwwReader.isDone;
                }

                if (string.IsNullOrEmpty(wwwReader.error))
                    data = wwwReader.downloadHandler.text;
            }
            else
            {
                var reader = new StreamReader(path);
                data = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                isComplete = true;
            }

            if (_action != null)
                _action(data);
        }
    }
}