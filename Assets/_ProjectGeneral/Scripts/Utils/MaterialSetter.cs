using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
	[SerializeField] private MeshRenderer _meshRenderer;

	private MeshRenderer meshRenderer
	{ 
		get
		{
			return _meshRenderer;
		}
	}

	public void SetSingleMaterial(Material material)
	{
		meshRenderer.material = material;
	}
}
