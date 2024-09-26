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

    private bool faceUp = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        back = spriteRenderer.sprite;
    }

    private void Flip()
    {
        if(faceUp) spriteRenderer.sprite=back;
        else spriteRenderer.sprite = face;
        faceUp = !faceUp;
    }

    private void OnMouseDown()
    {
        animator.SetTrigger("flip");
    }

}
