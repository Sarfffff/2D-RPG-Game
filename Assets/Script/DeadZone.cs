using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [Header("Lost Currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;

    [SerializeField] private Vector2 spawnOffset = new Vector2(1f, 0.5f);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Character_Stats>()!= null)
        {
            // 设置死亡类型为DeadZone死亡
           GameManager.instance.SetDeathType(true);
            collision.GetComponent<Character_Stats>().KillEntity();//进入deadzone，调用KillEntity，直接死亡
                                                                

            // 获取玩家当前的货币数量
            int currentCurrency = GameManager.instance.lostCurrencyAmount;
            Debug.Log(currentCurrency);
            if (GameManager.instance.closestCheckpointId != null&& currentCurrency > 0)
            {
                Vector3 basePosition = GameManager.instance.FindClosestCheckpoint().transform.position;
                Vector3 offsetPosition = basePosition + new Vector3(spawnOffset.x, spawnOffset.y, 0);

                GameObject newLostCurrency = Instantiate(lostCurrencyPrefab,offsetPosition,Quaternion.identity);
                newLostCurrency.GetComponent<LostCurrencyController>().currency = currentCurrency;
            }
            //重置丢失的灵魂数量
            currentCurrency = 0;

            // 重置死亡类型
            GameManager.instance.SetDeathType(false);
        }
        
    }
    
}
