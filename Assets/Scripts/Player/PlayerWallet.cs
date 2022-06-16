using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MidlandFarm.Scripts.Player
{
    public class PlayerWallet : MonoBehaviour
    {
        public float Balance => _balance;

        public delegate void BalanceChanges(float balance);
        public BalanceChanges balanceChanged;

        public void Increase(float value)
        {
            _balance += value;
            balanceChanged?.Invoke(_balance);
        }

        public bool Decrease(float value)
        {
            if (_balance - value >= 0)
            {
                _balance -= value;
                return true;
            }
            else
            {
                return false;
            }
        }

        private float _balance;
    }
}
