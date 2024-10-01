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
    
    private void StartFlip() // starts anim
    {
        animator.SetTrigger("flip");
        animator.SetBool("mouse",false);
    }


    private void Flip() // mid anim -- change sprite
    {
        if(faceUp) spriteRenderer.sprite=back;
        else  spriteRenderer.sprite=face;  
    }

    private void FlipDone() // end anim 
    {
        faceUp=!faceUp;
        if (faceUp) manager.CardHasBeenFlipped(this);
        else if (!faceUp && manager.cardsFlipped.Count > 1 && this == manager.cardsFlipped[1])
        {
            manager.cardsFlipped.Clear();
            Debug.Log(manager.cardsFlipped.Count);
        }
    }



    public void FlipBack()
    {
        animator.SetTrigger("flipback");

    }

    private void OnMouseDown()
    {
        if (manager.cardsFlipped.Count < 2) StartFlip();
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

    void Update()
    {
        manager.CheckCards(animator);
    }

}
