using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LoadNamespace
{
    [System.Serializable]
    public class SerializableGameObject
    {
        public string name;  // GameObject name
        public Vector3 position;  // GameObject position
        public Quaternion rotation;  // GameObject rotation
        public string prefabName;  // Name of the prefab
    }

    [System.Serializable]
    public class SerializationWrapper<T>
    {
        public List<T> items;  // List of serializable items
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

                // Load the prefab from the Resources folder
                string prefabPath = "Platforms/" + data.prefabName;
                GameObject prefab = Resources.Load<GameObject>(prefabPath);

                if (prefab != null)
                {
                    // Instantiate the prefab at the specified position and rotation
                    GameObject newObj = Instantiate(prefab, data.position, data.rotation);

                    // Set the name of the new GameObject
                    newObj.name = data.name;

                    // Log the successful creation of the GameObject
                    Debug.Log("Created GameObject: " + newObj.name);
                }
                else
                {
                    // Log an error if the prefab was not found
                    Debug.LogError("Prefab not found at path: " + prefabPath);
                }
            }
        }
    }
}
