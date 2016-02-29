using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace gamadev.xtable
{
    public static class XTableExtensions
    {
   //     public static IEnumerable<GroupResult> GroupByMany<TElement>(
   //     this IEnumerable<TElement> elements,
   //     params Func<TElement, object>[] groupSelectors)
   //     {
   //         if (groupSelectors.Length > 0)
   //         {
   //             var selector = groupSelectors.First();

   //             //reduce the list recursively until zero
   //             var nextSelectors = groupSelectors.Skip(1).ToArray();
   //             return
   //                 elements.GroupBy(selector).Select(
   //                     g => new GroupResult
   //                     {
   //                         Key = g.Key,
   //                         Count = g.Count(),
   //                         Items = g,
   //                         SubGroups = g.GroupByMany(nextSelectors)
   //                     });
   //         }
   //         else
   //             return null;
   //     }

   //     public static object GetValue(this PropertyPath path, object obj)
   //     {
   //         if (string.IsNullOrEmpty(path.Path))
   //         {
   //             return obj;
   //         }
   //         PropertyInfo info = obj.GetType().GetProperty(path.Path);
   //         return info.GetValue(obj);
   //     }

   //     public static IEnumerable<GroupResult> GroupByMany<T>(
   //this IEnumerable<T> elements,
   //params PropertyPath[] groupSelectors)  where T : class
   //     {
   //         if (groupSelectors.Length > 0)
   //         {
   //             var selector = groupSelectors.First();

   //             //reduce the list recursively until zero
   //             var nextSelectors = groupSelectors.Skip(1).ToArray();
   //             return
   //                 elements.GroupBy(item => selector.GetValue(item)).Select(
   //                     g => new GroupResult
   //                     {
   //                         Key = g.Key,
   //                         Count = g.Count(),
   //                         Items = g,
   //                         SubGroups = g.GroupByMany(nextSelectors)
   //                     });
   //         }
   //         else
   //             return null;
   //     }

        public static int LevelsCount(this XTableColumn column)
        {
            if (column.Columns == null || column.Columns.Count == 0)
            {
                return 1;
            }

            List<int> levels = new List<int>();
            foreach (var col in column.Columns)
            {
                levels.Add(1 + LevelsCount(col));
            }
            return levels.Max();
        }

        public static List<XTableColumn> GetDeepColumns(this XTableColumn column)
        {
            List<XTableColumn> result = new List<XTableColumn>();
            if (column.Columns == null || column.Columns.Count == 0)
            {
                result.Add(column);
                return result;
            }

            foreach (var item in column.Columns)
            {
                result.AddRange(item.GetDeepColumns());
            }

            return result;
        }

        public static XTableColumn Clone(this XTableColumn column, bool withNestedColumns = true)
        {
            if (column == null)
            {
                return null;
            }
            XTableColumn clone = new XTableColumn()
            {
                Header = column.Header,
                Width = column.Width,
                IsEnabled = column.IsEnabled,
                SharedSizeGroup = column.SharedSizeGroup,
                Binding = column.Binding,
                MaxWidth = column.MaxWidth,
                MinWidth = column.MinWidth,
                CellStyle = column.CellStyle,
                IsTransitive = column.IsTransitive,
                IsGrouped = column.IsGrouped
            };

            if (withNestedColumns && column.Columns != null && column.Columns.Count > 0)
            {
                clone.Columns = new ColumnCollection();
                foreach (var col in column.Columns)
                {
                    clone.Columns.Add(col.Clone());
                }
            }

            return clone;
        }

        public static int GetChildCount(this XTableColumn column)
        {
            if (column.Columns == null || column.Columns.Count == 0)
            {
                return 1;
            }

            int children = 0;
            foreach (var col in column.Columns)
            {
                children += GetChildCount(col);
            }
            return children;
        }

        public static List<XTableColumn> GetLevelColumns(this ColumnCollection cols, int level)
        {
            List<XTableColumn> lcols = new List<XTableColumn>();
            foreach (var col in cols)
            {
                lcols.AddRange(col.GetColumns(level));
            }

            return lcols;
        }

        public static List<XTableColumn> GetColumns(this XTableColumn column, int level)
        {
            List<XTableColumn> lcols = new List<XTableColumn>();
            if (level - 1 == 0 || (column.Columns == null || column.Columns.Count == 0))
            {
                lcols.Add(column);
                return lcols;
            }

            List<XTableColumn> columns = new List<XTableColumn>();
            foreach (var col in column.Columns)
            {
                columns.AddRange(col.GetColumns(--level));
            }

            return columns;
        }
    }
}