using MidlandFarm.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace MidlandFarm.Scripts.UI
{
    public class BackpackIndicator : MonoBehaviour
    {
        public BackpackController backpackController;
        public TextMeshProUGUI text;
        public RectTransform indicator;
        public Vector3 fromPosition;
        public Vector3 toPosition;
        public Vector2 fromSize;
        public Vector2 toSize;
        public float animationDuration = 0.5f;

        private void Awake()
        {
            backpackController.backpackSizeChanged += UpdateIndicator;
        }

        private void UpdateIndicator(int count)
        {
            float progress = (float)count / backpackController.blocksLimit;

            text.text = count + " / " + backpackController.blocksLimit;

            var targetPosition = fromPosition + (toPosition - fromPosition) * progress;
            var targetSize = fromSize + (toSize - fromSize) * progress;

            targetPosition.y = fromPosition.y;
            targetSize.y = fromSize.y;

            indicator.DOAnchorPos(targetPosition, animationDuration);
            indicator.DOSizeDelta(targetSize, animationDuration);
        }
    }
}