using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DevilBaseState
{
    public abstract void EnterState(DevilUnit unit);
    public virtual void Update(DevilUnit unit)
    {

    }

    public virtual void LateUpdate(DevilUnit unit)
    {

    }

    public virtual void OnTriggerEnter(DevilUnit unit, Collider collider)
    {

    }

    public virtual void OnTriggerExit(DevilUnit unit, Collider collider)
    {

    }

    public virtual void OnCollisionEnter(DevilUnit unit, Collision collision)
    {

    }

    public virtual void OnDisable(DevilUnit unit)
    {

    }
}
