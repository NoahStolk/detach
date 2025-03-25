using CollisionFormats.Model;
using Demos.Collisions.Interactable.Services.States;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.Collisions.Interactable.Services.Ui;

internal sealed class ScenarioDataWindow(CollisionScenarioState collisionScenarioState)
{
	private int _selectedAlgorithmIndex;

	public void Render()
	{
		ImGui.SetNextWindowSizeConstraints(new Vector2(960, 320), new Vector2(4096));
		if (ImGui.Begin("Scenarios"))
		{
			ImGui.Combo("Algorithm", ref _selectedAlgorithmIndex, collisionScenarioState.ComboString, 50);

			RenderAlgorithm();
		}

		ImGui.End();
	}

	private void RenderAlgorithm()
	{
		CollisionAlgorithm algorithm = collisionScenarioState.CollisionAlgorithms[_selectedAlgorithmIndex];

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
		if (ImGui.BeginTable("ScenarioTable", columnCount, ImGuiTableFlags.Resizable))
		{
			foreach (CollisionAlgorithmParameter parameter in algorithm.Parameters)
				ImGui.TableSetupColumn($"{parameter.TypeName} {parameter.Name}");

			foreach (CollisionAlgorithmParameter parameter in algorithm.OutParameters)
				ImGui.TableSetupColumn($"{parameter.TypeName} {parameter.Name}");

			ImGui.TableSetupColumn("Return");
			ImGui.TableSetupScrollFreeze(0, 1);
			ImGui.TableHeadersRow();

			foreach (CollisionAlgorithmScenario scenario in algorithm.Scenarios)
			{
				ImGui.TableNextRow();

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

				ImGui.TableNextColumn();
				ImGui.Text(scenario.ReturnValue?.ToString() ?? "null");
			}

			ImGui.EndTable();
		}
	}
}
