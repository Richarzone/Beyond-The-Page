using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Management")]
    [SerializeField] private SceneLoaderManager sceneManager;

    [Header("UI")]
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject characterSelectScreen;
    [SerializeField] private Button startButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button backButton;

    [SerializeField] private Button felixButton;
    [SerializeField] private Button sophieButton;

    [SerializeField] private GameObject felixSelected;
    [SerializeField] private GameObject sophieSelected;

    /*private void Start()
    {
        Cursor.visible = false;
        startButtonAnimator = startButton.GetComponent<Animator>();
    }*/

    public void PressStart()
    {
        startScreen.SetActive(false);
        characterSelectScreen.SetActive(true);
    }

    public void PressPlay()
    {
        //StartCoroutine(GameStart());
        sceneManager.LoadNextScene();
    }

    public void PressBack()
    {
        startScreen.SetActive(true);
        characterSelectScreen.SetActive(false);

        felixSelected.SetActive(false);
        sophieSelected.SetActive(false);
    }

    public void FelixButton()
    {
        felixSelected.SetActive(true);
        sophieSelected.SetActive(false);
    }

    public void SophieButton()
    {
        felixSelected.SetActive(false);
        sophieSelected.SetActive(true);
    }

    /*private IEnumerator GameStart()
    {
        //startButtonAnimator.SetTrigger("Start");
        //yield return new WaitForSeconds(1);
        //startEffect.Play();
        //yield return new WaitForSeconds(1);
        sceneManager.LoadNextScene();
    }*/
}
