using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private string statName;

    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    private UI ui;
    [SerializeField] private string statDescription;

    private void OnValidate() 
    {
        gameObject.name = "Stat  - "+ statName;
        if(statNameText != null )
            statNameText.text = statName;
    }
    void Start()
    {
        UpdateStatValueUI();
        ui = GetComponentInParent<UI>();

    }
    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if(playerStats != null ) {
            statValueText.text = playerStats.GetType(statType).GetValue().ToString();
        }
        switch(statType)
        {
            case StatType.health:
                statValueText.text =playerStats.GetMaxHealthValue().ToString();
                break;
            case StatType.damage:
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
                break;
            case StatType.critChance:
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();
                break;
            case StatType.critPower:
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
                break;
            case StatType.evasion:
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();
                break;
            case StatType.magicRes:
                statValueText.text = (playerStats.magicResistance.GetValue() + (playerStats.intelligence.GetValue() * 3)).ToString();
                break;
            default:
                break;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ui != null && ui.statToolTip != null)
        {
            ui.statToolTip.HideStatToolTip();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ui != null && ui.statToolTip != null)
        {
            ui.statToolTip.ShowStatToolTip(statDescription);
        }
    }
}
