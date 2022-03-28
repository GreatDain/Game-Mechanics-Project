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
        if (visible.Count < 3 && visible.Count > 0){
            for (int i = 0; i < visible.Count; i++){
                Dueling.Add(visible[i].gameObject);
            }
        }
        else if (visible.Count == 3){
            for (int i = 0; i < 3; i++){
                Dueling.Add(visible[i].gameObject);
            }
        }
        
        if (Dueling.Count < 3 && Dueling.Count > 0){
            this.transform.position = Dueling[0].transform.position;
            SphereCollider col = this.gameObject.AddComponent<SphereCollider>();
            col.isTrigger = true;
            for (float x = 0; x < 1; x += Time.deltaTime/0.5f){
                col.radius = Mathf.Lerp(0, 20, x);
            }
            // col.radius = 20;
        }
        StartDuel();
    }

    public void StartDuel(){
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy"){
            for (int i = 0; i < Dueling.Count; i++){
                print("Dueling Count: " + Dueling.Count);
                if (Dueling.Count == 1){
                    if (other.gameObject.name != Dueling[i].name){
                        Dueling.Add(other.gameObject);
                    }
                }else if(Dueling.Count == 2){
                    if ((other.gameObject.name != Dueling[i].name) && (other.gameObject.name != Dueling[i+1].name)){
                        Dueling.Add(other.gameObject);
                    }
                }
                else if(Dueling.Count >= 3){
                    if(GetComponent<SphereCollider>() == true){
                        Destroy(GetComponent<SphereCollider>());
                    }
                    break;
                }
            }
        }
    }


}
