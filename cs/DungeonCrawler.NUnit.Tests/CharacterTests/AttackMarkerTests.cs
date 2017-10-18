using DungeonCrawler.Character;
using NUnit.Framework;


namespace DungeonCrawler.NUnit.Tests.CharacterTests
{
    [TestFixture]
    public class AttackMarkerTests
    {
        [Test]
        public void Progress_of_Attack()
        {
            Character.Character attacker = new Character.Character();
            AttackMarker marker = new AttackMarker(attacker);
            marker.Start(new int[][] { new int[] { 0, 1 } }, "MeleeWeapons", 1, 2);
            Assert.AreEqual(0f, marker.Progress());

            marker.CurrentTime = 0.5f;
            Assert.AreEqual(0.25f, marker.Progress());

            marker.CurrentTime = 1f;
            Assert.AreEqual(0.5f, marker.Progress());

            marker.CurrentTime = 1.5f;
            Assert.AreEqual(0.625f, marker.Progress());

            marker.CurrentTime = 2f;
            Assert.AreEqual(0.75f, marker.Progress());

            marker.CurrentTime = 2.5f;
            Assert.AreEqual(0.875f, marker.Progress());

            marker.CurrentTime = 3f;
            Assert.AreEqual(1f, marker.Progress());
        }
    }
}
