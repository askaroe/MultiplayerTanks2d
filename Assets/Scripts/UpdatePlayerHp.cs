using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePlayerHp : MonoBehaviour
{
    [SerializeField] private Sprite[] _playerHealthPoints;
    [SerializeField] private Image _playerHealthPoint;

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
        if (newValue > 0) _playerHealthPoint.sprite = _playerHealthPoints[newValue];
        else if(newValue == 0)
        {
            _playerHealthPoint.sprite = _playerHealthPoints[newValue];
            GetComponent<PlayerSettings>().PlayerDestroyed();
            GetComponent<PlayerNetwork>().PlayerDestroyed();
        }
    }
}
