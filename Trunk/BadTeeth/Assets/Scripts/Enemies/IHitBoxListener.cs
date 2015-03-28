using UnityEngine;
using System.Collections;

public interface IHitBoxListener 
{
	void OnHitBoxEnter(Collider otherCollider);
}
