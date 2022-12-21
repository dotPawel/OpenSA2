using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AirplaneHangar : UIController
{
	public AirplaneParamsBar airplaneParamsBar;

	public UIIndicator background;

	public UIButton backButton;

	public UIButton armorUpgradeButton;

	public UIButton damageUpgradeButton;

	public UIButton mobilityUpgradeButton;

	public UIButton unlockButton;

	public UIButton switchRightButton;

	public UIButton switchLeftButton;

	public AirplanePreview airplanePreview;

	public UIText titleLabel;

	private AirplaneProfiler airplaneProfiler;

	private AirplaneType selectedAirplane;

	private int factionIndex;

	private bool isFocused;

	[method: MethodImpl(32)]
	public event Action<AirplaneHangar> onBackClicked;

	[method: MethodImpl(32)]
	public event Action<AirplaneHangar> onPurchased;

	private void Start()
	{
		backButton.onClicked += _003CStart_003Em__3;
		armorUpgradeButton.onClicked += OnArmorUpgradeButtonClicked;
		damageUpgradeButton.onClicked += OnDamageUpgradeButtonClicked;
		mobilityUpgradeButton.onClicked += OnMobilityUpgradeButtonClicked;
		airplaneProfiler = AirplaneProfiler.Load();
		selectedAirplane = Player.airplaneType;
		factionIndex = Player.factionIndex;
		switchLeftButton.onClicked += _003CStart_003Em__4;
		switchRightButton.onClicked += _003CStart_003Em__5;
		unlockButton.onClicked += OnUnlockButtonClicked;
	}

	private void OnUnlockButtonClicked(UIButton button)
	{
		AirplaneProfiler.AirplaneInfo airplaneInfo = airplaneProfiler.GetAirplaneInfo(factionIndex, selectedAirplane);
		if (Player.Payment(airplaneInfo.price))
		{
			airplaneInfo.Unlock();
			NotifyPurchase();
			UpdateAirplaneInfo();
		}
	}

	private void SwitchAirplane(int offset)
	{
		AirplaneProfiler.AirplaneInfo[] airplanes = airplaneProfiler.GetAirplanes(factionIndex);
		for (int i = 0; i < airplanes.Length; i++)
		{
			AirplaneProfiler.AirplaneInfo airplaneInfo = airplanes[i];
			if (airplaneInfo.type == selectedAirplane)
			{
				int num = i + offset;
				if (num < 0)
				{
					num = airplanes.Length - 1;
				}
				else if (num >= airplanes.Length)
				{
					num = 0;
				}
				SetAirplane(airplanes[num].type);
				break;
			}
		}
	}

	private void OnMobilityUpgradeButtonClicked(UIButton button)
	{
		AirplaneProfiler.AirplaneInfo airplaneInfo = airplaneProfiler.GetAirplaneInfo(factionIndex, selectedAirplane);
		if (Player.Payment(airplaneInfo.mobilityUpgradePrice))
		{
			airplaneInfo.UpgradeMobility();
			NotifyPurchase();
			UpdateAirplaneInfo();
		}
	}

	private void OnDamageUpgradeButtonClicked(UIButton button)
	{
		AirplaneProfiler.AirplaneInfo airplaneInfo = airplaneProfiler.GetAirplaneInfo(factionIndex, selectedAirplane);
		if (Player.Payment(airplaneInfo.damageUpgradePrice))
		{
			airplaneInfo.UpgradeDamage();
			NotifyPurchase();
			UpdateAirplaneInfo();
		}
	}

	private void OnArmorUpgradeButtonClicked(UIButton button)
	{
		AirplaneProfiler.AirplaneInfo airplaneInfo = airplaneProfiler.GetAirplaneInfo(factionIndex, selectedAirplane);
		if (Player.Payment(airplaneInfo.armorUpgradePrice))
		{
			airplaneInfo.UpgradeArmor();
			NotifyPurchase();
			UpdateAirplaneInfo();
		}
	}

	public void Show()
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		uIStatesController.Show("AirplaneHangar");
	}

	public override void ViewDidAppear()
	{
		airplanePreview.show = true;
		AirplaneProfiler.AirplaneInfo[] airplanes = airplaneProfiler.GetAirplanes(factionIndex);
		int num = airplanes.Length;
		switchLeftButton.disable = num == 1;
		switchRightButton.disable = num == 1;
		SetAirplane(Player.airplaneType);
		background.SetState(factionIndex);
	}

	public override void ViewDidDisappear()
	{
		airplanePreview.show = false;
	}

	public void SetAirplane(AirplaneType airplaneType)
	{
		selectedAirplane = airplaneType;
		airplanePreview.SetModel(factionIndex, (int)airplaneType);
		UpdateAirplaneInfo();
	}

	public void UpdateAirplaneInfo()
	{
		AirplaneProfiler.AirplaneInfo airplaneInfo = airplaneProfiler.GetAirplaneInfo(factionIndex, selectedAirplane);
		titleLabel.text = airplaneInfo.name;
		if (airplaneInfo.unlocked)
		{
			Player.airplaneType = airplaneInfo.type;
		}
		airplaneParamsBar.SetDamageAvaible(airplaneInfo.maxDamageIndex);
		airplaneParamsBar.SetArmorAvaible(airplaneInfo.maxArmorIndex);
		airplaneParamsBar.SetMobilityAvaible(airplaneInfo.maxMobilityIndex);
		airplaneParamsBar.damage = airplaneInfo.damageIndex;
		airplaneParamsBar.armor = airplaneInfo.armorIndex;
		airplaneParamsBar.mobility = airplaneInfo.mobilityIndex;
		if (airplaneInfo.unlocked)
		{
			if (airplaneInfo.isMaxArmorUpgrade)
			{
				armorUpgradeButton.show = false;
			}
			else
			{
				int armorUpgradePrice = airplaneInfo.armorUpgradePrice;
				UIUpgradeButtonView uIUpgradeButtonView = (UIUpgradeButtonView)armorUpgradeButton.view;
				armorUpgradeButton.show = true;
				armorUpgradeButton.disable = !Player.Payment(armorUpgradePrice, true);
				uIUpgradeButtonView.text = armorUpgradePrice.ToString();
				uIUpgradeButtonView.icon = factionIndex;
			}
			if (airplaneInfo.isMaxDamageUpgrade)
			{
				damageUpgradeButton.show = false;
			}
			else
			{
				int damageUpgradePrice = airplaneInfo.damageUpgradePrice;
				UIUpgradeButtonView uIUpgradeButtonView2 = (UIUpgradeButtonView)damageUpgradeButton.view;
				damageUpgradeButton.show = true;
				damageUpgradeButton.disable = !Player.Payment(damageUpgradePrice, true);
				uIUpgradeButtonView2.text = damageUpgradePrice.ToString();
				uIUpgradeButtonView2.icon = factionIndex;
			}
			if (airplaneInfo.isMaxMobilityUpgrade)
			{
				mobilityUpgradeButton.show = false;
			}
			else
			{
				int mobilityUpgradePrice = airplaneInfo.mobilityUpgradePrice;
				UIUpgradeButtonView uIUpgradeButtonView3 = (UIUpgradeButtonView)mobilityUpgradeButton.view;
				mobilityUpgradeButton.show = true;
				mobilityUpgradeButton.disable = !Player.Payment(mobilityUpgradePrice, true);
				uIUpgradeButtonView3.text = mobilityUpgradePrice.ToString();
				uIUpgradeButtonView3.icon = factionIndex;
			}
			unlockButton.show = false;
		}
		else
		{
			unlockButton.show = true;
			armorUpgradeButton.show = false;
			damageUpgradeButton.show = false;
			mobilityUpgradeButton.show = false;
			int price = airplaneInfo.price;
			unlockButton.disable = !Player.Payment(price, true);
			UIUnlockButtonView uIUnlockButtonView = (UIUnlockButtonView)unlockButton.view;
			uIUnlockButtonView.text = price.ToString();
			uIUnlockButtonView.icon = factionIndex;
		}
	}

	private void NotifyPurchase()
	{
		if (this.onPurchased != null)
		{
			this.onPurchased(this);
		}
	}

	private void Update()
	{
		if (!base.show)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isFocused = true;
		}
		if (isFocused && Input.GetKeyUp(KeyCode.Escape))
		{
			isFocused = false;
			if (this.onBackClicked != null)
			{
				this.onBackClicked(this);
			}
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__3(UIButton P_0)
	{
		if (this.onBackClicked != null)
		{
			this.onBackClicked(this);
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__4(UIButton P_0)
	{
		SwitchAirplane(-1);
	}

	[CompilerGenerated]
	private void _003CStart_003Em__5(UIButton P_0)
	{
		SwitchAirplane(1);
	}
}
