using UnityEngine;

namespace Defong
{
    public static class EditorUtils
    {
        public static Rect[] SplitHorizontal(this Rect rect, int count)
        {
            var rects = new Rect[count];
            for (int i = 0; i < count; i++)
            {
                var partWidth = rect.width / count;
                var newPos = new Vector2(rect.position.x + i * partWidth, rect.position.y);
                rects[i] = new Rect(rect){position = newPos, width = partWidth};
            }
            return rects;
        }
    }
}