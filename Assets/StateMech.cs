using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/**
 * StateMech is used to record movements of the player and play them back within the feedforward section of the activity
 * Public variables allow for changes in time before feedforward starts and how long the game will run for
 */

public class StateMech : MonoBehaviour
{

    private Hashtable saved = new Hashtable();
    private int position;
    private int max = 0;
    public float timeWithoutFeedForward = (float)10.0;
    private float timeOffset = (float)0;
    private GameObject camera3rdPerson;
    private GameObject camera1stPerson;
    private ArrayList spheres = new ArrayList();
    public bool OVRActive = false;
    public float timeLoop = (float)30.0;
    public Boundaries boundaries;
    public Zig zigFu;
    private string outputScore = "";
    private SnowSchoolMenu initialGUI;
    private RunEndGUI runEndGUI;
    private RunEndGUI thirdPRunEndGUI;
    private bool playBack = false;
    private float startTime = 0f;
    private float endHitTime = 0f;
    private float runStartTime = 0f;
    private int runEndShownFor = 10;
	private bool feedForwardON = false;
    public float jointAngleThreshold = 10f;
    public float optimalJointAngle = 140f;

    public float maxRunTime = (float)120.0;



    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	/**
	 * Called by EndTrigger to notify that it has reached the end
	 */
    public void returnToStart(bool timedout = false)
    {
        if (!playBack)
        {
            //slow physics
            gameObject.rigidbody.drag = 0.5f;
            endHitTime = Time.time;
            runEndGUI.timeLeft = runEndShownFor;

            if ((endHitTime + runEndShownFor) > timeWithoutFeedForward)
            {
				if(feedForwardON){
                	runEndGUI.resetBools();
                	runEndGUI.playbackNext = true;
                	runEndGUI.runTimeEnd = timedout;
					
					runEndGUI.enabled = true;
				}else{
					//feed forward hasn't been done yet - give them a break
					feedForwardON = true;
					initialGUI.showBreak = true;
					initialGUI.enabled = true;
					gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
				}
            }
            else
            {
                runEndGUI.resetBools();
                runEndGUI.runTimeEnd = timedout;
                runEndGUI.playbackNext = false;
				
				runEndGUI.enabled = true;
			}
			
		}
        else
        {
            endHitTime = Time.time;
            thirdPRunEndGUI.timeLeft = runEndShownFor;

            thirdPRunEndGUI.resetBools();
            thirdPRunEndGUI.endOfPlayback = true;
            thirdPRunEndGUI.enabled = true;
        }

    }

