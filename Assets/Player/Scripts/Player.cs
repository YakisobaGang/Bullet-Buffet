using System;
using System.Collections.Generic;
using Explosive.Script;
using GameMaster;
using Sirenix.OdinInspector;
using UnityEngine;
using YakisobaGang.Scripts;

namespace Player.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IDamageable, ICanShot
    {
        // hashing walk animation 
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int DamageTick  = Animator.StringToHash("damageTick");

        [Space,Tooltip("Unlock the pistol (Ket-9MR)")] public bool unlockThirdGun = false;

        #region Inspector Filds
        
        [SerializeField,Space] private int health = 10;
        [SerializeField, Space] private Animator anim;
        [SerializeField,Tooltip("The player555 transform"),LabelWidth(190)] private Transform playerRootTransform;
        
        [Header("Inventory | Items"),Space]
        [SerializeField] public List<GameObject> inventory = new List<GameObject>(3);
        [SerializeField,Space] private int bombsCount = 3;
        
        [Header("Movement")]
        [SerializeField] private float controladorDodge = 300f;
        [SerializeField,Tooltip("Shop GameObject on the canvas")] private GameObject store;
        [SerializeField] private float speed = 8;
        [SerializeField,Space] private Transform hand;
        
        #endregion

        #region Events
        
        public Action<bool> ONFlip;
        public Action<bool> ShopIsEnable;
        public event EventHandler onShot;
        public event EventHandler OnPlayerDeth;

        #endregion

        #region Dont see on Inspector or privete fields

        [HideInInspector] public int currentBombsCount = 3;
        private readonly Dictionary<string, GameObject> _itemsInstance = new Dictionary<string, GameObject>();
        private float _slideSpeed;
        private float _timeLastShot;
        private Vector3 _slideDir;
        private State _state;
        private bool _flip;
        private GenericGun _gun;
        private Camera _mainCamera;
        private Rigidbody2D _physics;
        private Transform _transform;
        
        #endregion

        #region Guns Info

        public (GameObject gunGameObject, GenericGun genericGun) PrimaryGun;
        public (GameObject gunGameObject, GenericGun genericGun) SecondaryGun;
        public (GameObject gunGameObject, GenericGun genericGun) ThirdGun;
        
        #endregion
        
        public int Health => health;

        private void Awake() => (_transform, _physics, _mainCamera) = 
            (GetComponent<Transform>(), GetComponent<Rigidbody2D>(), Camera.main);
        
        private void Start()
        {
            _state = State.Normal;
            _slideSpeed = controladorDodge;

            currentBombsCount = bombsCount;
            GetComponent<Aim>().FlipPlayer += b => _flip = b;
            
            // spawning the primary and secondary guns
            var index = 0;
            inventory.ForEach((item)=>
            {
                if(item.TryGetComponent(out Mine _)) return;
                
                var temp = Instantiate(item, hand);
                _itemsInstance.Add(index == 0 ? "primary" : index == 1 ? "secondary" : "third", temp);
                temp.SetActive(index == 0 ? true : false);
                index++;
            });

            if (_itemsInstance.ContainsValue(null)) return;
            // get the gameObject and genericGun from _itemsInstance and caching
            _itemsInstance.TryGetValue("primary", out GameObject gunGameObjectPrimary);
            PrimaryGun = (gunGameObjectPrimary, gunGameObjectPrimary.GetComponent<GenericGun>());
            
            _itemsInstance.TryGetValue("secondary", out var gunGameObjectSecondary);
            SecondaryGun = (gunGameObjectSecondary, gunGameObjectSecondary.GetComponent<GenericGun>());
            
            _itemsInstance.TryGetValue("third", out var gunGameObjectThird);
            ThirdGun = (gunGameObjectThird, gunGameObjectThird.GetComponent<GenericGun>());

            _gun = PrimaryGun.genericGun;

        }

        private void Update()
        {
            var moveY = Input.GetAxisRaw("Vertical");
            var moveX = Input.GetAxisRaw("Horizontal");
            var mousePos = Input.mousePosition;

            if (KeyDown(KeyCode.B))
            {
                store.SetActive(!store.activeSelf);
                ShopIsEnable?.Invoke(store.activeSelf);

                Time.timeScale = Math.Abs(Time.timeScale - 1) < 1 ? 0 : 1 ;

            }
            if (Health <= 0) OnPlayerDeth?.Invoke(this, EventArgs.Empty);

            if (KeyDown(KeyCode.R)) _gun.ReloadGun();
            
            if (KeyDown(KeyCode.Alpha1)) ChangeWeapon(1);
            if (KeyDown(KeyCode.Alpha2)) ChangeWeapon(2);
            if (KeyDown(KeyCode.Alpha3) && unlockThirdGun) ChangeWeapon(3);
            if (KeyDown(KeyCode.G)) PlantC4();

            _timeLastShot += Time.deltaTime;
            if (FireKeyPress() && Time.timeScale != 0)
                if (_timeLastShot >= _gun.gunInfo.FireRate)
                {
                    _gun.FireBullet();
                    _timeLastShot = 0;
                }

            if (!_flip)
            {
                playerRootTransform.localScale = new Vector3(0.51f, 0.51f, 0.51f);
                ONFlip?.Invoke(_flip);
            }
            else
            {
                playerRootTransform.localScale = new Vector3(-0.51f, 0.51f, 0.51f);
                ONFlip?.Invoke(_flip);
            }
               

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
            if (currentBombsCount <= 0) return;

            Instantiate(inventory[2], _transform.position, Quaternion.identity);
            currentBombsCount--;
        }
        
        [ContextMenu("Do damage")]
        public void TakeDamage()
        {
            anim.Play(DamageTick);
            
            if (health <= 0)
            {
                Time.timeScale = 0;
                return;
            }
            health -= 1;
        }

        public void TakeDamage(int damage)
        {
            anim.Play(DamageTick);
            
            if (health <= 0)
            {
                Time.timeScale = 0;
                return;
            }

            health -= damage;
        }
        
        private bool FireKeyPress()
        {
            return Input.GetMouseButton(0);
        }

        private bool KeyDown(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode);
        }

        private void ChangeWeapon(int gunSlot)
        {
            switch (gunSlot)
            {
                case 1:
                    SecondaryGun.gunGameObject.SetActive(false);   // Disable secondary gun
                    ThirdGun.gunGameObject.SetActive(false);      // Disable third gun
                    PrimaryGun.gunGameObject.SetActive(true);    //  Enable primary gun

                    _gun = PrimaryGun.genericGun;
                    break;
                case 2:
                    PrimaryGun.gunGameObject.SetActive(false);     // Disable primary gun
                    ThirdGun.gunGameObject.SetActive(false);      // Disable third gun
                    SecondaryGun.gunGameObject.SetActive(true);  //  Enable secondary gun
                    
                    _gun = SecondaryGun.genericGun;
                    break;
                case 3:
                    PrimaryGun.gunGameObject.SetActive(false);      // Disable primary gun
                    SecondaryGun.gunGameObject.SetActive(false);   // Disable secondary gun
                    ThirdGun.gunGameObject.SetActive(true);       //  Enable third gun

                    _gun = ThirdGun.genericGun;
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
                _slideSpeed = controladorDodge;
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
    }
}