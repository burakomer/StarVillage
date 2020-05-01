using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

namespace DwarfEngine
{
    public class WeaponFactoryWindow : EditorWindow
    {
        private string weaponName;
        private int selectedType;
        private int selectedProcessor;

        #region Aim Settings
        private AimType aimType;
        private float gripAngleOffset;
        private float gripLength;
        #endregion

        #region Charge Settings
        private bool hasCharge;
        private float chargeTime;
        private bool prematureAttack;
        #endregion

        #region Resource Settings
        private bool hasResource;
        private ResourceType resourceType;
        private string resourceName;
        private int consumeAmount; 
        #endregion

        [MenuItem("Window/Weapon Factory")]
        public static void ShowWindow()
        {
            GetWindow(typeof(WeaponFactoryWindow));
        }

        private void OnGUI()
        {
            titleContent = new GUIContent("Weapon Factory");
            GUILayout.Space(10);
            GUILayout.Label("Create a Weapon", EditorStyles.boldLabel);

            weaponName = EditorGUILayout.TextField("Weapon Name", weaponName);

            GUILayout.Space(10);
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            var weaponTypes = GetTypes<Weapon>();
            var weaponTypeNames = GetTypeNames(weaponTypes);

            var processorTypes = GetTypes<WeaponProcessor>();
            var processorTypeNames = GetTypeNames(processorTypes);

            selectedType = EditorGUILayout.Popup("Weapon Type", selectedType, weaponTypeNames);
            // Type Settings

            selectedProcessor = EditorGUILayout.Popup("Weapon Processor", selectedProcessor, processorTypeNames);
            // Processor settings

            GUILayout.Space(10);
            GUILayout.Label("Components", EditorStyles.boldLabel);
            GUILayout.Space(10);

            GUILayout.Label("Aim");
            GUILayout.Space(10);
            aimType = (AimType)EditorGUILayout.EnumPopup("Aim Type", aimType);
            gripAngleOffset = EditorGUILayout.FloatField("Grip Angle Offset", gripAngleOffset);
            gripLength = EditorGUILayout.FloatField("Grip Length", gripLength);
            GUILayout.Space(10);

            hasCharge = EditorGUILayout.Toggle("Charge", hasCharge);
            if (hasCharge)
            {
                GUILayout.Space(10);
                chargeTime = EditorGUILayout.FloatField("Charge Time", chargeTime);
                prematureAttack = EditorGUILayout.Toggle("Premature Attack", prematureAttack);
                GUILayout.Space(10);
            }

            hasResource = EditorGUILayout.Toggle("Resource", hasResource);
            if (hasResource)
            {
                GUILayout.Space(10);
                resourceType = (ResourceType)EditorGUILayout.EnumPopup("Resource Type", resourceType);
                resourceName = EditorGUILayout.TextField("Resource Name", resourceName);
                consumeAmount = EditorGUILayout.IntField("Consume Amount", consumeAmount);
                GUILayout.Space(10);
            }

            if (GUILayout.Button("Create Weapon", EditorStyles.miniButton))
            {
                GameObject weapon = new GameObject(weaponName);
                GameObject weaponModel = new GameObject("Model");
                weaponModel.transform.SetParent(weapon.transform);

                foreach (var type in weaponTypes)
                {
                    if (weaponTypeNames[selectedType] == type.Name)
                    {
                        weapon.AddComponent(type);

                        // Set options

                        break;
                    }
                }

                foreach (var type in processorTypes)
                {
                    if (processorTypeNames[selectedType] == type.Name)
                    {
                        weapon.AddComponent(type);
                        
                        // Set options

                        break;
                    }
                }

                var weaponAim = weapon.GetComponent<WeaponAim>();
                weaponAim.aimType = aimType;
                weaponAim.gripAngleOffset = gripAngleOffset;
                weaponAim.gripLength = gripLength;

                if (hasCharge)
                {
                    var weaponCharge = weapon.AddComponent<WeaponCharge>();
                    weaponCharge.chargeTime.BaseValue = chargeTime;
                    weaponCharge.prematureAttack = prematureAttack;
                }

                if (hasResource)
                {
                    var weaponResource = weapon.AddComponent<WeaponResource>();
                    weaponResource.resourceType = resourceType;
                    weaponResource.resourceName = resourceName;
                    weaponResource.consumeAmount = consumeAmount;
                }
            }

        }

        public Type[] GetTypes<T>()
        {
            return typeof(Weapon).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(T)))
                .ToArray();
        }

        public string[] GetTypeNames(Type[] types)
        {
            string[] typeNames = new string[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                typeNames[i] = types[i].Name;
            }
            return typeNames;
        }
    }
}
