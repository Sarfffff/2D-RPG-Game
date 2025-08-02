using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Data/Item effect/Ice and Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;//���ڴ洢һ�� GameObject ���͵�Ԥ�������ã����Ԥ����Ӧ���ǰ����˱����Ч����ص���Ϸ����

    [SerializeField] private float xVelocity;
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player player = PlayerManager.instance.player;
        bool thirdAttack = player.GetComponent<Player>().primaryattack.comboCounter == 2;//�����ҵ���Ҫ���������combocounter ==2��thirdattack=true
        if (thirdAttack) { //����ǵ����ι������򴥷�
             GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPosition.position,player.transform.rotation);//
             newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir,0);     //��ice fire���ٶ�����Ϊ  x���ٶ� * ����y���ٶ�Ϊ0  
            Destroy(newIceAndFire, 5f);
        }
    }
}
