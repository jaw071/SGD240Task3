using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

// Serializable class to hold GameObject data
[System.Serializable]
public class SerializableGameObject
{
    public string name;  // GameObject name
    public Vector3 position;  // GameObject position
    public Quaternion rotation;  // GameObject rotation
    public string prefabName;  // Name of the prefab
}

public class GameObjectSerializer : MonoBehaviour
{
    void Start()
    {
        List<SerializableGameObject> serializableObjects = new List<SerializableGameObject>();  // List to hold serializable GameObject data
        List<GameObject> allGameObjects = new List<GameObject>();  // List to hold all root GameObjects in the scene

        Scene scene = SceneManager.GetActiveScene();  // Get the active scene
        scene.GetRootGameObjects(allGameObjects);  // Get all root GameObjects in the scene

        // Iterate through each root GameObject
        foreach (GameObject rootObject in allGameObjects)
        {
            // Check if the GameObject name contains "platform"
            if (rootObject.name.ToLower().Contains("platform"))
            {
                // Serialize the GameObject
                SerializeGameObject(rootObject, serializableObjects);
            }
        }

        // Convert the list of serializable objects to JSON format and write to a file
        string json = JsonUtility.ToJson(new SerializationWrapper<SerializableGameObject>(serializableObjects), true);
        File.WriteAllText(Application.dataPath + "/GameObjects.json", json);
    }

    void SerializeGameObject(GameObject obj, List<SerializableGameObject> list)
    {
        string prefabName = obj.name;
        // Remove any duplicate indicators like "(1)", "(2)", etc.
        if (prefabName.Contains("(") && prefabName.Contains(")"))
        {
            prefabName = prefabName.Substring(0, prefabName.IndexOf("(")).Trim();
        }

        // Create a new SerializableGameObject and populate its fields
        SerializableGameObject serializableObject = new SerializableGameObject
        {
            name = obj.name,
            position = obj.transform.position,
            rotation = obj.transform.rotation,
            prefabName = prefabName
        };

        // Add the serializable object to the list
        list.Add(serializableObject);

        // Iterate through each child Transform
        foreach (Transform child in obj.transform)
        {
            // Check if the child GameObject name contains "platform"
            if (child.gameObject.name.ToLower().Contains("platform"))
            {
                // Recursively serialize each child GameObject
                SerializeGameObject(child.gameObject, list);
            }
        }
    }
}

// Wrapper class to hold a list of serializable objects
[System.Serializable]
public class SerializationWrapper<T>
{
    public List<T> items;  // List of serializable items
    public SerializationWrapper(List<T> items)
    {
        this.items = items;
    }
}
