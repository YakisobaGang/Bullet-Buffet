using UnityEngine;
using UnityEngine.UI;

namespace Canvas.Health_Bar
{

    public class LifeManager : MonoBehaviour
    {

        [SerializeField] private Player.Scripts.Player player;
        [SerializeField] private Image fill;
        private Slider slider;
        
        void Awake()
        {
            slider = GetComponent<Slider>();
        }

        
        void Update()
        { 
            slider.value = player.Health;
            if(player.Health <= 0)
            {
                Destroy(fill);
            }
        }
    }   
}

