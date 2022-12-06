using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRen;
    private Vector3 lastPos;
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRen = GetComponent<SpriteRenderer>();
        lastPos = transform.position;
    }
    public void playSound(string sound)
    {
        if (sound == "Jump")
            GetComponent<AudioSource>().clip = PrefabHolder.instance.getJumpSound();
        if (sound == "Hit")
            GetComponent<AudioSource>().clip = PrefabHolder.instance.getHitSound();
        GetComponent<AudioSource>().Play();
    }
    void FixedUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            this.gameObject.SetActive(false);
        bool movingInYAxis = Mathf.Abs(transform.position.y - lastPos.y) > 0.001f;
        bool movingInXAxis = Mathf.Abs(transform.position.x - lastPos.x) > 0.001f;
        if (movingInXAxis)
        {
            if (transform.position.x > lastPos.x)
                spriteRen.flipX = true;
            if (transform.position.x < lastPos.x)
                spriteRen.flipX = false;
        }
        if (movingInXAxis)
        {
            anim.enabled = true;
            anim.SetBool("isMoving", true);
        }
        else
            anim.SetBool("isMoving", false);
        if(movingInYAxis)
            anim.SetBool("onAir", true);
        else
            anim.SetBool("onAir", false);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Ladder"))
            anim.enabled = movingInYAxis;
        lastPos = transform.position;
    }
}
