using UnityEngine;

namespace DwarfEngine
{
    [RequireComponent(typeof(CharacterBrain), typeof(Health), typeof(Collider2D))]
    public class Character : MonoBehaviour
    {
        public bool isAlive => health.isAlive.Value;
        
        [Header("Properties")]
        public string characterName;
        public CharacterType characterType;

        #region Components
        [HideInInspector] public new Collider2D collider;
        [HideInInspector] public SpriteModel model;
        [HideInInspector] public Health health;
        [HideInInspector] public CharacterBrain brain;
        [HideInInspector] public CharacterEquipmentManager equipmentManager;
        #endregion

        protected virtual void Awake()
        {
            collider = GetComponent<Collider2D>();
            model = GetComponentInChildren<SpriteModel>();
            health = GetComponent<Health>();
            health.model = model.gameObject;
            
            brain = GetComponent<CharacterBrain>();
            equipmentManager = GetComponent<CharacterEquipmentManager>();
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
