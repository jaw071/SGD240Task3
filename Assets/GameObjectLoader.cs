using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LoadNamespace
{
    [System.Serializable]
    public class SerializableGameObject
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
    }

    [System.Serializable]
    public class SerializationWrapper<T>
    {
        public List<T> items;
    }

    public class GameObjectLoader : MonoBehaviour
    {
        void Start()
        {
            string path = Application.dataPath + "/GameObjects.json";
            string json = File.ReadAllText(path);

            SerializationWrapper<SerializableGameObject> wrapper = JsonUtility.FromJson<SerializationWrapper<SerializableGameObject>>(json);

            foreach (SerializableGameObject data in wrapper.items)
            {
                GameObject newObj = new GameObject(data.name);
                newObj.transform.position = data.position;
                newObj.transform.rotation = data.rotation;
            }
        }
    }
}
