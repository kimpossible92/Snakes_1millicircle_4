using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Objects : MonoBehaviour
{
    [SerializeField]private bool isLast = false;
    public void SetLast(bool lst)
    {
        isLast = lst;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [SerializeField] private List<GameObject> GetBlocks = new List<GameObject>();
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "corm")
        {
            FindObjectOfType<MyProjectile>().SetBlocks(coll.gameObject);
            GetBlocks.Add(coll.gameObject);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Block>() != null)
        {
            if (isLast)
            {
                if(collision.gameObject.GetComponent<Block>()!=null)collision.gameObject.GetComponent<Block>().Visible();
            }
            FindObjectOfType<MyProjectile>().RemoveBlocks(collision.gameObject);
            GetBlocks.Remove(collision.gameObject);
        }
    }
}
