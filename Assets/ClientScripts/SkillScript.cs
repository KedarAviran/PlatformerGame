using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScript : MonoBehaviour
{
    [SerializeField]
    public int skillID;
    private Animator anim;
    void Start()
    {
        anim=GetComponent<Animator>();
    }
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            Destroy(this.gameObject);
    }
}
