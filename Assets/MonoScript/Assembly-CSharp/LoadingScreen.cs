public class LoadingScreen : UIController
{
	public static void ShowScreen()
	{
		LoadingScreen loadingScreen = UIController.Find<LoadingScreen>();
		if (loadingScreen != null)
		{
			loadingScreen.Show();
		}
	}

	public void Show()
	{
		UIStatesController uIStatesController = rootControler as UIStatesController;
		uIStatesController.Show("LoadingScreen");
	}
}
