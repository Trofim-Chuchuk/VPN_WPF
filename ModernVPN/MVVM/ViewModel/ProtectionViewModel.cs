using ModernVPN.Core;
using ModernVPN.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace ModernVPN.MVVM.ViewModel {
    public class ProtectionViewModel : ObservableObject {

        public ObservableCollection<ServerModel> Servers { get; set; }
        private string _сonectionStatus;

        public string ConectionStatus {
            get {
                return _сonectionStatus;
            }
            set {
                _сonectionStatus = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ConectComand { get; set; }
        public ProtectionViewModel() {

            Servers = new ObservableCollection<ServerModel>();
            for (int i = 0; i < 30; i++) {
                Servers.Add(new ServerModel {
                    Country = "USA"
                });
            }
            ServerBuilder();
            ConectComand = new RelayCommand(x => {
                ConectionStatus = "Подключается...";
                var process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                process.StartInfo.ArgumentList.Add($"/c rasdial MyServer vpnjantit.com 1 /phonebook:\"C:\\Users\\trofi\\OneDrive\\Рабочий стол\\впн\\ModernVPN\\VPN\\premifr1.vpnjantit.com.pbk\"");


                process.Start();
                process.WaitForExit();
                switch (process.ExitCode) {

                    case 0:
                        Debug.WriteLine("Все ок");
                        ConectionStatus = "Подключено";
                        break;
                    case 691:
                        Debug.WriteLine("691 не Все ок");
                        ConectionStatus = "не Подключено";
                        break;
                    default:
                        Debug.WriteLine($"{process.ExitCode} не Все ок");
                        ConectionStatus = $"{process.ExitCode} не Все ок";
                        break;
                }

            });

        }

        private void ServerBuilder() {
            var address = "premifr1.vpnjantit.com";
            var FolderPath = $"{Directory.GetCurrentDirectory()}/VPN";

            var PdkPath = $"{FolderPath}/{address}.pbk";

            if (!Directory.Exists(FolderPath)) {
                Directory.CreateDirectory(FolderPath);
            }

            if (File.Exists(PdkPath)) {
                MessageBox.Show("Соединение уже существует");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("[MyServer]");
            sb.AppendLine("MEDIA=rastapi");
            sb.AppendLine("Port=VPN2-0");
            sb.AppendLine("Devise=WAN Miniport (IKEv2)");
            sb.AppendLine("DEVISE=vpn ");
            sb.AppendLine($"PhoneNumber={address}");
            File.WriteAllText(PdkPath, sb.ToString());

        }
    }
}
