using Detach.Demos.Collisions.CollisionScenes;
using Detach.Numerics;
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
		new LineLine(),
		new LineCircle(),
		new LineRectangle(),
		new LineOrientedRectangle(),
		new LineTriangle(),
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
						if (collisionScene.AllocatedBytes > 0)
							ImGui.TextColored(Rgba.Red, Inline.Span($"Allocated bytes: {collisionScene.AllocatedBytes:N0}"));
						else
							ImGui.TextColored(Rgba.Green, "No memory allocated");

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
