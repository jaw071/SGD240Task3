using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LoadNamespace
{
    // Serializable class for loading GameObject data
    [System.Serializable]
    public class SerializableGameObject
    {
        public string name;           // Name of the GameObject
        public Vector3 position;      // Position of the GameObject
        public Quaternion rotation;   // Rotation of the GameObject
    }

    // Wrapper class for deserialization
    [System.Serializable]
    public class SerializationWrapper<T>
    {
        public List<T> items;    // List of serializable items
    }

    public class GameObjectLoader : MonoBehaviour
    {
        void Start()
        {
            // Path to the JSON file
            string path = Application.dataPath + "/GameObjects.json";

            // Read the JSON data from the file
            string json = File.ReadAllText(path);

            // Deserialize the JSON data into a list of SerializableGameObjects
            SerializationWrapper<SerializableGameObject> wrapper = JsonUtility.FromJson<SerializationWrapper<SerializableGameObject>>(json);

            // Print the list of items for debugging
            Debug.Log("Loaded GameObjects:");
            foreach (SerializableGameObject data in wrapper.items)
            {
                Debug.Log($"Name: {data.name}, Position: {data.position}, Rotation: {data.rotation}");
            }

            // Instantiate GameObjects in the scene
            foreach (SerializableGameObject data in wrapper.items)
            {
                // Create a new GameObject with the saved name
                GameObject newObj = new GameObject(data.name);

                // Set the position and rotation of the new GameObject
                newObj.transform.position = data.position;
                newObj.transform.rotation = data.rotation;

                // Additional properties can be applied here if needed
            }
        }
    }
}
