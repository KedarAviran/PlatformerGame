using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRen;
    private Vector3 lastPos;
    [SerializeField]
    bool hasJumpAnimation;
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRen = GetComponent<SpriteRenderer>();
        lastPos = transform.position;
    }
    void FixedUpdate()
    {
        if (transform.position.x > lastPos.x)
            spriteRen.flipX = true;
        if (transform.position.x < lastPos.x)
            spriteRen.flipX = false;
        if (transform.position == lastPos)
            setAnimation("Idle");
        else
        if (hasJumpAnimation && Mathf.Abs(transform.position.y - lastPos.y)>0.001f)
            setAnimation("Jump");
        else
            setAnimation("Move");
        lastPos = transform.position;
    }
    public void setAnimation(string ani)
    {
        if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") && ani == "Idle"))
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(ani))
                anim.SetTrigger(ani);
    }
}
