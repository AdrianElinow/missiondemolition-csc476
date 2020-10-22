using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    // Start is called before the first frame update

    void Start () {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.Sleep();
    }

    void OnCollisionEnter(Collision coll){
       
        GameObject collisionWith = coll.gameObject;
        if (collisionWith.tag == "Projectile"){
            Destroy(gameObject);
        }

    }

}
