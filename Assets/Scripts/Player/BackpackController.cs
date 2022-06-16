using MidlandFarm.Scripts.Collectables.Wheat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MidlandFarm.Scripts.Player
{
    public class BackpackController : MonoBehaviour
    {
        public bool IsEmpty => _wheatBlocks.Count == 0;
        public bool HasSpace => _wheatBlocks.Count < blocksLimit;
        public delegate void BackpackSizeChanges(int size);
        public BackpackSizeChanges backpackSizeChanged;

        public int blocksLimit = 40;
        public Vector3 blockOffset;
        public float shakeStrength = 1;
        public float shakeTime = 0.1f;

        [SerializeField] private Transform stackBasePoint;

        private Stack<WheatBlock> _wheatBlocks = new Stack<WheatBlock>(40);

        private Tweener _shaking;
        private bool _isShaking = false;
        private bool _shouldShake = false;

        private void Awake()
        {
            StartCoroutine(ShakeCoroutine());
        }

        public void AddBlock(WheatBlock block)
        {
            if (_wheatBlocks.Count < blocksLimit)
            {
                _wheatBlocks.Push(block);
                backpackSizeChanged?.Invoke(_wheatBlocks.Count);
                block.transform.parent = stackBasePoint;
                PushToAnimation(block.transform);
            }
        }

        public WheatBlock RemoveBlock()
        {
            WheatBlock wheat;
            if (_wheatBlocks.Count > 0)
            {
                wheat = _wheatBlocks.Pop();
                backpackSizeChanged?.Invoke(_wheatBlocks.Count);
            }
            else
            {
                wheat = null;
            }
            return wheat;
        }

        public void PlayShakeAnimation()
        {
            _shouldShake = true;
        }

        public void StopShakeAnimation()
        {
            _shouldShake = false;
        }

        private IEnumerator ShakeCoroutine()
        {
            for (; ; )
            {
                if (!_isShaking && _shouldShake)
                {
                    _shaking = stackBasePoint.DOShakeRotation(shakeTime, strength: Vector3.forward * shakeStrength);
                    _isShaking = true;
                    yield return new WaitForSeconds(shakeTime);
                    _isShaking = false;
                }
                yield return null;
            }
        }

        private void PushToAnimation(Transform block)
        {
            block.DOLocalMove(_wheatBlocks.Count * blockOffset, 0.2f);
            block.rotation = stackBasePoint.rotation;
        }
    }
}
