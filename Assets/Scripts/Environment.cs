using TMPro;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public Destination destinationPrefab;
    public Menhir menhirPrefab;
    public Material matMenhirInPlace;

    public int menhirCount = 8;
    public float circleRadiusDestinations = 9;

    private Obelix obelix;
    private TextMeshPro cumulativeRewardText;
    private GameObject menhirs;
    private GameObject destinations;

    public void OnEnable()
    {
        destinations = transform.Find("Destinations").gameObject;
        menhirs = transform.Find("Menhirs").gameObject;
        cumulativeRewardText = transform.GetComponentInChildren<TextMeshPro>();
        obelix = transform.GetComponentInChildren<Obelix>();

        spawnStoneHenge();
    }

    private void FixedUpdate()
    {
        cumulativeRewardText.text = obelix.GetCumulativeReward().ToString("f2");
    }

    public Vector3 randomPosition(float up)
    {
        float x = Random.Range(-9.75f, 9.75f);
        float z = Random.Range(-9.75f, 9.75f);

        return new Vector3(x, up, z);
    }

    public void spawnStoneHenge()
    {
        for (int i = 0; i < menhirCount; i++)
        {
            GameObject newDestination = Instantiate(destinationPrefab.gameObject);

            float angle = (float)i / (float)menhirCount * 2f * Mathf.PI;
            float x = circleRadiusDestinations * Mathf.Cos(angle) + transform.position.x;
            float z = circleRadiusDestinations * Mathf.Sin(angle) + transform.position.z;

            newDestination.transform.localPosition = new Vector3(x, 1f + transform.position.y, z);
            newDestination.transform.localRotation = Quaternion.Euler(0f, angle, 0f);

            newDestination.transform.SetParent(destinations.transform);
        }
    }
    public void clearEnvironment()
    {
        foreach (Transform menhir in menhirs.transform)
        {
            GameObject.Destroy(menhir.gameObject);
        }

        foreach (Transform menhir in destinations.transform)
        {
            GameObject.Destroy(menhir.gameObject);
        }
    }

    public void spawnMenhirs()
    {
        for (int i = 0; i < menhirCount; i++)
        {
            GameObject newMenhir = Instantiate(menhirPrefab.gameObject);

            newMenhir.transform.SetParent(menhirs.transform);
            newMenhir.transform.localPosition = randomPosition(1f);
            newMenhir.transform.localRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        }
    }
}
