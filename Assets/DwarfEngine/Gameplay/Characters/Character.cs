using UnityEngine;

namespace DwarfEngine
{
    [RequireComponent(typeof(CharacterBrain), typeof(Health), typeof(Collider2D))]
    public class Character : MonoBehaviour
    {
        [Header("Properties")]
        public string characterName;
        public CharacterType characterType;
        public bool isAlive => health.isAlive.Value;

        #region Components

        [HideInInspector] public new Collider2D collider;
        [HideInInspector] public SpriteModel model;
        [HideInInspector] public Health health;
        [HideInInspector] public CharacterBrain brain;
        
        #endregion

        protected virtual void Awake()
        {
            collider = GetComponent<Collider2D>();
            model = GetComponentInChildren<SpriteModel>();
            health = GetComponent<Health>();
            health.model = model.gameObject;
            
            brain = GetComponent<CharacterBrain>();
        }

        protected virtual void Start()
        {

        }

        #region Management Methods 

        public virtual void DisableCharacter(bool modelDisabled = false)
        {
            brain.active = false;
            collider.enabled = false;

            if (modelDisabled)
            {
                model.gameObject.SetActive(false);
            }
        }

        #endregion
    }

    public enum CharacterType { Player, AI }
}
