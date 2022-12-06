using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupScript : MonoBehaviour
{
    private TextMeshPro textmesh;
    private float moveSpeed = 1f;
    private float dissapearTime = 1f;
    private float dissapearSpeed = 3f;
    private void Awake()
    {
        textmesh = GetComponent<TextMeshPro>();
    }
    void Update()
    {
        transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;
        dissapearTime -= Time.deltaTime;
        if(dissapearTime<=0)
        {
            Color textColor = textmesh.color;
            textColor.a -= dissapearSpeed * Time.deltaTime;
            textmesh.color = textColor;
            if (textColor.a < 0)
                Destroy(this.gameObject);
        }
    }
}
