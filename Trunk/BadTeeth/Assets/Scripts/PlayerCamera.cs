using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour 
{
	GameObject m_Player;

	public Vector3 m_Offset;
	public float m_Overhead;

	public Rect m_Bounds;

	public float m_CameraSpeed;

	public float m_ThresholdX;
	public float m_ThresholdY;

	bool m_IsRecentering;

	// Use this for initialization
	void Start () 
	{
		m_Player = getPlayer.Instance.gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float deltaX = m_Player.transform.position.x + m_Player.transform.forward.x * m_Offset.x - transform.position.x;
		float deltaY = m_Player.transform.position.y + m_Offset.y - transform.position.y;

		float speedX = 0.0f;
		float speedY = 0.0f;

		if(m_IsRecentering || Mathf.Abs (deltaX) >= m_ThresholdX)
		{
			speedX = (deltaX / m_ThresholdX) * m_CameraSpeed;
		}

		if(m_IsRecentering || Mathf.Abs (deltaY) >= m_ThresholdY)
		{
			speedY = (deltaY / m_ThresholdY) * m_CameraSpeed;
		}

		if(Mathf.Abs (deltaX) >= m_ThresholdX || Mathf.Abs (deltaY) >= m_ThresholdY)
		{
			m_IsRecentering = true;
		}
		else if(Mathf.Abs (deltaX) < 0.1f && Mathf.Abs (deltaY) < 0.1f)
		{
			m_IsRecentering = false;
		}

		transform.position += (transform.up * speedY + transform.right * speedX) * Time.deltaTime;

		Vector3 newPosition = transform.position;
		newPosition.x = Mathf.Clamp (transform.position.x, m_Bounds.xMin, m_Bounds.xMax);
		newPosition.y = Mathf.Clamp (transform.position.y, m_Bounds.yMin, m_Bounds.yMax);
		transform.position = newPosition;
	}
}
