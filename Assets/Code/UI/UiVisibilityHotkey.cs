using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    [RequireComponent(typeof(Button))]
    public class UiVisibilityHotkey : MonoBehaviour
    {
        [SerializeField] private KeyCode _toggleKey;
        [SerializeField] private GameObject[] _visibilityAffectedObjects;

        private void Update()
        {
            if (Input.GetKeyDown(_toggleKey))
            {
                ToggleVisibility();
            }
        }
        
        private void ToggleVisibility()
        {
            foreach (var obj in _visibilityAffectedObjects)
            {
                if (!obj)
                {
                    continue;
                }
                
                obj.SetActive(!obj.activeSelf);
            }
        }
    }
}