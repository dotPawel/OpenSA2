using System.Collections.Generic;

public class UIOrderComparer : Comparer<UIEventListener>
{
	public override int Compare(UIEventListener a, UIEventListener b)
	{
		int order = a.order;
		int order2 = b.order;
		return (order != order2) ? ((order >= order2) ? 1 : (-1)) : 0;
	}
}
