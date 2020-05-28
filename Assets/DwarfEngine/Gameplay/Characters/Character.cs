using UnityEngine;

namespace DwarfEngine
{
    public enum CharacterType { Player, AI }

    /// <summary>
    /// The essential component of a character object.
    /// </summary>
    [RequireComponent(typeof(CharacterBrain), typeof(Health), typeof(Collider2D))]
    public class Character : MonoBehaviour
    {
        public bool isAlive => health.isAlive.Value;
        
        /// <summary>
        /// Name of the character. Used in the game UI.
        /// </summary>
        [Header("Properties")]
        public string characterName;

        /// <summary>
        /// Type of the character.
        /// </summary>
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

        #region Management Methods 

        /// <summary>
        /// Disables the character.
        /// </summary>
        /// <param name="modelDisabled"></param>
        public virtual void DisableCharacter(bool modelDisabled = false)
        {
            // TODO: Disable the abilities too.

            brain.active = false;
            collider.enabled = false;

            if (modelDisabled)
            {
                model.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
