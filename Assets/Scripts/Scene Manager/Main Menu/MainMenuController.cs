using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Management")]
    [SerializeField] private SceneLoaderManager sceneManager;

    [Header("UI")]
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject characterSelectScreen;
    [SerializeField] private GameObject bestiaryScreen;
    [SerializeField] private Button MultiplayerButton;
    [SerializeField] private Button SingleplayerButton;
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button backBestiary;

    [Header("Character Select")]
    [SerializeField] private GameObject felixButton;
    [SerializeField] private GameObject sophieButton;

    [SerializeField] private Sprite felixPruebaSprite;
    [SerializeField] private Sprite sophiePruebaSprite;

    [SerializeField] private bool felixSelected = false;
    [SerializeField] private bool sophieSelected = false;

    // Se guardan la informacion de image y button
    private Image felixImage;
    private Button felixInfoButton;
    private Image sophieImage;
    private Button sophieInfoButton;
    private Sprite felixSprite;
    private Sprite sophieSprite;

    /* Declaracion de spirte state
    public SpriteState sprStateFelix = new SpriteState();
    public SpriteState sprStateSophie = new SpriteState();*/

    private void Start()
    {
        /*Cursor.visible = false;
        startButtonAnimator = startButton.GetComponent<Animator>();*/
        //felixButton.spriteState = sprStateFelix;
        felixImage = felixButton.GetComponent<Image>();
        felixInfoButton = felixButton.GetComponent<Button>();
        sophieImage = sophieButton.GetComponent<Image>();
        sophieInfoButton = sophieButton.GetComponent<Button>();

    }

    public void PressMultiplayer()
    {
        //sceneManager.LoadNextScene();
        SceneManager.LoadScene("Net Lobby Scene");
    }

    public void PressSinglePlayer()
    {
        startScreen.SetActive(false);
        characterSelectScreen.SetActive(true);
    }

    public void PressBestiary()
    {
        startScreen.SetActive(false);
        bestiaryScreen.SetActive(true);
    }

    public void PressExit()
    {
        Application.Quit();
    }

    public void PressPlay()
    {
        //StartCoroutine(GameStart());
        //sceneManager.LoadNextScene();
        if (felixSelected == false && sophieSelected == false)
        {
            playButton.gameObject.SetActive(false);
        }
        else
        {
            playButton.gameObject.SetActive(true);
            //SceneManager.LoadScene("Game View");
            SceneManager.LoadScene("EnemyTest");
        }
    }

    public void CheckSelected()
    {
        if (felixSelected == true)
        {
            //felixButton.spriteState = sprStateFelix;
            //felixButton.sprStateFelix.pressedSprite = FelixSelectedCharacterBTP;
        } else if (sophieSelected == true){ 

        } else if (felixSelected == false)
        {

        } else if (sophieSelected == false)
        {
                
        }
    }

    public void PressBackCharacterSelect()
    {
        startScreen.SetActive(true);
        characterSelectScreen.SetActive(false);

        //felixSelected.SetActive(false);
        //sophieSelected.SetActive(false);
    }

    public void PressBackBestiary()
    {
        startScreen.SetActive(true);
        bestiaryScreen.SetActive(false);
    }

    public void FelixButton()
    {
        felixSelected = true;
        sophieSelected = false;
        
        playButton.gameObject.SetActive(true);
        felixImage.sprite = felixInfoButton.spriteState.pressedSprite;
        sophieImage.sprite = sophiePruebaSprite;
    }

    public void SophieButton()
    {
        felixSelected = false;
        sophieSelected = true;

        playButton.gameObject.SetActive(true);
        sophieImage.sprite = sophieInfoButton.spriteState.pressedSprite;
        felixImage.sprite = felixPruebaSprite;
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
