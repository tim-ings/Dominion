﻿// Dominion - Copyright (C) Timothy Ings
// IListItem.cs
// This file contains interfaces for list items

using Microsoft.Xna.Framework.Graphics;

namespace ArwicEngine.Forms
{
    public interface IListItem
    {
        Button Button { get; set; }
        RichText Text { get; set; }
    }
}
