using UnityEngine;

namespace GJAM5.Menu
{
    public class SceneCallerGameOverMenu : SceneCallerMenu
    {
        #region Variables

        [SerializeField] private string _mainMenuScene;

        #endregion

        #region Methods

        public void OnRestartPressed()
        {
            loader.ReloadScene(_sceneName);
        }

        public async void OnMenuButtonPressed()
        {
            //loader.ReloadScene(_mainMenuScene);
            await loader.OnAsyncLoadScene(_mainMenuScene);
        }

        #endregion
    }
}