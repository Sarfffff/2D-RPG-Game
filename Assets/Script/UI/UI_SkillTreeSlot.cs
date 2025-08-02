using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,ISavedManager
{
    protected UI ui;
    private Image skillImage;

    [SerializeField] private int skillcost;//技能价格

    [SerializeField] private string skillName;

    
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;


    public bool unLocked;  //技能槽是否已解锁。
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;  //解锁当前技能槽之前必须解锁的前置技能槽。
    [SerializeField] private UI_SkillTreeSlot[] shouldBelocked;  //解锁当前技能槽的分支技能槽，只能解锁当前技能和其分支技能

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(()=> TryUnlockSkillSlot());
        
    }
    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }
    private void Start()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();
        skillImage.color = lockedSkillColor;//未解锁未red
        if (unLocked)
        {
            skillImage.color = Color.white;
        }
    }
    // 尝试解锁技能槽的方法
    public void TryUnlockSkillSlot()
    {
        

        // 遍历当前技能的前置技能槽，如果未解锁，则直接返回，不能解锁
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unLocked == false)
            {
                return;
            }
        }

        // 遍历当前技能的分支技能槽，如果分支技能解锁，则直接返回，不能解锁当前技能
        for (int i = 0; i < shouldBelocked.Length; i++)
        {
            if (shouldBelocked[i].unLocked == true)
            {
                return;
            }
        }

        // 满足前置技能已解锁，分支未解锁
        unLocked = true;
        // 解锁后将技能槽颜色设置为绿色
        skillImage.color = Color.white;

        if (PlayerManager.instance.HaveEnoughMOney(skillcost) == false)
            return;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTreeToolTip.ShowSkillToolTip(skillDescription,skillName, skillcost);

        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTreeToolTip.HideSkillToolTip();
    }

    public void LoadData(GameData gameData)
    {
         if(gameData.skillTree.TryGetValue(skillName, out bool value))
        {
            unLocked = value;
        }
    }

    public void SaveData(ref GameData gameData)
    {
        if(gameData.skillTree.TryGetValue(skillName,out bool value))
        {
            gameData.skillTree.Remove(skillName);
            gameData.skillTree.Add(skillName,unLocked);
        }
        else 
        {
            gameData.skillTree.Add(skillName, unLocked);    
        }
    }
}
