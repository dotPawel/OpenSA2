public abstract class PlayerListener : AObject
{
	public abstract void OnAirplaneSet(Airplane airplane);

	public virtual void OnControlTypeUpdated(ControlType controlType)
	{
	}
}
