using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GubGubBaseState
{
    public abstract void EnterState(GubGubUnit unit);
    public abstract void Update(GubGubUnit unit);
    public abstract void LateUpdate(GubGubUnit unit);
    public abstract void OnTriggerEnter(GubGubUnit unit, Collider collider);
    public abstract void OnCollisionEnter(GubGubUnit unit, Collision collider);
    public abstract void OnDisable(GubGubUnit unit);
}
