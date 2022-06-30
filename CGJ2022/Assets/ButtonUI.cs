using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
	public Image[] DamageImage;
	public Image[] GroundImage;
	public Button DamageButton;
	public Button GroundButton;

	public void setNumberDamage(int num)
	{
		foreach (var item in DamageImage)
		{
			item.gameObject.SetActive(false);
		}
		DamageImage[num].gameObject.SetActive(true);
		DamageButton.interactable = num > 0;
	}

	public void setNumberGround(int num)
	{
		foreach (var item in GroundImage)
		{
			item.gameObject.SetActive(false);
		}
		GroundImage[num].gameObject.SetActive(true);
		GroundButton.interactable = num > 0;
	}
}
