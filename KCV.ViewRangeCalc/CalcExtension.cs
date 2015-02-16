using Grabacr07.KanColleViewer.ViewModels;
using Grabacr07.KanColleViewer.ViewModels.Contents;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KCVApp = Grabacr07.KanColleViewer.App;

namespace Gizeta.KCV.ViewRangeCalc
{
    public class CalcExtension
    {
        private static readonly CalcExtension instance = new CalcExtension();

        private ContentPresenter contentView;

        private CalcExtension()
        {
        }

        public static CalcExtension Instance
        {
            get { return instance; }
        }
        
        public void Initialize()
        {
            KCVUIHelper.OperateMainWindow(() =>
            {
                KCVUIHelper.KCVWindow.ContentRendered += KCVWindow_ContentRendered;

                KCVApp.ViewModelRoot.Settings.ToolPlugins = KCVApp.ViewModelRoot.Settings.ToolPlugins.Where(x => x.ToolName != "ViewRangeCalc").ToList();

                KCVApp.ViewModelRoot.Settings.ViewRangeSettingsCollection.Add(new SettingsViewModel.ViewRangeSettingsViewModel(new ViewRangeType4()));
            });
        }

        private void KCVWindow_ContentRendered(object sender, EventArgs e)
        {
            KCVUIHelper.KCVWindow.ContentRendered -= KCVWindow_ContentRendered;

            KCVUIHelper.KCVContent = KCVUIHelper.KCVWindow.FindVisualChildren<ContentControl>().Where(x => x.Content is StartContentViewModel || x.Content is MainContentViewModel).First();

            KCVUIHelper.KCVContent.FindVisualChildren<ContentPresenter>().Where(x => x.DataContext is StartContentViewModel || x.DataContext is MainContentViewModel).First().DataContextChanged += ContentView_DataContextChanged;
        }

        private void ContentView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            contentView = sender as ContentPresenter;
            contentView.LayoutUpdated += ContentView_LayoutUpdated;
        }

        private void ContentView_LayoutUpdated(object sender, EventArgs e)
        {
            contentView.LayoutUpdated -= ContentView_LayoutUpdated;
            contentView = null;

            if (KCVApp.ViewModelRoot.Content is MainContentViewModel)
            {
                var toolsViewModel = (KCVApp.ViewModelRoot.Content as MainContentViewModel).TabItems.Where(x => x is ToolsViewModel).First() as ToolsViewModel;
                toolsViewModel.Tools = toolsViewModel.Tools.Where(x => x.ToolName != "ViewRangeCalc").ToList();
            }
        }
    }
}
