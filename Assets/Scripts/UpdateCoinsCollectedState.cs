using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpdateCoinsCollectedState : MonoBehaviour
{
    [SerializeField] private Text coinsCollectedText;
    
    private void OnEnable()
    {
        GetComponent<NetworkCoinsCollectedState>().coinsCollected.OnValueChanged += CoinsCollectedStateChanged;
    }

    private void OnDisable()
    {
        GetComponent<NetworkCoinsCollectedState>().coinsCollected.OnValueChanged -= CoinsCollectedStateChanged;
    }

    private void CoinsCollectedStateChanged(int previousvalue, int newvalue)
    {
        coinsCollectedText.text = newvalue.ToString();
    }
}
