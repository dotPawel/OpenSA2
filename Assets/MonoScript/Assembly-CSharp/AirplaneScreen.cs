using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AirplaneScreen : UIController
{
	private const float PARAM_COUNT = 6f;

	public AirplanePreview airplanePreview;

	public AirplaneEquipment airplaneEquipment;

	public AirplaneParamsBar airplaneParams;

	public UIButton switchRightButton;

	public UIButton switchLeftButton;

	public UIText titleLabel;

	public UIButton doneButton;

	public UIIndicator background;

	private int factionIndex;

	private int airplaneCapacity;

	private AirplaneProfiler.AirplaneInfo[] airplaneInfos;

	private AirplaneProfiler airplaneProfilerCache;

	private AirplaneProfiler airplaneProfiler
	{
		get
		{
			if (airplaneProfilerCache == null)
			{
				airplaneProfilerCache = AirplaneProfiler.Load();
			}
			return airplaneProfilerCache;
		}
	}

	private string title
	{
		set
		{
			titleLabel.text = value;
		}
	}

	[method: MethodImpl(32)]
	public event Action<AirplaneScreen> onCompleted;

	private void Start()
	{
		switchRightButton.onClicked += _003CStart_003Em__6;
		switchLeftButton.onClicked += _003CStart_003Em__7;
		airplaneEquipment.onEquipmentChanged += OnAirplaneEquipmentChanged;
		doneButton.onClicked += _003CStart_003Em__8;
	}

	private void OnAirplaneEquipmentChanged(AirplaneEquipment airplaneEquipment, int value)
	{
		Player.airplaneEquipment = value;
	}

	public void Show()
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		uIStatesController.Show("AirplaneScreen");
	}

	private void SwitchAirplane(int offset)
	{
		AirplaneType airplaneType = Player.airplaneType;
		for (int i = 0; i < airplaneInfos.Length; i++)
		{
			AirplaneProfiler.AirplaneInfo airplaneInfo = airplaneInfos[i];
			if (airplaneInfo.type == airplaneType)
			{
				int num = i + offset;
				if (num < 0)
				{
					num = airplaneInfos.Length - 1;
				}
				else if (num >= airplaneInfos.Length)
				{
					num = 0;
				}
				SetAirplane(airplaneInfos[num].type);
				break;
			}
		}
	}

	public override void ViewDidAppear()
	{
		factionIndex = Player.factionIndex;
		airplaneInfos = airplaneProfiler.GetAvailableAirplanes(factionIndex);
		background.SetState(factionIndex);
		int num = airplaneInfos.Length;
		switchLeftButton.disable = num <= 1;
		switchRightButton.disable = num <= 1;
		airplanePreview.show = true;
		AirplaneType type = airplaneInfos[0].type;
		for (int i = 0; i < airplaneInfos.Length; i++)
		{
			if (airplaneInfos[i].type == Player.airplaneType)
			{
				type = airplaneInfos[i].type;
				break;
			}
		}
		SetAirplane(type);
	}

	public override void ViewDidDisappear()
	{
		airplanePreview.show = false;
	}

	private void SetAirplane(AirplaneType airplaneType)
	{
		Player.airplaneType = airplaneType;
		for (int i = 0; i < airplaneInfos.Length; i++)
		{
			AirplaneProfiler.AirplaneInfo airplaneInfo = airplaneInfos[i];
			if (airplaneInfo.type == airplaneType)
			{
				title = airplaneInfo.name;
				airplaneParams.armor = Mathf.RoundToInt(6f * airplaneInfo.armorIndexRate);
				airplaneParams.damage = Mathf.RoundToInt(6f * airplaneInfo.damageIndexRate);
				airplaneParams.mobility = Mathf.RoundToInt(6f * airplaneInfo.mobilityIndexRate);
				break;
			}
		}
		airplanePreview.SetModel(factionIndex, (int)airplaneType);
		airplaneEquipment.SetEquipment(Player.airplaneEquipment);
	}

	[CompilerGenerated]
	private void _003CStart_003Em__6(UIButton P_0)
	{
		SwitchAirplane(1);
	}

	[CompilerGenerated]
	private void _003CStart_003Em__7(UIButton P_0)
	{
		SwitchAirplane(-1);
	}

	[CompilerGenerated]
	private void _003CStart_003Em__8(UIButton P_0)
	{
		if (this.onCompleted != null)
		{
			this.onCompleted(this);
		}
	}
}
