using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour  //存档系统的核心管理类
{

    public static SaveManager instance;  //单例模式
    private GameData gameData;//用于存储游戏数据

    [SerializeField] private bool encryptData;
    [SerializeField] private string fileName;  //存档文件名
    private List<ISavedManager> savedManagers = new List<ISavedManager>();  //所有的存档组件
    private FindDataHandler dataHandler;  // 数据处理器

    [ContextMenu("删除已经保存的文件")] 
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
        savedManagers = FindAllSaveManger();  //调用函数
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
        gameData = dataHandler.Load();  //从数据处理器中处理（文件夹）之前保存的存档 赋值给gamedata（数据类型）
        if(this.gameData == null)
        { 
            Debug.Log("无存档数据，创建新游戏");
            NewGame();
        }
        // 通知所有组件加载数据
        foreach (ISavedManager savedManager in savedManagers)
        {
            savedManager.LoadData(gameData);
        }

    }
    public void SaveGame() {
        // 收集所有组件的数据
        foreach (ISavedManager saveManager in savedManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        //使用数据处理器保存数据
        dataHandler.Save(gameData);
    }
    private void OnApplicationQuit()
    {//退出时自动保存
        SaveGame();
    }
    //使用LINQ查找所有实现ISavedManager的MonoBehaviour     返回组件列表
    private List<ISavedManager> FindAllSaveManger()
    {
        IEnumerable<ISavedManager> savedManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISavedManager>();
        return new List<ISavedManager>(savedManagers);
    }
    public bool HasSavedData()
    {
        if(dataHandler.Load()!=null)  //如果保存的场景的里面存在数据，也就是游玩过，return true
            return true;

        return false;
    }

}
