using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UniRx;
using UniRx.Triggers;

public class EntryButtons : MonoBehaviour
{
    void Start()
    {
        GetComponentsInChildren<Button>().ToList().ForEach(button =>
        {
            string indexString = button.transform.GetSiblingIndex().ToString();

            InputField input = button.GetComponentInChildren<InputField>();
            input.textComponent = button.GetComponentInChildren<Text>();

            PlayerPrefs.SetString(indexString, PlayerPrefs.GetString(indexString));
            PlayerPrefs.SetString(indexString + "active", PlayerPrefs.GetString(indexString + "active"));
            input.textComponent.text = PlayerPrefs.GetString(indexString);
            input.text = PlayerPrefs.GetString(indexString);

            bool isActive = PlayerPrefs.GetString(indexString + "active") == Color.black.ToString();
            button.image.color = isActive ? Color.black : Color.gray;

            input.OnValueChangedAsObservable()
                .Do(name => PlayerPrefs.SetString(indexString, name))
                .Subscribe().AddTo(this);

            button.OnClickAsObservable()
                .Do(_ => button.image.color = button.image.color == Color.black ? Color.gray : Color.black)
                .Do(_ => PlayerPrefs.SetString(indexString + "active", button.image.color.ToString()))
                .Subscribe().AddTo(this);
        });
    }
}
