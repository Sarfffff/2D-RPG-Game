using System.Collections;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityFx : MonoBehaviour
{
    protected SpriteRenderer sr;
    [Header("PopUpText")]
    [SerializeField] private GameObject popupTextPrefab;

 

    [Header("�ܻ���Ч")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float FlashDuration;
    private Material originaMat;

    [Header("ħ����Ч")]  //����ħ���˺�ʱ����ɫ�仯
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] ignitedColor;
    [SerializeField] private Color[] shockColor;

    

    [Header("������Ч")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem ChillFX;
    [SerializeField] private ParticleSystem ShockFX;

    [Header("������Ч")]
    [SerializeField] private GameObject hitFX_01;
    [SerializeField] private GameObject critHitFx;

    [Space]
    [SerializeField] private GameObject[] injurySprites;

    private GameObject myHealthBar;
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originaMat = sr.material;

        myHealthBar = GetComponentInChildren<HealthBar_UI>().gameObject;
        //HideAllInjurySprites();
    }
    private void Update()
    {
        
    }

    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1, 5);
        Vector3 positionOffSet = new Vector3(randomX,randomY,0);
        GameObject newText = Instantiate(popupTextPrefab, transform.position + positionOffSet, Quaternion.identity);
        newText.GetComponent<TextMeshPro>().text = _text;
    }

    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
        {
            myHealthBar.SetActive(false);
            sr.color = Color.clear;
        }
        else {
            myHealthBar.SetActive(true);
            sr.color = Color.white;
        }
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(FlashDuration);  //�ȴ�FlashDurationʱ��󣬿�ʼ
        sr.color = currentColor;
        sr.material = originaMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
            sr.color = Color.red;
    }

    public void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
      //  HideAllInjurySprites();
        igniteFX.Stop();
        ShockFX.Stop();
        ChillFX.Stop();
    }

    public void IgniteFxfor(float _seconds)
    {
        igniteFX.Play();
        InvokeRepeating("IgniteColorFx", 0, .3f);//�ӵ�ǰʱ�䣨�ӳ�ʱ��Ϊ0�룩��ʼ,ÿ��0.3s����һ��IgnitedColorFx����
        Invoke("CancelColorChange", _seconds);  //��_second ����ӳٵ���CancelColorChange����
        ShowInjurySprite(0);
    }

    public void ChillFxfor(float _seconds)
    {
        ChillFX.Play();
        InvokeRepeating("ChillColorFx", 0, .3f);//�ӵ�ǰʱ�䣨�ӳ�ʱ��Ϊ0�룩��ʼ,ÿ��0.3s����һ��IgnitedColorFx����
        Invoke("CancelColorChange", _seconds);  //��_second ����ӳٵ���CancelColorChange����
        ShowInjurySprite(1);
    }

    public void ShockFxfor(float _seconds)
    {
        ShockFX.Play();
        InvokeRepeating("ShockColorFx", 0, .3f);//�ӵ�ǰʱ�䣨�ӳ�ʱ��Ϊ0�룩��ʼ,ÿ��0.3s����һ��IgnitedColorFx����
        Invoke("CancelColorChange", _seconds);  //��_second ����ӳٵ���CancelColorChange����
        ShowInjurySprite(2);
    }

    private void IgniteColorFx()
    {
        if (sr.color != ignitedColor[0])  //ignitedColor[0]Ϊ��ʼ��ɫ
            sr.color = ignitedColor[0];
        else
            sr.color = ignitedColor[1]; //ignitedColor[1]Ϊ���߽׵���ɫ
    }

    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])  //chillColor[0]Ϊ��ʼ��ɫ
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])  //shockColor[0]Ϊ��ʼ��ɫ
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1]; //shockColor[1]Ϊ���߽׵���ɫ
    }

    private void ShowInjurySprite(int index)
    {
        if (index < injurySprites.Length)
        {
            injurySprites[index].SetActive(true);
        }
    }

    private void HideAllInjurySprites()
    {
        foreach (GameObject sprite in injurySprites)
        {
            sprite.SetActive(false);
        }
    }
    public void CreateHitFx(Transform _target,bool _critcal)
    {
        float zRotation = Random.Range(-90, 90);

        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0,0, zRotation);

        GameObject hitPrefab = hitFX_01; //Ĭ�ϲ��Ǳ���
        if(_critcal)
        {
            hitPrefab = critHitFx;  //�����˺�

            float yRotation = 0;
            zRotation  = Random.Range(-45,45);

            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;

            hitFxRotation = new Vector3(0,yRotation,zRotation);
        }

        GameObject newHit = Instantiate(hitPrefab, _target.position + new Vector3(xPosition,yPosition), Quaternion.identity);

        newHit.transform.Rotate(hitFxRotation);

        Destroy(newHit,.5f);
    }

}