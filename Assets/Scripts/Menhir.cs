using UnityEngine;

public class Menhir : MonoBehaviour
{
    public float speed = 1;

    private float randomizedSpeed = 0f;
    private float nextActionTime = -1f;
    private Vector3 targetPosition;
    private Environment environment;

    private void FixedUpdate()
    {
        if (speed > 0f)
        {
            move();
        }
    }

    private void move()
    {
        if (environment == null)
        {
            environment = GetComponentInParent<Environment>();
        }

        if (Time.fixedTime >= nextActionTime)
        {
            randomizedSpeed = speed * Random.Range(.5f, 1.5f);
            targetPosition = environment.randomPosition(1f);
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);
            float timeToGetThere = Vector3.Distance(transform.localPosition, targetPosition) / randomizedSpeed;
            nextActionTime = Time.fixedTime + timeToGetThere;
        }
        else
        {
            Vector3 moveVector = randomizedSpeed * transform.forward * Time.fixedDeltaTime;

            if (moveVector.magnitude <= Vector3.Distance(transform.localPosition, targetPosition))
            {
                transform.localPosition += moveVector;
            }
            else
            {
                transform.localPosition = targetPosition;
                nextActionTime = Time.fixedTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        nextActionTime = Time.fixedTime;
    }
}
