using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
   [SerializeField] private GameObject mainCanvas;
   [SerializeField] private GameObject loadingCanvas;
   
   public void OnExitGameButton()
   {
      Application.Quit();
   }

   public void OnNewGameButton()
   {
      mainCanvas.SetActive(false);
      loadingCanvas.SetActive(true);
      //ToDo add audio Manager
      StartCoroutine(LoadSceneAsynchro());
   }

   private IEnumerator LoadSceneAsynchro()
   {
      AsyncOperation load= SceneManager.LoadSceneAsync(1);
      while (!load.isDone)
      {
         yield return null;
      }
   }
   
   
}
