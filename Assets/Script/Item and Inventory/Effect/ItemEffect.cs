using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//[CreateAssetMenu(fileName = "Item effect", menuName = "Data/Item effect")]

public class ItemEffect : ScriptableObject 
{
    [TextArea]
    public string effectDescription;
    public virtual void ExecuteEffect(Transform _enemyPosition)  //提供一个父类供之类进行重写每个效果effect
    {
        Debug.Log("Effect executed");
    }

}
