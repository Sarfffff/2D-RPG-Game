using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour  //�浵ϵͳ�ĺ��Ĺ�����
{

    public static SaveManager instance;  //����ģʽ
    private GameData gameData;//���ڴ洢��Ϸ����

    [SerializeField] private bool encryptData;
    [SerializeField] private string fileName;  //�浵�ļ���
    private List<ISavedManager> savedManagers = new List<ISavedManager>();  //���еĴ浵���
    private FindDataHandler dataHandler;  // ���ݴ�����

    [ContextMenu("ɾ���Ѿ�������ļ�")] 
    public void DeleteSaveData()
    {
        dataHandler = new FindDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();
    }
    private void Awake()
    {
        if(instance!=null)
            Destroy(instance.gameObject);
        else
            instance = this;
        dataHandler = new FindDataHandler(Application.persistentDataPath, fileName, encryptData);
        savedManagers = FindAllSaveManger();  //���ú���
        LoadGame();
    }

    public void Start()
    {
    }
    public void NewGame()
    {
        gameData = new GameData();
    }
    public void LoadGame()
    {
        gameData = dataHandler.Load();  //�����ݴ������д����ļ��У�֮ǰ����Ĵ浵 ��ֵ��gamedata���������ͣ�
        if(this.gameData == null)
        { 
            Debug.Log("�޴浵���ݣ���������Ϸ");
            NewGame();
        }
        // ֪ͨ���������������
        foreach (ISavedManager savedManager in savedManagers)
        {
            savedManager.LoadData(gameData);
        }

    }
    public void SaveGame() {
        // �ռ��������������
        foreach (ISavedManager saveManager in savedManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        //ʹ�����ݴ�������������
        dataHandler.Save(gameData);
    }
    private void OnApplicationQuit()
    {//�˳�ʱ�Զ�����
        SaveGame();
    }
    //ʹ��LINQ��������ʵ��ISavedManager��MonoBehaviour     ��������б�
    private List<ISavedManager> FindAllSaveManger()
    {
        IEnumerable<ISavedManager> savedManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISavedManager>();
        return new List<ISavedManager>(savedManagers);
    }
    public bool HasSavedData()
    {
        if(dataHandler.Load()!=null)  //�������ĳ���������������ݣ�Ҳ�����������return true
            return true;

        return false;
    }

}
