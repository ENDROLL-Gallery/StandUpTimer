using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using UnityEngine.UI;

using UniRx;
using UniRx.Triggers;

public class ENDROLL : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] new Text name;
    [SerializeField] Transform start, end;

    void Start()
    {
        Image backGround = GetComponentInChildren<Image>();

        List<string> entryList = Enumerable
            .Range(0, 18)
            .Where(i => PlayerPrefs.GetString(i.ToString() + "active") == Color.black.ToString())
            .Select(i => PlayerPrefs.GetString(i.ToString()))
            .Where(name => name != string.Empty)
            .ToList();

        Debug.Log(entryList.Count);

        FloatReactiveProperty t = new FloatReactiveProperty();
        this.UpdateAsObservable()
            .Do(_ => t.Value += Time.deltaTime)
            .Subscribe().AddTo(this);
        t.Where(value => value >= 0)
            .Select(value => value / time)
            .Do(progress => name.transform.position = Vector3.Lerp(start.position, end.position, progress))
            .Subscribe().AddTo(this);

        int index = 0;
        Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(time))
            .TakeWhile(_ => index < entryList.Count)
            .Do(_ => t.Value = 0)
            .Do(_ => name.text = entryList[index])
            .Do(_ => name.color = index % 2 != 0 ? Color.white : Color.black)
            .Do(_ => backGround.color = index % 2 != 0 ? Color.black : Color.white)
            .Do(_ => index++)
            .DoOnCompleted(() => gameObject.SetActive(false))
            .Subscribe().AddTo(this);
    }
}
