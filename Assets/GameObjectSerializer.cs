using System.Collections;
using System.Collections.Generic;
using UnityEngine; // default 

using System.IO;
using UnityEngine.SceneManagement; // required for this task

// Serializable class to hold GameObject data
[System.Serializable]
public class SerializableGameObject
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    // Add other properties you want to serialize
}

public class GameObjectSerializer : MonoBehaviour
{
    void Start()
    {
        // List to hold serializable GameObject data
        List<SerializableGameObject> serializableObjects = new List<SerializableGameObject>();

        // List to hold all root GameObjects in the scene
        List<GameObject> allGameObjects = new List<GameObject>();

        // Get the active scene
        Scene scene = SceneManager.GetActiveScene();

        // Get all root GameObjects in the scene
        scene.GetRootGameObjects(allGameObjects);

        // Iterate through each root GameObject
        foreach (GameObject rootObject in allGameObjects)
        {
            // Serialize each GameObject and its children
            SerializeGameObject(rootObject, serializableObjects);
        }

        // Convert the list of serializable objects to JSON format
        string json = JsonUtility.ToJson(new SerializationWrapper<SerializableGameObject>(serializableObjects), true);

        // Write the JSON data to a file
        File.WriteAllText(Application.dataPath + "/GameObjects.json", json);
    }

    // Method to serialize a GameObject and its children
    // This is used in the above method
    void SerializeGameObject(GameObject obj, List<SerializableGameObject> list)
    {
        // Create a new SerializableGameObject and populate its fields
        SerializableGameObject serializableObject = new SerializableGameObject
        {
            name = obj.name,
            position = obj.transform.position,
            rotation = obj.transform.rotation
        };

        // Add the serializable object to the list
        list.Add(serializableObject);

        // Iterate through each child Transform
        foreach (Transform child in obj.transform)
        {
            // Recursively serialize each child GameObject
            SerializeGameObject(child.gameObject, list);
        }
    }
}

// Wrapper class to hold a list of serializable objects
[System.Serializable]
public class SerializationWrapper<T>
{
    public List<T> items;

    public SerializationWrapper(List<T> items)
    {
        this.items = items;
    }
}
