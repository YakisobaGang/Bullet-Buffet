using System;
using System.Collections.Generic;
using Explosive.Script;
using GameMaster;
using UnityEngine;
using YakisobaGang.Scripts;

namespace Player.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IDamageable, ICanShot
    {
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        
        [SerializeField] private int health = 10;
        [SerializeField] private Animator anim;
        [SerializeField] private Transform player555;
        [SerializeField] private int bombsCount = 3;
        [SerializeField] public List<GameObject> inventory = new List<GameObject>(3);
        private readonly Dictionary<string, GameObject> _itemsInstance = new Dictionary<string, GameObject>();
        public Transform hand;
        public float speed = 8;
        public float ControladorDodge = 300f;
        public GameObject _store;
        private readonly int _currentItemIndex = 0;
        private Aim _aim;
        private GenericGun _gun;
        private Camera _mainCamera;
        private Rigidbody2D _physics;
        private Vector3 _slideDir;
        private float _slideSpeed;
        private State _state;
        private Transform _transform;

        private bool flip;
        private float timeLastShot;

        private (GameObject gunGameObject, GenericGun genericGun) _primaryGun;
        private (GameObject gunGameObject, GenericGun genericGun) _secondaryGun;
        private ObjectPooler _objectPooler;

        public int CurrentBombsCount { get; private set; }

        public int Health => health;

        private void Awake() => (_transform, _physics, _mainCamera) = 
            (GetComponent<Transform>(), GetComponent<Rigidbody2D>(), Camera.main);
            

        private void Start()
        {
            _state = State.Normal;
            _slideSpeed = ControladorDodge;
            _aim = GetComponent<Aim>();
            
            
            CurrentBombsCount = bombsCount;
            GetComponent<Aim>().FlipPlayer += b => flip = b;
            
            // spawning the primary and secondary guns
            var index = 0;
            inventory.ForEach((item)=>
            {
                if(item.TryGetComponent(out Mine _)) return;
                
                var temp = Instantiate(item, hand);
                _itemsInstance.Add(index == 0 ? "primary": "secondary", temp);
                temp.SetActive(index == 0 ? true : false);
                index++;
            });
            
            // get the gameObject and genericGun from _itemsInstance and caching
            _itemsInstance.TryGetValue("primary", out GameObject gunGameObjectPrimary);
            _primaryGun = (gunGameObjectPrimary, gunGameObjectPrimary.GetComponent<GenericGun>());
            
            _itemsInstance.TryGetValue("secondary", out var gunGameObjectSecondary);
            _secondaryGun = (gunGameObjectSecondary, gunGameObjectSecondary.GetComponent<GenericGun>());

            _gun = _primaryGun.genericGun;
        }

        private void Update()
        {
            var moveY = Input.GetAxisRaw("Vertical");
            var moveX = Input.GetAxisRaw("Horizontal");
            var mousePos = Input.mousePosition;

            // if (KeyDown(KeyCode.B))
            // {
            //     _store.SetActive(true);
            //     Time.timeScale = 0;
            //
            // }
            if (Health <= 0) OnPlayerDeth?.Invoke(this, EventArgs.Empty);

            if (KeyDown(KeyCode.R)) _gun.ReloadGun();

            if (KeyDown(KeyCode.Alpha1)) ChangeWeapon("primary");
            if (KeyDown(KeyCode.Alpha2)) ChangeWeapon("secondary");
            if (KeyDown(KeyCode.G)) PlantC4();

            timeLastShot += Time.deltaTime;
            if (FireKeyPress() && Time.timeScale != 0)
                if (timeLastShot >= _gun.gunInfo.FireRate)
                {
                    _gun.FireBullet();
                    timeLastShot = 0;
                }

            if (!flip)
                player555.localScale = new Vector3(0.51f, 0.51f, 0.51f);
            else
                player555.localScale = new Vector3(-0.51f, 0.51f, 0.51f);

            switch (_state)
            {
                case State.Normal:
                    CanDodge();
                    Movement(moveX, moveY);
                    break;
                case State.Dodge:
                    Dodge();
                    break;
            }
        }

        private void PlantC4()
        {
            if (CurrentBombsCount <= 0) return;

            Instantiate(inventory[2], _transform.position, Quaternion.identity);
            CurrentBombsCount--;
        }

        public event EventHandler onShot;

        public void TakeDamage()
        {
            if (health <= 0)
            {
                Time.timeScale = 0;
                return;
            }

            health -= 1;
        }

        public void TakeDamage(int damage)
        {
            if (health <= 0)
            {
                Time.timeScale = 0;
                return;
            }

            health -= damage;
        }

        public event EventHandler OnPlayerDeth;

        private bool FireKeyPress()
        {
            return Input.GetMouseButton(0);
        }

        private bool KeyDown(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode);
        }

        private void ChangeWeapon(string gunSlotName)
        {
            switch (gunSlotName)
            {
                case "primary":
                    _secondaryGun.gunGameObject.SetActive(false);    // Disable secondary gun
                    _primaryGun.gunGameObject.SetActive(true);      //  Enable primary gun

                    _gun = _primaryGun.genericGun;
                    break;
                case "secondary":
                    _primaryGun.gunGameObject.SetActive(false);      // Disable primary gun
                    _secondaryGun.gunGameObject.SetActive(true);    //  Enable secondary gun
                    
                    _gun = _secondaryGun.genericGun;
                    break;
            }
            
        }

        private void Movement(float moveX, float moveY)
        {
            var transformLocalScale = transform.localScale;
            var velocity = _physics.velocity;

            velocity = new Vector2(moveY * speed, velocity.y);
            velocity = new Vector2(moveX * speed, velocity.x);
            _physics.velocity = velocity;

            if (velocity.x != 0 || velocity.y != 0)
                anim.SetBool(IsWalking, true);
            else
                anim.SetBool(IsWalking, false);
        }

        private void CanDodge()
        {
            if (Input.GetKeyDown("space")) _state = State.Dodge;
        }

        private void Dodge()
        {
            var position = _transform.position;
            _slideDir = (MousePosition() - position).normalized;
            _slideDir.z = 0;
            position += _slideDir * _slideSpeed * Time.deltaTime;
            transform.position = position;
            _slideSpeed -= _slideSpeed * 10f * Time.deltaTime;

            if (_slideSpeed <= 10f)
            {
                _slideSpeed = ControladorDodge;
                _state = State.Normal;
            }
        }

        private Vector3 MousePosition()
        {
            var mousePos = Input.mousePosition;
            var worldPosition = _mainCamera.ScreenToWorldPoint(mousePos);
            mousePos.z = _mainCamera.nearClipPlane;

            return worldPosition;
        }

        private enum State
        {
            Normal,
            Dodge
        }

        public class FlipEvent : EventArgs
        {
            public Transform currentFirePoint;
            public Vector3 newScale;
        }
    }
}