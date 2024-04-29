﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Plugin.CloudFirestore;
using NiisMenu.Builder;

namespace NiisMenu.Database
{
    public class DatabaseHandler
    {
        public async Task<List<Menu>> GetMenuItems(string category)
        {
            List<Menu> menuItems = new List<Menu>();

            var querySnapshot = await CrossCloudFirestore.Current
                .Instance
                .Collection("Menu")
                .WhereEqualsTo("Category", category)
                .GetAsync();

            foreach (var document in querySnapshot.Documents)
            {
                Menu menu = document.ToObject<Menu>();
                menuItems.Add(menu);
            }

            return menuItems;
        }
    }
}
