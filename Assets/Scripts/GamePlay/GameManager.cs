using DG.Tweening;
using UnityEngine;

namespace GamePlay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private CardEffect _hoverCard;

        public CardEffect hoverCard
        {
            get { return _hoverCard;}
            set
            {
                _hoverCard = value;
            }
        }


        private void Awake()
        {
            Instance = this;
        }
    }
}