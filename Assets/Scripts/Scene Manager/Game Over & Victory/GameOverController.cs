using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    

    [Header("UI")]
    [SerializeField] private Button GameOverBackToMainBtn;


    public void PressBackToMain()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
