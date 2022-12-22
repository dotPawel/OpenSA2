![opensa banner](https://user-images.githubusercontent.com/89011403/208998468-e3871749-32b9-4cbf-aeef-ad5f80cdcf74.png)

# The OpenSA2 project aims to open source Sky Aces 2 and make it more accessible
![image1](https://user-images.githubusercontent.com/89011403/209125901-8b14c92b-9e19-4368-be62-b2fb2199fcb5.png)

# ![opensa banner2](https://user-images.githubusercontent.com/89011403/209126271-ec879a55-29b0-4242-abc1-984fbfed10d9.png)
While OpenSA2 is fully playable it contains some issues
  
+ Broken audio in levels
+ Wierdly placed AudioSource in the campaign map
+ Finished provinces arent highlighted

# ![opensa banner3](https://user-images.githubusercontent.com/89011403/209131036-3b673609-ef89-41e6-99dc-48d7f71f3162.png)
When OpenSA2 is ran on a platform other than Android or iOS button controlls are disabled and replaced by the following

+ Arrow up and down to control the plane
+ Left shift to shoot
+ Space to drop bomb

this can be patched out by removing the following from PlayerControl.cs

```csharp
if (Application.platform != RuntimePlatform.Android || Application.platform != RuntimePlatform.IPhonePlayer) { // disable controll butons when not on a phone
	hud.moveDownButton.gameObject.SetActive(false);
	hud.moveUpButton.gameObject.SetActive(false);
	hud.fireButton.gameObject.SetActive(false);
	hud.bombButton.gameObject.SetActive(false);
}
```

# ![opensa banner4](https://user-images.githubusercontent.com/89011403/209134278-b1cd851f-a69c-496c-a351-c9de93954511.png)

OpenSA2 is built using Unity 2019.1.2f1
<br />
<br />
<br />
<br />
*original Sky Aces 2 was made by GameDevTeam, visit http://www.gamedevteam.com for more info*
