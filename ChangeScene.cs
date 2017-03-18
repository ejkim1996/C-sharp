using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeScene : MonoBehaviour {
	bool sceneChange = false;
	AsyncOperation async;


	// Use this for initialization
	void Start () {
		StartCoroutine(LoadNewScene());
	}
	
	// Update is called once per frame
	void Update () {
		if (sceneChange) {
			ActivateScene();
		}
	}

	private void OnTriggerEnter(Collider collider) {
		sceneChange = true;
    }

	IEnumerator LoadNewScene() {
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        async = SceneManager.LoadSceneAsync("l1");
		async.allowSceneActivation = false; //setting this to false allows us to control when the scene changes.t
        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone) {
            yield return null;
        }
    }
	private void ActivateScene() {
		//Activate the scene that was loaded
        async.allowSceneActivation = true;
     }
}
