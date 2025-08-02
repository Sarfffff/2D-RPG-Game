using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private RectTransform myTransform;
    private Slider slider;
    private Character_Stats stats => GetComponentInParent<Character_Stats>();
        
    private void Awake()
    {
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        slider.interactable = false;  //不支持鼠标和键盘对血条进行互动
    }

    private void Start()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (stats != null && slider != null)
        {
            slider.maxValue = stats.GetMaxHealthValue();
            slider.value = stats.currentHealth;
        }
    }

    private void OnEnable()
    {
        if (stats != null)
            stats.onHealthChanged += UpdateHealthUI;

        if (entity != null)
            entity.onFilpped += FilpUI;
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.onFilpped -= FilpUI;

        if (stats != null)
            stats.onHealthChanged -= UpdateHealthUI;
    }

    private void FilpUI()
    {
        if (myTransform == null)
            myTransform = GetComponent<RectTransform>();

        myTransform.Rotate(0, 180, 0);
    }
}