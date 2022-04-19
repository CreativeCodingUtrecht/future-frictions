using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField]
    private Button firstActionButton;

    [SerializeField]
    private Button secondActionButton;

    public void Init(string text, Action firstCallback, Action secondCallback) {
        firstActionButton.onClick.RemoveAllListeners();
        secondActionButton.onClick.RemoveAllListeners();

        firstActionButton.onClick.AddListener(() => { 
            Close();
            firstCallback?.Invoke(); 
        });
        
        secondActionButton.onClick.AddListener(() => { 
            Close();
            secondCallback?.Invoke(); 
        });

        Open();
    }

    public void Open() {
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}
