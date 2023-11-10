using UnityEngine;

namespace UIScripts
{
    public class OpenBriefManager : MonoBehaviour
    {
        [SerializeField] private GameObject briefingText;
        
        public void OpenOrCloseBrief()
        {
            if (briefingText.activeSelf)
            {
                briefingText.SetActive(false);
            }
            else
            {
                briefingText.SetActive(true);
            }
        }
    }
}
