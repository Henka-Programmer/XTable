using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace gamadev.xtable
{
    [TemplatePart(Name = "PART_XGrid", Type = typeof(Grid))]
    [Serializable]
    public class XTable : Control
    {
        protected internal Grid xgrid = null;
        protected bool isCreated = false;
        private CollectionViewSource viewSource = new CollectionViewSource();
        internal int firtDataRowIndex = 0;

        #region Properties

        public SortDescriptionCollection SortDescriptions
        {
            get { return (SortDescriptionCollection)GetValue(SortDescriptionsProperty); }
            set { SetValue(SortDescriptionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SortDescriptions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SortDescriptionsProperty =
            DependencyProperty.Register("SortDescriptions", typeof(SortDescriptionCollection), typeof(XTable), new PropertyMetadata(new SortDescriptionCollection()));

        public GridLength RowHeight
        {
            get { return (GridLength)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowHight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(GridLength), typeof(XTable), new PropertyMetadata(GridLength.Auto));

        public Brush LinesBrush
        {
            get { return (Brush)GetValue(LinesBrushProperty); }
            set { SetValue(LinesBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LinesBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinesBrushProperty =
            DependencyProperty.Register("LinesBrush", typeof(Brush), typeof(XTable), new PropertyMetadata(Brushes.Black));

        public Style HeaderCellStyle
        {
            get { return (Style)GetValue(HeaderCellStyleProperty); }
            set { SetValue(HeaderCellStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderCellStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderCellStyleProperty =
            DependencyProperty.Register("HeaderCellStyle", typeof(Style), typeof(XTable), new PropertyMetadata(null));

        public XTableGridLinesVisibility LinesVisibility
        {
            get { return (XTableGridLinesVisibility)GetValue(LinesVisibilityProperty); }
            set { SetValue(LinesVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LinesVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinesVisibilityProperty =
            DependencyProperty.Register("LinesVisibility", typeof(XTableGridLinesVisibility), typeof(XTable), new PropertyMetadata(XTableGridLinesVisibility.All));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(XTable), new PropertyMetadata(null));

        //public ObservableCollection<object> ItemsSource
        //{
        //    get { return (ObservableCollection<object>)GetValue(ItemsSourceProperty); }
        //    set { SetValue(ItemsSourceProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ItemsSourceProperty =
        //    DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<object>), typeof(XTable), new PropertyMetadata(null, OnItemsSourcePropertyChanged));

        //private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    XTable table = d as XTable;
        //    if (table == null) return;
        //    if (e.NewValue == null)
        //    {
        //        table.Clear();
        //        return;
        //    }
        //}

        public ObservableCollection<XTableRow> Rows
        {
            get { return (ObservableCollection<XTableRow>)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(ObservableCollection<XTableRow>), typeof(XTableRow), new PropertyMetadata(null, OnRowsPropertyChanged));

        private static void OnRowsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var table = d as XTable;
            if (table == null)
                return;

            INotifyCollectionChanged rows = e.NewValue as INotifyCollectionChanged;
            if (rows != null)
            {
                rows.CollectionChanged += table.OnRowsChanged;
            }
        }

        private void OnRowsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        int rows_count = xgrid.RowDefinitions.Count;
                        foreach (XTableRow item in e.NewItems)
                        {
                            Binding background_bd = new Binding("Background");
                            background_bd.Mode = BindingMode.OneWay;
                            background_bd.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                            background_bd.Source = this;

                            ((XTableRow)item).SetBinding(XTableRow.BackgroundProperty, background_bd);

                            xgrid.RowDefinitions.Add(new RowDefinition() { Height = RowHeight });
                            xgrid.Children.Add(item);
                            Grid.SetColumnSpan(item, item.Cells.Count);
                            for (int i = 0; i < item.Cells.Count; i++)
                            {
                                xgrid.Children.Add(item.Cells[i]);
                                if (i == 0)
                                {
                                    if (ActualHeaderColumns[i].CellStyle != null)
                                        item.Style = ActualHeaderColumns[i].CellStyle;
                                }
                                Grid.SetColumn(item[i], i);
                            }
                            item.Index = rows_count++;
                            Grid.SetRow(item, item.Index);
                        }
                    }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        foreach (XTableRow item in e.OldItems)
                        {
                            item.RemoveFromTable();
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public Visibility HeaderVisibility
        {
            get { return (Visibility)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderVisibilityProperty =
            DependencyProperty.Register("HeaderVisibility", typeof(Visibility), typeof(XTable), new PropertyMetadata(Visibility.Visible));

        public GridLength HeaderHeight
        {
            get { return (GridLength)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.Register("HeaderHeight", typeof(GridLength), typeof(XTable), new PropertyMetadata(new GridLength(1, GridUnitType.Auto)));

        /// <summary>
        /// Represents the Columns Definition of the XTable
        /// </summary>
        //public ColumnCollection Columns
        //{
        //    get { return (ColumnCollection)GetValue(ColumnsProperty); }
        //    set { SetValue(ColumnsProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ColumnsProperty =
        //    DependencyProperty.Register("Columns", typeof(ColumnCollection), typeof(XTable), new PropertyMetadata(new ColumnCollection()));

        public ColumnCollection Columns
        {
            get { return (ColumnCollection)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(ColumnCollection), typeof(XTable), new PropertyMetadata(new ColumnCollection()));

        public List<XTableColumn> ActualHeaderColumns { get; set; }

        #endregion Properties

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            xgrid = Template.FindName("PART_XGrid", this) as Grid;
            //if (!isCreated)
            //{
            //    Create();
            //}
        }

        #region Methods

        protected internal XTableRow NewRow(List<XTableColumn> columns, object boundedDataContext)
        {
            Binding bd = new Binding(".");
            bd.Source = boundedDataContext;
            bd.Mode = BindingMode.TwoWay;
            bd.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            XTableRow row = new XTableRow() { parentXGrid = xgrid };

            row.SetBinding(DataContextProperty, bd);
            foreach (var col in columns)
            {
                XTableCell cell = new XTableCell();
                row.Cells.Add(cell);
                if (col.CellStyle != null)
                {
                    cell.Style = col.CellStyle;
                }
                if (col.ContentTemplate != null)
                {
                    cell.ContentTemplate = col.ContentTemplate;
                }
                if (col.Binding != null)
                {
                    cell.SetBinding(ContentControl.ContentProperty, col.Binding);
                }
            }

            return row;
        }

        protected internal List<XTableColumn> GetActualTableColumns()
        {
            List<XTableColumn> columns = new List<XTableColumn>();
            foreach (var col in Columns)
            {
                columns.AddRange(col.GetDeepColumns());
            }

            return columns;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

        public void Clear()
        {
            ClearDefinitions();
            // clear contents
            if (xgrid == null)
            {
                throw new System.NullReferenceException("XTable Container");
            }
            xgrid.Children.Clear();
            Rows.Clear();
            isCreated = false;
        }

        public void ClearDefinitions()
        {
            if (xgrid == null)
            {
                throw new System.NullReferenceException("XTable Container");
            }

            xgrid.RowDefinitions.Clear();
            xgrid.ColumnDefinitions.Clear();
        }

        public virtual void Create()
        {
            if (xgrid != null)
            {
                //clear old data
                Clear();
                // init header columnds
                InitializeHeader();
                xgrid.Margin = new Thickness(-.5);
                CreateRows();

                if (ActualHeaderColumns != null && Rows != null && Rows.Count > 0)
                {
                    int ParentGroupingIndex = -1;
                    for (int i = 0; i < ActualHeaderColumns.Count; i++)
                    {
                        if (ActualHeaderColumns[i].IsGrouped)
                        {
                            XTableCell firstCol = null;
                            if (ParentGroupingIndex == -1)
                            {
                                ParentGroupingIndex = i;
                            }
                            int indicatorItems = 1;

                            for (int j = 1; j < Rows.Count; j++)
                            {
                                if (firstCol == null)
                                {
                                    //Indicator = Rows[j - 1][i].Content;
                                    firstCol = Rows[j - 1].Cells[i];
                                }

                                if (firstCol.Content.ToString() == Rows[j][i].Content.ToString() && (Rows[j][ParentGroupingIndex].Content.ToString() == Rows[j - 1][ParentGroupingIndex].Content.ToString() || ActualHeaderColumns[i].IsTransitive))
                                {
                                    // Rows[j - 1][i].BorderThickness = new Thickness(Rows[j][i].BorderThickness.Left, Rows[j][i].BorderThickness.Top, Rows[j][i].BorderThickness.Right, 0);
                                    xgrid.Children.Remove(Rows[j][i]);
                                    indicatorItems++;
                                    //Grid.SetRowSpan(firstCol, indicatorItems);
                                    //Panel.SetZIndex(firstCol, int.MaxValue);
                                }
                                else
                                {
                                    indicatorItems = 1;
                                    firstCol = null;
                                    //  Indicator = Rows[j][i].Content;
                                }

                                if (firstCol != null)
                                {
                                    Grid.SetRowSpan(firstCol, indicatorItems);
                                    Panel.SetZIndex(firstCol, int.MaxValue);
                                }
                            }

                            ParentGroupingIndex = i;
                        }
                    }
                }

                isCreated = true;
            }
        }

        internal void CreateRows()
        {
            // get the number of header rows
            //  int Levels = GetHeaderRowsCount();
            if (ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                {
                    Rows.Add(NewRow(ActualHeaderColumns, item));
                    //xgrid.RowDefinitions.Add(new RowDefinition() { Height = RowHeight });
                    //xgrid.Children.Add(row);
                    //Grid.SetColumnSpan(row, row.Cells.Count);
                    //for (int i = 0; i < row.Cells.Count; i++)
                    //{
                    //    xgrid.Children.Add(row[i]);
                    //    if (i == 0)
                    //    {
                    //        if (ActualHeaderColumns[i].CellStyle != null)
                    //            row[i].Style = ActualHeaderColumns[i].CellStyle;
                    //    }
                    //    Grid.SetColumn(row[i], i);
                    //}
                    //row.Index = Levels++;
                    //Grid.SetRow(row, row.Index);
                }
            }
        }

        private int GetHeaderRowsCount()
        {
            List<int> ColumnsLevels = new List<int>();
            foreach (var col in Columns)
            {
                ColumnsLevels.Add(col.LevelsCount());
            }

            if (ColumnsLevels.Count == 0)
            {
                return 0;
            }
            return ColumnsLevels.Max();
        }

        protected virtual void InitializeHeader()
        {
            // to avoid the known issue at the design time.
            if (Template == null)
            {
                return;
            }
            if (xgrid == null)
            {
                xgrid = (Grid)Template.FindName("PART_XGrid", this);
                if (xgrid == null)
                {
                    xgrid = new Grid();
                }
            }

            GridLength headerrowHeight = HeaderHeight;
            if (HeaderVisibility == Visibility.Collapsed || HeaderVisibility == Visibility.Hidden)
            {
                headerrowHeight = new GridLength(0, GridUnitType.Pixel);
            }
            // get the number of levels
            int Levels = GetHeaderRowsCount();

            ActualHeaderColumns = GetActualTableColumns();

            // naming the columns definitions to be used in shared group
            int col_counter = 0;

            foreach (var coldef in ActualHeaderColumns)
            {
                var c = coldef.Clone(false);
                c.SharedSizeGroup = string.Format("C{0}", col_counter++);
                xgrid.ColumnDefinitions.Add(c);
            }

            for (int i = 0; i < Levels; i++)
            {
                xgrid.RowDefinitions.Add(new RowDefinition() { Name = string.Format("R_L{0}", i), Height = headerrowHeight });
            }
            // xgrid.RowDefinitions.Add(new RowDefinition() { Name = "R_Body", Height = RowHight });

            int colCount = 0;
            //List<XTableColumn> cols = GetActualTableColumns();
            List<XTableColumn> cols = new List<XTableColumn>();

            for (int i = Levels; i > 0; i--)
            {
                int colIndex = 0;

                cols = Columns.GetLevelColumns(i);
                colCount = cols.Count;

                for (int j = 0; j < colCount; j++)
                {
                    var cell = new XTableCell() { Content = cols[j].Header };
                    if (HeaderCellStyle != null)
                    {
                        cell.Style = HeaderCellStyle;
                    }
                    if (i == 1)
                    {
                        cell.BorderThickness = new Thickness(cell.BorderThickness.Left, 1, cell.BorderThickness.Right, cell.BorderThickness.Bottom);
                    }
                    xgrid.Children.Add(cell);
                    cell.ColumnIndex = colIndex;
                    cell.RowIndex = i - 1;
                    Grid.SetColumn(cell, colIndex);
                    int span = cols[j].Span;

                    Grid.SetColumnSpan(cell, span);
                    Grid.SetRow(cell, i - 1);

                    List<XTableCell> cells = xgrid.Children.OfType<XTableCell>().ToList();
                    if (cells != null)
                    {
                        var belowcell = cells.Where(c => c.ColumnIndex == colIndex && c.RowIndex == i).FirstOrDefault();
                        if (belowcell != null && belowcell.Content == cell.Content)
                        {
                            xgrid.Children.Remove(belowcell);
                            Grid.SetRow(cell, i - 1);
                            Grid.SetRowSpan(cell, Grid.GetRowSpan(belowcell) + 1);
                        }
                    }
                    colIndex += span;
                }
            }

            InvalidateProperty(HeaderVisibilityProperty);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Name == "HeaderVisibility")
            {
                if (HeaderVisibility == Visibility.Collapsed || HeaderVisibility == Visibility.Collapsed)
                {
                    for (int i = 0; i < GetHeaderRowsCount(); i++)
                    {
                        xgrid.RowDefinitions[i].Height = new GridLength(0, GridUnitType.Pixel);
                    }
                }
                else
                {
                    for (int i = 0; i < GetHeaderRowsCount(); i++)
                    {
                        xgrid.RowDefinitions[i].Height = HeaderHeight;
                    }
                }
            }
            else if (e.Property.Name == "ItemsSource")
            {
                if (e.NewValue == null)
                {
                    Clear();
                }
                else
                if (!(e.NewValue is ICollectionView))
                {
                    viewSource.Source = e.NewValue;

                    if (this.SortDescriptions != null && this.SortDescriptions.Count > 0)
                    {
                        foreach (var sortdes in this.SortDescriptions)
                        {
                            viewSource.SortDescriptions.Add(sortdes);
                        }
                    }

                    var cols = GetActualTableColumns();
                    if (cols != null)
                    {
                        foreach (var col in cols)
                        {
                            if (col.IsGrouped)
                            {
                                var bd = (col.Binding as Binding);
                                if (bd != null && !string.IsNullOrEmpty(bd.Path.Path))
                                {
                                    viewSource.SortDescriptions.Add(new System.ComponentModel.SortDescription(bd.Path.Path,
                                        ListSortDirection.Ascending));
                                }
                            }
                        }
                    }

                    var notifiable = ItemsSource as INotifyCollectionChanged;
                    if (notifiable != null)
                    {
                        ((INotifyCollectionChanged)ItemsSource).CollectionChanged -= XTable_ItemsSourceChanged;
                        ((INotifyCollectionChanged)ItemsSource).CollectionChanged += XTable_ItemsSourceChanged;
                    }

                    ItemsSource = viewSource.View;
                }

                Create();
            }
        }

        private void XTable_ItemsSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //switch (e.Action)
            //{
            //    case NotifyCollectionChangedAction.Replace:
            //        {
            //            ;
            //        }
            //        break;

            //    case NotifyCollectionChangedAction.Move:
            //        {
            //            ;
            //        }
            //        break;

            //    case NotifyCollectionChangedAction.Add:
            //        {
            //            foreach (var item in e.NewItems)
            //            {
            //                Rows.Add(NewRow(ActualHeaderColumns, item));
            //            }
            //        }
            //        break;

            //    case NotifyCollectionChangedAction.Remove:
            //        {
            //            foreach (var item in e.OldItems)
            //            {
            //                var row = Rows.Where(r => r.DataContext == item).FirstOrDefault();
            //                if (row != null)
            //                {
            //                    Rows.Remove(row);
            //                }
            //            }
            //        }
            //        break;

            //    case NotifyCollectionChangedAction.Reset:
            //        Rows.Clear();
            //        break;

            //    default:
            //        break;
            //}

            Clear();
            Create();
        }

        #endregion Methods

        public XTable()
        {
            Rows = new ObservableCollection<XTableRow>();
            Rows.CollectionChanged += this.OnRowsChanged;
            Columns = new ColumnCollection();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                Columns.CollectionChanged += (s, e) =>
                {
                    InitializeHeader();
                };
            }
        }

        static XTable()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(XTable), new FrameworkPropertyMetadata(typeof(XTable)));
        }
    }
}