using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerControl : PlayerListener
{
	public ControlType controlType;

	public AudioSource audioSource;

	private Airplane airplane;

	private AirplaneStatusBar airplaneStatusBar;

	private Vector3 direction;

	private float rotateDirection;

	private HUD hud;

	private void Update()
    {
		// shooting / bombs
		if (Input.GetKey(KeyCode.LeftShift))
        {
			Fire();
		}
		if (Input.GetKey(KeyCode.Space))
		{
			DropBomb();
		}

		// controlling the plane
		if (Input.GetKey(KeyCode.UpArrow))
		{
			MoveAirplane(Vector3.up);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			MoveAirplane(Vector3.down);
		}

		// resetting plane movement
		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			MoveAirplane(Vector3.zero);
		}
		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			MoveAirplane(Vector3.zero);
		}

	}

	private void Awake()
	{
		hud = UIController.Find<HUD>();
		airplaneStatusBar = hud.airplaneStatusBar;
		hud.moveUpButton.onPressed += _003CAwake_003Em__29;
		hud.moveUpButton.onReleased += _003CAwake_003Em__2A;
		hud.moveDownButton.onPressed += _003CAwake_003Em__2B;
		hud.moveDownButton.onReleased += _003CAwake_003Em__2C;
		hud.fireButton.onPressed += _003CAwake_003Em__2D;
		hud.bombButton.onClicked += _003CAwake_003Em__2E;
		hud.takeoffButton.onClicked += _003CAwake_003Em__2F;
		controlType = Player.controlType;
		UIControlButtonView uIControlButtonView = (UIControlButtonView)hud.moveUpButton.view;
		uIControlButtonView.SetIcon((int)controlType);
		UIControlButtonView uIControlButtonView2 = (UIControlButtonView)hud.moveDownButton.view;
		uIControlButtonView2.SetIcon((int)controlType);

		if (Application.platform != RuntimePlatform.Android || Application.platform != RuntimePlatform.IPhonePlayer) { // disable controll butons when not on a phone
			hud.moveDownButton.gameObject.SetActive(false);
			hud.moveUpButton.gameObject.SetActive(false);
			hud.fireButton.gameObject.SetActive(false);
			hud.bombButton.gameObject.SetActive(false);
		}
	}

	private void MoveAirplane(Vector3 vector)
	{
		if (airplane != null && vector != direction)
		{
			direction = vector;
			if (controlType == ControlType.Direction)
			{
				airplane.Move(direction);
			}
			else
			{
				airplane.Move((0f - Mathf.Sign(airplane.right.z)) * direction);
			}
		}
	}

	private void Fire()
	{
		if (!(airplane != null))
		{
			return;
		}
		AirMovement.State state = airplane.movement.state;
		if (state == AirMovement.State.Flying || state == AirMovement.State.Falling)
		{
			if (airplane.machineGun.ammoRate > 0f)
			{
				airplane.Fire();
			}
			else if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}
	}

	private void DropBomb()
	{
		if (airplane != null)
		{
			AirMovement.State state = airplane.movement.state;
			if (state == AirMovement.State.Flying || state == AirMovement.State.Falling)
			{
				airplane.DropBomb();
			}
		}
	}

	private void Takeoff()
	{
		if (airplane != null)
		{
			AirMovement.State state = airplane.movement.state;
			if (state == AirMovement.State.Landed)
			{
				airplane.movement.Takeoff();
			}
		}
	}

	public override void OnAirplaneSet(Airplane airplane)
	{
		if (this.airplane != null)
		{
			this.airplane.onHealthUpdated -= OnAirplaneArmorUpdated;
			this.airplane.machineGun.onAmmoUpdated -= OnMachineGunAmmoUpdated;
			this.airplane.bombLauncher.onBombCountUpdated -= OnBombCountUpdated;
			this.airplane.movement.onFuelUpdated -= OnFuelUpdated;
		}
		this.airplane = airplane;
		if (airplane != null)
		{
			airplane.onHealthUpdated += OnAirplaneArmorUpdated;
			airplane.machineGun.onAmmoUpdated += OnMachineGunAmmoUpdated;
			airplane.bombLauncher.onBombCountUpdated += OnBombCountUpdated;
			airplane.movement.onFuelUpdated += OnFuelUpdated;
			hud.bombButton.disable = airplane.bombLauncher.bombCount == 0;
			airplaneStatusBar.ammoRate = airplane.machineGun.ammoRate;
			airplaneStatusBar.bombCount = airplane.bombLauncher.bombCount;
			airplaneStatusBar.armorRate = airplane.healthRate;
			airplaneStatusBar.fuelRate = airplane.movement.fuelRate;
		}
	}

	public override void OnControlTypeUpdated(ControlType controlType)
	{
		this.controlType = controlType;
		UIControlButtonView uIControlButtonView = (UIControlButtonView)hud.moveUpButton.view;
		uIControlButtonView.SetIcon((int)controlType);
		UIControlButtonView uIControlButtonView2 = (UIControlButtonView)hud.moveDownButton.view;
		uIControlButtonView2.SetIcon((int)controlType);
	}

	private void OnBombCountUpdated(BombLauncher bombLauncher, int count)
	{
		airplaneStatusBar.bombCount = count;
		hud.bombButton.disable = count == 0;
	}

	private void OnFuelUpdated(AirMovement movement)
	{
		airplaneStatusBar.fuelRate = movement.fuelRate;
	}

	private void OnMachineGunAmmoUpdated(MachineGun machineGun)
	{
		airplaneStatusBar.ammoRate = machineGun.ammoRate;
	}

	private void OnAirplaneArmorUpdated(Target airplane)
	{
		airplaneStatusBar.armorRate = airplane.healthRate;
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__29(UIRepeatButton P_0)
	{
		MoveAirplane(Vector3.up);
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__2A(UIRepeatButton P_0)
	{
		MoveAirplane(Vector3.zero);
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__2B(UIRepeatButton P_0)
	{
		MoveAirplane(Vector3.down);
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__2C(UIRepeatButton P_0)
	{
		MoveAirplane(Vector3.zero);
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__2D(UIRepeatButton P_0)
	{
		Fire();
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__2E(UIButton P_0)
	{
		DropBomb();
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__2F(UIButton P_0)
	{
		Takeoff();
	}
}
