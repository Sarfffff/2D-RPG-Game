using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,ISavedManager
{
    protected UI ui;
    private Image skillImage;

    [SerializeField] private int skillcost;//���ܼ۸�

    [SerializeField] private string skillName;

    
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;


    public bool unLocked;  //���ܲ��Ƿ��ѽ�����
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;  //������ǰ���ܲ�֮ǰ���������ǰ�ü��ܲۡ�
    [SerializeField] private UI_SkillTreeSlot[] shouldBelocked;  //������ǰ���ܲ۵ķ�֧���ۣܲ�ֻ�ܽ�����ǰ���ܺ����֧����

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
        skillImage.color = lockedSkillColor;//δ����δred
        if (unLocked)
        {
            skillImage.color = Color.white;
        }
    }
    // ���Խ������ܲ۵ķ���
    public void TryUnlockSkillSlot()
    {
        

        // ������ǰ���ܵ�ǰ�ü��ۣܲ����δ��������ֱ�ӷ��أ����ܽ���
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unLocked == false)
            {
                return;
            }
        }

        // ������ǰ���ܵķ�֧���ۣܲ������֧���ܽ�������ֱ�ӷ��أ����ܽ�����ǰ����
        for (int i = 0; i < shouldBelocked.Length; i++)
        {
            if (shouldBelocked[i].unLocked == true)
            {
                return;
            }
        }

        // ����ǰ�ü����ѽ�������֧δ����
        unLocked = true;
        // �����󽫼��ܲ���ɫ����Ϊ��ɫ
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
