using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewItem : MonoBehaviour {

    public float timeMax = float.PositiveInfinity;
    public float time = 0f;
    public float timeRatio => time / timeMax;
    public float hpMax = 100f;
    public float hp = 100f;
    public GameObject self;
    private Vector3 newPos1, newPos2;

    private bool alive;
    public bool canDouble;

    public Vector3 velocity = new Vector3(0f, 0f, 0f);
    public Vector3 angularVelocity = new Vector3(0f, 0f, 0f);

    [Range(0, 1), Tooltip("frottement dans \"l'air\": 0 = rien, 1 = statique.")]
    public float drag = 0f;

   private void Start ()
    {
        hp = hpMax;
        alive = true;
    }
    public void SetDamage(float damage) {

        hp += -damage;

        if (hp <= 0 && alive) {
            alive = false;
            hp = 0;
            Debug.Log("debug");
            Spawner();
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {

        time += Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;
        transform.rotation *= Quaternion.Euler(angularVelocity * Time.fixedDeltaTime);

        velocity *= Mathf.Pow(1f - drag, Time.fixedDeltaTime);

        // test de la durée de vie (auto-suicide si trop vieux)
        if (time > timeMax) {
            Destroy(gameObject);
        }

        // test des limites de jeu (auto-suicide si sortie)
        if (Stage.instance != null && Stage.instance.IsInsideMargin(transform.position) == false) {
            Destroy(gameObject);
        }
    }
    
    private void Spawner() {
        
        if (canDouble)
        {
            newPos1 = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
            newPos2 = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            Instantiate(self, newPos1, transform.rotation);
            Instantiate(self, newPos2, transform.rotation);
        }
        
    }

#if UNITY_EDITOR
    void OnValidate() {
        // élégant à l'usage, mais compliqué à écrire : dans l'inspecteur, empêcher "hp" d'être supérieur à "hpMax"
        System.Func<System.Threading.Tasks.Task> CheckHp = async () => {
            await System.Threading.Tasks.Task.Delay(400);
            if (hp > hpMax) {
                hp = hpMax;
            }    
        };
        CheckHp();
    }
#endif
}

