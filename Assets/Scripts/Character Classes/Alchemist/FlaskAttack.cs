using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskAttack : MonoBehaviour
{
    [SerializeField] private GameObject rightFlask;
    [SerializeField] private GameObject leftFlask;
    [SerializeField] private float deacivationTime;

    private bool side = true;

    public void DeactivateFlask()
    {
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(deacivationTime);

        if (side)
        {
            rightFlask.SetActive(true);
        }
        else
        {
            leftFlask.SetActive(true);
        }
    }

}
