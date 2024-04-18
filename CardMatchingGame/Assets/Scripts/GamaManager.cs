using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamaManager : MonoBehaviour
{
    public static GamaManager instance;

    [HideInInspector]
    public bool isGameStarted;

    private CardManager lastCard;
    private AudioSource audioSource;
    private int posPoints, negPoints;
    public delegate void CardsToComp(string card1,string card2);
    public static event CardsToComp cardsToComp;


    [SerializeField] private AudioClip cardWinSound, cardMissSound;
    [SerializeField] private TextMeshProUGUI posPointText, negPointText, gameEndMsg;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Or can be Serialized as well depending on structure
        audioSource = GetComponent<AudioSource>();
    }

    public void RegisterCardClicked(CardManager card)
    {
        if (isGameStarted)
        {
            if (lastCard != null)
            {
                //This can be compared via Icon Id as well with custom structure
                if (string.Equals(card.GetSpriteName(), lastCard.GetSpriteName()))
                {
                    cardsToComp(card.name, lastCard.name);
                    card.CardMatched();
                    lastCard.CardMatched();
                    lastCard = null;
                    UpdatePoints(true);
                }
                else
                {
                    lastCard.ShowCard(false);
                    card.ShowCard(false);
                    lastCard = null;
                    UpdatePoints(false);
                }
            }
            else
            {
                lastCard = card;
            }
        }
    }

    private void UpdatePoints(bool isPositive)
    {
        if (isPositive)
        {
            posPoints++;
            posPointText.text = posPoints.ToString();
            audioSource.clip = cardWinSound;
        }
        else
        {
            negPoints++;
            negPointText.text = negPoints.ToString();
            audioSource.clip = cardMissSound;
        }
        audioSource.Play();
    }

    public void SaveGame()
    {
        int oldScore = PlayerPrefs.HasKey("Score") ? PlayerPrefs.GetInt("Score") : 0;
        gameEndMsg.text = oldScore > negPoints ?
            "Wonderfull !! You did good only " + negPoints + " Negetive Points" :
             "You made Record of bad Score " + negPoints + " Negetive Points";

        PlayerPrefs.SetInt("Score", oldScore > negPoints ? oldScore : negPoints);
        gameOverPanel.SetActive(true);
    }

}
