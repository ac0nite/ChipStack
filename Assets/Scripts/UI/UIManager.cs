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
        Debug.Log($"panels: {_panels.Count}");
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowPanel(UITypePanel.LogoScreen);
    }

    public void ShowPanel(UITypePanel type)
    {
        foreach (var panel in _panels)
        {
           // if (panel.panel == UITypePanel.Message) continue;
            panel.gameObject.SetActive(panel.panel == type);
        }
    }
}
