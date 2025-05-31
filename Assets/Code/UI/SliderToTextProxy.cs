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
            _inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        }

        private void OnInputFieldValueChanged(string value)
        {
            _slider.SetValueWithoutNotify(float.TryParse(value, out float parsedValue) ? parsedValue : _slider.value);
        }

        private void OnSliderValueChanged(float value)
        {
            _inputField.text = _slider.wholeNumbers 
                ? Mathf.RoundToInt(value).ToString() 
                : value.ToString("F2");
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