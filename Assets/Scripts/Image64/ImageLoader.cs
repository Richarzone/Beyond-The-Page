using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    public string base64ImageString;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadImageFromBase64();
    }

    public void LoadImageFromBase64()
    {
        byte[] imageData = System.Convert.FromBase64String(base64ImageString);

        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(imageData);

        RawImage rawImage = gameObject.GetComponent<RawImage>();
        rawImage.texture = texture;
    }
}
