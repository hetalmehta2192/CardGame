using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip cardFlipSound;
    [SerializeField] private Image iconImg;
    [SerializeField] private GameObject cardMatchEffect;

    private Button btnComp;
    private AudioSource audioSource;
    private Animator animationComp;
    private bool isFlipped;

    // Start is called before the first frame update
    void Start()
    {
        CardInit();
    }

    private void CardInit()
    {
        btnComp = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
        animationComp = GetComponent<Animator>();

        if (audioSource)
            audioSource.clip = cardFlipSound;
        if (btnComp)
        {
            btnComp.onClick.AddListener(() =>
            {
                StartCoroutine(CardFlip(true));
                GamaManager.instance.RegisterCardClicked(this);
            });
        }
    }

    public string GetSpriteName()
    {
        if(iconImg!=null)
        {
            return iconImg.sprite.name;
        }
        return string.Empty;
    }


    public void UpdateIcon(Sprite spriteImg)
    {
        if(iconImg != null)
        {
            iconImg.sprite = spriteImg;
        }
    }
    public void ShowCard(bool flag)
    {
        StartCoroutine(CardFlip(flag));
    }

    private IEnumerator CardFlip(bool flag)
    {
        if(!flag)
        {
            yield return new WaitForSeconds(2f);
        }
        isFlipped = flag;
        animationComp.SetFloat("Direction", (isFlipped ? 1 : -1));
        animationComp.SetBool("isFlipped", isFlipped);
        audioSource.Play();
        btnComp.interactable = !isFlipped;        
    }

    public void CardMatched()
    {
        StartCoroutine(SelfDestroy());
    }

    IEnumerator SelfDestroy()
    {
        if(cardMatchEffect!=null)
        {
            cardMatchEffect.SetActive(true);
            yield return new WaitForSeconds(2f);
            Destroy(cardMatchEffect);
        }
        Destroy(gameObject);
    }
}
