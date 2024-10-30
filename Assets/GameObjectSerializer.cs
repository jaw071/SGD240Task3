using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

namespace SaveNamespace
{
    [System.Serializable]
    public class SerializableGameObject
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
    }

    public class GameObjectSerializer : MonoBehaviour
    {
        void Start()
        {
            List<SerializableGameObject> serializableObjects = new List<SerializableGameObject>();
            List<GameObject> allGameObjects = new List<GameObject>();

            Scene scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects(allGameObjects);

            foreach (GameObject rootObject in allGameObjects)
            {
                SerializeGameObject(rootObject, serializableObjects);
            }

            string json = JsonUtility.ToJson(new SerializationWrapper<SerializableGameObject>(serializableObjects), true);
            File.WriteAllText(Application.dataPath + "/GameObjects.json", json);
        }

        void SerializeGameObject(GameObject obj, List<SerializableGameObject> list)
        {
            SerializableGameObject serializableObject = new SerializableGameObject
            {
                name = obj.name,
                position = obj.transform.position,
                rotation = obj.transform.rotation
            };

            list.Add(serializableObject);

            foreach (Transform child in obj.transform)
            {
                SerializeGameObject(child.gameObject, list);
            }
        }
    }

    [System.Serializable]
    public class SerializationWrapper<T>
    {
        public List<T> items;
        public SerializationWrapper(List<T> items)
        {
            this.items = items;
        }
    }
}
