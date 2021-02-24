using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : SingletoneGameObject<UIManager>
{
    private List<UIBasePanel> _panels = null;

    protected override void Awake()
    {
        base.Awake();
        _panels = GetComponentsInChildren<UIBasePanel>().ToList();
    }

    void Start()
    {
        ShowPanel(UITypePanel.LogoScreen);
    }

    public void ShowPanel(UITypePanel type)
    {
        foreach (var panel in _panels)
        {
            panel.gameObject.SetActive(panel.panel == type);
        }
    }
}
