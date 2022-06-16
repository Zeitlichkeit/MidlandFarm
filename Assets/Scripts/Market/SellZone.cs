using MidlandFarm.Scripts.Collectables.Wheat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MidlandFarm.Scripts.Player;
using MidlandFarm.Scripts.UI;

namespace MidlandFarm.Scripts.Market
{
    public class SellZone : MonoBehaviour
    {
        public PlayerWallet playerWallet;
        public WalletUI walletUI;
        public Transform sellPoint;
        public float blockFlyTime = 1.0f;

        public void ReceiveWheat(WheatBlock wheatBlock)
        {
            wheatBlock.transform.DOMove(sellPoint.position, blockFlyTime).OnComplete(() => SellCallback(wheatBlock));
        }

        public void SellCallback(WheatBlock wheatBlock)
        {
            playerWallet.Increase(wheatBlock.price);
            walletUI.SpawnCoin();
            Destroy(wheatBlock.gameObject);
        }
    }
}
