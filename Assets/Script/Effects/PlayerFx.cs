using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFx : EntityFx
{
    [Header("屏幕震动")]
    private CinemachineImpulseSource screenShake;  // 屏幕震动
    [SerializeField] private float shakeMultiplier;
    public Vector3 defaultImpactShack;  // 普通攻击的震动强度
    public Vector3 shakeHighDamage;  // 高伤害攻击的震动强度
    [Space]

    [Header("残影效果")]
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;

    [Space]
    [SerializeField] private ParticleSystem dustFx;

    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0 && afterImagePrefab != null)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);

            AfterImageFx afterImage = newAfterImage.GetComponent<AfterImageFx>();
            if (afterImage != null && sr != null)
            {
                afterImage.SetupImage(colorLooseRate, sr.sprite);
            }
            else
            {
                Debug.LogError("未找到AfterImageFx组件或SpriteRenderer组件");
            }
        }
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * PlayerManager.instance.player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void PlayDustFx()
    {
        if (dustFx != null)
            dustFx.Play();
    }
}