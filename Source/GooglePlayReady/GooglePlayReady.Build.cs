// Copyright 2024 Metaseven. All Rights Reserved.

using System.IO;
using UnrealBuildTool;

public class GooglePlayReady : ModuleRules
{
	public GooglePlayReady(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
			}
		);

		if (Target.Platform == UnrealTargetPlatform.Android)
		{
			AdditionalPropertiesForReceipt.Add("AndroidPlugin", Path.Combine(ModuleDirectory, GetType().Name + "_UPL.xml"));

			// Fix aar-imports.txt
			string AarImportsPath = Path.Combine(EngineDirectory, "Build", "Android", "Java", "aar-imports.txt");

			if (File.Exists(AarImportsPath))
			{
				string Text = File.ReadAllText(AarImportsPath);

				Text = Text.Replace(
					"com.google.android.gms,play-services-games,11.8.0",
					"com.google.android.gms,play-services-games,18.0.1"
				);

				File.WriteAllText(AarImportsPath, Text);
			}

			// Fix app/build.gradle
			string AppBuildGradlePath = Path.Combine(EngineDirectory, "Build", "Android", "Java", "gradle", "app", "build.gradle");

			if (File.Exists(AppBuildGradlePath))
			{
				string Text = File.ReadAllText(AppBuildGradlePath);

				if (!Text.Contains("compileOptions"))
				{
					Text = Text.Replace(
						"\tsourceSets.main {\r\n",
						"\tcompileOptions {\r\n" +
						"\t\tsourceCompatibility JavaVersion.VERSION_1_8\r\n" +
						"\t\ttargetCompatibility JavaVersion.VERSION_1_8\r\n" +
						"\t}\r\n" +
						"\r\n" +
						"\tsourceSets.main {\r\n"
					);
				}

				File.WriteAllText(AppBuildGradlePath, Text);
			}

			// Fix GooglePAD_APL.xml
			string GooglePadAplPath = Path.Combine(EngineDirectory, "Plugins", "Runtime", "GooglePAD", "Source", "GooglePAD", "GooglePAD_APL.xml");

			if (File.Exists(GooglePadAplPath))
			{
				string Text = File.ReadAllText(GooglePadAplPath);

				Text = Text.Replace(
					"\timplementation('com.google.android.play:core:1.10.0')\r\n",
					"\timplementation('com.google.android.play:asset-delivery:2.2.2')\r\n" +
					"\timplementation('com.google.android.play:asset-delivery-ktx:2.2.2')\r\n"
				);
				Text = Text.Replace(
					"import com.google.android.play.core.tasks.",
					"import com.google.android.gms.tasks."
				);

				File.WriteAllText(GooglePadAplPath, Text);
			}
		}
	}
}
