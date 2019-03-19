using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HidePlayer : MonoBehaviour
{
    [SerializeField]
    private Text hidePrompt;

    Renderer[] renderers;
    private MovePlayer moveScript;


    private bool isHidden = false;
    private bool isNearHidingSpot = false;

    private void Start()
    {
        moveScript = GetComponent<MovePlayer>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HidingSpot"))
        {
            isNearHidingSpot = true;
            PromptHide();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HidingSpot"))
        {
            isNearHidingSpot = false;
            RemovePromptHide();
        }
    }

    private void Update()
    {
        if (isNearHidingSpot && !isHidden)
        {
            if (Input.GetButtonDown("Hide"))
                Hide();
        }
        else if(isNearHidingSpot && isHidden)
        {
            if (Input.GetButtonDown("Hide"))
            {
                ComeOut();
            }
        }
    }

    private void Hide()
    {
        RemovePromptHide();
        isHidden = true;
        moveScript.canMove = false;

        //MAKE INVISIBLE
        foreach (Renderer r in renderers)
            r.enabled = false;
    }

    private void ComeOut()
    {
        isHidden = false;
        moveScript.canMove = true;

        //MAKE VISIBLE
        foreach (Renderer r in renderers)
            r.enabled = true;
    }

    private void PromptHide()
    {
        hidePrompt.gameObject.SetActive(true);
    }

    private void RemovePromptHide()
    {
        hidePrompt.gameObject.SetActive(false);
    }
}
