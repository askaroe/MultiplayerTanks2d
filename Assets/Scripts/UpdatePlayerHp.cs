using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePlayerHp : MonoBehaviour
{
    [SerializeField]
    private Text _playerHpText;

    private void OnEnable()
    {
        GetComponent<NetworkHealthState>().healthPoint.OnValueChanged += HealthChanged;
    }

    private void OnDisable()
    {
        GetComponent<NetworkHealthState>().healthPoint.OnValueChanged -= HealthChanged;
    }

    private void HealthChanged(int previousValue, int newValue)
    {
        _playerHpText.text = "HP: " + newValue.ToString();
    }
}
