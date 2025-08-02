using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavedManager  //定义存档系统的接口
{
    void LoadData(GameData gameData);    // 从GameData加载数据到组件
    void SaveData(ref GameData gameData); // 将组件数据保存到GameData

}
