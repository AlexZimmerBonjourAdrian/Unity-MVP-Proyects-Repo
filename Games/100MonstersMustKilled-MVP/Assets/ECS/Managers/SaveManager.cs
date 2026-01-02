using UnityEngine;
using System.IO;
using ECS.Core;
using ECS.Components.Save;
using ECS.Components.State;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        private static SaveManager instance;
        public static SaveManager Instance { get { return instance; } }

        private World world;
        private string savePath;
        private bool needsSave;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            Initialize();
        }

        private void Initialize()
        {
            savePath = Application.persistentDataPath + "/save.json";
            needsSave = false;
        }

        private void Start()
        {
        }

        private void Update()
        {
        }

        public void SaveGame()
        {
        }

        public void LoadGame()
        {
        }

        public void AutoSave()
        {
        }

        public bool HasSaveFile()
        {
            return File.Exists(savePath);
        }

        public void MarkForSave()
        {
            needsSave = true;
        }

        public void SetWorld(World world)
        {
            this.world = world;
        }

        private void SerializeGameState()
        {
        }

        private void DeserializeGameState(string json)
        {
        }
    }
}

