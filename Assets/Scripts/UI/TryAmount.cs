using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TryAmount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = GameManager.Instance.Attempts.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = GameManager.Instance.Attempts.ToString();
    }
}
