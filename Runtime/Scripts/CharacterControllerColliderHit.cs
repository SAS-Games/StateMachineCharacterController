using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerColliderHit : MonoBehaviour
{
	public ControllerColliderHit LastHit { get; private set; }
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		LastHit = hit;
	}
}
