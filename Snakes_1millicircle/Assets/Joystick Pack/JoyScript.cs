using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyScript : MonoBehaviour
{
    [SerializeField]protected Joystick joystick;
    [SerializeField]protected JoyButton joyButton;
    public float joyHorizontal;
    public float joyVertical;
    protected bool jump;
    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        //joyButton = FindObjectOfType<JoyButton>();
    }

    // Update is called once per frame
    void Update()
    {
        joyHorizontal = joystick.Horizontal * 100;
        joyVertical = joystick.Vertical * 100;
        //if (!jump && joyButton.Pressed)
        //{
        //    print("jump");
        //    jump = true;
        //}
        //if (jump && !joyButton.Pressed)
        //{
        //    jump = false;
        //}
    }
}
