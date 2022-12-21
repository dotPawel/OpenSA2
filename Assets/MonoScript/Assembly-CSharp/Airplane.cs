using System;
using UnityEngine;

public class Airplane : Unit
{
	[Serializable]
	public class Equipment
	{
		public int ammo;

		public int bomb;

		public int fuel;
	}

	public AirMovement movement;

	public AirplaneView view;

	public MachineGun machineGun;

	public BombLauncher bombLauncher;

	public Equipment[] equipments;

	public Vector3 right
	{
		get
		{
			return base.goTransform.right;
		}
	}

	private void OnEnable()
	{
		RestoreHealth();
		type = TargetType.Air;
	}

	private void Start()
	{
		movement.onSmashed += OnSmashed;
		movement.onStateChanged += OnMovementStateChanged;
	}

	private void OnMovementStateChanged(AirMovement movement, AirMovement.State state)
	{
		switch (state)
		{
		case AirMovement.State.Takeoff:
			type = TargetType.Air;
			break;
		case AirMovement.State.Landed:
			type = TargetType.Ground;
			break;
		}
	}

	private void OnSmashed(AirMovement movement)
	{
		Shotdown(true);
	}

	public void Fire()
	{
		machineGun.Fire();
	}

	public void DropBomb()
	{
		bombLauncher.Fire();
	}

	public void Move(Vector3 direction)
	{
		movement.Move(direction);
	}

	public void SetAvailabelLanding(bool value)
	{
		movement.availabelLanding = value;
		view.availabelLanding = value;
	}

	public override void SetTargetId(int value)
	{
		base.SetTargetId(value);
		machineGun.SetId(value);
		bombLauncher.SetId(value);
	}

	private void Shotdown(bool explode = false)
	{
		view.Shotdown(explode);
		Destroy();
	}

	public override void Explode()
	{
		Shotdown();
	}

	public void SetEquipment(int index)
	{
		Equipment equipment = equipments[Mathf.Clamp(index, 0, equipments.Length - 1)];
		machineGun.SetAmmo(equipment.ammo);
		bombLauncher.SetBombCount(equipment.bomb);
		movement.SetFuel(equipment.fuel);
	}

	public void SetSoundType(int index)
	{
		view.SetSoundClip(index);
		machineGun.SetSoundType(index);
	}

	public void Fill()
	{
		movement.FillFuel();
		machineGun.Reload();
	}
}
