using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MusketeerBaseState
{
    public abstract void EnterState(MusketeerUnit unit);
    public virtual void Update(MusketeerUnit unit)
    {

    }

    public virtual void LateUpdate(MusketeerUnit unit)
    {

    }

    public virtual void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {

    }

    public virtual void OnCollisionEnter(MusketeerUnit unit, Collision collision)
    {

    }
}
