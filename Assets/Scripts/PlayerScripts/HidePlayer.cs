using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HidePlayer : MonoBehaviour
{
    [SerializeField]
    private Text hidePrompt, hiddenPrompt;

    Renderer[] renderers;
    private MovePlayer moveScript;

    private GameObject hidingSpot;//keep track of the hiding spot that we're closest to. Set every time player gets in range

    public bool isHidden { get; private set; }
    private bool isNearHidingSpot = false;

    private void Start()
    {
        //HIDE PROMPT TEXTS
        RemovePromptHidden();
        RemovePromptHide();

        isHidden = false;
        moveScript = GetComponent<MovePlayer>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HidingSpot"))
        {
            isNearHidingSpot = true;
            PromptHide();
            hidingSpot = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HidingSpot"))
        {
            isNearHidingSpot = false;
            RemovePromptHide();
            hidingSpot = null;//set it back to null to make sure you absolutely can't hide there 
        }
    }

    private void Update()
    {
        if (isNearHidingSpot && !isHidden)
        {
            if (Input.GetButtonDown("Hide"))
                Hide(hidingSpot);
        }
        else if(isHidden)
        {
            if (Input.GetButtonDown("Hide"))
            {
                ComeOut(hidingSpot);
            }
        }
    }

    private void Hide(GameObject spot)
    {
        RemovePromptHide();//disable the "hide?"
        PromptHidden();//show that the player is hidden

        isHidden = true;
        moveScript.canMove = false;

        spot.GetComponentInChildren<HidingSpot>().PlayerIsHiddenHere = true;

        //MAKE INVISIBLE
        foreach (Renderer r in renderers)
            r.enabled = false;
    }

    private void ComeOut(GameObject spot)
    {
        RemovePromptHidden();

        isHidden = false;
        moveScript.canMove = true;

        spot.GetComponentInChildren<HidingSpot>().PlayerIsHiddenHere = false;

        //MAKE VISIBLE
        foreach (Renderer r in renderers)
            r.enabled = true;
    }

    private void PromptHide()
    {
        hidePrompt.gameObject.SetActive(true);//tell the player that they can hide      
    }

    private void RemovePromptHide()
    {
        hidePrompt.gameObject.SetActive(false);//player can't hide here
    }

    private void PromptHidden()
    {
        hiddenPrompt.gameObject.SetActive(true);//show the player that they're hiding
    }

    private void RemovePromptHidden()
    {
        hiddenPrompt.gameObject.SetActive(false);//also take away hidden prompt
    }

}
