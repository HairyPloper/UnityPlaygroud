using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class FallingObjectScript : MonoBehaviour
{

    public GameObject[] items;
    private float totalProbability;
    private List<float> probabilities = new List<float>();
    public float secondSpawn = 0.5f;
    public float minTras;
    public float maxTras;


    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in items)
        {
            var itemInfo = (ItemInfo)item.GetComponent("ItemInfo");
            totalProbability += itemInfo.ItemSpawnBoost;
            Debug.Log("TotalProb" + totalProbability);
        }
        foreach (GameObject item in items)
        {
            var itemInfo = (ItemInfo)item.GetComponent("ItemInfo");
            probabilities.Add(itemInfo.ItemSpawnBoost / totalProbability);
            Debug.Log("Prob for" + itemInfo.ItemName + "is " + probabilities.Last());
        }

        StartCoroutine(ItemSpawn());
    }

    IEnumerator ItemSpawn()
    {
        while (true)
        {
            var wanted = UnityEngine.Random.Range(minTras, maxTras);
            var position = new Vector2(wanted, transform.position.y);
            var itemToSpawn = GetRandomItemToSpawn();

            GameObject gameObject = Instantiate(itemToSpawn, position, quaternion.identity);
            var rb = gameObject.GetComponent<Rigidbody2D>();
            yield return new WaitForSeconds(secondSpawn);
            Destroy(gameObject, 5f);
        }
    }

    public void ChangeItemSpawningSpeed(float gravityScale, float mass)
    {
        foreach (GameObject item in items)
        {
            ChangeItemSpeed(item, gravityScale, mass);
        }
    }
    public void ChangeAllSpawnedItemsSpeed(float gravityScale, float mass)
    {
        var allItems = GameObject.FindGameObjectsWithTag("falling_item");
        foreach (GameObject item in allItems)
        {
            ChangeItemSpeed(item, gravityScale, mass);
        }
    }

    public void ChangeItemSpeed(GameObject gameObject, float gravity, float mass)
    {
        var rb = gameObject.GetComponent<Rigidbody2D>();
        rb.mass = mass;
        rb.gravityScale = gravity;
    }

    private GameObject GetRandomItemToSpawn()
    {
        double randomValue = new System.Random().NextDouble();
        Debug.Log("Random number" + randomValue);
        double cumulativeProbability = 0;
        for (int i = 0; i < items.Count(); i++)
        {
            cumulativeProbability += probabilities[i];
            Debug.Log("CumulativeProb:" + cumulativeProbability);
            if (randomValue < cumulativeProbability)
            {
                return items[i];
            }
        }

        // This should not happen, but just in case
        return items[items.Count() - 1];
    }
    public IEnumerator UpdateAllFallingItems(float seconds)
    {
        ChangeItemSpawningSpeed(0.2f, 1);
        ChangeAllSpawnedItemsSpeed(0.2f, 1);
        yield return new WaitForSeconds(seconds);
        ChangeItemSpawningSpeed(1, 1);
        ChangeAllSpawnedItemsSpeed(1, 1);
    }

}
