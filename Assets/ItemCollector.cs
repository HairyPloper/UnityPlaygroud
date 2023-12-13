using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{

    public Text WeedCollectedText;
    private int weed_collected = 0;
    public Player playerScript;
    public FallingObjectScript fallingItemsScript;
    private void OnTriggerEnter2D(Collider2D collison)
    {
        var collidedItem = (ItemInfo)collison.gameObject.GetComponent("ItemInfo");
        if (collidedItem.ItemTag == "weed")
        {
            playerScript.playerCollectItem.Play();
            Destroy(collison.gameObject);
            weed_collected += 5;
            WeedCollectedText.text = "GRAMI: " + weed_collected + "g";
        }

        if (collidedItem.ItemTag == "frog")
        {
            playerScript.Die();
        }

        if (weed_collected != 0 && weed_collected % 50 == 0)
        {
            float buffDuration = 10f;
            StartCoroutine(playerScript.ShowJointForSecond(buffDuration));
            StartCoroutine(fallingItemsScript.UpdateAllFallingItems(buffDuration));

        }
    }

    public int GetWeedCounter()
    {
        return weed_collected;
    }
}

