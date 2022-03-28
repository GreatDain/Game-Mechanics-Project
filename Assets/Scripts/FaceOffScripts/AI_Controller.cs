using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.LookAt(player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.LookAt(player.transform);
    }
}
