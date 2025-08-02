using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;//��Ч��С����
    [SerializeField] private AudioSource[] sfx;//��Ч
    [SerializeField] private AudioSource[] bgm;//��������


    public bool playBgm;
    private int bgmIndex;

    private bool canPlaySFX;
 
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        Invoke("AllowSFX", 1f);

    }
    private void Update()
    {
        if (!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)//�����������û�в���
                PlayBGM(bgmIndex);
        }
    }


    public void PlaySFX(int _sfxIndex, Transform _source)//������Ч
    {

        if (canPlaySFX == false)
            return;


        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance)//�����Զ������
            return;

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.15f);//������Ч������
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();//ֹͣ��Ч

    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));//ֹͣ��Ӧ��Ч�Ĳ��ţ������𽥼�С����


    private IEnumerator DecreaseVolume(AudioSource _audio)//��С����
    {
        float defaultVolume = _audio.volume;//����Ĭ������

        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;//ÿ�μ�С20%����
            yield return new WaitForSeconds(.65f);

            if (_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;//�����ָ�
                break;
            }
        }
    }


    public void PlayRandomBGM()//���������������
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }



    public void PlayBGM(int _bgmIndex)//���ű�������
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[bgmIndex].Play();
    }


    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
    private void AllowSFX() => canPlaySFX = true;

}
