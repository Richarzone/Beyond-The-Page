using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{

    [SerializeField] ImgSet imgSet;
    [SerializeField] GameObject imgPrefab;

    // Start is called before the first frame update
    void Start()
    {
        DecodeArray();
    }

    void DecodeArray()
    {
        imgSet = JsonUtility.FromJson<ImgSet>(Resources.Load<TextAsset>("EnemyCard").text);
        foreach(ImgObj img in imgSet.imgArray)
        {
            GameObject newImgObj;
            newImgObj = Instantiate(imgPrefab, transform.position, transform.rotation, transform);
            ImageLoader tempLoader = newImgObj.GetComponent<ImageLoader>();
            tempLoader.base64ImageString = img.image;
        }
    }

    public void DecodeApiArray()
    {
        foreach (ImgObj img in FileHandler.Instance.arrayImg.imgArray)
        {
            GameObject newImgObj;
            newImgObj = Instantiate(imgPrefab, transform.position, transform.rotation, transform);
            ImageLoader tempLoader = newImgObj.GetComponent<ImageLoader>();
            tempLoader.base64ImageString = img.image;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
