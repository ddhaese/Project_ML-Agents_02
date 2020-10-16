using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Obelix : Agent
{
    public float speed = 10;
    public float rotationSpeed = 350;

    private float carriesMenhir = 0;
    private Rigidbody body;
    private Environment environment;
    private Material matMenhirInPlace;
    private int menhirCount;
    private int menhirsDone = 0;

    public override void Initialize()
    {
        base.Initialize();
        body = GetComponent<Rigidbody>();
        environment = GetComponentInParent<Environment>();
        matMenhirInPlace = environment.matMenhirInPlace;
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0f, 1.5f, 0f);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        body.angularVelocity = Vector3.zero;
        body.velocity = Vector3.zero;
        
        environment.clearEnvironment();
        environment.spawnMenhirs();
        environment.spawnStoneHenge();

        menhirsDone = 0;
        menhirCount = environment.menhirCount;
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) // Moving fwd
        {
            actionsOut[0] = 2f;
        }
        else if (Input.GetKey(KeyCode.DownArrow)) // Turning left
        {
            actionsOut[0] = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) // Turning left
        {
            actionsOut[1] = 1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow)) // Turning right
        {
            actionsOut[1] = 2f;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(carriesMenhir);

        if (transform.localPosition.y < 0)
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (vectorAction[0] == 0 & vectorAction[1] == 0)
        {
            AddReward(-0.001f);
            return;
        }

        if (vectorAction[0] != 0)
        {
            Vector3 translation = transform.forward * speed * (vectorAction[0] * 2 - 3) * Time.deltaTime;
            transform.Translate(translation, Space.World);
        }

        if (vectorAction[1] != 0)
        {
            float rotation = rotationSpeed * (vectorAction[1] * 2 - 3) * Time.deltaTime;
            transform.Rotate(0, rotation, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Destination") & carriesMenhir == 1f)
        {
            AddReward(1f);
            carriesMenhir = 0;
            collision.gameObject.tag = "MenhirInPlace";
            collision.gameObject.GetComponent<Renderer>().material = matMenhirInPlace;
            menhirsDone++;

            if (menhirsDone == menhirCount)
            {
                EndEpisode();
            }
        }
        else if (collision.transform.CompareTag("Menhir") & carriesMenhir == 0)
        {
            AddReward(0.1f);
            Destroy(collision.gameObject);
            carriesMenhir = 1f;
        }
        else if (collision.transform.CompareTag("Menhir") & carriesMenhir == 1f)
        {
            AddReward(-0.1f);
        }
        else if (collision.transform.CompareTag("Destination") & carriesMenhir == 0f)
        {
            AddReward(-0.1f);
        }
    }
}
