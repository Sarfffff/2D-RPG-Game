using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavedManager  //����浵ϵͳ�Ľӿ�
{
    void LoadData(GameData gameData);    // ��GameData�������ݵ����
    void SaveData(ref GameData gameData); // ��������ݱ��浽GameData

}
