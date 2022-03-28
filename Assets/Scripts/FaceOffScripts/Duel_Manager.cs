using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duel_Manager : MonoBehaviour
{
    public List<GameObject> Dueling = new List<GameObject>();
    public List<Transform> visible = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        visible = FindObjectOfType<FieldOfView>().visibleTargets;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            FindDuelists();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)){
            Dueling.Clear();
        }
    }

    public void FindDuelists(){
        if (visible.Count < 3){
            for (int i = 0; i < visible.Count; i++){
                Dueling.Add(visible[i].gameObject);
            }
        }
        else{
            for (int i = 0; i < 3; i++){
                Dueling.Add(visible[i].gameObject);
            }
        }

        this.transform.position = Dueling[0].transform.position;
        SphereCollider col = this.gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = Mathf.Lerp(0, 20, 1);
        // StartDuel();
    }

    // public void StartDuel(){

    // }

    private void OnTriggerEnter(Collider other) {
        List<GameObject> enemies = new List<GameObject>();
        if (other.gameObject.tag == "Enemy"){
            enemies.Add(other.gameObject);
        }

        for (int x = 0; x < enemies.Count; x++){
            if (Dueling.Count >= 3){
                break;
            }
            for (int y = 0; y < Dueling.Count; y++){
                if (enemies[x] != Dueling[y]){
                    Dueling.Add(enemies[x]);
                }

                if (Dueling.Count >= 3){
                    break;
                }
            }
        }

        enemies.Clear();
        if(GetComponent<SphereCollider>() == true){
            Destroy(GetComponent<SphereCollider>());
        }
    }
}
