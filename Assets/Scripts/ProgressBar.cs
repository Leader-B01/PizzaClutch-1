using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
	public Image progressFill; 
	private float progress = 0f; 
	private bool isFilling = false; 

	void Start()
	{
		progressFill.fillAmount = 0f; 
	}

	void Update()
	{
		if (isFilling && progress < 1f)
		{
			progress += Time.deltaTime * 0.5f; 
			progressFill.fillAmount = progress;
		}
	}

	public void OnClick()
	{
		isFilling = true; 
	}
}
