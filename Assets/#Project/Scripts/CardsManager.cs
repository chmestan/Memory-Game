using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    [SerializeField] Vector2 gameSize = Vector2.one*4;
    [SerializeField] float gapX = 0.5f;
    [SerializeField] float gapY = 0.5f;
    [SerializeField] GameObject prefab;

    [SerializeField] Sprite[] possibleFaces;
    private List<CardsBehavior> cards = new();
    private List<CardsBehavior> cardsFlipped = new();
    private int countCard {
        get {return (int)(gameSize.x*gameSize.y);}
    }

    void Start()
    {

        
        if (countCard %2 != 0) 
        {
            Debug.LogError("You need to have an even number of cards");
            return;
        }

        if (countCard /2 > possibleFaces.Length) 
        {
            Debug.LogError($"You can't have more cards than {possibleFaces.Length*2}");
            return;
        }
        
        Initialize();
    }

    private void Initialize()
    {

        int nbFaces = countCard /2; 
        List<int> faces = new();
        for(int _=0; _<nbFaces; _++) // underscore pour dire que c'est juste un compteur
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
                int index = Random.Range(0,faces.Count); //Count pour listes, Length pour tableaux
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
            Debug.LogError($"Prefab {prefab.name} does not have a Card Behavior script");
            return;
        }

        if(card.TryGetComponent(out Collider2D collider))
        {
            Vector3 cardSize = collider.bounds.size;
            float cardX = x * (gapX + cardSize.x); //position en X 
            float cardY = y * (gapY + cardSize.y); //position en Y 

            Vector3 position = new(cardX,cardY,0);
            // position += transform.position ; //on repositionne par rapport au Card Manager
            // card.transform.position = position;

            // est équivalent à
            card.transform.parent= transform; // = transform du Card Manager
            //card.transform.SetParent(transform) veut dire pareil
            card.transform.localPosition = position;

        }
        else 
        {
            Debug.LogError($"Prefab {prefab.name} does not have a Collider2D");
            return;
        }
    }

    private void GiveFace(CardsBehavior cardBehavior, int faceId)
    {
        cardBehavior.faceId = faceId;
        cardBehavior.face = possibleFaces[faceId];
    }

    public void CardHasBeenFlipped(CardsBehavior card)
    {
        cardsFlipped.Add(card);
        if (cardsFlipped.Count > 1) 
        {
            if (cardsFlipped[0].faceId != cardsFlipped[1].faceId)
            {
                cardsFlipped[0].FlipBackAction();
                cardsFlipped[1].FlipBackAction();
            }
            cardsFlipped.Clear();
            Debug.Log(cardsFlipped.Count);
        }
    }

    void Update()
    {
        
    }
}
