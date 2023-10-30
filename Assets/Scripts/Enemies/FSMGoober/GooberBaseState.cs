using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GooberBaseState
{
    public abstract void EnterState(GooberUnit unit);
    public abstract void Update(GooberUnit unit);
    public abstract void LateUpdate(GooberUnit unit);
    public abstract void OnTriggerEnter(GooberUnit unit, Collider collider);
    public abstract void OnCollisionEnter(GooberUnit unit, Collision collider);
    public abstract void OnDisable(GooberUnit unit);
}
