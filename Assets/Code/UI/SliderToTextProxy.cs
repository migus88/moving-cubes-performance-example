using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderToTextProxy : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_InputField _inputField;

        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            _inputField.text = value.ToString("F2");
        }

        private void OnValidate()
        {
            if(!_slider)
            {
                _slider = GetComponent<Slider>();
            }
        }
    }
}