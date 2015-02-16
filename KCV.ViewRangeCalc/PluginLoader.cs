﻿using Grabacr07.KanColleViewer.Composition;
using System.ComponentModel.Composition;
using System.Linq;

namespace Gizeta.KCV.ViewRangeCalc
{
    [Export(typeof(IToolPlugin))]
    [ExportMetadata("Title", "KCV.ViewRangeCalc")]
    [ExportMetadata("Description", "KanColleViewer索敌值计算插件。")]
    [ExportMetadata("Version", "1.1.1")]
    [ExportMetadata("Author", "@Gizeta")]
    public class PluginLoader : IToolPlugin
    {
        internal static bool hasInitialized = false;

        public PluginLoader()
        {
            if (!hasInitialized)
            {
                hasInitialized = true;

                CalcExtension.Instance.Initialize();
            }
        }

        public string ToolName
        {
            get { return "ViewRangeCalc"; }
        }

        public object GetSettingsView()
        {
            return null;
        }

        public object GetToolView()
        {
            return null;
        }
    }
}
