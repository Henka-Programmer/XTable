using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace gamadev.xtable
{
    [Serializable]
    public class XTableRow : Control
    {
        internal Grid parentXGrid;

        public ObservableCollection<XTableCell> Cells
        {
            get { return (ObservableCollection<XTableCell>)GetValue(CellsProperty); }
            set { SetValue(CellsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Cells.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellsProperty =
            DependencyProperty.Register("Cells", typeof(ObservableCollection<XTableCell>), typeof(XTableRow), new PropertyMetadata(OnCellsPropertyChanged));

        private static void OnCellsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            XTableRow row = d as XTableRow;
            if (row != null && e.NewValue == null) return;
            var cells = (ObservableCollection<XTableCell>)e.NewValue;
            cells.CollectionChanged -= row.OnCellsChanged;
            cells.CollectionChanged += row.OnCellsChanged;
        }

        public virtual void OnCellsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        foreach (XTableCell cell in e.NewItems)
                        {
                            //// background binding
                            //Binding background_bd = new Binding("Background");
                            //background_bd.Mode = BindingMode.OneWay;
                            //background_bd.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                            //background_bd.Source = this;

                            //cell.SetBinding(BackgroundProperty, background_bd);

                            // data context binding
                            Binding datacontext_bd = new Binding("DataContext");
                            datacontext_bd.Mode = BindingMode.OneWay;
                            datacontext_bd.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                            datacontext_bd.Source = this;

                            cell.SetBinding(DataContextProperty, datacontext_bd);
                            cell.RowIndex = Index;
                        }
                    }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    break;

                default:
                    break;
            }
        }

        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(XTableRow), new PropertyMetadata(-1));

        static XTableRow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(XTableRow), new FrameworkPropertyMetadata(typeof(XTableRow)));
        }

        public XTableRow()
        {
            Cells = new ObservableCollection<XTableCell>();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Name == "Index")
            {
                if (Cells != null && Cells.Count > 0)
                {
                    foreach (var cell in Cells)
                    {
                        cell.RowIndex = Index;
                        Grid.SetRow(cell, Index);
                    }
                }
            }
        }

        internal void RemoveFromTable()
        {
            if (parentXGrid != null)
            {
                var rdindex = Grid.GetRow(this);
                foreach (var cell in Cells)
                {
                    parentXGrid.Children.Remove(cell);
                }
                parentXGrid.Children.Remove(this);
                parentXGrid.RowDefinitions.RemoveAt(rdindex - 1);

                foreach (var item in parentXGrid.Children.OfType<UIElement>().ToList())
                {
                    int originalrowIndex = Grid.GetRow(item);
                    if (originalrowIndex > rdindex)
                    {
                        Grid.SetRow(item, --originalrowIndex);
                    }
                }
            }
        }

        #region Operators

        public XTableCell this[int index]
        {
            get { return Cells[index]; }
            set { Cells[index] = value; }
        }

        #endregion Operators
    }
}