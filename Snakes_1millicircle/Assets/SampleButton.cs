using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleButton : MonoBehaviour
{
    public static bool isInvincible = false;
    public static float timeSpentInvincible;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("isvisible", 5, 5);
    }
    private void isvisible()
    {
        isInvincible = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            timeSpentInvincible += Time.deltaTime;

            if (timeSpentInvincible < 3f)
            {
                float remainder = timeSpentInvincible % .1f;
                GetComponent<UnityEngine.UI.Image>().enabled = remainder > .10f;
            }

            else
            {
                GetComponent<UnityEngine.UI.Image>().enabled = true;
                isInvincible = false;
            }
        }
    }
}
