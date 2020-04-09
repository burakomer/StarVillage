using UnityEngine;
using UnityEngine.UI;

namespace DwarfEngine
{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColums
        }

        public FitType fitType;
        public int rows;
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;

        public bool fitX;
        public bool fitY;
        public bool autoSizeForScrolling;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (fitX && fitY)
            {
                autoSizeForScrolling = false;
            }

            if (autoSizeForScrolling)
            {
                if (fitX)
                {
                    fitType = FitType.FixedColums;
                }
                else if (fitY)
                {
                    fitType = FitType.FixedRows;
                }
            }

            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
            {
                fitX = true;
                fitY = true;
                autoSizeForScrolling = false;

                float sqrRt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrRt);
                columns = Mathf.CeilToInt(sqrRt);
            }

            if (fitType == FitType.Width || fitType == FitType.FixedColums)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float) columns);
            }
            else if (fitType == FitType.Height || fitType == FitType.FixedRows)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float autoCellWidth = (parentWidth / columns) - (spacing.x / ((float)columns / (columns - 1))) - (padding.left / (float)columns) - (padding.right / (float)columns);
            float autoCellHeight = (parentHeight / rows) - (spacing.y / ((float)rows / (rows - 1))) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

            cellSize.x = fitX ? autoCellWidth : cellSize.x;
            cellSize.y = fitY ? autoCellHeight : cellSize.y;

            if (autoSizeForScrolling)
            {
                rectTransform.pivot = new Vector2(0, 1);

                if (!fitX && fitY)
                {
                    rectTransform.sizeDelta = new Vector2(((cellSize.x + spacing.x) * (columns)) - spacing.x + padding.left + padding.right, rectTransform.sizeDelta.y);
                }
                if (!fitY && fitX)
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, ((cellSize.y + spacing.y) * (rows)) - spacing.y + padding.top + padding.bottom);
                }
            }

            float columnCount = 0;
            int rowCount = 0;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                rowCount = i / columns;
                columnCount = i % columns;

                float columnAlignment = GetAlignmentOnAxis(0);
                float rowAlignment = GetAlignmentOnAxis(1);

                var item = rectChildren[i];

                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                var yPos = (cellSize.y * rowCount)  + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
                
            }
        }

        public override void CalculateLayoutInputVertical()
        {
            
        }

        public override void SetLayoutHorizontal()
        {
            
        }

        public override void SetLayoutVertical()
        {
           
        }
    }
}