using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ImgSet
{
    public List<ImgObj> imgArray = new List<ImgObj>();
}

[Serializable]
public class ImgObj
{
    public string image;
}