	/**
	 * Accumulates all score variables from each node for the past run
	 */
    Score calculateScore(Transform transform)
    {
        float upleg = 0;
        float foot = 0;

        if (saved.Contains(transform.name))
        {
            GOReference reference = (GOReference)((ArrayList)saved[transform.name])[position];
            if (transform.name.Contains("UpLeg"))
            {
                upleg += reference.score;
            }
            if (transform.name.Contains("Foot"))
            {
                foot += reference.score;
            }
        }

        foreach (Transform child in transform)
        {
            if (!child.name.Contains("Camera"))
            {
                Score temp = calculateScore(child);
                upleg += temp.upleg;
                foot += temp.foot;
            }
        }
        Score s = new Score();
        s.foot = foot;
        s.upleg = upleg;
        return s;
    }
	/**
	 * Returns player back up to top of run. 
	 * 
	 */
    void resetToTop(bool preventPlayback = false)
    {
        runStartTime = Time.time;
        gameObject.transform.position = ((GOReference)((ArrayList)saved[gameObject.name])[0]).position;
        gameObject.transform.rotation = ((GOReference)((ArrayList)saved[gameObject.name])[0]).rotation;
        gameObject.rigidbody.velocity = new Vector3(0, 0, 0);
		if(!playBack){
			Score s = calculateScore(gameObject.transform);
			outputScore = outputScore + (Time.time - startTime) + "," + ((boundaries.leftHits + boundaries.rightHits).ToString()) + "," + s.foot + "," + s.upleg + "\n";
			boundaries.leftHits = 0;
			boundaries.rightHits = 0;
		}

		if (!playBack && ((Time.time - startTime) > timeWithoutFeedForward) && !preventPlayback)
        {
            playBack = true;
            switch3rdPerson(true);
            zigFu.enabled = false;
            Debug.Log("Zig Fu Disabled");
        }
        else
        {
            gameObject.transform.position = ((GOReference)((ArrayList)saved[gameObject.name])[0]).position;
            gameObject.transform.rotation = ((GOReference)((ArrayList)saved[gameObject.name])[0]).rotation;
            gameObject.rigidbody.velocity = new Vector3(0, 0, 0);
            saved = new Hashtable();
            position = 0;
            max = 0;
            playBack = false;
            switch3rdPerson(false);
            zigFu.enabled = true;
            Debug.Log("Zig Fu Enabled");
        }
    }
    // Use this for initialization
    void Start()
    {

        if (!OVRActive)
        {
            camera1stPerson = findGameObject("1stPersonCamera", gameObject);
            camera3rdPerson = findGameObject("3rdPersonCamera", gameObject);
        }
        else
        {
            camera1stPerson = findGameObject("1stPersonOVRCameraController", gameObject);
            camera3rdPerson = findGameObject("3rdPersonOVRCameraController", gameObject);
        }

        turnOff(true, camera3rdPerson);

        initialGUI = gameObject.GetComponentInChildren<SnowSchoolMenu>();
        initialGUI.enabled = false;

        runEndGUI = camera1stPerson.GetComponentInChildren<RunEndGUI>();
        runEndGUI.enabled = false;

        thirdPRunEndGUI = camera3rdPerson.GetComponentInChildren<RunEndGUI>();
        thirdPRunEndGUI.enabled = false;

        // hide the cursor
        Screen.lockCursor = true;
        Screen.showCursor = false;

    }

    // Update is called once per frame
    void Update()
    {
        //detect if health and safety warning is dismissed and show initial message if so
        if (Input.anyKeyDown && startTime == 0 && initialGUI.enabled == false && Time.time > 5.0f)
        {
            //show initial gui
            initialGUI.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //recenter rift
            OVRCamera.ResetCameraPositionOrientation(Vector3.one, Vector3.zero, Vector3.up, Vector3.zero);
        }

        if (startTime != 0 && (Time.time - startTime) > timeLoop)
        {
            int timeLeft = (int)(runEndShownFor - (Time.time - startTime - timeLoop));
            if (timeLeft <= 0)
            {
                //save and quit
                //save and quit
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                System.IO.File.WriteAllText(path + @"\SnowSchoolData" + DateTime.Now.ToString("yyyyMMddHHmmssfff")
+ ".csv", outputScore);
                Application.Quit();
            }
            else
            {
                if (camera1stPerson.camera.enabled == true)
                {
                    runEndGUI.timeLeft = timeLeft;
                    runEndGUI.gameTimeEnd = true;
                    runEndGUI.enabled = true;
                }
                else
                {
                    //show in 3rd person cam
                    thirdPRunEndGUI.timeLeft = timeLeft;
                    thirdPRunEndGUI.gameTimeEnd = true;
                    thirdPRunEndGUI.enabled = true;
                }
            }
            return;
        }

        if (runEndGUI.enabled)
        {
            runEndGUI.timeLeft = (int)(runEndShownFor - (Time.time - endHitTime));
            if (runEndGUI.timeLeft <= 0)
            {
                gameObject.rigidbody.drag = 0;
                runEndGUI.enabled = false;
                resetToTop();
            }
        }

        if (thirdPRunEndGUI.enabled)
        {
            thirdPRunEndGUI.timeLeft = (int)(runEndShownFor - (Time.time - endHitTime));
            if (thirdPRunEndGUI.timeLeft <= 0)
            {
                gameObject.rigidbody.drag = 0;
                thirdPRunEndGUI.enabled = false;
                resetToTop();
            }
        }
        if (initialGUI.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                initialGUI.enabled = false;
				gameObject.rigidbody.constraints = RigidbodyConstraints.None;
				gameObject.rigidbody.drag = 0;
				if(feedForwardON){
					//return to start and skip playback

					resetToTop(true);
				}else{
                	startTime = Time.time;
					runStartTime = Time.time;
				}
            }
        }
        else if (!(startTime == 0))
        {
            //Debug.Log ("Start time not 0 Playback: " +playBack);
            if (!playBack)
            {
                //Debug.Log("Saving position");
                updateHash(gameObject.transform);
                if (saved["weight"] == null)
                {
                    saved["weight"] = new ArrayList();
                }
                ((ArrayList)saved["weight"]).Add(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
                max++;
            }
            else
            {
                //Debug.Log("FeedForward");
                feedForward();
            }
        }

        if (startTime != 0 && (Time.time - runStartTime) > maxRunTime)
        {
            returnToStart(true);
            runStartTime = Time.time; // set to be large so that doesn't get called to reset again
        }

    }

