using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISavedManager
{
    public static GameManager instance;
    [SerializeField] private CheckPoint[] checkPoints;

    [SerializeField] public string closestCheckpointId;


    private Transform player;//玩家当前的位置
    [Header("Lost Currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;//数量
    [SerializeField] private float lostCurrencyX;//丢失灵魂的位置
    [SerializeField] private float lostCurrencyY;

    [SerializeField] private bool isDeadZoneDeath; // 标记是否为DeadZone死亡

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        checkPoints = FindObjectsOfType<CheckPoint>();  //找到所有已经经过的checkPoints
    }
    private void Start()
    {
       player = PlayerManager.instance.player.transform;

    }
    public void RestartScene()  //重新开始场景
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


    public void LoadData(GameData gameData) => StartCoroutine(LoadWithDelay(gameData));

    private void LoadLostCurrency(GameData gameData)
    {
        lostCurrencyAmount = gameData.lostCurrencyAmount;
        lostCurrencyX = gameData.lostCurrencyX;
        lostCurrencyY = gameData.lostCurrencyY;

        if(lostCurrencyAmount > 0 )
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }
        //重置丢失的灵魂数量
        lostCurrencyAmount = 0;
    }
    private IEnumerator LoadWithDelay(GameData _data)//延迟加载，防止其他数据未加载
    {
        yield return new WaitForSeconds(.5f);

        LoadCheckpoint(_data);
        LoadClosetCheckPoint(_data);
        LoadLostCurrency(_data);
    }
    //  2
    public void SaveData(ref GameData gameData)
    {
        // 只有在非DeadZone死亡时才保存当前位置作为丢失灵魂位置
        if (!isDeadZoneDeath)
        {
            gameData.lostCurrencyAmount = lostCurrencyAmount;
            gameData.lostCurrencyX = player.position.x;
            gameData.lostCurrencyY = player.position.y;
        }

        if (FindClosestCheckpoint() != null)//如果最近的检查点不为空
            gameData.closetCheckPointId = FindClosestCheckpoint().Id;//将最近的检查点ID存入数据

        gameData.checkpoints.Clear(); // 清空原有数据

        foreach(CheckPoint checkPoint in checkPoints)
        {
            gameData.checkpoints.Add(checkPoint.Id,checkPoint.activated);///将检查点的ID和激活状态存入数据
        }
    }
    #region CheckPoint
    public void LoadCheckpoint(GameData gameData)
    {

        foreach (KeyValuePair<string, bool> pair in gameData.checkpoints)
        //遍历字典，如果字典的键与checkPoints（每个复活点的Id）中的id相符合，并且已经激活，则让复活点的动画播放
        {
            foreach (CheckPoint check in checkPoints)
            {
                if (check.Id == pair.Key)
                {
                    if (pair.Value == true)
                        check.ActivateCheckPoint();
                }
            }
        }

    }

    //  3 
    private void LoadClosetCheckPoint(GameData _data)
    {
        if(_data.closetCheckPointId == null)    
            return;
        closestCheckpointId = _data.closetCheckPointId; ;//将最近检查点ID存入变量
        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (closestCheckpointId == checkPoint.Id)
                player.position = checkPoint.transform.position;
        }
    }


    //  1 
    public CheckPoint FindClosestCheckpoint()//找到最近的检查点
    {
        float closestDistance = Mathf.Infinity;//正无穷
        CheckPoint closestCheckpoint = null;

        foreach (var checkpoint in checkPoints)//遍历所有的检查点
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);//计算玩家和检查点之间的距离

            if (distanceToCheckpoint < closestDistance && checkpoint.activated == true)//如果距离小于最近距离且检查点激活
            {
                closestDistance = distanceToCheckpoint;//更新最近距离
                closestCheckpoint = checkpoint;//更新最近检查点
            }

        }
        return closestCheckpoint;
    }
    #endregion
    // 添加设置死亡类型的方法
    public void SetDeathType(bool isDeadZone)
    {
        isDeadZoneDeath = isDeadZone;
    }
    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
