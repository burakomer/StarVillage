using Cinemachine;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace DwarfEngine
{
	public class CinemachineFollowTarget : MonoBehaviour
	{
		private CinemachineVirtualCamera cmCamera;

		private void Start()
		{
			cmCamera = GetComponent<CinemachineVirtualCamera>();

			this.UpdateAsObservable()
				.Select(_ => LevelManager.Instance.player)
				.Subscribe(p =>
				{
					if (p != null && cmCamera.Follow == null)
					{
						cmCamera.Follow = p.transform;
					}

					if (p == null && cmCamera.Follow != null)
					{
						cmCamera.Follow = null;
					}
				})
				.AddTo(this);
		}
	}
}