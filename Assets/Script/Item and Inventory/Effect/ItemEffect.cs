using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//[CreateAssetMenu(fileName = "Item effect", menuName = "Data/Item effect")]

public class ItemEffect : ScriptableObject 
{
    [TextArea]
    public string effectDescription;
    public virtual void ExecuteEffect(Transform _enemyPosition)  //�ṩһ�����๩֮�������дÿ��Ч��effect
    {
        Debug.Log("Effect executed");
    }

}
