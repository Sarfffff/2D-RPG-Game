using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFx : EntityFx
{
    [Header("��Ļ��")]
    private CinemachineImpulseSource screenShake;  // ��Ļ��
    [SerializeField] private float shakeMultiplier;
    public Vector3 defaultImpactShack;  // ��ͨ��������ǿ��
    public Vector3 shakeHighDamage;  // ���˺���������ǿ��
    [Space]

    [Header("��ӰЧ��")]
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
                Debug.LogError("δ�ҵ�AfterImageFx�����SpriteRenderer���");
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