using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckCamera : MonoBehaviour
{
	[SerializeField] GameObject player;
	[SerializeField] Camera targetCamera; // 映っているか判定するカメラへの参照
	bool canInteract = false;
	[SerializeField]
	Transform targetObj; // 映っているか判定する対象への参照。inspectorで指定する

	Rect rect = new Rect(0.4f, 0.4f, 0.2f, 0.2f); // 画面内か判定するためのRect


	void Start()
	{
	}

	void Update()
	{
		var viewportPos = targetCamera.WorldToViewportPoint(targetObj.position);

		if (rect.Contains(viewportPos))
        {
			ShowText("画面内");
			canInteract = true;

		}
		else
		{
			ShowText("画面外");
			canInteract = false;
		}

        if(Input.GetKeyDown(KeyCode.E))
		{
			if(canInteract == true)
            {
				interacted();
			}
			
        }

	}

	// 以下はサンプルのUI表示用
	[SerializeField]
	Text uiText;
	void ShowText(string message)
	{
		uiText.text = message;
	}

	void interacted()
    {
		StartCoroutine("updatePosition");
	}

	IEnumerator Jump()
    {
		Vector3 directionVector = (this.transform.position - player.transform.position).normalized;
		float distance = directionVector.magnitude;
		yield return null;
		player.transform.position += directionVector * 5f;
		Vector3 normVector = Vector3.Cross(directionVector, Vector3.up).normalized;
		//Plane plane = new Plane(normVector,this.transform.position);
		float cos = Vector3.Dot(directionVector, Vector3.left);
		float distanceX = distance * cos;
		//平面空間の単位ベクトル→これが新しい軸になる。
		Vector3 dirX = (directionVector * cos).normalized;
		Vector3 dirY = (directionVector - dirX).normalized;
		Vector3 dirVector = Vector3.Cross(directionVector, normVector).normalized;
		Vector3 newOrigin = this.transform.position - dirY * (this.transform.position - player.transform.position).magnitude;
	}

	IEnumerator updatePosition()
    {
		Vector3 directionVector = (this.transform.position - player.transform.position).normalized;
		float cos = Vector3.Dot(directionVector, Vector3.left);
		player.transform.position += directionVector * 8f;
		Vector3 dirX = (directionVector * cos).normalized;
		Vector3 dirY = (directionVector - dirX).normalized;
		while (true)
        {
			directionVector = (this.transform.position - player.transform.position).normalized;
			Vector3 normVector = Vector3.Cross(directionVector, Vector3.up).normalized;
			Vector3 dirVector = Vector3.Cross(directionVector, normVector).normalized;
			player.transform.position += dirVector * Time.deltaTime * 1f;
            if (normVector.magnitude == 0)
            {
				
            }
			yield return null;
		}
		
	}



}