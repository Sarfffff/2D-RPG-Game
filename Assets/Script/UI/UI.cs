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

    [SerializeField] private UI_volumeSlider[] volumeSettings;  //��������
    private void Awake()
    {
        //��ʼ������������ر��˼���������ô���޷��ٴ򿪵�bug    ���޸�Ϊ����Ĭ�ϵ��ü����� 
        if (skillTreeUI != null) // ȷ�� skillTreeUI ���� null
            SwitchTo(skillTreeUI);
        fadeScene.gameObject.SetActive(true);
    }
    void Start()
    {

        SwitchTo(inGameUI); //����Ϸ����ʱ���ر����� UI ����
        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        skillTreeToolTip.gameObject.SetActive(false);

 
    }
    void Update()//������Ұ��µİ������л���ͬ�� UI ���档
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
        

        //����������µ����������壬������ȫ������Ϊ�������������_menu��Ϊ�գ��򼤻Ĭ������Ϊ�������Ϊû�д���menu
        for (int i = 0; i < transform.childCount; i++) {
            bool FadeSceen = transform.GetChild(i).GetComponent<UI_FadeScene>()!=null;
            if(FadeSceen == false) 
                transform.GetChild(i).gameObject.SetActive(false);
        }
        if (transform == null) return;
        if (_menu != null)
        {
            if (AudioManager.instance != null) // ��ֹ AudioManager δ��ʼ��
                AudioManager.instance.PlaySFX(7, null);
            _menu.SetActive(true);
        }
        if(GameManager.instance != null)
        {
            if (_menu == inGameUI)
                GameManager.instance.PauseGame(false); //Ingame������Ϸ������ͣ����ΪIngameһֱ����
            else
                GameManager.instance.PauseGame(true); //����Ui���棬��Ϸ��ͣ
        }
    }
    public void SwitchWithKeyTo(GameObject _menu)  //���û����¶�ӦUI�İ����󣬽���ͬ��Ui���洫�븳ֵ��menu��
    {// _menu.activeSelf����Ƿ��Ѿ��������Ѿ������Ϊfalse��δ����Ϊtrue��

        if (_menu != null && _menu.activeSelf) //������°��������ҽ����Ѿ��򿨣��������رգ�
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);  //����򿪽���

    }
    public void CheckForInGameUI()  //���InGameUI�Ƿ񼤻����Ѿ������return��û���򼤻�
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

        fadeScene.FadeOut(); //����
        StartCoroutine(EndScreenCorutione());
    }

    IEnumerator EndScreenCorutione()//�ȴ�1.5s����Ļ�����ı�
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        RestartButton.SetActive(true);
    }
    public void RestartGameButton() =>GameManager.instance.RestartScene();

    public void LoadData(GameData gameData)  //��ȡ�������������
    {  //����һ���ֵ�ı���ȥ����volumeSettings��ÿһ����ֵ��
        foreach (KeyValuePair<string,float >  pair in gameData.volumeSettings)
        {
            //����һ��UI_volumeSlider��� ����ȥ���� �����µ�ÿһ��������ҵ��뱣�������м�ֵ��ͬ��parameterֵ����keyֵ���뺯����
            foreach (UI_volumeSlider item in volumeSettings)
            {    
                if (item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }

    }

    public void SaveData(ref GameData gameData) //��������������   
    {
        gameData.volumeSettings.Clear();

        foreach(UI_volumeSlider item in volumeSettings)  //����һ��UI_volumeSlider��� ����ȥ���� �����µ�ÿһ�����
        {
            gameData.volumeSettings.Add(item.parameter, item.slider.value);//������е�parameter�ͻ�������ֵ��ӵ��ֵ���
        }
    }

}

