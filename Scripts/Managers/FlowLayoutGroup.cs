// using UnityEngine;
// using UnityEngine.UI;

// namespace CustomLayouts
// {
//     [AddComponentMenu("Layout/Flow Layout Group (Custom)")]
//     public class FlowLayoutGroup : LayoutGroup
//     {
//         public float spacingX = 0f;
//         public float spacingY = 0f;
//         public bool childForceExpandWidth = false;
//         public bool childForceExpandHeight = false;

//         public override void CalculateLayoutInputHorizontal()
//         {
//             base.CalculateLayoutInputHorizontal();
//             float minWidth = GetGreatestMinimumChildWidth() + padding.horizontal;
//             float preferredWidth = GetGreatestPreferredChildWidth() + padding.horizontal;
//             SetLayoutInputForAxis(minWidth, preferredWidth, -1, 0);
//         }

//         public override void CalculateLayoutInputVertical()
//         {
//             float minHeight = GetGreatestMinimumChildHeight() + padding.vertical;
//             float preferredHeight = GetGreatestPreferredChildHeight() + padding.vertical;
//             SetLayoutInputForAxis(minHeight, preferredHeight, -1, 1);
//         }

//         public override void SetLayoutHorizontal()
//         {
//             SetCells(0);
//         }

//         public override void SetLayoutVertical()
//         {
//             SetCells(1);
//         }

//         private void SetCells(int axis)
//         {
//             // Traversal state
//             float currentX = padding.left;
//             float currentY = padding.top;
//             float maxRowHeight = 0f;

//             // Container Width (width of this RectTransform)
//             float containerWidth = rectTransform.rect.width;

//             for (int i = 0; i < rectChildren.Count; i++)
//             {
//                 RectTransform child = rectChildren[i];
//                 float childWidth = LayoutUtility.GetPreferredWidth(child);
//                 float childHeight = LayoutUtility.GetPreferredHeight(child);

//                 // Jika elemen ini melebihi sisa lebar container, pindah ke baris baru (Newline)
//                 if (currentX + childWidth > containerWidth - padding.right)
//                 {
//                     currentX = padding.left;
//                     currentY += maxRowHeight + spacingY;
//                     maxRowHeight = 0f; // Reset tinggi baris baru
//                 }

//                 // Posisikan elemen
//                 if (axis == 0) // Horizontal pass
//                 {
//                     SetChildAlongAxis(child, 0, currentX, childWidth);
//                 }
//                 else // Vertical pass
//                 {
//                     SetChildAlongAxis(child, 1, currentY, childHeight);
//                 }

//                 // Update posisi X untuk elemen berikutnya
//                 currentX += childWidth + spacingX;

//                 // Update tinggi baris (mengambil elemen tertinggi di baris ini)
//                 if (childHeight > maxRowHeight)
//                 {
//                     maxRowHeight = childHeight;
//                 }
//             }
            
//             // Update tinggi total container agar bisa discroll
//             if (axis == 1)
//             {
//                 float totalHeight = currentY + maxRowHeight + padding.bottom;
//                 rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
//             }
//         }

//         // Helper functions
//         private float GetGreatestMinimumChildWidth()
//         {
//             float max = 0;
//             foreach (var child in rectChildren)
//             {
//                 float min = LayoutUtility.GetMinWidth(child);
//                 if (min > max) max = min;
//             }
//             return max;
//         }

//         private float GetGreatestPreferredChildWidth()
//         {
//             float max = 0;
//             foreach (var child in rectChildren)
//             {
//                 float pref = LayoutUtility.GetPreferredWidth(child);
//                 if (pref > max) max = pref;
//             }
//             return max;
//         }
        
//         private float GetGreatestMinimumChildHeight()
//         {
//             float max = 0;
//             foreach (var child in rectChildren)
//             {
//                 float min = LayoutUtility.GetMinHeight(child);
//                 if (min > max) max = min;
//             }
//             return max;
//         }

