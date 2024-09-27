using Detach.Demos.Collisions.Services.CollisionScenes;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.Services.Ui;

public sealed class CollisionSelectWindow
{
	private readonly List<ICollisionScene> _collisionScenes =
	[
		new PointOnLine(),
		new PointInCircle(),
		new PointInRectangle(),
		new PointInOrientedRectangle(),
		new PointInTriangle(),
	];

	public void Render(float dt)
	{
		ImGui.SetNextWindowSizeConstraints(new Vector2(1024, 768), new Vector2(4096, 4096));
		if (ImGui.Begin("Collisions"))
		{
			if (ImGui.BeginTabBar("CollisionsTabBar"))
			{
				foreach (ICollisionScene collisionScene in _collisionScenes)
				{
					if (ImGui.BeginTabItem(collisionScene.GetType().Name))
					{
						ImGui.Text(Inline.Span($"Total time: {collisionScene.ExecutionTime.TotalMicroseconds:N2} microseconds"));
						ImGui.Text(Inline.Span($"Allocated bytes (should always be 0): {collisionScene.AllocatedBytes:N0}"));

						collisionScene.Update(dt);
						collisionScene.Collide();
						collisionScene.Render();

						ImGui.EndTabItem();
					}
				}

				ImGui.EndTabBar();
			}
		}

		ImGui.End();
	}
}
