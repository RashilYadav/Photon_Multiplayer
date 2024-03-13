using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    // menu script ka array banaya hai...toh jinn gameobject par menu script lagi hogi unhi ko hum array ke andar daal payenge inspector m!!
    public Menu[] menus;

    private void Awake()
    {
        instance = this;
    }

    // openMenu ko jaha bhi call karenge aur uske through pass kiya hua "menuName" agar menu array ke kisi menu ke match karega toh woh activate hoga
    public void OpenMenu(string menuName)
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if(menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if(menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
