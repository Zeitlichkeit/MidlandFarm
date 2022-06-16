using MidlandFarm.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace MidlandFarm.Scripts.UI
{
    public class WalletUI : MonoBehaviour
    {
        public RectTransform walletBackground;
        public TextMeshProUGUI text;
        public PlayerWallet playerWallet;
        public float animationTime = 0.1f;
        public float coinFlyTime = 0.1f;
        public float shakeStrength = 1;

        public GameObject coinPrefab;
        public RectTransform spawnedCoinPosition;
        public RectTransform endCoinPosition;

        private float _currentValue = 0.0f;
        private bool _isShaking = false;

        public void SpawnCoin()
        {
            var coin = Instantiate(coinPrefab, this.transform);
            coin.GetComponent<RectTransform>().anchoredPosition = spawnedCoinPosition.anchoredPosition;
            coin.GetComponent<RectTransform>().DOAnchorPos(endCoinPosition.anchoredPosition, coinFlyTime).OnComplete(() => Destroy(coin));
        }

        private void Awake()
        {
            _currentValue = playerWallet.Balance;
            UpdateText();
            playerWallet.balanceChanged += UpdateIndicator;
        }

        private void UpdateIndicator(float balance)
        {
            DOTween.To(() => _currentValue, value => { _currentValue = value; UpdateText(); }, balance, animationTime);
            if (!_isShaking)
            {
                walletBackground.DOShakeRotation(animationTime, strength: Vector3.forward * shakeStrength).OnComplete(() => StopShaking());
            }
        }

        private void UpdateText()
        {
            text.text = Mathf.RoundToInt(_currentValue).ToString();
        }

        private void StopShaking()
        {
            walletBackground.rotation = Quaternion.Euler(Vector3.zero);
            _isShaking = false;
        }
    }
}