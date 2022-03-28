using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{

// // **** PATH 1: Drawing straight line paths between points****    
// // **** (NB!!! TO USE, CREATE AN OBJECT WITH CHILDREN OBJECTS CONTAINING THE NUMBER OF PATH POINTS YOU REQUIRE AND ATTACH SCRIPTS TO PARENT!) ****

//     [Range(0f, 2f)]
//     [SerializeField] private float waypointSize = 1f;

//     private void OnDrawGizmos() {
//         foreach (Transform trans in transform){
//             Gizmos.color = Color.blue;
//             Gizmos.DrawWireSphere(trans.position, 1f);
//         }

//         Gizmos.color = Color.red;
//         for(int i = 0; i < transform.childCount - 1; i++){
//             Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i+1).position);
//         }

//         Gizmos.DrawLine(transform.GetChild(transform.childCount-1).position, transform.GetChild(0).position);
//     }

// // **** END OF PATH 1 ****



// **** PATH 2: Drawing paths between points using Bezier Curves ****
// // **** (NB!!! TO USE, CREATE AN OBJECT WITH FOUR(4) CHILDREN OBJECTS AND ATTACH SCRIPT TO PARENT!) ****
    [SerializeField] private float waypointSize = 0.1f;

    private Vector3 waypointPosition;

    private void OnDrawGizmos() {
        foreach (Transform trans in transform){
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(trans.position, 0.5f);
        }

        Gizmos.color = Color.red;
        for (float i = 0; i < 1; i += 0.01f){
            waypointPosition = Mathf.Pow(1 - i, 3) * transform.GetChild(0).position +
                3 * Mathf.Pow(1 - i, 2) * i * transform.GetChild(1).position +
                3 * (1 - i) * Mathf.Pow(i, 2) * transform.GetChild(2).position +
                Mathf.Pow(i, 3) * transform.GetChild(3).position;
            
            Gizmos.DrawSphere(waypointPosition, waypointSize);
        }

        Gizmos.DrawSphere(transform.GetChild(transform.childCount - 1).position, waypointSize);

        Gizmos.DrawLine(new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y, transform.GetChild(0).position.z), 
            new Vector3 (transform.GetChild(1).position.x, transform.GetChild(1).position.y, transform.GetChild(1).position.z));

        Gizmos.DrawLine(new Vector3(transform.GetChild(2).position.x, transform.GetChild(2).position.y, transform.GetChild(2).position.z), 
            new Vector3 (transform.GetChild(3).position.x, transform.GetChild(3).position.y, transform.GetChild(3).position.z));
    }

// **** END OF PATH 2 ****
}
