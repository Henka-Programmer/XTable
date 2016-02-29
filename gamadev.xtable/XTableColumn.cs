using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace gamadev.xtable
{
    [Serializable]
    [ContentProperty("Columns")]
    public class XTableColumn : ColumnDefinition
    {
        public virtual BindingBase Binding
        {
            get; set;
        }

        public virtual bool IsGrouped { get; set; }

        public virtual bool IsTransitive { get; set; }

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(XTableColumn), new PropertyMetadata(null));

        public Style CellStyle { get; set; }
        //public Style CellStyle
        //{
        //    get { return (Style)GetValue(CellStyleProperty); }
        //    set { SetValue(CellStyleProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for CellStyle.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty CellStyleProperty =
        //    DependencyProperty.Register("CellStyle", typeof(Style), typeof(XTableColumn), new PropertyMetadata(null));

        //public bool IsGrouped
        //{
        //    get { return (bool)GetValue(IsGroupedProperty); }
        //    set { SetValue(IsGroupedProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for IsGrouped.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty IsGroupedProperty =
        //    DependencyProperty.Register("IsGrouped", typeof(bool), typeof(XTableColumn), new PropertyMetadata(false));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(XTableColumn), new PropertyMetadata(string.Empty));

        //public virtual ColumnCollection Columns { get; set; }

        public ColumnCollection Columns
        {
            get { return (ColumnCollection)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(ColumnCollection), typeof(XTableColumn), new FrameworkPropertyMetadata(new ColumnCollection()));

        //public ColumnCollection Columns
        //{
        //    get { return (ColumnCollection)GetValue(ColumnsProperty); }
        //    set { SetValue(ColumnsProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ColumnsProperty =
        //    DependencyProperty.Register("Columns", typeof(ColumnCollection), typeof(XTableColumn), new PropertyMetadata(null));

        /// <summary>
        /// Gets the Column Span that should this columns take.
        /// </summary>
        internal int Span
        {
            get
            {
                int l = this.LevelsCount();
                return this.GetColumns(l).Count;
            }
        }

        public XTableColumn()
        {
            Columns = new ColumnCollection();
        }
    }
}