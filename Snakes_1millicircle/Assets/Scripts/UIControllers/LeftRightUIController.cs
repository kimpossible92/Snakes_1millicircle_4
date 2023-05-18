using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UIControllers
{
    public class LeftRightUIController : MonoBehaviour
    {
        [Header("Advanced")]
        [SerializeField] Text heading;
        [SerializeField] Text currentSelected;
        [SerializeField] Button leftArrow;
        [SerializeField] Button rightArrow;

        [Header("Heading")]
        [SerializeField] string headingText;
    
        [Header("Value")]
        [SerializeField] int minValue;
        [SerializeField] int maxValue;
        [SerializeField] int step;
        [SerializeField] int defaultValue;
        int currentValue;
        
        public int GetCurrentValue()
        {
            return currentValue;
        }
        
        void Awake()
        {
            if (maxValue < minValue)
                throw new ArgumentException("MaxValue must be more then minValue");
            if (maxValue - minValue < step)
                throw new ArgumentException("Step must be less or equal then (maxValue - minValue)");

            currentValue = math.clamp(defaultValue, minValue, maxValue);
            currentSelected.text = currentValue.ToString();
            heading.text = headingText;
            
            leftArrow.onClick.AddListener(LeftArrowPressed);
            rightArrow.onClick.AddListener(RightArrowPressed);
        }

        void LeftArrowPressed()
        {
            currentValue -= step;

            if (currentValue < minValue)
                currentValue = maxValue;
            
            currentSelected.text = currentValue.ToString();
        }

        void RightArrowPressed()
        {
            currentValue += step;
            
            if (currentValue > maxValue)
                currentValue = minValue;

            currentSelected.text = currentValue.ToString();
        }
    }
}
