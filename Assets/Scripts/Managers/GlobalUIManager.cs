using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GlobalUIManager : Singleton<GlobalUIManager>
{
    [Header("Prompt Message")]

    [SerializeField] GameObject promptCanvasObject;
    [SerializeField] TMP_Text promptText;
    [SerializeField] Image promptImage;
    [SerializeField] Button confirmPromptButton;
    [SerializeField] Button cancelPromptButton;
    [SerializeField] PromptHandler promptHandler;
    private PromptType actualPromptType= PromptType.Wear;
    public enum PromptType
    {
        Wear
    }

    private void OnEnable()
    {
        confirmPromptButton.onClick.AddListener(AcceptPrompt);
        cancelPromptButton.onClick.AddListener(CancelPrompt);
    }

    private void OnDisable()
    {
        confirmPromptButton.onClick.RemoveAllListeners();
        cancelPromptButton.onClick.RemoveAllListeners();
    }


    public void ShowPrompt(string text, Sprite icon, PromptType promptType = PromptType.Wear)
    {
        promptCanvasObject.gameObject.SetActive(true);
        promptText.text = text;
        promptImage.sprite = icon;
        actualPromptType = promptType;
    }

    public void AcceptPrompt()
    {
        promptHandler.HandlePromptAccepted(actualPromptType);
        promptCanvasObject.gameObject.SetActive(false);
    }
    public void CancelPrompt()
    {

    }

  
}