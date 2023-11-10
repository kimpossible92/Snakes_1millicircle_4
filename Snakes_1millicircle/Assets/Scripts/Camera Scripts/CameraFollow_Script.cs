using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow_Script : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 cameraOffset;
    private bool SetSkill = false;
    [Range(0.01f, 1.0f)]
    [SerializeField] private float cameraSmoothness = 0.5f;
    [SerializeField] private GameObject objCoockibg;
    // Start is called before the first frame update
    void Start()
    {
        if (!SetSkill) { return; }
        cameraOffset = transform.position - player.transform.position;
    }
    public void SetHero(GameObject playerSl)
    {
        player = playerSl.transform; 
        cameraOffset = transform.position - player.transform.position;
        SetSkill = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!SetSkill) { return; }
        Vector3 newPos = player.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, cameraSmoothness);
    }
}
