using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class roomItem : MonoBehaviour
{
    public TextMeshProUGUI roomName;

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }
}
