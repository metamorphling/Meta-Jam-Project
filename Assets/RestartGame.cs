using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void Restart()
    {
        EasterEgg.showEgg = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
