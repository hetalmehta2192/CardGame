using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardsContainer;

    //Based on no of icons provided it will generate grid
    [Tooltip("No of icons*2 cards will be created at random Positions")]
    [SerializeField] private Sprite[] icons;

    private int maxSameCard = 2;
    private List<GameObject> cards = new List<GameObject>();

    private void OnEnable()
    {
        GamaManager.cardsToComp += UpdateCardList;
    }


    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            Sprite icon = icons[i];
            for (int j = 0; j < maxSameCard; j++)
            {
                int index = (i + j);
                GameObject newCard=GameObject.Instantiate(cardPrefab);
                newCard.name = "" + i + "" + j;
                newCard.transform.SetParent(cardsContainer, false);
                newCard.GetComponent<CardManager>().UpdateIcon(icon);
                newCard.transform.SetSiblingIndex(Random.Range(0,index));
                cards.Add(newCard);
            }
        }
        //Alternative to place cards randomly
        //int totalCards=icons.Length*maxSameCard;
        //for (int i = 0; i < totalCards; i++)
        //{
        //    cardsContainer.GetChild(i).transform.SetSiblingIndex(Random.Range(0, totalCards));
        //}

        StartCoroutine(ShowGrid());
    }

    IEnumerator ShowGrid()
    {
        yield return new WaitForSeconds(2);

        foreach (GameObject card in cards)
        {
            card.GetComponent<CardManager>().ShowCard(true);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        
        foreach (GameObject card in cards)
        {
            card.GetComponent<CardManager>().ShowCard(false);
            yield return new WaitForSeconds(0.05f);
        }
        GamaManager.instance.isGameStarted=true;
        cardsContainer.GetComponent<GridLayoutGroup>().enabled = false;
    }

    public void UpdateCardList(string card1,string card2)
    {
        cards.RemoveAll(x => x.name == card1 || x.name==card2);
        if(cards.Count == 0 )
        {
            GamaManager.instance.SaveGame();
        }
    }
    private void OnDisable()
    {
        GamaManager.cardsToComp -= UpdateCardList;
    }
}
