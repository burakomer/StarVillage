﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DwarfEngine
{
    /// <summary>
    /// Handles saving and loading.
    /// </summary>
    public static class SaveManager
    {
        /// <summary>
        /// The default save path. In development phase, this is in C:\Users\'user_file'\AppData\LocalLow\BurakOmer\Woof of the Stars\saves
        /// </summary>
        public static string SavePath = Application.persistentDataPath + "/saves/";

        /// <summary>
        /// Saves the object in JSON with given type.
        /// </summary>
        /// <typeparam name="T">Object type to be saved.</typeparam>
        /// <param name="key">The file name without extension.</param>
        /// <returns></returns>
        public static string Save<T>(T objectToSave, string key)
        {
            Directory.CreateDirectory(SavePath);
            string json = JsonUtility.ToJson(objectToSave, true);

            Debug.Log(json);

            File.WriteAllText(SavePath + key + ".json", json);
            return json;
        }

        /// <summary>
        /// Loads the file with given key and returns it as an object of given type.
        /// </summary>
        /// <typeparam name="T">Object type to be loaded as.</typeparam>
        /// <param name="key">The file name without extension.</param>
        /// <returns></returns>
        public static T Load<T>(string key)
        {
            T returnValue = default;

            string json = File.ReadAllText(SavePath + key + ".json");
            returnValue = JsonUtility.FromJson<T>(json);
            return returnValue;
        }

        /// <summary>
        /// Checks if the save file exists with the given key.
        /// </summary>
        /// <param name="key">The file name without extension.</param>
        /// <returns></returns>
        public static bool SaveExists(string key)
        {
            return File.Exists(SavePath + key + ".json");
        }

        /* #region Encryption
        public static void EncryptedSave<T>(T objectToSave, string key)
        {
            string json = Save(objectToSave, key);

            string encryption = Cipher.Encrypt(json, Cipher.pw);

            Debug.Log(encryption);

            using (FileStream stream = new FileStream(SavePath + key + GameManager.activeSaveSlot + ".sav", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, encryption);
            }
        }

        public static T EncryptedLoad<T>(string key)
        {
            string json = File.ReadAllText(SavePath + key + GameManager.activeSaveSlot + ".json");
            string encryption = string.Empty;

            if (EncryptionExists(key))
            {
                using (FileStream stream = new FileStream(SavePath + key + GameManager.activeSaveSlot + ".sav", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    encryption = formatter.Deserialize(stream) as string;
                }
            }

            if (Cipher.Decrypt(encryption, Cipher.pw) == json)
            {
                return JsonUtility.FromJson<T>(json);
            }
            else
            {
                Debug.Log("Save is altered outside of the game!");
                return default;
            }
        }

        public static bool EncryptionExists(string key)
        {
            return File.Exists(SavePath + key + GameManager.activeSaveSlot + ".sav");
        }

        #endregion
    */
    }

    public static class SaveKeyContainer
    {
        public const string PlayerData = "PlayerData";
        public const string InventoryData = "InventoryData_";
    }
}