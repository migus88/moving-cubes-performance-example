using UnityEngine;
using TMPro;

namespace Code
{
    /// <summary>
    /// Displays the average FPS over a one-second interval using TextMeshPro.
    /// </summary>
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _fpsText;
        
        [SerializeField] 
        private string _format = "FPS: {0:0.0}";
        
        [SerializeField] 
        private Color _normalColor = Color.green;
        
        [SerializeField] 
        private Color _warningColor = Color.yellow;
        
        [SerializeField] 
        private Color _criticalColor = Color.red;
        
        [SerializeField] 
        private float _warningThreshold = 60f;
        
        [SerializeField] 
        private float _criticalThreshold = 30f;
        
        private int _frameCount;
        private float _timeElapsed;
        private float _averageFps;
        
        private void Update()
        {
            _frameCount++;
            _timeElapsed += Time.unscaledDeltaTime;
            
            // Update FPS display once per second
            if (_timeElapsed >= 1.0f)
            {
                // Calculate average FPS over the last second
                _averageFps = _frameCount / _timeElapsed;
                
                // Update the text with the current FPS
                UpdateFpsText();
                
                // Reset counters
                _frameCount = 0;
                _timeElapsed = 0;
            }
        }
        
        private void UpdateFpsText()
        {
            if (!_fpsText)
                return;
                
            // Update text with formatted FPS value
            _fpsText.text = string.Format(_format, _averageFps);
            
            // Change color based on FPS thresholds
            if (_averageFps <= _criticalThreshold)
            {
                _fpsText.color = _criticalColor;
            }
            else if (_averageFps <= _warningThreshold)
            {
                _fpsText.color = _warningColor;
            }
            else
            {
                _fpsText.color = _normalColor;
            }
        }
        
        private void OnValidate()
        {
            // Ensure _fpsText is assigned
            if (!_fpsText && Application.isPlaying)
            {
                Debug.LogWarning("FpsCounter: No TextMeshProUGUI component assigned!");
            }
        }
    }
}
