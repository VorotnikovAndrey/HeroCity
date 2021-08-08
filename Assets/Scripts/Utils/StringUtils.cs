using UnityEngine;

namespace Defong.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Used for HTML colors. (#C52806)
        /// https://htmlcolorcodes.com
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string AddColorTag(this object message, string color)
        {
            string result = $"<color=#{color.Replace("#", string.Empty)}>{message}</color>";
            return result;
        }

        public static string AddColorTag(Color color, string tag)
        {
            string result = $"<color=#{GetColorHexString(color)}>{tag}</color>";
            return result;
        }

        public static string AddColorTag(this string message, Color color)
        {
            string result = $"<color=#{GetColorHexString(color)}>{message}</color>";
            return result;
        }

        public static string AddColorTag(this object message, Color color)
        {
            string result = $"<color=#{GetColorHexString(color)}>{message}</color>";
            return result;
        }

        private static string GetColorHexString(Color color)
        {
            string colorString = string.Empty;
            colorString += ((int)(color.r * 255)).ToString("X02");
            colorString += ((int)(color.g * 255)).ToString("X02");
            colorString += ((int)(color.b * 255)).ToString("X02");
            return colorString;
        }
    }
}
