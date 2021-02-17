using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuyProperties : MonoBehaviour
{
    public float walkSpeed = 2.0f; // if for CPU, set to -1
    bool sentOut {get; set;}
    bool isBottomGuy; // update when the list of guys is fully updated
    [SerializeField]
    public bool hitBoundary;

    [SerializeField]
    float power = 0; // 0 to 1, random value when sent out. higher values beat lower values
    [SerializeField]
    float hp = 1f;
    public MasterController ms;
    //public int index {get; set;} // in list

    void Start()
    {
        isBottomGuy = false;
        setPower();
        ms = GameObject.Find("MasterController").GetComponent<MasterController>();
    }

    public void moveGuy(bool isPlayer){
        if (isPlayer)   walkSpeed *= 1;
        else            walkSpeed *= -1;

        StartCoroutine(startMovement());
    }

    public IEnumerator startMovement(){
        // Start walk animation
        if (isBottomGuy){
            gameObject.GetComponent<Animator>().SetBool("walk", true);
        }
        sentOut = true;

        // Set velocity to 1
        while (!ms.matchFinished){
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(walkSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);

            yield return null;
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Boundary"){
            Debug.Log("B");
            if (ms.started)     ms.switchDirection(gameObject.tag);
        }
        if (gameObject.tag == col.gameObject.tag){
            Debug.Log("Tx");
            //StartCoroutine(switchDirection(col.gameObject));
        }
        else if (col.gameObject.tag == "LB"){
            Debug.Log("LB");
            StartCoroutine(switchDirection(2));
            //ms.switchDirection(gameObject.tag);
        }
        else if (col.gameObject.tag == "RB"){
            Debug.Log("RB");
            StartCoroutine(switchDirection(-2));
        }
        else if (col.gameObject.tag != gameObject.tag && col.gameObject.tag != "Boundary" && col.gameObject.tag != "Untagged"){
            Debug.Log("A");// Remove this object if power is less than the enemy
            //if (power < col.gameObject.GetComponent<GuyProperties>().power){
            col.gameObject.GetComponent<AudioSource>().Play(0);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(walkSpeed * 40f, 15f), ForceMode2D.Impulse);
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(walkSpeed * 40f, 15f), ForceMode2D.Impulse);
            
            hp -= col.gameObject.GetComponent<GuyProperties>().power;
                
            if (hp <= 0) {
                ms.GetComponent<MasterController>().destroyUnit(gameObject);
            }
        }
    }

    public void OnCollisionStay2D(Collision2D col){
        if (gameObject.tag == col.gameObject.tag){
            Debug.Log("Tx");
            //StartCoroutine(switchDirection(col.gameObject));
        }
    }


    IEnumerator switchDirection(int direction){
        walkSpeed = direction;
        //gameObject.GetComponent<Rigidbody2D>().AddForce(transform. * 5f, ForceMode2D.Impulse);
        hitBoundary = true;
        yield return new WaitForSeconds(2f);
    }

    IEnumerator switchDirection(GameObject ally){
        if (hitBoundary){
            ally.GetComponent<GuyProperties>().walkSpeed = walkSpeed;
            ally.GetComponent<GuyProperties>().hitBoundary = true;
        }
        yield return new WaitForSeconds(5f);
    }

    void setPower(){
        power = Random.Range(0, 1.0f);
    }

}
