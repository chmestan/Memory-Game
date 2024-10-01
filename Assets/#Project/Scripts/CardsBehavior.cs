using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]

public class CardsBehavior : MonoBehaviour
{
    internal CardsManager manager;
    [SerializeField] internal Sprite face;
    internal int faceId;
    private Sprite back;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool faceUp;



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        back = spriteRenderer.sprite;
    }

    private void Flip()
    {
        if(faceUp) spriteRenderer.sprite=back;
        else 
        {
            spriteRenderer.sprite=face;
        }

        faceUp=!faceUp;
    }

    private void FlipAction()
    {
        animator.SetTrigger("flip");
        animator.SetBool("mouse",false);
        manager.CardHasBeenFlipped(this);
    }


    public void FlipBackAction()
    {
        animator.SetTrigger("flipback");

    }

    private void OnMouseDown()
    {
        FlipAction();
    }
    void OnMouseEnter()
    {
        if (!faceUp) animator.SetBool("mouse",true);
        else animator.SetBool("mouse",false);
    }
    void OnMouseExit()
    {
        animator.SetBool("mouse",false);
    }

    void HasFinishedFlipping()
    {

    }

    void Update()
    {
        manager.CheckCards(animator);
    }

}
