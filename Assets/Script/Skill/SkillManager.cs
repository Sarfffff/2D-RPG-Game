using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{//类是一个单例类，负责管理所有技能的实例，并且分别引用不同类型的技能实例。
//SkillManager 类作为单例类，负责管理所有技能的实例。
 //这样可以方便地在其他脚本中访问和调用各个技能，避免了在每个需要使用技能的地方都要手动查找和获取技能组件的麻烦。
 //同时，单例模式确保了 SkillManager 只有一个实例，避免了多个实例带来的冲突和混乱。
    public static SkillManager instance;
    public Dash_Skill dash{  get; private set; }
    public Clone_Skill clone{ get; private set; }
    public Sword_Skill sword{ get; private set; }
    public Blackhole_Skill blackhole{ get; private set; }
    public Crystal_Skill crystal{ get; private set; }
    public Parry_Skill parry{ get; private set; }   
    public Dodge_Skill dodge{ get; private set; }   
    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
        clone = GetComponent<Clone_Skill>();
        sword = GetComponent<Sword_Skill>();
        blackhole = GetComponent<Blackhole_Skill>();
        crystal = GetComponent<Crystal_Skill>();
        parry = GetComponent<Parry_Skill>();
        dodge = GetComponent<Dodge_Skill>();
    }
}
