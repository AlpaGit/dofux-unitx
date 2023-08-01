using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Models
{
    [CreateAssetMenu(fileName = "BinaryDataObject", menuName = "Custom Objects/Binary Data Object")]
    public class BinaryDataObject : ScriptableObject
    {
        [FormerlySerializedAs("Data")]
        [SerializeField]
        public byte[] data;
    
        // menu item to load data
        #if UNITY_EDITOR
        [ContextMenu("Load Data")]
        public void LoadData()
        {
            var path = UnityEditor.EditorUtility.OpenFilePanel("Select a file", "", "d2o");
            if (path.Length != 0)
            {
                LoadData(path);
            }
        }
        #endif


        public void LoadData(string path)
        {
            if (System.IO.File.Exists(path))
            {
                data = System.IO.File.ReadAllBytes(path);
            }
            else
            {
                throw new Exception("File does not exist: " + path);
            }
        }


    }
}