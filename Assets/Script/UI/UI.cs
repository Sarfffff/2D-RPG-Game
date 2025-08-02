using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour,ISavedManager
{
    [Header("Dead")]
    [SerializeField] private GameObject endText;
    [SerializeField] private UI_FadeScene fadeScene;
    [SerializeField] private GameObject RestartButton;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_SkillTreeToolTip skillTreeToolTip;
    public UI_CraftWindow craftWindow;

    [SerializeField] private UI_volumeSlider[] volumeSettings;  //保存音量
    private void Awake()
    {
        //初始如果在主场景关闭了技能树，那么就无法再打开的bug    ，修改为启动默认调用技能树 
        if (skillTreeUI != null) // 确保 skillTreeUI 不是 null
            SwitchTo(skillTreeUI);
        fadeScene.gameObject.SetActive(true);
    }
    void Start()
    {

        SwitchTo(inGameUI); //在游戏启动时，关闭所有 UI 界面
        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        skillTreeToolTip.gameObject.SetActive(false);

 
    }
    void Update()//依据玩家按下的按键，切换不同的 UI 界面。
    {
        if (Input.GetKeyDown(KeyCode.C)) 
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(craftUI);

        if(Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionsUI);

        if (Input.GetKeyDown(KeyCode.L))
            SwitchWithKeyTo(skillTreeUI);
    }
    public void SwitchTo(GameObject _menu)
    {
        

        //遍历该组件下的所有子物体，将他们全部设置为不激活，如果传入的_menu不为空，则激活，默认设置为不激活，因为没有传入menu
        for (int i = 0; i < transform.childCount; i++) {
            bool FadeSceen = transform.GetChild(i).GetComponent<UI_FadeScene>()!=null;
            if(FadeSceen == false) 
                transform.GetChild(i).gameObject.SetActive(false);
        }
        if (transform == null) return;
        if (_menu != null)
        {
            if (AudioManager.instance != null) // 防止 AudioManager 未初始化
                AudioManager.instance.PlaySFX(7, null);
            _menu.SetActive(true);
        }
        if(GameManager.instance != null)
        {
            if (_menu == inGameUI)
                GameManager.instance.PauseGame(false); //Ingame界面游戏不会暂停，因为Ingame一直开启
            else
                GameManager.instance.PauseGame(true); //进入Ui界面，游戏暂停
        }
    }
    public void SwitchWithKeyTo(GameObject _menu)  //在用户按下对应UI的按键后，将不同的Ui界面传入赋值给menu。
    {// _menu.activeSelf检查是否已经激活，如果已经激活，则为false，未激活为true。

        if (_menu != null && _menu.activeSelf) //如果按下按键，并且界面已经打卡，则将其界面关闭，
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);  //否则打开界面

    }
    public void CheckForInGameUI()  //检查InGameUI是否激活，如果已经激活就return，没有则激活
    {
        for(int i  = 0; i < transform.childCount;i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScene>() == null)
                return;
        }
        SwitchTo(inGameUI);
    }
    public void SwitchOnEndScreen()
    {

        fadeScene.FadeOut(); //渐入
        StartCoroutine(EndScreenCorutione());
    }

    IEnumerator EndScreenCorutione()//等待1.5s后屏幕出现文本
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        RestartButton.SetActive(true);
    }
    public void RestartGameButton() =>GameManager.instance.RestartScene();

    public void LoadData(GameData gameData)  //读取保存的音量数据
    {  //定义一个字典的变量去遍历volumeSettings的每一个键值对
        foreach (KeyValuePair<string,float >  pair in gameData.volumeSettings)
        {
            //定义一个UI_volumeSlider类的 变量去遍历 该类下的每一个组件，找到与保存数据中键值相同的parameter值，将key值传入函数中
            foreach (UI_volumeSlider item in volumeSettings)
            {    
                if (item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }

    }

    public void SaveData(ref GameData gameData) //保存音量的数据   
    {
        gameData.volumeSettings.Clear();

        foreach(UI_volumeSlider item in volumeSettings)  //定义一个UI_volumeSlider类的 变量去遍历 该类下的每一个组件
        {
            gameData.volumeSettings.Add(item.parameter, item.slider.value);//将组件中的parameter和滑动条的值添加到字典中
        }
    }

}

