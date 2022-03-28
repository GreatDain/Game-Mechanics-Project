using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRoute : MonoBehaviour
{
    [SerializeField] private List<Transform> routes = new List<Transform>();
    private int routeToGo;
    private float tParam;
    private Vector3 nextPosition;
    private float speedModifier;
    private bool coroutineAllowed;
    // Start is called before the first frame update
    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.5f;
        coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (coroutineAllowed){
            StartCoroutine(GoByRoute(routeToGo));
        }
    }

    private IEnumerator GoByRoute(int routeNumber){
        coroutineAllowed = false;

        Vector3 p0 = routes[routeNumber].GetChild(0).position;
        Vector3 p1 = routes[routeNumber].GetChild(1).position;
        Vector3 p2 = routes[routeNumber].GetChild(2).position;
        Vector3 p3 = routes[routeNumber].GetChild(3).position;

// **** BEZIER CURVE FOLLOW ROUTE. ****
        while (tParam < 1){
            tParam += Time.deltaTime * speedModifier;

            nextPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            transform.position = nextPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;

        routeToGo += 1;

// **** IF YOU WANT TO REPEAT ROUTE, USE BELOW. ****

        if (routeToGo > routes.Count -1){
            routeToGo = 0;
        }

// ____________________________________________________________

        coroutineAllowed = true;
    }
}
