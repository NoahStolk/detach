using CollisionFormats.Execution;
using Demos.Collisions.Interactable.Services.States;
using Detach;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.Collisions.Interactable.Services.Ui;

internal sealed class AlgorithmSelectWindow(CollisionAlgorithmState collisionAlgorithmState, CollisionScenarioState collisionScenarioState, SelectionState selectionState)
{
	public void Render()
	{
		ImGui.SetNextWindowSizeConstraints(new Vector2(960, 320), new Vector2(4096));
		if (ImGui.Begin("Algorithm Selector"))
			RenderAlgorithmSelector();

		ImGui.End();
	}

	private void RenderAlgorithmSelector()
	{
		if (!ImGui.BeginTable("AlgorithmSelectorTable", 2, ImGuiTableFlags.ScrollY))
			return;

		ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthStretch);
		ImGui.TableSetupColumn("Scenario Count", ImGuiTableColumnFlags.WidthFixed, 128f);

		ImGui.TableSetupScrollFreeze(0, 1);
		ImGui.TableHeadersRow();

		for (int i = 0; i < ExecutableCollisionAlgorithms.All.Count; i++)
		{
			IExecutableCollisionAlgorithm algorithm = ExecutableCollisionAlgorithms.All[i];

			ImGui.TableNextRow();

			ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, collisionScenarioState.CollisionAlgorithms[i].Scenarios.Count == 0 ? 0x88000000 : 0x88444444);

			ImGui.TableNextColumn();
			if (ImGui.Selectable(algorithm.Name, selectionState.SelectedAlgorithmIndex == i, ImGuiSelectableFlags.SpanAllColumns))
			{
				selectionState.SelectedAlgorithmIndex = i;
				collisionAlgorithmState.SelectAlgorithm(ExecutableCollisionAlgorithms.All[selectionState.SelectedAlgorithmIndex]);
			}

			ImGui.TableNextColumn();
			ImGui.Text(Inline.Utf8(collisionScenarioState.CollisionAlgorithms[i].Scenarios.Count));
		}

		ImGui.EndTable();
	}
}