//         private float GetGreatestPreferredChildHeight()
//         {
//             float max = 0;
//             foreach (var child in rectChildren)
//             {
//                 float pref = LayoutUtility.GetPreferredHeight(child);
//                 if (pref > max) max = pref;
//             }
//             return max;
//         }
//     }
// }

using UnityEngine;
using UnityEngine.UI;

namespace CustomLayouts
{
    [AddComponentMenu("Layout/Flow Layout Group (Custom)")]
    public class FlowLayoutGroup : LayoutGroup
    {
        public float spacingX = 0f;
        public float spacingY = 0f;
        public bool childForceExpandWidth = false;
        public bool childForceExpandHeight = false;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            float minWidth = GetGreatestMinimumChildWidth() + padding.horizontal;
            float preferredWidth = GetGreatestPreferredChildWidth() + padding.horizontal;
            SetLayoutInputForAxis(minWidth, preferredWidth, -1, 0);
        }

        public override void CalculateLayoutInputVertical()
        {
            float minHeight = GetGreatestMinimumChildHeight() + padding.vertical;
            float preferredHeight = GetGreatestPreferredChildHeight() + padding.vertical;
            SetLayoutInputForAxis(minHeight, preferredHeight, -1, 1);
        }

        public override void SetLayoutHorizontal()
        {
            SetCells(0);
        }

        public override void SetLayoutVertical()
        {
            SetCells(1);
        }

        private void SetCells(int axis)
        {
            // Traversal state
            float currentX = padding.left;
            float currentY = padding.top;
            float maxRowHeight = 0f;

            // Container Width (width of this RectTransform)
            float containerWidth = rectTransform.rect.width;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                RectTransform child = rectChildren[i];
                float childWidth = LayoutUtility.GetPreferredWidth(child);
                float childHeight = LayoutUtility.GetPreferredHeight(child);

                // Jika elemen ini melebihi sisa lebar container, pindah ke baris baru (Newline)
                if (currentX + childWidth > containerWidth - padding.right)
                {
                    currentX = padding.left;
                    currentY += maxRowHeight + spacingY;
                    maxRowHeight = 0f; // Reset tinggi baris baru
                }

                // Posisikan elemen
                if (axis == 0) // Horizontal pass
                {
                    SetChildAlongAxis(child, 0, currentX, childWidth);
                }
                else // Vertical pass
                {
                    SetChildAlongAxis(child, 1, currentY, childHeight);
                }

                // Update posisi X untuk elemen berikutnya
                currentX += childWidth + spacingX;

                // Update tinggi baris (mengambil elemen tertinggi di baris ini)
                if (childHeight > maxRowHeight)
                {
                    maxRowHeight = childHeight;
                }
            }
            
            // Update tinggi total container agar bisa discroll
            if (axis == 1)
            {
                float totalHeight = currentY + maxRowHeight + padding.bottom;
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
            }
        }

        // Helper functions
        private float GetGreatestMinimumChildWidth()
        {
            float max = 0;
            foreach (var child in rectChildren)
            {
                float min = LayoutUtility.GetMinWidth(child);
                if (min > max) max = min;
            }
            return max;
        }

        private float GetGreatestPreferredChildWidth()
        {
            float max = 0;
            foreach (var child in rectChildren)
            {
                float pref = LayoutUtility.GetPreferredWidth(child);
                if (pref > max) max = pref;
            }
            return max;
        }
        
        private float GetGreatestMinimumChildHeight()
        {
            float max = 0;
            foreach (var child in rectChildren)
            {
                float min = LayoutUtility.GetMinHeight(child);
                if (min > max) max = min;
            }
            return max;
        }

        private float GetGreatestPreferredChildHeight()
        {
            float max = 0;
            foreach (var child in rectChildren)
            {
                float pref = LayoutUtility.GetPreferredHeight(child);
                if (pref > max) max = pref;
            }
            return max;
        }
    }
}