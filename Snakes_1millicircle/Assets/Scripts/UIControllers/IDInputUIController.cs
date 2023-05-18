using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UIControllers
{
    public class IDInputUIController : MonoBehaviour
    {
        [SerializeField] InputField inputFieldID;
        [SerializeField] Button connectByIDButton;
    
        // Start is called before the first frame update
        void Start()
        {
            inputFieldID.onValueChanged.AddListener(OnValueChange);
        }

        void OnValueChange(string input)
        {
            //Only positive numbers
            int.TryParse(input, out var intInput);
            intInput = math.clamp(intInput, 0, Int16.MaxValue);
            inputFieldID.text = intInput.ToString(); 
        
            connectByIDButton.interactable = intInput != 0;
        }

        public int GetCurrentInput()
        {
            return connectByIDButton.interactable ? int.Parse(inputFieldID.text) : 0;
        }
    }
}
