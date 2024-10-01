using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;


public class CardsManager : MonoBehaviour
{
    [Header ("Game Size")]
    [SerializeField] Vector2 gameSize = Vector2.one*4;
    [SerializeField] float gapX = 0.5f;
    [SerializeField] float gapY = 0.5f;

    [Header ("Cards")]
    [SerializeField] GameObject prefab;
    [SerializeField] Sprite[] possibleFaces;

    [Header ("Win Event")]
    [SerializeField] private UnityEvent whenPlayerWins;

    private List<CardsBehavior> cards = new();
    public List<CardsBehavior> cardsFlipped = new();
    private List<CardsBehavior> cardsCorrect = new();

    private int countCard {
        get {return (int)(gameSize.x*gameSize.y);}
    }

    void Start()
    {
        if (countCard %2 != 0) 
        {
            Debug.LogError("You need to have an even number of cards"); return;
        }

        if (countCard /2 > possibleFaces.Length) 
        {
            Debug.LogError($"You can't have more cards than {possibleFaces.Length*2}"); return;
        }
        
        Initialize();
    }

    private void Initialize()
    {

        int nbFaces = countCard /2; 
        List<int> faces = new();
        for(int _=0; _<nbFaces; _++) 
        {
            int face = Random.Range(0, possibleFaces.Length);
            while (faces.Contains(face))
            {
                face = Random.Range(0, possibleFaces.Length);
            }
            faces.Add(face);
        }
        faces.AddRange(faces);

        for (int x=0; x<gameSize.x; x++)
        {
            for (int y=0; y<gameSize.y; y++)
            {
                int index = Random.Range(0,faces.Count); 
                InstantiateCard(x,y,faces[index]);
                faces.RemoveAt(index);
            }
        }
    }

    private void InstantiateCard(int x, int y, int faceId)
    {
        GameObject card = Instantiate(prefab);

        

        if (card.TryGetComponent(out CardsBehavior cardsBehavior))
        {
            cards.Add(cardsBehavior);
            cardsBehavior.manager = this;
            GiveFace(cardsBehavior, faceId);
        }
        else 
        {
            Debug.LogError($"Prefab {prefab.name} does not have a Card Behavior script"); return;
        }

        if(card.TryGetComponent(out Collider2D collider))
        {
            Vector3 cardSize = collider.bounds.size;

            // Get horizontal & vertical size for centering
            float totalGridWidth = (gameSize.x - 1) * (gapX + cardSize.x);
            float totalGridHeight = (gameSize.y - 1) * (gapY + cardSize.y);
            // cardSize needs to be taken off once because from the first card to the last we take back one unit
            // due to the pivot being at the center 
            // so for X for example, we need to take away the left half of the first card and the right half of the last

            float cardX = x * (gapX + cardSize.x) - totalGridWidth / 2;
            float cardY = y * (gapY + cardSize.y) - totalGridHeight / 2; 

            Vector3 position = new(cardX,cardY,0);
            card.transform.parent= transform; 
            card.transform.localPosition = position;

        }
        else 
        {
            Debug.LogError($"Prefab {prefab.name} does not have a Collider2D"); return;
        }
    }

    private void GiveFace(CardsBehavior cardBehavior, int faceId)
    {
        cardBehavior.faceId = faceId;
        cardBehavior.face = possibleFaces[faceId];
    }

    public void CardHasBeenFlipped(CardsBehavior card)
    {
        if (cardsFlipped.Count > 1) 
        {
            if (cardsFlipped[0].faceId != cardsFlipped[1].faceId)
            {
                cardsFlipped[0].animator.Play("Wait",0,0f);
                cardsFlipped[1].animator.Play("Wait",0,0f);
                // cardsFlipped[0].FlipBack();
                // cardsFlipped[1].FlipBack();
            }
            else
            {
                cardsCorrect.Add(cardsFlipped[0]);
                cardsCorrect.Add(cardsFlipped[1]);
                cardsFlipped.Clear();
            }
        }

    }

    public void CheckCards(Animator animator)
    {
        if (cardsCorrect.Count == cards.Count)
        {
            whenPlayerWins.Invoke();
            animator.SetBool("win", true);
        }
    }


    void Update()
    {

    }
}
