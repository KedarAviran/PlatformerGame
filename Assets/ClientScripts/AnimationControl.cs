using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRen;
    private Vector3 lastPos;
    private bool onLadder = false;
    [SerializeField]
    bool hasJumpAnimation, hasLadderAnimation;
    public void setOnLadder(bool onLadder)
    {
        this.onLadder = onLadder;
    }
    public void setTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRen = GetComponent<SpriteRenderer>();
        lastPos = transform.position;
    }
    void FixedUpdate()
    {
        bool movingInYAxis = Mathf.Abs(transform.position.y - lastPos.y) > 0.001f;
        bool movingInXAxis = Mathf.Abs(transform.position.x - lastPos.x) > 0.001f;
        if (movingInXAxis)
        {
            if (transform.position.x > lastPos.x)
                spriteRen.flipX = true;
            if (transform.position.x < lastPos.x)
                spriteRen.flipX = false;
        }
        if (transform.position.x != lastPos.x)
            anim.SetBool("isMoving", true);
        else
            anim.SetBool("isMoving", false);
    }
}
