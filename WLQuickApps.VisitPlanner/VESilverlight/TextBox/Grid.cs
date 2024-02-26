//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;
using System.Windows;

namespace VESilverlight
{
    class Grid : LayoutContainerControl
    {
        protected override Size MeasureCore(Size availableSize)
        {
            Size desiredSize = new Size(
                LayoutStorage.WidthWasSpecified ? LayoutStorage.OriginallySpecifiedSize.Width : availableSize.Width,
                LayoutStorage.HeightWasSpecified ? LayoutStorage.OriginallySpecifiedSize.Height : availableSize.Height);

            _columnDefinitions.Reset();
            _rowDefinitions.Reset();

            foreach (ILayout child in Children)
            {
                IGrid gridChild = GetValidGridChild(child);
                if (gridChild != null)
                {
                    CellInfo colCell = _columnDefinitions.Cells[gridChild.GridColumn];
                    CellInfo rowCell = _rowDefinitions.Cells[gridChild.GridRow];

                    Size availableForChild = new Size(
                        colCell.Type == CellType.Specified ? colCell.Value : double.PositiveInfinity,
                        rowCell.Type == CellType.Specified ? rowCell.Value : double.PositiveInfinity);

                    child.Measure(availableForChild);

                    if (colCell.Type != CellType.Specified)
                        colCell.Value = Math.Max(colCell.Value, child.LayoutStorage.DesiredSize.Width);
                    if (rowCell.Type != CellType.Specified)
                        rowCell.Value = Math.Max(rowCell.Value, child.LayoutStorage.DesiredSize.Height);
                }
                else
                {
                    child.Measure(desiredSize);
                }
            }

            if (LayoutStorage.WidthWasSpecified)
                desiredSize.Width = LayoutStorage.OriginallySpecifiedSize.Width;

            if (LayoutStorage.HeightWasSpecified)
                desiredSize.Height = LayoutStorage.OriginallySpecifiedSize.Height;

            _columnDefinitions.CalculateOffsets(desiredSize.Width);
            _rowDefinitions.CalculateOffsets(desiredSize.Height);

            return new Size(
                LayoutStorage.WidthWasSpecified ? LayoutStorage.OriginallySpecifiedSize.Width : _columnDefinitions.Total, 
                LayoutStorage.HeightWasSpecified ? LayoutStorage.OriginallySpecifiedSize.Height : _rowDefinitions.Total);
        }

        protected override void ArrangeCore(Rect finalRect)
        {
            base.ArrangeCore(finalRect);

            LayoutHelpers.SetClipRect(this, new Rect(0, 0, finalRect.Width, finalRect.Height));

            foreach (ILayout child in Children)
            {
                IGrid gridChild = GetValidGridChild(child);
                if (gridChild != null)
                {
                    CellInfo colCell = _columnDefinitions.Cells[gridChild.GridColumn];
                    CellInfo rowCell = _rowDefinitions.Cells[gridChild.GridRow];

                    Rect childRect = new Rect(
                        colCell.Offset,
                        rowCell.Offset,
                        colCell.Value,
                        rowCell.Value);

                    child.Arrange(childRect);
                }
                else
                {
                    double actualChildWidth = child.LayoutStorage.WidthWasSpecified ? child.LayoutStorage.OriginallySpecifiedSize.Width : finalRect.Width;
                    double actualChildHeight = child.LayoutStorage.HeightWasSpecified ? child.LayoutStorage.OriginallySpecifiedSize.Height : finalRect.Height;

                    Rect childRect = new Rect(
                        (Width - actualChildWidth) / 2,
                        (Height - actualChildHeight) / 2,
                        actualChildWidth,
                        actualChildHeight);

                    child.Arrange(childRect);
                }
            }
        }

        private IGrid GetValidGridChild(object child)
        {
            IGrid gridChild = child as IGrid;

            return (child != null && _columnDefinitions != null && _rowDefinitions != null) ? gridChild : null;
        }

        public string RowDefinitions
        {
            get { return _rowDefinitions.ToString(); }
            set { _rowDefinitions = new GridInfo(value); }
        }

        public string ColumnDefinitions
        {
            get { return _columnDefinitions.ToString(); }
            set { _columnDefinitions = new GridInfo(value); }
        }
        GridInfo _rowDefinitions, _columnDefinitions;

        public const int Default = 0;
    }

    public interface IGrid
    {
        int GridColumn { get; set; }
        int GridRow { get; set; }
    }
}
