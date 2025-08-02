using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFx : MonoBehaviour
{
    private SpriteRenderer sr;
    private float colorLooseRate;
    private void Awake() // 改用 Awake，确保在 Start 之前初始化
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    public void SetupImage(float _loosingSpeed, Sprite _spriteImage)
    {
        if (sr == null) // 检查 sr 是否为空
        {
            Debug.LogError("SpriteRenderer 未初始化！");
            return;
        }

        sr.sprite = _spriteImage;
        colorLooseRate = _loosingSpeed;
    }

    private void Update()
    {
        if (sr == null) return; // 防止 sr 被意外销毁

        float alpha = sr.color.a - colorLooseRate * Time.deltaTime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (sr.color.a <= 0)
            Destroy(gameObject);
    }
}
