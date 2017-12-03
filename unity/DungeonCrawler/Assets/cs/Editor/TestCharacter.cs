using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using DungeonCrawler.Character;
using System.IO;

public class TestCharacter {

	[Test]
	public void PlayerCharacter_initialized_from_Json() {
        string path = Path.Combine(Application.dataPath, @"json\TestData\Hero.json");
        string json = File.ReadAllText(path);
        Character character = Character.DeserializeFromJson(json);
        PlayerCharacter pc = GameObject.FindObjectOfType<PlayerCharacter>();
        pc.Character.Data = character;
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator TestCharacterWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
