using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;  // For Button and UI checks
using UnityEngine.EventSystems; // For checking if the click was on UI

public class Interact : MonoBehaviour
{
	public InteractInputAction interactionAction;
	private InputAction fire;
	public Animator animator;
	private GameManager gameManager;
	public Button upgradeBtn;  // Reference to the Upgrade button

	void Awake()
	{
		gameManager = FindAnyObjectByType<GameManager>();
		interactionAction = new InteractInputAction();
		animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		fire = interactionAction.Player.Fire;
		fire.Enable();
	}

	private void OnDisable()
	{
		fire.Disable();
	}

	void Start()
	{
		fire = InputSystem.actions.FindAction("Fire");
	}

	void Update()
	{
		// Only increment the click count if the click wasn't on a button
		if (fire.WasPressedThisFrame() && !IsClickOnUIButton())
		{
			animator.SetBool("IsPush", true);
			gameManager.IncrementClickCount();

			Debug.Log("Click");
		}
		else if (fire.WasReleasedThisFrame())
		{
			animator.SetBool("IsPush", false);
			Debug.Log("UnClick");
		}
	}

	// Checks if the click is specifically on a button
	private bool IsClickOnUIButton()
	{
		// Return true if the click is specifically on a UI Button
		return EventSystem.current.IsPointerOverGameObject() &&
			   EventSystem.current.currentSelectedGameObject != null &&
			   EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null;
	}
}
