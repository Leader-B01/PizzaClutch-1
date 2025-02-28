using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Interact : MonoBehaviour
{
	public InteractInputAction interactionAction;
	private InputAction fire;
	public Animator animator;
	private GameManager gameManager;
	public Button upgradeBtn;

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

	private bool IsClickOnUIButton()
	{
		return EventSystem.current.IsPointerOverGameObject() &&
			   EventSystem.current.currentSelectedGameObject != null &&
			   EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null;
	}
}
