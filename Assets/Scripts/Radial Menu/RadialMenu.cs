using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class RadialMenu : MonoBehaviour
{
	[SerializeField] private RingPiece ringPiecePrefab;
	[SerializeField] private float manualRotation;
	[SerializeField] private Color normalColor;
	[SerializeField] private Color highlightedColor;

	private List<CharacterClass> classes = new List<CharacterClass>();
	private List<RingPiece> ringPieces = new List<RingPiece>();
	private Vector2 menuInput;
	private float degreesPerPiece;
	private int numberOfSlices;
	private int currentButton;

	private void Start()
	{
		transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + manualRotation);

		degreesPerPiece = 360f / numberOfSlices; //classes.Count;
		float distanceToIcon = Vector3.Distance(ringPiecePrefab.GetIcon().transform.position, ringPiecePrefab.GetBackground().transform.position);
		//Debug.Log(distanceToIcon);

		for (int i = 0; i < numberOfSlices; i++)
		{
			ringPieces.Add(Instantiate(ringPiecePrefab, transform));

			ringPieces[i].SetID(i);
			ringPieces[i].GetBackground().fillAmount = (1f / numberOfSlices);
			ringPieces[i].GetBackground().transform.localRotation = Quaternion.Euler(0, 0, (degreesPerPiece + i * degreesPerPiece));

			Vector3 directionVector = Quaternion.AngleAxis(i * degreesPerPiece, Vector3.forward) * Vector3.up;
			//Vector3 movementVector = directionVector * distanceToIcon;

			//ringPieces[i].GetIcon().transform.localPosition = ringPieces[i].GetBackground().transform.localPosition + movementVector;

			ringPieces[i].GetIcon().transform.localEulerAngles = new Vector3(0, 0, -ringPieces[i].GetBackground().transform.localEulerAngles.z);
			//Debug.Log(ringPieces[i].GetBackground().transform.localPosition + movementVector);
		}
	}

	private void Update()
	{
		GetCenterScreen();
	}

	private void GetCenterScreen()
	{
		menuInput.x = Input.mousePosition.x - (Screen.width / 2f);
		menuInput.y = Input.mousePosition.y - (Screen.height / 2f);
		menuInput.Normalize();

		if (menuInput != Vector2.zero)
		{
			float angle = Mathf.Atan2(menuInput.y, menuInput.x)* Mathf.Rad2Deg;
			angle -= 90f;

			if (angle < 0)
			{
				angle += 360f;
			}

			for (int i = 0; i < numberOfSlices; i++)
            {
				if (angle > i * degreesPerPiece && angle < (i + 1) * degreesPerPiece)
                {
					ringPieces[i].GetBackground().color = highlightedColor;
					currentButton = ringPieces[i].GetID();

				}
				else
                {
					ringPieces[i].GetBackground().color = normalColor;
				}
            }
		}
	}

	public void SetNumberOfSlices(int num)
    {
		numberOfSlices = num;
    }

	public int GetButton()
    {
		return currentButton;
	}
}