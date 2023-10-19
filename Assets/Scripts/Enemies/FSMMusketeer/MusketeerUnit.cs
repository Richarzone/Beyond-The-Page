using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketeerUnit : MonoBehaviour
{
    private Animator animator;
    public Animator Animator
    {
        get { return animator; }
    }

    private MusketeerBaseState currentState;
    public MusketeerBaseState CurrentState
    {
        get { return currentState; }
    }

    public readonly MusketeerIdle IdleState = new MusketeerIdle();
    public readonly MusketeerWalk WalkState = new MusketeerWalk();

    // Start is called before the first frame update
    void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ContinueAim()
    {
        throw new NotImplementedException();
    }
}
