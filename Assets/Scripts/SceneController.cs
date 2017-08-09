using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : SingletonComponent<SceneController> {
    
    public SCENE CurrentScene
    {
        get { return currentScene; }
        set {
            currentScene = value;
            switch (currentScene)
            {
                case SCENE.NONE:
                    break;
                case SCENE.CIRCLES_1:
                    break;
                case SCENE.TAKEOFF_2:
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


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
