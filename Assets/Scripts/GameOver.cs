using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      StartCoroutine (Coroutine());
    }

    // Update is called once per frame
    IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("StageZero");
    
    }
}
