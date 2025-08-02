using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpTextFx : MonoBehaviour
{
    private TextMeshPro mytext;
    [SerializeField] private float Speed;
    [SerializeField] private float disappearinSpeed;
    [SerializeField] private float colorDisappearSpeed;

    [SerializeField] private float lifeTime;

    private float textTimer;
    private void Start()
    {
        mytext = GetComponent<TextMeshPro>();
        textTimer = lifeTime;
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1f), Speed * Time.deltaTime);
        textTimer -= Time.deltaTime;
        if(textTimer < 0)
        {
            float alpha = mytext.color.a - colorDisappearSpeed * Time.deltaTime;
            mytext.color = new Color(mytext.color.r, mytext.color.g, mytext.color.b, alpha);

            if (mytext.color.a < 50)
                Speed = disappearinSpeed;

            if (mytext.color.a < 50)
                Destroy(gameObject);

        }
    }
}
