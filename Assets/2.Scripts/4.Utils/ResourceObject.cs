using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ResourceObject : MonoBehaviour
{
    public static ResourceObject instance;
    public List<Object> objects;

    private void Awake()
    {
        instance = this;
    }

    public static T GetResource<T>(string name) where T : Object
    {
        if (instance != null)
        {
            string realName = Path.GetFileNameWithoutExtension(name);
            foreach (Object prefab in instance.objects)
            {
                if (prefab.name.Equals(realName))
                {
                    return prefab as T;
                }
            }
        }

        return null;
    }

    public static void LoadTexture(RawImage rawImage, string url)
    {
        Uri uriResult;
        bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        if (!result)
        {
            Texture texture = GetResource<Texture>(Path.GetFileNameWithoutExtension(url));
            if (texture != null)
            {
                rawImage.texture = texture;
            }
            return;
        }
        rawImage.LoadTexture(url);
    }
}