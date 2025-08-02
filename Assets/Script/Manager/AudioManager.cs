using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;//音效最小距离
    [SerializeField] private AudioSource[] sfx;//音效
    [SerializeField] private AudioSource[] bgm;//背景音乐


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
            if (!bgm[bgmIndex].isPlaying)//如果背景音乐没有播放
                PlayBGM(bgmIndex);
        }
    }


    public void PlaySFX(int _sfxIndex, Transform _source)//播放音效
    {

        if (canPlaySFX == false)
            return;


        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance)//距离过远不播放
            return;

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.15f);//设置音效的音调
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();//停止音效

    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));//停止对应音效的播放，并且逐渐减小音量


    private IEnumerator DecreaseVolume(AudioSource _audio)//减小音量
    {
        float defaultVolume = _audio.volume;//保存默认音量

        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;//每次减小20%音量
            yield return new WaitForSeconds(.65f);

            if (_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;//音量恢复
                break;
            }
        }
    }


    public void PlayRandomBGM()//播放随机背景音乐
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }



    public void PlayBGM(int _bgmIndex)//播放背景音乐
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
