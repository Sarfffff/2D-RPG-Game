using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Data/Item effect/Ice and Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;//用于存储一个 GameObject 类型的预制体引用，这个预制体应该是包含了冰与火效果相关的游戏对象

    [SerializeField] private float xVelocity;
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player player = PlayerManager.instance.player;
        bool thirdAttack = player.GetComponent<Player>().primaryattack.comboCounter == 2;//如果玩家的主要攻击里面的combocounter ==2，thirdattack=true
        if (thirdAttack) { //如果是第三次攻击，则触发
             GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPosition.position,player.transform.rotation);//
             newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir,0);     //将ice fire的速度设置为  x的速度 * 朝向，y的速度为0  
            Destroy(newIceAndFire, 5f);
        }
    }
}
