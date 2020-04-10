using Malee;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
	[CreateAssetMenu(fileName = "New SpriteBlendData", menuName = "Sprite Animations/Sprite Blend Data")]
	public class SpriteBlendData : BaseSpriteAnimatorNode
	{
		[Reorderable] public SBNList blendSpace;
	} 

	[Serializable]
	public class SpriteBlendNode
	{
		public Vector2 blendPosition;
		public SpriteAnimationData animationData;
	}

	[Serializable]
	public class SBNList : ReorderableArray<SpriteBlendNode>
	{

	}
}
