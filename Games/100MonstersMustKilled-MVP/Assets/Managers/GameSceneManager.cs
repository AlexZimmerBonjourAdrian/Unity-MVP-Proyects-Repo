using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameSceneManager : MonoBehaviour
    {
        private static GameSceneManager instance;
        public static GameSceneManager Instance { get { return instance; } }

        private string currentSceneName;
        private string previousSceneName;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
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
            currentSceneName = SceneManager.GetActiveScene().name;
        }

        private void Start()
        {
        }

        public void LoadScene(string sceneName)
        {
        }

        public void LoadSceneAsync(string sceneName)
        {
        }

        public void ReloadCurrentScene()
        {
        }

        public void LoadNextScene()
        {
        }

        public void LoadPreviousScene()
        {
        }

        public string GetCurrentSceneName()
        {
            return currentSceneName;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
        }
    }
}