	/**
	 * used to place player and all inner nodes from previous run.
	 * Places red nodes where player should improve
	 */
    void feedForward()
    {
        foreach (GameObject sphere in spheres)
        {
            Destroy(sphere);
        }
        spheres.Clear();

        setFromHash(gameObject.transform);

        position++;
        if (position == max)
        {
            timeOffset = Time.time;

            // Destroy the last frame of spheres
            foreach (GameObject sphere in spheres)
            {
                Destroy(sphere);
            }
            spheres.Clear();
        }
    }

	/**
	 * Recursively cycles through nodes and places them according to list in saved hash variable
	 */
    void setFromHash(Transform transform)
    {
        if (saved.Contains(transform.name))
        {
            GOReference reference = (GOReference)((ArrayList)saved[transform.name])[position];
            Vector3 gamePosition = reference.position;
            Quaternion gamerotation = reference.rotation;
            transform.position = gamePosition;
            transform.rotation = gamerotation;

            // Create red highlighted portions for all transforms that contain a certain string
            if (transform.name.Contains("UpLeg"))
            {

                GameObject mySphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                spheres.Add(mySphere);
                mySphere.collider.enabled = false;
                mySphere.transform.localScale = new Vector3(1, 1, 1) / 5;
                mySphere.transform.position = transform.position;
                Color color = mySphere.renderer.material.color;

                color = Color.red;
                color.a = 1.0f - reference.score / 10.0f;
                mySphere.renderer.material.color = color;
                mySphere.renderer.material.shader = Shader.Find("Transparent/Diffuse");
                //Debug.Log ("Shader - " + mySphere.renderer.material.shader);
            }
            else if (transform.name.Contains("Foot"))
            {
                Vector2 weight = ((Vector2)((ArrayList)saved["weight"])[position]);
                if (transform.name.Contains("Left"))
                {
                    if (reference.velocity.x < -5 && weight.x > 0)
                    {
                        GameObject mySphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        spheres.Add(mySphere);
                        mySphere.collider.enabled = false;
                        mySphere.transform.localScale = new Vector3(1, 1, 1) / 5;
                        mySphere.transform.position = transform.position;
                        Color color = mySphere.renderer.material.color;

                        color = Color.red;
                        color.a = (float)(1.0 - weight.x / 15.0);
                        reference.score = 10 - color.a * 10;
                        mySphere.renderer.material.color = color;
                        mySphere.renderer.material.shader = Shader.Find("Transparent/Diffuse");
                    }
                    else
                    {
                        reference.score = 10;
                    }

                }
                else
                {
                    if (reference.velocity.x > 5 && weight.x < 0)
                    {
                        GameObject mySphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        spheres.Add(mySphere);
                        mySphere.collider.enabled = false;
                        mySphere.transform.localScale = new Vector3(1, 1, 1) / 5;
                        mySphere.transform.position = transform.position;
                        Color color = mySphere.renderer.material.color;

                        color = Color.red;
                        color.a = (float)(1.0 - weight.x / 15.0);
                        reference.score = 10 - color.a * 10;
                        mySphere.renderer.material.color = color;
                        mySphere.renderer.material.shader = Shader.Find("Transparent/Diffuse");
                    }
                    else
                    {
                        reference.score = 10;
                    }
                }

            }

            foreach (Transform child in transform)
            {
                if (!child.name.Contains("Camera"))
                {
                    setFromHash(child);
                }
            }
        }
    }

