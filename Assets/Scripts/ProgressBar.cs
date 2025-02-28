using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
	public Image progressFill; // The bar that will fill up
	private float progress = 0f; // Start at 0%
	private bool isFilling = false; // Start hidden

	void Start()
	{
		progressFill.fillAmount = 0f; // Make sure it starts empty
	}

	void Update()
	{
		if (isFilling && progress < 1f)
		{
			progress += Time.deltaTime * 0.5f; // Adjust speed of filling
			progressFill.fillAmount = progress;
		}
	}

	public void OnClick()
	{
		isFilling = true; // Start filling when clicked
	}
}
