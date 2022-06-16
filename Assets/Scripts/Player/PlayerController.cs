using MidlandFarm.Scripts.Collectables;
using MidlandFarm.Scripts.Collectables.Wheat;
using MidlandFarm.Scripts.Cuttables;
using MidlandFarm.Scripts.Cuttables.Wheat;
using MidlandFarm.Scripts.Market;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MidlandFarm.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        enum PlayerAction
        {
            Staying,
            Moving,
            Cutting
        }

        public string StayAnimationName;
        public string CutAnimationName;
        public string RunAnimationName;
        public float blockSellingTime = 0.1f;

        [SerializeField] private VariableJoystick joystick;
        [SerializeField] private float moveSpeed = 2;
        [SerializeField] private float cutTime = 1;
        [SerializeField] private GameObject instrument;

        [SerializeField] private Animator animator = null;
        [SerializeField] private new Rigidbody rigidbody = null;

        private float _currentVerticalSpeed = 0;
        private float _currentHorizontalSpeed = 0;
        private Vector3 _currentDirection = Vector3.zero;
        private PlayerAction _playerAction;
        private const string SellZoneTag = "SellZone";

        private float _timeScale = 10;
        private bool _ableToSell = false;
        private bool _sellingInProcess = false;

        [SerializeField] private BackpackController _backpackController;

        private void Awake()
        {
            if (!rigidbody)
            {
                rigidbody = GetComponent<Rigidbody>();
            }
        }

        private void FixedUpdate()
        {
            MovementUpdate();
        }

        private void MovementUpdate()
        {
            if (_playerAction == PlayerAction.Cutting)
            {
                return;
            }

            float v = joystick.Vertical;
            float h = joystick.Horizontal;

            Transform camera = Camera.main.transform;

            _currentVerticalSpeed = Mathf.Lerp(_currentVerticalSpeed, v, Time.deltaTime * _timeScale);
            _currentHorizontalSpeed = Mathf.Lerp(_currentHorizontalSpeed, h, Time.deltaTime * _timeScale);

            Vector3 direction = camera.forward * _currentVerticalSpeed + camera.right * _currentHorizontalSpeed;

            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                _playerAction = PlayerAction.Moving;
                _ableToSell = false;
                _currentDirection = Vector3.Slerp(_currentDirection, direction, Time.deltaTime * _timeScale);

                transform.rotation = Quaternion.LookRotation(_currentDirection);
                transform.position += _currentDirection * moveSpeed * Time.deltaTime;

                animator.SetBool(RunAnimationName, true);
                animator.SetBool(StayAnimationName, false);
                _backpackController.PlayShakeAnimation();
            }
            if (v == 0 && h == 0)
            {
                _playerAction = PlayerAction.Staying;
                animator.SetBool(StayAnimationName, true);
                animator.SetBool(RunAnimationName, false);
                _backpackController.StopShakeAnimation();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            var wheat = other.GetComponent<WheatTrigger>();
            if (wheat != null)
            {
                if (wheat.ShouldTrigger && _playerAction == PlayerAction.Staying)
                {
                    Vector3 directionToWheat = wheat.Wheat.transform.position - transform.position;
                    directionToWheat.y = 0.0f;

                    _currentDirection = Vector3.zero;
                    transform.rotation = Quaternion.LookRotation(directionToWheat.normalized);
                    //instrument.GetComponent<ICutter>().SetTargetCuttable(wheat.Wheat);
                    StartCut();
                }
            }

            var collectable = other.GetComponent<ICollectable>();
            if (collectable != null)
            {
                if (collectable.GetType() == typeof(WheatBlock))
                {
                    if (_backpackController.HasSpace)
                    {
                        bool collectResult = collectable.Collect();
                        collectable.Collectable(false);
                        if (collectResult)
                        {
                            _backpackController.AddBlock((WheatBlock)collectable);
                        }
                    }
                }
            }

            var sellZone = other.GetComponent<SellZone>();
            if (sellZone != null)
            {
                if (_playerAction != PlayerAction.Moving && !_sellingInProcess)
                {
                    _ableToSell = true;
                    StartCoroutine(SellCoroutine(sellZone));
                }
            }
        }

        private void StartCut()
        {
            _playerAction = PlayerAction.Cutting;
            StartCoroutine(CutCoroutine());
        }

        private IEnumerator CutCoroutine()
        {
            animator.SetBool(CutAnimationName, true);
            animator.SetBool(RunAnimationName, false);
            animator.SetBool(StayAnimationName, false);
            instrument.SetActive(true);
            yield return new WaitForSeconds(cutTime);
            instrument.SetActive(false);
            _playerAction = PlayerAction.Staying;
            animator.SetBool(CutAnimationName, false);
        }

        private IEnumerator SellCoroutine(SellZone sellZone)
        {
            for (; ; )
            {
                if (!_ableToSell)
                {
                    _sellingInProcess = false;
                    yield break;
                }
                _sellingInProcess = true;
                var wheatBlock = _backpackController.RemoveBlock();
                if (wheatBlock != null)
                {
                    wheatBlock.transform.parent = null;
                    sellZone.ReceiveWheat(wheatBlock);
                }
                else
                {
                    _sellingInProcess = false;
                    yield break;
                }
                yield return new WaitForSeconds(blockSellingTime);
            }
        }
    }

}