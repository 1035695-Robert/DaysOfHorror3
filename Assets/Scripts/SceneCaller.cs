using System.Threading.Tasks;
using UnityEngine;
using static Interfaces;

public class SceneCaller : MonoBehaviour, IInteractable
{
    public string sceneName;
    public SceneLoaderManager loader;
    private void Start()
    {
        loader = new SceneLoaderManager();
    }
    public async void OnInteract(GameObject target)
    {
        await loader.OnAsyncLoadScene(sceneName);
    }
    private async void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;

            await loader.OnAsyncLoadScene(sceneName);
        }
    }
}
