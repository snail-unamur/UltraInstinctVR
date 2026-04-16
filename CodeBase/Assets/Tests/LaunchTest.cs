using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class LaunchTest
{

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator LaunchTestWithEnumeratorPasses()
    {
        yield return SceneManager.LoadSceneAsync("SampleScene");

        // laisse ton expérience tourner
        yield return new WaitForSeconds(200f);

        Assert.Pass();
    }
}
