using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Interfaces;

public class SceneLoaderManager
{
    public async Task OnAsyncLoadScene(string sceneName)
    {
        Debug.Log("active");
       AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if(asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            await Task.Yield();
        }
    }
}
