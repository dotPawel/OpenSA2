using System.Collections.Generic;

public class UINavigationController : UIController
{
	private Stack<UIController> stack = new Stack<UIController>();

	public UIController activeController
	{
		get
		{
			return stack.Peek();
		}
	}

	public void Push(UIController controller)
	{
		if (stack.Count > 0)
		{
			Hide(stack.Peek());
		}
		Show(controller);
		stack.Push(controller);
	}

	public void Pop()
	{
		if (stack.Count > 0)
		{
			Hide(stack.Pop());
		}
		if (stack.Count > 0)
		{
			Show(stack.Peek());
		}
	}

	private void Show(UIController controller)
	{
		controller.rootControler = this;
		controller.ViewWillAppear();
		controller.show = true;
		controller.ViewDidAppear();
	}

	private void Hide(UIController controller)
	{
		controller.rootControler = null;
		controller.ViewWillDisappear();
		controller.show = false;
		controller.ViewDidDisappear();
	}
}
