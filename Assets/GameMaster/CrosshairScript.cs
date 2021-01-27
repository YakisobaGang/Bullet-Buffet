using UnityEngine;
using CodeMonkey.Utils;

namespace YakisobaGang.GameMaster
{
    public class CrosshairScript : MonoBehaviour
    {
        private Player.Scripts.Player player;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player.Scripts.Player>();
        }

        void Start()
        {
            Cursor.visible = false;
            player.ShopIsEnable = b =>
            {
                Cursor.visible = b;
                gameObject.SetActive(!b);
            };
        }

        private void Update()
        {
            Vector3 cursorPos = UtilsClass.GetMouseWorldPosition();
            transform.position = cursorPos;
            
        }
    }
}
