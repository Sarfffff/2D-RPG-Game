using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arror_Controller : MonoBehaviour
{

    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool flipped;
    [SerializeField] private bool canMove;

    protected Character_Stats myStats;

    private void Update()
    {
        if (canMove)
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }
    public void SetupArrow(float speed,Character_Stats _mystats)
    {
        xVelocity =speed;
        myStats = _mystats;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))  //�����ײ����Χ�ڼ�⵽����targetLayerName��game object
        {

            myStats.DoDamage(collision.GetComponent<Character_Stats>());  //�Ը���������˺�
            StuckInto(collision);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckInto(collision);
        }
    }

    private void StuckInto(Collider2D collision)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
        Destroy(gameObject,5f);
    }

    public void Filp() //��ת��ʸ�ķ����Լ��ٶ�
    {
        if (flipped)
            return;

        xVelocity = xVelocity * -1;
        flipped = true;
        transform.Rotate(0, 180, 0);

        targetLayerName = "Enemy";
    }
}
