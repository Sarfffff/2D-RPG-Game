using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFx : MonoBehaviour
{
    private SpriteRenderer sr;
    private float colorLooseRate;
    private void Awake() // ���� Awake��ȷ���� Start ֮ǰ��ʼ��
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    public void SetupImage(float _loosingSpeed, Sprite _spriteImage)
    {
        if (sr == null) // ��� sr �Ƿ�Ϊ��
        {
            Debug.LogError("SpriteRenderer δ��ʼ����");
            return;
        }

        sr.sprite = _spriteImage;
        colorLooseRate = _loosingSpeed;
    }

    private void Update()
    {
        if (sr == null) return; // ��ֹ sr ����������

        float alpha = sr.color.a - colorLooseRate * Time.deltaTime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (sr.color.a <= 0)
            Destroy(gameObject);
    }
}
