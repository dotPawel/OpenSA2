using UnityEngine;

public class MissionButton : UIButton
{
	public Mission mission;

	public Camera mapCamera;

	private void Start()
	{
		mapCamera = Camera.main;
	}

	public Vector3 GetScreenPosition()
	{
		return mapCamera.WorldToScreenPoint(mission.area.position);
	}

	public new MissionButton Instantiate(Vector3 position, Quaternion rotation)
	{
		return (MissionButton)Object.Instantiate(this, position, rotation);
	}

	public void SetType(Mission.Type type, int background)
	{
		UIMissionButtonView uIMissionButtonView = view as UIMissionButtonView;
		uIMissionButtonView.SetIcon((int)type);
		uIMissionButtonView.SetBackground(background);
	}
}
