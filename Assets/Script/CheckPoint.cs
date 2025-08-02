using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string Id;
    public bool activated;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    [ContextMenu("���������Id")] //����һ����ť���������ID
    public void GenerateId()
    {
        Id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            ActivateCheckPoint();
            SaveManager.instance.SaveGame();//�������㱣����Ϸ
        }

    }

    public void ActivateCheckPoint()
    {
        if (activated == false)
        {
            AudioManager.instance.PlaySFX(5, null);           
        }
        activated = true;
        anim.SetBool("Active", true);
    }
}
