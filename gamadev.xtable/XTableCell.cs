using System;
using System.Windows;
using System.Windows.Controls;

namespace gamadev.xtable
{
    [Serializable]
    public class XTableCell : ContentControl
    {
        /// <summary>
        /// its may be deleted, we can get the row index from the parent row.
        /// </summary>
        internal int RowIndex { get; set; }

        internal int ColumnIndex { get; set; }

        static XTableCell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(XTableCell), new FrameworkPropertyMetadata(typeof(XTableCell)));
        }
        
    }
}