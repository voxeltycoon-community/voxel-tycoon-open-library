using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace VTOL.Utils
{
    /// <summary>
    /// Helper class for dumping a structure of given gameObject
    /// </summary>
    [UsedImplicitly]
    internal static class VTOLGameObjectDumper
    {
        private static StringWriter stringWriter;

        private static StringWriter StringWriter => stringWriter ?? (stringWriter = new StringWriter());

        /// <summary>
        /// Dumps given gameObject's components and its descendants into a string
        /// </summary>
        /// <param name="gameObject">GameObject to dump</param>
        /// <returns>Dumped GameObject components and Transforms</returns>
        [UsedImplicitly]
        public static string DumpGameObject(GameObject gameObject)
        {
            StringWriter strWriter = StringWriter;
            strWriter.GetStringBuilder().Clear();
            DumpGameObjectInternal(gameObject, strWriter);
            return strWriter.ToString();
        }

        /// <summary>
        /// Dumps given gameObject's components and its descendants into the writer
        /// </summary>
        /// <param name="gameObject">GameObject to dump</param>
        /// <param name="writer">writer that receives dumped GameObject components and Transforms</param>
        [UsedImplicitly]
        public static void DumpGameObject(GameObject gameObject, TextWriter writer)
        {
            DumpGameObjectInternal(gameObject, writer);
        }

        private static void DumpGameObjectInternal(GameObject gameObject, TextWriter writer, string indent = "  ")
        {
            writer.WriteLine("{0}+{1} ({2})", indent, gameObject.name, gameObject.transform.GetType().Name);

            foreach (Component component in gameObject.GetComponents<Component>())
            {
                DumpComponent(component, writer, indent + "  ");
            }

            foreach (Transform child in gameObject.transform)
            {
                DumpGameObjectInternal(child.gameObject, writer, indent + "  ");
            }
        }

        private static void DumpComponent(Component component, TextWriter writer, string indent)
        {
            writer.WriteLine("{0}{1}", indent, (component == null ? "(null)" : component.GetType().Name));
        }
    }
}