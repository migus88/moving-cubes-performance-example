using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.UI
{
    [RequireComponent(typeof(Button))]
    public class UiVisibilityButton : MonoBehaviour
    {
        [SerializeField] private KeyCode _toggleKey;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject[] _visibilityAffectedObjects;

        private void Awake()
        {
            _button.onClick.AddListener(ToggleVisibility);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_toggleKey))
            {
                ToggleVisibility();
            }
        }

        private void OnValidate()
        {
            if (!_button)
            {
                _button = GetComponent<Button>();
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

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}