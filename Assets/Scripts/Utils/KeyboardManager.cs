using System;
using System.Collections;
using UnityEngine;

public class KeyboardManager : Singleton<KeyboardManager>
{
    private TouchScreenKeyboard _keyboardInstance;

    private Coroutine _waitForUserInputCoroutine;

    public void GetInput(Action<string> onSuccess, string defaultText = "", TouchScreenKeyboardType keyboardType=TouchScreenKeyboardType.Default, int maxInputSize=9999)
    {
# if UNITY_EDITOR
        onSuccess?.Invoke("Temp");
#else
        _keyboardInstance = TouchScreenKeyboard.Open(defaultText, keyboardType);

        if (_waitForUserInputCoroutine != null ) StopCoroutine( _waitForUserInputCoroutine );

        _waitForUserInputCoroutine = StartCoroutine(WaitForUserInputCoroutine(result => onSuccess?.Invoke(result), maxInputSize));
#endif
    }

    private IEnumerator WaitForUserInputCoroutine(Action<string> callback, int maxInputSize)
    {
        while (_keyboardInstance.status == TouchScreenKeyboard.Status.Visible)
        {
            yield return null;
        }

        var result = _keyboardInstance.text;

        _keyboardInstance = null;

        callback?.Invoke(result);
    }
}
