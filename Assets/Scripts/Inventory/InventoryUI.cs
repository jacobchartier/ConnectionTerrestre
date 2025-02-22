using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MenuHandler
{
    [SerializeField] private GameObject shopUI;

    [SerializeField] private EntityStats player;

    [SerializeField] private TMP_Text coins;

    private void Awake()
    {
        GetMenu<InventoryUI>().Refresh();

        Refresh();
    }

    private void OnEnable()
    {
        if (shopUI.activeInHierarchy)
        {
            transform.localPosition = new Vector3(-160, transform.localPosition.y, transform.localPosition.z);
        }
    }

    public void Refresh()
    {
        this.coins.text = player.Coins.ToString();
    }

    public override void Show()
    {
        base.Show();

        HeadUpDisplay.Instance?.Hide();

        Player.Instance.LoadInventory();
    }

    public override void Hide()
    {
        base.Hide();


        CameraManager.Instance?.ResetCameras();

        Tooltip.Instance?.Hide();

        HeadUpDisplay.Instance?.Show();

        this.gameObject.transform.localPosition = new Vector3(0, this.transform.localPosition.y, this.transform.localPosition.z);

        foreach (var slot in Player.Instance.inventory.slots)
        {
            if (slot.name.Contains("Hotbar Slot")) continue;

            slot.gameObject.GetComponent<Image>().color = new Color(0.2641509f, 0.2641509f, 0.2641509f, 0.8627451f);

            foreach (var item in slot.GetComponentsInChildren<Item>())
            {
                Player.Instance.inventory.Remove(item.GetComponent<Item>());
                ItemManager.Instance.items.Remove(item);
                Destroy(item.gameObject);
            }

            if (GameObject.Find("Items")?.gameObject == null) return;

            foreach (Transform item in GameObject.Find("Items")?.gameObject.transform)
            {
                Player.Instance.inventory.Remove(item.GetComponent<Item>());
                ItemManager.Instance.items.Remove(item.GetComponent<Item>());
                Destroy(item.gameObject);
            }
        }
    }
}
