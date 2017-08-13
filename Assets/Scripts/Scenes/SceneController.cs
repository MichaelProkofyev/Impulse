using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : SingletonComponent<SceneController> {

    public SCENE currentScene = SCENE.NONE;


    public void SetScene(SCENE newScene, int subSceneIdx) {
        //DISABLE ALL OTHER SCENES HERE AND DELETE THEIR SHAPES

        Scene1.Instance.InitScene(Scene1.SUBSCENE.NONE_0);
        Scene3.Instance.DisableScene();
        Scene_Cocoon.Instance.DisableScene();
        KinectController.Instance.DisableScene();

        if (newScene != currentScene) {
            Laser.Instance.ClearPatterns();
            switch (newScene) {
                case SCENE.NONE:
                    break;
                case SCENE.CIRCLES_1:
                    Scene1.Instance.InitScene(Scene1.SUBSCENE.FOUR_DOTS_1);
                    break;
                case SCENE.SQUARES_3:
                    Scene3.Instance.InitScene();
                    break;
                case SCENE.COCOON_5:
                    Scene_Cocoon.Instance.InitScene();
                    break;
                case SCENE.KINECT:
                    KinectController.Instance.InitScene();
                    break;
                default:
                    Debug.LogWarning("UNKNOWN FUCKIGN SCENE " + newScene);
                    break;
            }
        }else {
            switch (newScene)
            {
                case SCENE.CIRCLES_1:
                    Scene1.Instance.InitScene((Scene1.SUBSCENE)subSceneIdx);
                    break;
                default:
                    Debug.LogWarning("UNKNOWN FUCKIGN SUB-SCENE " + newScene);
                    break;
            }
        }
        currentScene = newScene;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Keypad0)) {
            SetScene(SCENE.NONE, 0);
            print("Scene 0 NONE");

        }
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            SetScene(SCENE.CIRCLES_1, 1);
            print("Scene 1.1 CIRCLES-4DOTS");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SetScene(SCENE.CIRCLES_1, 2);
            print("Scene 1.2 CIRCLES-CIRCLEDOTS");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            SetScene(SCENE.CIRCLES_1, 3);
            print("Scene 1.3 CIRCLES-CIRCLES");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            SetScene(SCENE.CIRCLES_1, 4);
            print("Scene 1.4 CIRCLES-FOCUS");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            SetScene(SCENE.SQUARES_3, 1);
            print("Scene 3 SQUARES - started");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            SetScene(SCENE.COCOON_5, 0);
            print("Scene COCOON - started");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7)) {
            SetScene(SCENE.KINECT, 0);
            print("Scene KINECT - started");
        }
    }
}
