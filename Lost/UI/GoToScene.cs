//-----------------------------------------------------------------------
    using UnityEngine.SceneManagement;

    public class GoToScene : MonoBehaviour
        {
            SceneManager.LoadScene(sceneName);
        }

        public void GoToSceneIndex(int sceneBuildIndex)
        {
            SceneManager.LoadScene(sceneBuildIndex);
        }