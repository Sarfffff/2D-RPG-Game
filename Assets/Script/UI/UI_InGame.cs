using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;

    [SerializeField] private float increaseRate = 100;  //增加的速率
    [SerializeField] private float soulsAmount;  
    [SerializeField] private TextMeshProUGUI currentSouls;  //当前的灵魂


    void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;

        skills = SkillManager.instance;
    }


    void Update()
    {
        UpdataSoulsUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
            SetCooldownOf(parryImage);

        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked)
            SetCooldownOf(swordImage);

        if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackHoleUnlocked)
            SetCooldownOf(blackholeImage);

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.Instance.GetEquipment(EquipmentType.Flask) != null)//必须获取药水
            SetCooldownOf(flaskImage);



        CheckCoolDownOf(dashImage, skills.dash.cooldown);
        CheckCoolDownOf(parryImage, skills.parry.cooldown);
        CheckCoolDownOf(crystalImage, skills.crystal.cooldown);
        CheckCoolDownOf(swordImage, skills.sword.cooldown);
        CheckCoolDownOf(blackholeImage, skills.blackhole.cooldown);
        CheckCoolDownOf(flaskImage, Inventory.Instance.flaskCooldown);
    }

    private void UpdataSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrency())
        {
            soulsAmount += Time.deltaTime * increaseRate;
        }
        else
        {
            soulsAmount = PlayerManager.instance.GetCurrency();
        }


        currentSouls.text = ((int)soulsAmount).ToString();
    }

    private void UpdateHealthUI()//更新血条值
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }


    private void SetCooldownOf(Image _image)//设置技能冷却时间动画
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCoolDownOf(Image _image, float _coolDown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _coolDown * Time.deltaTime;
    }
}
