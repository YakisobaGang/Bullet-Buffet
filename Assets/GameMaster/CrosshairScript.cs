using UnityEngine;
using CodeMonkey.Utils;

namespace YakisobaGang.GameMaster
{
    public class CrosshairScript : MonoBehaviour
    {
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        void Start()
        {
            Cursor.visible = false;
        }

        private void Update()
        {
            Vector3 cursorPos = UtilsClass.GetMouseWorldPosition();
            transform.position = cursorPos;
        }
    }
}
