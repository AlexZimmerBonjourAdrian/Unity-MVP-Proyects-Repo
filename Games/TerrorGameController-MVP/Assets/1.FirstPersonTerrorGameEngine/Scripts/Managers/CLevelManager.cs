using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HorrorEngine.LevelManager
{
    public class CLevelManager : MonoBehaviour
    {
        public static event System.Action<string> OnSceneLoadStarted;
        public static event System.Action<string> OnSceneLoadCompleted;

        public static CLevelManager Inst
        {
            get
            {
                if (_inst == null)
                {
                    GameObject obj = new GameObject("Level");
                    return obj.AddComponent<CLevelManager>();
                }

                return _inst;
            }
        }
        private static CLevelManager _inst;

        private AsyncOperation _CurrentLoadScene;

        public void Awake()
        {
            if (_inst != null && _inst != this)
            {
                Destroy(gameObject);
                return;
            }
             DontDestroyOnLoad(this.gameObject);
            _inst = this;
        }

        public bool IsLoadingScene()
        {
            return _CurrentLoadScene != null && !_CurrentLoadScene.isDone;
        }

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void LoadSceneAsync(string name)
        {
            _CurrentLoadScene = SceneManager.LoadSceneAsync(name);
        }

        public void LoadSceneAsyncAdditive(string name)
        {
            _CurrentLoadScene = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        }

        public int GetCurrentSceneID()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

        public void LoadNextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void ApplicationQuit()
        {
            Application.Quit();
        }

        public void ReloadCurrentScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }

        public void UnloadSceneAsync(string name)
        {
            if (SceneManager.GetSceneByName(name).isLoaded)
            {
                SceneManager.UnloadSceneAsync(name);
            }
        }

        public bool SceneExists(string name)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                if (sceneName == name)
                {
                    return true;
                }
            }
            return false;
        }

        public void LoadSceneWithEvents(string name)
        {
            if (!SceneExists(name))
            {
                Debug.LogError($"Scene '{name}' does not exist in build settings.");
                return;
            }

            OnSceneLoadStarted?.Invoke(name);
            _CurrentLoadScene = SceneManager.LoadSceneAsync(name);
            StartCoroutine(WaitForSceneLoad(name));
        }

        private IEnumerator WaitForSceneLoad(string name)
        {
            while (_CurrentLoadScene != null && !_CurrentLoadScene.isDone)
            {
                yield return null;
            }
            OnSceneLoadCompleted?.Invoke(name);
        }
    }
}
