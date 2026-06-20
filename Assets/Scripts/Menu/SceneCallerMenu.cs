using System.Runtime.CompilerServices;
using UnityEngine;

namespace GJAM5.Menu
{
    /// <summary>
    /// This class is used for loading the Parachute and GaGa Ball scenes
    /// from the main menu when the respective button is pressed
    /// </summary>
    public class SceneCallerMenu : MonoBehaviour
    {
        #region Variables

        [Header("Variables")]

        [SerializeField] protected string _sceneName;

        [Header("Classes")]

        public SceneLoaderManager loader;

        #endregion

        #region Methods

        public async void OnGameButtonPressed()
        {
            await loader.OnAsyncLoadScene(_sceneName);
        }

        protected void InitialiseScript()
        {
            loader = new SceneLoaderManager();
        }

        #endregion

        #region Unity Methods

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected void Start()
        {
            InitialiseScript();
        }

        #endregion 
    }
}