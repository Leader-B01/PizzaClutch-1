using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public bool is_Push = false;
	public float clickPower = 1f;
	private int clickCount = 0;
	public int coin = 0;
	private int upgradeCost = 2;
	private bool firstUpgrade = true;
	public TextMeshProUGUI coinTxt;
	public TextMeshProUGUI powerTxt;
	public TextMeshProUGUI upgradeCostTxt;
	public Button upgradeBtn;
	public Button hawaBtn;
	public Button meatBtn;
	public Button deluxBtn;
	public Button meatBuyBtn;
	public Button deluxBuyBtn;
	public Button newGameBtn;
	public GameObject menuPanel;
	public Image progressBar;

	private int currentMenuIndex = 0;
	private Menu[] menus = new Menu[]
	{
		new Menu("Hawaiian", 50, 10),
		new Menu("Meat Supreme", 500, 100),
		new Menu("Deluxe Cheese", 5000, 4000)
	};

	private float autoSaveInterval = 15f;
	private float timer;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		LoadGame();
	}

	private void Start()
	{
		hawaBtn.onClick.AddListener(SelectHawaiian);
		meatBtn.onClick.AddListener(SelectMeatSupreme);
		deluxBtn.onClick.AddListener(SelectDeluxeCheese);
		meatBuyBtn.onClick.AddListener(BuyMeatSupreme);
		deluxBuyBtn.onClick.AddListener(BuyDeluxCheese);
		newGameBtn.onClick.AddListener(NewGame);

		UpdateUI();
	}

	private void Update()
	{
		coinTxt.text = coin.ToString();
		powerTxt.text = clickPower.ToString("F0");
		upgradeCostTxt.text = upgradeCost.ToString();
		upgradeBtn.interactable = coin >= upgradeCost;

		timer += Time.deltaTime;
		if (timer >= autoSaveInterval)
		{
			AutoSave();
			timer = 0f;
		}
	}

	public void IncrementClickCount()
	{
		int clickIncrease = Mathf.FloorToInt(clickPower);
		clickCount += clickIncrease;

		if (clickCount >= menus[currentMenuIndex].clickThreshold)
		{
			int overflow = clickCount - menus[currentMenuIndex].clickThreshold;
			coin += menus[currentMenuIndex].reward;
			clickCount = overflow;
		}

		UpdateProgressBar();
		UpdateUI();
	}

	public void UpgradeClickPower()
	{
		if (coin >= upgradeCost)
		{
			coin -= upgradeCost;

			if (firstUpgrade)
			{
				clickPower = 3f;
				firstUpgrade = false;
			}
			else
			{
				clickPower *= 1.1f;
			}

			upgradeCost = Mathf.CeilToInt(upgradeCost * 1.2f);
			UpdateUI();
		}
	}

	private void UpdateProgressBar()
	{
		float progress = (float)clickCount / menus[currentMenuIndex].clickThreshold;
		progressBar.fillAmount = progress;
	}

	private void UpdateUI()
	{
		coinTxt.text = coin.ToString();
		powerTxt.text = clickPower.ToString("F0");
		upgradeCostTxt.text = upgradeCost.ToString();
		upgradeBtn.interactable = coin >= upgradeCost;
	}

	public void SelectHawaiian()
	{
		currentMenuIndex = 0;
		clickCount = 0;
		UpdateUI();
	}

	public void SelectMeatSupreme()
	{
		currentMenuIndex = 1;
		clickCount = 0;
		UpdateUI();
	}

	public void SelectDeluxeCheese()
	{
		currentMenuIndex = 2;
		clickCount = 0;
		UpdateUI();
	}

	public void BuyMeatSupreme()
	{
		if (coin >= 500)
		{
			coin -= 500;
			meatBuyBtn.gameObject.SetActive(false);
		}
	}

	public void BuyDeluxCheese()
	{
		if (coin >= 8000)
		{
			coin -= 8000;
			deluxBuyBtn.gameObject.SetActive(false);
		}
	}

	public void MenuSelect()
	{
		menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
	}

	private void AutoSave()
	{
		PlayerPrefs.SetInt("Coin", coin);
		PlayerPrefs.SetFloat("ClickPower", clickPower);
		PlayerPrefs.SetInt("ClickCount", clickCount);
		PlayerPrefs.SetInt("UpgradeCost", upgradeCost);
		PlayerPrefs.SetInt("CurrentMenuIndex", currentMenuIndex);

		PlayerPrefs.SetInt("MeatBuyBtnActive", meatBuyBtn.gameObject.activeSelf ? 1 : 0);
		PlayerPrefs.SetInt("DeluxBuyBtnActive", deluxBuyBtn.gameObject.activeSelf ? 1 : 0);

		PlayerPrefs.Save();
	}

	private void LoadGame()
	{
		if (PlayerPrefs.HasKey("Coin"))
		{
			coin = PlayerPrefs.GetInt("Coin");
			clickPower = PlayerPrefs.GetFloat("ClickPower");
			clickCount = PlayerPrefs.GetInt("ClickCount");
			upgradeCost = PlayerPrefs.GetInt("UpgradeCost");
			currentMenuIndex = PlayerPrefs.GetInt("CurrentMenuIndex");

			bool isMeatBuyBtnActive = PlayerPrefs.GetInt("MeatBuyBtnActive") == 1;
			bool isDeluxBuyBtnActive = PlayerPrefs.GetInt("DeluxBuyBtnActive") == 1;
			meatBuyBtn.gameObject.SetActive(isMeatBuyBtnActive);
			deluxBuyBtn.gameObject.SetActive(isDeluxBuyBtnActive);

			UpdateUI();
		}
	}

	public void NewGame()
	{
		coin = 0;
		clickPower = 1f;
		clickCount = 0;
		upgradeCost = 2;
		firstUpgrade = true;
		currentMenuIndex = 0;

		meatBuyBtn.gameObject.SetActive(true);
		deluxBuyBtn.gameObject.SetActive(true);

		UpdateUI();
	}
}

public class Menu
{
	public string name;
	public int clickThreshold;
	public int reward;

	public Menu(string name, int clickThreshold, int reward)
	{
		this.name = name;
		this.clickThreshold = clickThreshold;
		this.reward = reward;
	}
}
