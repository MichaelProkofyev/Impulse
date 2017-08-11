using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : SingletonComponent<SceneController> {
    
    public SCENE CurrentScene
    {
        get { return currentScene; }
        set {
            currentScene = value;
            Scene1.Instance.gameObject.SetActive(false);
            Scene2.Instance.gameObject.SetActive(false);
            switch (currentScene)
            {
                case SCENE.NONE:
                    break;
                case SCENE.CIRCLES_1:
                    Scene1.Instance.gameObject.SetActive(true);
                    Scene1.Instance.InitScene();
                    break;
                case SCENE.TAKEOFF_2:
                    Scene2.Instance.gameObject.SetActive(true);
                    break;
                case SCENE.SQUARES_3:
                    break;
                case SCENE.GRID_4:
                    break;
                case SCENE.COCOON_5:
                    break;
            }
        }
    }

    public SCENE currentScene = SCENE.NONE;


    public void SetScene(SCENE sceneIdx, int subSceneIdx)
    {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
