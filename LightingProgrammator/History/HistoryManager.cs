using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightingProgrammator.History
{
    internal static class HistoryManager
    {
        private static List<IHistoryAction> actions = new List<IHistoryAction>();
        private static int pointer = 0;

        public static void ResetHistory()
        {
            actions.Clear();
            pointer = 0;
        }
        public static void RegisterAction(IHistoryAction action)
        {
            if (pointer < actions.Count) actions.RemoveRange(pointer, actions.Count-pointer);
            actions.Add(action);
            pointer++;

            if (actions.Count > 200)
            {
                actions.RemoveAt(0);
                pointer--;
            }
        }
        public static void Undo()
        {
            if (pointer > 0) actions[--pointer].Undo();
            Debug.WriteLine("Undo: "+pointer);
        }
        public static void Redo()
        {
            if (pointer<actions.Count) actions[pointer++].Redo();
            Debug.WriteLine("Redo: " + pointer);
        }
    }
}
