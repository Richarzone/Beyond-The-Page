using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MusketeerBaseState
{
    public abstract void EnterState(MusketeerUnit unit);
    public abstract void Update(MusketeerUnit unit);
    public abstract void LateUpdate(MusketeerUnit unit);
    public abstract void OnTriggerEnter(MusketeerUnit unit);
    public abstract void OnCollisionEnter(MusketeerUnit unit);
}
