using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    

    [Header("UI")]
    [SerializeField] private Button GameOverBackToMainBtn;

    private AudioSource Bowomp;

    private void Start()
    {
        Bowomp = GetComponent<AudioSource>();
        Bowomp.Play();
    }
    public void PressBackToMain()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
