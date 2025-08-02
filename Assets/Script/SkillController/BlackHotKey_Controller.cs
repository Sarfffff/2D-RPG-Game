using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;  //�Զ��尴��
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackHole_Skill_Controller blackhole;

    public void SetupHotKey(KeyCode _myHotKey,Transform _myEnemy, BlackHole_Skill_Controller _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText=  GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackhole = _myBlackHole;


        myHotKey = _myHotKey;
        myText.text = myHotKey.ToString();
    }
    private void Update()
    {
        if (Input.GetKeyUp(myHotKey)) {
            blackhole.AddEnemyToList(myEnemy);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
