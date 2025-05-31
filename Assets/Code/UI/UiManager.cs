using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private Button _toggleVisibilityButton;
        [SerializeField] private GameObject[] _visibilityAffectedObjects;

        private void Awake()
        {
            _toggleVisibilityButton.onClick.AddListener(ToggleVisibility);
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

        private void OnDestroy()
        {
            _toggleVisibilityButton.onClick.RemoveAllListeners();
        }
    }
}