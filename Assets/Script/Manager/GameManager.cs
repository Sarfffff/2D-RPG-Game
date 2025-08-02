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


    private Transform player;//��ҵ�ǰ��λ��
    [Header("Lost Currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;//����
    [SerializeField] private float lostCurrencyX;//��ʧ����λ��
    [SerializeField] private float lostCurrencyY;

    [SerializeField] private bool isDeadZoneDeath; // ����Ƿ�ΪDeadZone����

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        checkPoints = FindObjectsOfType<CheckPoint>();  //�ҵ������Ѿ�������checkPoints
    }
    private void Start()
    {
       player = PlayerManager.instance.player.transform;

    }
    public void RestartScene()  //���¿�ʼ����
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
        //���ö�ʧ���������
        lostCurrencyAmount = 0;
    }
    private IEnumerator LoadWithDelay(GameData _data)//�ӳټ��أ���ֹ��������δ����
    {
        yield return new WaitForSeconds(.5f);

        LoadCheckpoint(_data);
        LoadClosetCheckPoint(_data);
        LoadLostCurrency(_data);
    }
    //  2
    public void SaveData(ref GameData gameData)
    {
        // ֻ���ڷ�DeadZone����ʱ�ű��浱ǰλ����Ϊ��ʧ���λ��
        if (!isDeadZoneDeath)
        {
            gameData.lostCurrencyAmount = lostCurrencyAmount;
            gameData.lostCurrencyX = player.position.x;
            gameData.lostCurrencyY = player.position.y;
        }

        if (FindClosestCheckpoint() != null)//�������ļ��㲻Ϊ��
            gameData.closetCheckPointId = FindClosestCheckpoint().Id;//������ļ���ID��������

        gameData.checkpoints.Clear(); // ���ԭ������

        foreach(CheckPoint checkPoint in checkPoints)
        {
            gameData.checkpoints.Add(checkPoint.Id,checkPoint.activated);///�������ID�ͼ���״̬��������
        }
    }
    #region CheckPoint
    public void LoadCheckpoint(GameData gameData)
    {

        foreach (KeyValuePair<string, bool> pair in gameData.checkpoints)
        //�����ֵ䣬����ֵ�ļ���checkPoints��ÿ��������Id���е�id����ϣ������Ѿ�������ø����Ķ�������
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
        closestCheckpointId = _data.closetCheckPointId; ;//���������ID�������
        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (closestCheckpointId == checkPoint.Id)
                player.position = checkPoint.transform.position;
        }
    }


    //  1 
    public CheckPoint FindClosestCheckpoint()//�ҵ�����ļ���
    {
        float closestDistance = Mathf.Infinity;//������
        CheckPoint closestCheckpoint = null;

        foreach (var checkpoint in checkPoints)//�������еļ���
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);//������Һͼ���֮��ľ���

            if (distanceToCheckpoint < closestDistance && checkpoint.activated == true)//�������С����������Ҽ��㼤��
            {
                closestDistance = distanceToCheckpoint;//�����������
                closestCheckpoint = checkpoint;//�����������
            }

        }
        return closestCheckpoint;
    }
    #endregion
    // ��������������͵ķ���
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