	/**
	 * Recursively cycles through body nodes and records transform in saved hash
	 */
    void updateHash(Transform transform)
    {
        if (saved[transform.name] == null)
        {
            saved[transform.name] = new ArrayList();
        }
        GOReference reference = new GOReference();
        reference.position = transform.position;
        reference.rotation = transform.rotation;
        reference.velocity = transform.root.gameObject.rigidbody.velocity;
        ((ArrayList)saved[transform.name]).Add(reference);

        if (transform.name.Contains("UpLeg"))
        {
            reference.score += checkJointAngles(transform, transform.GetChild(0));
        }

        foreach (Transform child in transform)
        {
            if (!child.name.Contains("Camera"))
            {
                updateHash(child);
            }
        }
    }

	/**
	 * Switches to and from third person perspective
	 */
    void switch3rdPerson(bool to)
    {
        Camera[] cameras = new Camera[Camera.allCamerasCount];
        Camera.GetAllCameras(cameras);
        if (camera1stPerson != null)
        {
            if (to)
            {
                turnOff(true, camera1stPerson);
                turnOff(false, camera3rdPerson);
            }
            else
            {
                turnOff(false, camera1stPerson);
                turnOff(true, camera3rdPerson);
            }
        }
    }

	/**
	 * Turns off cameras attached to gameobject provided
	 */
    void turnOff(bool off, GameObject camera)
    {
        if (camera.name == "1stPersonCamera" || camera.name == "3rdPersonCamera")
        {
            camera.camera.enabled = !off;
        }
        else if (camera.name == "1stPersonOVRCameraController" || camera.name == "3rdPersonOVRCameraController")
        {
            foreach (Transform child in camera.transform)
            {
                child.gameObject.camera.enabled = !off;
            }
        }
    }

	/**
	 * Recursively cycles through child gameobjects to find node with name
	 */
    GameObject findGameObject(string name, GameObject go)
    {
        if (go.name == name)
        {
            return go;
        }
        foreach (Transform child in go.transform)
        {
            GameObject hope = findGameObject(name, child.gameObject);
            if (hope != null)
            {
                return hope;
            }
        }
        return null;
    }

	/**
	 * Checks for angle between gamobjects and assigns a score
	 */
    float checkJointAngles(Transform upperLeg, Transform lowerLeg)
    {
        float angle = Quaternion.Angle(upperLeg.rotation, lowerLeg.rotation);

        if (Mathf.Abs(angle - optimalJointAngle) < jointAngleThreshold)
        {
            return 10;
        }
        else if (Mathf.Abs(angle - optimalJointAngle) > 10)
        {
            return 0;
        }
        return 10 - Mathf.Abs(angle - optimalJointAngle);
    }

    static void SaveHashtableFile(Hashtable ht, string path)
    {
        BinaryFormatter bfw = new BinaryFormatter();
        FileStream file = File.OpenWrite(path);
        StreamWriter ws = new StreamWriter(file);
        bfw.Serialize(ws.BaseStream, ht);
        file.Close();
    }

    static Hashtable OpenHashtableFile(string path)
    {
        FileStream filer = File.OpenRead(path);
        StreamReader readMap = new StreamReader(filer);
        BinaryFormatter bf = new BinaryFormatter();
        Hashtable ret = (Hashtable)bf.Deserialize(readMap.BaseStream);
        filer.Close();
        return ret;
    }

}

// Object to record details of a GameObject transform 
class GOReference
{
    public Vector3 position;
    public Quaternion rotation;
    public float score = 0f;
    public Vector3 velocity;
}

// Object to record scores of body parts
class Score
{
    public float upleg;
    public float foot;
}
