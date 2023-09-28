using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    private float clipLenght;

    private void Start()
    {
        clipLenght = transitionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    public void LoadNextScene()
    {
        StartCoroutine(SceneTransition(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadStartScene()
    {
        StartCoroutine(SceneTransition(0));
    }

    private IEnumerator SceneTransition(int sceneIndex)
    {
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(clipLenght);

        SceneManager.LoadScene(sceneIndex);
    }
}