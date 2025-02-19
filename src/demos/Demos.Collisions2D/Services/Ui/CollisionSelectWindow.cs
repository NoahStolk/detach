using Demos.Collisions2D.CollisionScenes;
using Demos.Collisions2D.CollisionScenes.TwoDimensional;
using Detach;
using Detach.Numerics;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.Collisions2D.Services.Ui;

internal sealed class CollisionSelectWindow
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
		new CircleCircle(),
		new CircleRectangle(),
		new CircleOrientedRectangle(),
		new CircleTriangle(),
		new RectangleRectangle(),
		new RectangleRectangleSat(),
		new RectangleOrientedRectangleSat(),
		new RectangleTriangle(),
		new OrientedRectangleOrientedRectangleSat(),
		new OrientedRectangleTriangle(),
	];

	private ICollisionScene _selectedScene;

	public CollisionSelectWindow()
	{
		_selectedScene = _collisionScenes[0];
	}

	public void Render(float dt)
	{
		ImGui.SetNextWindowSizeConstraints(new Vector2(1024, 768), new Vector2(4096, 4096));
		if (ImGui.Begin("Collisions"))
		{
			if (ImGui.BeginChild("Selection", new Vector2(384, 0), ImGuiChildFlags.Borders))
			{
				ImGui.Text("Select a collision scene:");
				ImGui.Separator();

				foreach (ICollisionScene collisionScene in _collisionScenes)
				{
					if (ImGui.Selectable(collisionScene.GetType().Name, _selectedScene == collisionScene))
						_selectedScene = collisionScene;
				}

				ImGui.EndChild();
			}

			ImGui.SameLine();

			if (ImGui.BeginChild("Scene", default, ImGuiChildFlags.Borders))
			{
				ImGui.Text(Inline.Utf8($"Total time: {_selectedScene.ExecutionTime.TotalMicroseconds:N2} microseconds"));
				if (_selectedScene.AllocatedBytes > 0)
					ImGui.TextColored(Rgba.Red, Inline.Utf8($"Allocated bytes: {_selectedScene.AllocatedBytes:N0}"));
				else
					ImGui.TextColored(Rgba.Green, "No memory allocated");

				_selectedScene.Update(dt);
				_selectedScene.Collide();
				_selectedScene.Render();

				ImGui.EndChild();
			}
		}

		ImGui.End();
	}
}
