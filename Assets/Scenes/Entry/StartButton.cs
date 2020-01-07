using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UniRx.Async;

public class StartButton : MonoBehaviour
{
    async void Start()
    {
        Button button = GetComponent<Button>();

        CancellationToken ct = new CancellationToken();
        await button.OnClickAsync(ct);
        SceneManager.LoadScene("ENDROLL");
    }
}
