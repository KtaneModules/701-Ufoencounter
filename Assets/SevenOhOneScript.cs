using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class SevenOhOneScript: MonoBehaviour {

	//Setup
		public KMAudio Audio;
		public KMBombInfo bomb;
		public KMBombModule module;

		static int moduleIdCounter = 1;
		int moduleId;
		private bool moduleSolved = false;
		public KMSelectable[] buttons;

		string[] canines = new string[] { "Miguno", "Durham", "Jack", "Collot", "Legoshi", "Voss"};
		string temporarya = "Haru";
		string temporaryb = "Louis";
		int pressIndex = 0;
		
		void Awake(){
			moduleId = moduleIdCounter++;
			foreach (KMSelectable button in buttons) {
			KMSelectable pressedButton = button;
			button.OnInteract += delegate () { buttonPress(pressedButton); return false; };
			}
		}

		void Start(){
			if (!(bomb.GetPortCount(Port.Parallel) >= 1)){
				temporarya = canines[1];
				temporaryb = canines[2];
				canines[2] = temporarya;
				canines[1] = temporaryb;
			}
			if (!(bomb.GetPortCount(Port.Serial) >= 1)){
				temporarya = canines[1];
				temporaryb = canines[4];
				canines[4] = temporarya;
				canines[1] = temporaryb;
			}
			if (!(bomb.GetPortCount(Port.RJ45) >= 1)){
				temporarya = canines[2];
				temporaryb = canines[5];
				canines[5] = temporarya;
				canines[2] = temporaryb;
			}
			if (!(bomb.GetPortCount(Port.PS2) >= 1)){
				temporarya = canines[0];
				temporaryb = canines[4];
				canines[4] = temporarya;
				canines[0] = temporaryb;
			}
			if (!(bomb.GetPortCount(Port.DVI) >= 1)){
				temporarya = canines[0];
				temporaryb = canines[3];
				canines[3] = temporarya;
				canines[0] = temporaryb;
			}
			if (!(bomb.GetPortCount(Port.StereoRCA) >= 1)){
				temporarya = canines[3];
				temporaryb = canines[5];
				canines[5] = temporarya;
				canines[3] = temporaryb;
			}
			Debug.Log("[701 #" + moduleId + "] Dog Stack! The order this time is " + canines[0] + " " + canines[1] + " " + canines[2] + " " + canines[3] + " " + canines[4] + " " + canines[5]);
		}

		void buttonPress(KMSelectable button){
			button.AddInteractionPunch();
			Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);
			if (moduleSolved){
				return;
			}
			if (pressIndex > 4){
				module.HandlePass();
				Debug.Log("[701 #" + moduleId + "] Module Solved.");
			}
			
			else{
				if (button.name == canines[pressIndex]){
					pressIndex++;
					Debug.Log("[701 #" + moduleId + "] You pressed " + button.name + "! This is correct.");
				}
				else{
					GetComponent<KMBombModule>().HandleStrike();
					pressIndex = 0;
					Debug.Log("[701 #" + moduleId + "] You pressed " + button.name + "! This is incorrect. Strike.");
				}
			}
		}

	#pragma warning disable 0414
    private readonly string TwitchHelpMessage = "!{0} [Name] to press the portrait of that name. Chain names with spaces. Names are: Legoshi, Jack, Collot, Durham, Miguno, and Voss.";
	#pragma warning restore 0414

    private IEnumerator ProcessTwitchCommand(string command)
    {
       yield return null;
	   string[] split = command.ToUpperInvariant().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
	   for (int i = 0; i < split.Count(); i++){
		Debug.Log(split[i]);
		if (split[i] == "LEGOSHI"){
			yield return null;
			buttons[0].OnInteract();
		}
		else if (split[i] == "JACK"){
			yield return null;
			buttons[1].OnInteract();
		}
		else if (split[i] == "COLLOT"){
			yield return null;
			buttons[2].OnInteract();
		}
		else if (split[i] == "DURHAM"){
			yield return null;
			buttons[3].OnInteract();
		}
		else if (split[i] == "MIGUNO"){
			yield return null;
			buttons[4].OnInteract();
		}
		else if (split[i] == "VOSS"){
			yield return null;
			buttons[5].OnInteract();
		}
	   }
    }

    private IEnumerator TwitchHandleForcedSolve()
    {
		yield return null;
		module.HandlePass();
	}	
}