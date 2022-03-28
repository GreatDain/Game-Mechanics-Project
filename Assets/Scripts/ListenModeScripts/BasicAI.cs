using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    public Transform[] targets;
    public float speed = 1.5f;
    int t = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MovementPathing());
    }

    IEnumerator MovementPathing(){
        while (t <= targets.Length){
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targets[t].transform.position, speed * Time.deltaTime);
            if (gameObject.transform.position == targets[t].transform.position){
                t++;
            }
            yield return null;
        }
    }
}
