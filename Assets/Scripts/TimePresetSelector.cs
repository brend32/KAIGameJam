using System;
using TMPro;
using UnityEngine;

public class TimePresetSelector : MonoBehaviour
{
    public int Preset;
    public int[] Times;
    public TextMeshProUGUI Text;
    
    public int Time => Times[Preset];

    public void Start()
    {
        SyncText();
    }

    public void Next()
    {
        Preset =  (Preset + 1) % Times.Length;
        SyncText();
    }

    public void Previous()
    {
        Preset = (Preset - 1 + Times.Length) % Times.Length;
        SyncText();
    }

    public void SyncText()
    {
        Text.text = $"{Time} sec";
    }
}