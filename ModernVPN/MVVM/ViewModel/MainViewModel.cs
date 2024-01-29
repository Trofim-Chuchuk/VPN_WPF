using ModernVPN.Core;
using ModernVPN.MVVM.View;
using ModernVPN.MVVM.ViewModel;
using System.Windows;

namespace ModernVPN.MVVM.ViewModel {
    public class MainViewModel : ObservableObject {
        private object _currentView;
        public RelayCommand MoveWindowsCommand { get; set; }
        public RelayCommand ShutdownWindowsCommand { get; set; }
        public RelayCommand MaximizeWindowsCommand { get; set; }
        public RelayCommand MinimizeWindowsCommand { get; set; }


        public RelayCommand ShowProtectionView { get; set; }
        public RelayCommand ShowSettingsView { get; set; }


        public object CurrentView {
            get { return _currentView; }
            set {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ProtectionViewModel ProtectionVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }



        public MainViewModel() {

            ProtectionVM = new ProtectionViewModel();
            SettingsVM = new SettingsViewModel();
            CurrentView = ProtectionVM;
            Application.Current.MainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MoveWindowsCommand = new RelayCommand(o => { Application.Current.MainWindow.DragMove(); });
            ShutdownWindowsCommand = new RelayCommand(o => { Application.Current.Shutdown(); });
            MaximizeWindowsCommand = new RelayCommand(o => {
                if (Application.Current.MainWindow.WindowState == WindowState.Maximized) {
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                }
                else {
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
                }
            });
            MinimizeWindowsCommand = new RelayCommand(o => { Application.Current.MainWindow.WindowState = WindowState.Minimized; });

            ShowProtectionView = new RelayCommand(o => { CurrentView = ProtectionVM; });
            ShowSettingsView = new RelayCommand(o => { CurrentView = SettingsVM; });
        }
    }
}
