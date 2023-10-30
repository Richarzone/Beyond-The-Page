using UnityEngine;

public class MusketeerAggro : MusketeerBaseState
{
    private Vector3 walkPosition = new Vector3(0f, 0.8f, -1f);
    private Vector3 startPos;

    private float lerpSpeed;
    private float t = 0.0f;

    public override void EnterState(MusketeerUnit unit)
    {
        unit.agent.isStopped = false;
        unit.agent.speed = unit.pursueSpeed;
        Debug.Log("I am pursuing.");
        startPos = unit.spriteTransform.localPosition;
        unit.spriteTransform.localRotation = Quaternion.Euler(Vector3.zero);
        lerpSpeed = unit.billboardMusketeer.lerpSpeed;
        unit.billboardMusketeer.boolean = true;
        unit.SetAnimatorTrigger(MusketeerUnit.AnimatorTriggerStates.Walk);
    }

    public override void Update(MusketeerUnit unit)
    {
        if (unit.spriteTransform.localPosition != walkPosition)
        {
            t += lerpSpeed * Time.deltaTime;
            unit.spriteTransform.localPosition = Vector3.Lerp(startPos, walkPosition, t);
        }
        //Debug.Log(unit.agent.speed);
        unit.agent.SetDestination(unit.player.position);
        ChangeDirection(unit);
    }

    public override void LateUpdate(MusketeerUnit unit)
    {
    }

    public override void OnCollisionEnter(MusketeerUnit unit, Collision collision )
    {
    }

    public override void OnTriggerEnter(MusketeerUnit unit, Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            unit.agent.isStopped = true;
            unit.player = collider.gameObject.transform;
            unit.TransitionToState(unit.AimState);
            unit.sphereCollider.radius = unit.fleeRadius;
        }
    }

    public void ChangeDirection(MusketeerUnit unit)
    {
        if (unit.transform.eulerAngles.y < 270f && unit.transform.eulerAngles.y > 180f)
        {
            unit.TransitionToDirection(unit.FLeftState);
        }
        else if (unit.transform.eulerAngles.y < 180f && unit.transform.eulerAngles.y > 90f)
        {
            unit.TransitionToDirection(unit.FRightState);
        }
        else if (unit.transform.eulerAngles.y < 360f && unit.transform.eulerAngles.y > 270f)
        {
            unit.TransitionToDirection(unit.BLeftState);
        }
        else if (unit.transform.eulerAngles.y < 90 && unit.transform.eulerAngles.y > 0f)
        {
            unit.TransitionToDirection(unit.BRightState);
        }
    }
}