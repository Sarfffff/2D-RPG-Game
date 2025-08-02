using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallacBackGround : MonoBehaviour
{
    private Transform camTF;
    private Vector3 lastFrameCameraPos;
    private float LengthX;//±³¾°³¤¶È
    private float LengthY;
    [SerializeField] private Vector2 parallaxFacotr;
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    private void Start()
    {
        camTF = Camera.main.transform;
        lastFrameCameraPos = camTF.position;
        Sprite sprite = this.GetComponent<SpriteRenderer>().sprite;
        LengthX = sprite.texture.width / sprite.pixelsPerUnit;
        LengthY = sprite.texture.height / sprite.pixelsPerUnit;

    }
    private void Update()
    {
        Vector2 deltaMovement = camTF.position - lastFrameCameraPos;
        transform.position = transform.position + new Vector3(deltaMovement.x * parallaxFacotr.x, deltaMovement.y * parallaxFacotr.y);
        lastFrameCameraPos = camTF.position;
        if (lockX)
        {
            if (Mathf.Abs(camTF.position.x - transform.position.x) >= LengthX)
            {
                float offsetX = camTF.position.x - transform.position.x;
                transform.position = new Vector3(camTF.position.x + offsetX, transform.position.y);
            }
        }
        if (lockY)
        {
            if (Mathf.Abs(camTF.position.y - transform.position.y) >= LengthY)
            {
                float offsetY = camTF.position.y - transform.position.y;
                transform.position = new Vector3(camTF.position.x, camTF.position.y + offsetY);
            }

        }


    }
}
