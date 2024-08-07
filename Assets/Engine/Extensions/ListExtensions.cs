﻿using System.Collections.Generic;
using UnityEngine;

namespace Engine.Extensions
{
    public static class ListExtensions
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count )];
        }        
    }
}