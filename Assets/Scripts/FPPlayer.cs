using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FPPlayer : MonoBehaviour {
	//Movement variables
	public float speed;
	public float sprintSpeedMult;
	public float jumpForce;
	public float gravity;
	

	//States
	Vector3 movement;
	Vector3 input;
	bool isSprinting;
    public bool locked;
    public bool paused;
    public bool gameOver;

    //Menu Showing Stuff
    GameObject pauseMenu;
    bool shownMenu;
    GameObject optionsMenu;
    bool shownOptions;
    bool shouldShowOptions;
    Slider fovSlider;
    Slider sensSlider;
    Toggle invertTog;
    GameObject exitMenu;
    bool shownExit;
    bool shouldShowExit;
    GameObject gameOverScreen;
    bool showGameOverScreen;
	
	//Components
	CharacterController controller;
	public Transform camPos;
	Camera cam;

	//Options
    public bool invertMouse;
    public float mouseSensitivity;
    public float FoV;
	
	void Start(){
		//Find and assign things
		controller = GetComponent<CharacterController>();
		camPos = transform.Find("Camera").transform;
		cam = camPos.GetComponent<Camera>();
        pauseMenu = GameObject.Find("Canvas").transform.Find("Pause").gameObject;
        optionsMenu = GameObject.Find("Canvas").transform.Find("Options").gameObject;
        fovSlider = optionsMenu.transform.Find("FOVSlider").GetComponent<Slider>();
        sensSlider = optionsMenu.transform.Find("SensSlider").GetComponent<Slider>();
        invertTog = optionsMenu.transform.Find("InvertToggle").GetComponent<Toggle>();
        exitMenu = GameObject.Find("Canvas").transform.Find("Exit").gameObject;
        gameOverScreen = GameObject.Find("Canvas").transform.Find("GameOver").gameObject;
		
		//Hide mouse
		Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Default values for options
        FoV = cam.fieldOfView;
        mouseSensitivity = 1;
        invertMouse = false;

        //Release
        locked = false;

        //No menu shown
        shownMenu = false;
        shownOptions = false;
        shouldShowOptions = false;
        shownExit = false;
        shouldShowOptions = false;
        gameOver = false;
        showGameOverScreen = false;
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        exitMenu.SetActive(false);
        gameOverScreen.SetActive(false);
	}

    public void ChangeFoV()
    {
        FoV = fovSlider.value;
        cam.fieldOfView = FoV;
    }

    public void ChangeSensitivity()
    {
        mouseSensitivity = sensSlider.value;
    }

    public void ChangeInvertSetting()
    {
        invertMouse = invertTog.isOn;
    }

    public void ToggleShowOptions()
    {
        shouldShowOptions = !shouldShowOptions;
    }

    public void TogglePause()
    {
        paused = !paused;
    }

    public void ToggleExit()
    {
        shouldShowExit = !shouldShowExit;
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }

    public void QuitToMenu()
    {
        Application.LoadLevel("mainmenu");
    }

    public void ReloadLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

	void Update(){
        if (!locked && !paused && !gameOver)
        {
            input = new Vector3(0, 0, 0);
            if (!controller.isGrounded)
            {
                //Looking
                transform.eulerAngles = transform.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);
                //Up down, with optional inversion
                if (invertMouse) camPos.eulerAngles = camPos.eulerAngles + new Vector3(Input.GetAxis("Mouse Y") * mouseSensitivity, 0, 0);
                else camPos.eulerAngles = camPos.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y") * mouseSensitivity, 0, 0);

                //Gravity
                movement += gravity * Vector3.up * Time.deltaTime;
            }
            else
            {
                //On Ground
                //Looking
                transform.eulerAngles = transform.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);
                //Up down, with optional inversion
                if (invertMouse) camPos.eulerAngles = camPos.eulerAngles + new Vector3(Input.GetAxis("Mouse Y") * mouseSensitivity, 0, 0);
                else camPos.eulerAngles = camPos.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y") * mouseSensitivity, 0, 0);

                isSprinting = Input.GetAxis("Sprint") > 0;

                //Moving
                movement.x /= 10;
                movement.y /= 10;
                movement.z /= 10;
                if (isSprinting) input.x = Input.GetAxis("Horizontal") * sprintSpeedMult;
                else input.x = Input.GetAxis("Horizontal");
                input.z = Input.GetAxis("Vertical");
                input.Normalize();
                input = transform.rotation * input;
                if (isSprinting) input *= speed * sprintSpeedMult * Time.deltaTime;
                else input *= speed * Time.deltaTime;
                movement += input;

                //Jump
                if (Input.GetAxis("Jump") != 0)
                    movement += Vector3.up * jumpForce;
            }
            //Apply the movement
            controller.Move(movement);
        }
        else 
        {
            if (paused)
            {
                if (!shownMenu && !shownOptions && !shouldShowOptions)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    pauseMenu.SetActive(true);
                    shownMenu = true;
                }

                if (!shownOptions && shouldShowOptions)
                {
                    optionsMenu.SetActive(true);
                    pauseMenu.SetActive(false);
                    shownMenu = false;
                    shownOptions = true;
                }
                else if (shownOptions && !shouldShowOptions)
                {
                    optionsMenu.SetActive(false);
                    pauseMenu.SetActive(true);
                    shownMenu = true;
                    shownOptions = false;
                }

                if (!shouldShowExit && shownExit)
                {
                    exitMenu.SetActive(false);
                    pauseMenu.SetActive(true);
                    shownExit = true;
                    shownExit = false;
                }
                else if (shouldShowExit && !shownExit)
                {
                    exitMenu.SetActive(true);
                    pauseMenu.SetActive(false);
                    shownExit = false;
                    shownExit = true;
                }
            }
            if (gameOver)
            {
                paused = false;

                if (!showGameOverScreen)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    
                    optionsMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    exitMenu.SetActive(false);
                    gameOverScreen.SetActive(true);

                    showGameOverScreen = true;
                }
            }
        }

        if (!paused && shownMenu)
        {
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
            exitMenu.SetActive(false);
            shownMenu = false;
            shownOptions = false;
            shouldShowOptions = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (!gameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }
	}
}
