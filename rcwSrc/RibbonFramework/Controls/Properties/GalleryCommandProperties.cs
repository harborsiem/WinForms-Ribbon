using System;
using System.Collections.Generic;
using System.Text;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Definition for gallery properties provider interface for Command
    /// </summary>
    public interface IGallery2PropertiesProvider
    {
        /// <summary>
        /// Items source property for Command
        /// </summary>
        UICollection<GalleryCommandPropertySet>? GalleryCommandItemsSource { get; }
    }

    internal sealed class GalleryCommandProperties : IGallery2PropertiesProvider
    {
        /// <summary>
        /// Items source property for Command
        /// </summary>
        public UICollection<GalleryCommandPropertySet>? GalleryCommandItemsSource { get; internal set; }
    }
}
