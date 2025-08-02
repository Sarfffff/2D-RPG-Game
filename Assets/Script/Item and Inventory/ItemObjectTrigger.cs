using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject myItemObject =>GetComponentInParent<ItemObject>();
    private void OnTriggerEnter2D(Collider2D collision)  //Åö×²Æ÷¼ì²âµ½player£¬
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<Character_Stats>().isDead)
                return;

            myItemObject.PickUpItem();
        }
    }
}
