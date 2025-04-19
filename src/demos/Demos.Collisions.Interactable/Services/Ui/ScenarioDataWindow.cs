using CollisionFormats.Model;
using Demos.Collisions.Interactable.Services.States;
using Detach;
using Detach.Numerics;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.Collisions.Interactable.Services.Ui;

internal sealed class ScenarioDataWindow(CollisionAlgorithmState collisionAlgorithmState, CollisionScenarioState collisionScenarioState, SelectionState selectionState)
{
	public void Render()
	{
		ImGui.SetNextWindowSizeConstraints(new Vector2(960, 320), new Vector2(4096));
		if (ImGui.Begin("Scenarios"))
		{
			CollisionAlgorithm? algorithm = collisionScenarioState.GetAlgorithm(selectionState.SelectedAlgorithmIndex);
			if (algorithm != null)
				RenderAlgorithm(algorithm);
		}

		ImGui.End();
	}

	private void RenderAlgorithm(CollisionAlgorithm algorithm)
	{
		ImGui.Text(algorithm.MethodSignature);

		ImGui.SeparatorText("Parameters");
		foreach (CollisionAlgorithmParameter parameter in algorithm.Parameters)
			ImGui.Text($"{parameter.TypeName} {parameter.Name}");

		ImGui.SeparatorText("Out Parameters");
		foreach (CollisionAlgorithmParameter parameter in algorithm.OutParameters)
			ImGui.Text($"{parameter.TypeName} {parameter.Name}");

		ImGui.SeparatorText("Return Type");
		ImGui.Text(algorithm.ReturnTypeName);

		ImGui.SeparatorText("Scenarios");

		int columnCount = algorithm.Parameters.Count + algorithm.OutParameters.Count + 1;
		if (ImGui.BeginTable("ScenarioTable", columnCount, ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders))
		{
			ImGui.TableSetupColumn("Actions");

			foreach (CollisionAlgorithmParameter parameter in algorithm.Parameters)
				ImGui.TableSetupColumn($"{parameter.TypeName} {parameter.Name}");

			foreach (CollisionAlgorithmParameter parameter in algorithm.OutParameters)
				ImGui.TableSetupColumn($"{parameter.TypeName} {parameter.Name}");

			ImGui.TableSetupScrollFreeze(0, 1);
			ImGui.TableHeadersRow();

			for (int i = 0; i < algorithm.Scenarios.Count; i++)
			{
				CollisionAlgorithmScenario scenario = algorithm.Scenarios[i];
				ImGui.TableNextRow();

				ImGui.TableNextColumn();
				if (ImGui.Button(Inline.Utf8($"Select##{i}")))
					collisionAlgorithmState.SetArguments(scenario.Arguments);
				ImGui.SameLine();
				if (ImGui.Button(Inline.Utf8($"Delete##{i}")))
					collisionScenarioState.RemoveScenarioAt(algorithm.MethodSignature, i);
				ImGui.SameLine();
				if (ImGui.Checkbox(Inline.Utf8($"##Incorrect_{i}"), ref scenario.Incorrect))
					CollisionScenarioState.SaveFile(algorithm);
				ImGui.SameLine();
				ImGui.Text(scenario.Incorrect ? "INCORRECT" : "OK");

				ImGui.PushStyleColor(ImGuiCol.Text, scenario.Incorrect ? Rgba.Red : Rgba.White);

				foreach (object argument in scenario.Arguments)
				{
					ImGui.TableNextColumn();
					ImGui.Text(argument.ToString());
				}

				foreach (object outArgument in scenario.OutArguments)
				{
					ImGui.TableNextColumn();
					ImGui.Text(outArgument.ToString());
				}

				ImGui.PopStyleColor();
			}

			ImGui.EndTable();
		}
	}
}
