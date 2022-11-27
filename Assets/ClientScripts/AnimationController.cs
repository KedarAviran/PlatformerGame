using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRen;
    private Vector3 lastPos;
    [SerializeField]
    private bool onLadder = false;
    [SerializeField]
    bool hasJumpAnimation,hasLadderAnimation;
    public void setOnLadder(bool onLadder)
    {
        this.onLadder = onLadder;
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
        if (Mathf.Abs(transform.position.x - lastPos.x) > 0.001f)
        {
            if (transform.position.x > lastPos.x)
                spriteRen.flipX = true;
            if (transform.position.x < lastPos.x)
                spriteRen.flipX = false;
        }
        if (hasLadderAnimation && onLadder)
            setAnimation("Ladder");
        else
            if (transform.position == lastPos)
            setAnimation("Idle");
        else
        if (hasJumpAnimation && movingInYAxis)
            setAnimation("Jump");
        else
            setAnimation("Move");
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Ladder"))
            anim.enabled = movingInYAxis;
        else
            anim.enabled = true;
        lastPos = transform.position;
    }
    public void setAnimation(string ani)
    {
        if (ani == "Hit" && (anim.GetCurrentAnimatorStateInfo(0).IsName("Ladder")))
            return;
        if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") && ani == "Idle"))
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(ani))
                anim.SetTrigger(ani);
    }
}
