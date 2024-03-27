using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ServiceProcess;
using System.Timers;

namespace VeriketAppService
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer;
        // Kod tekrarına düşmemek için sabit bir değişken oluşturuyoruz
        private string logData;

        public Service1()
        {
            InitializeComponent();
            logData = $"{DateTime.Now}, {Environment.MachineName}, {GetLoggedInUser()}";
        }

        protected override void OnStart(string[] args)
        {
            // Timerı başlatıyoruz
            timer = new Timer();
            WriteToFile("Servis Başlatıldı: " + logData);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; // 5 saniyede bir çalışacak
            timer.Enabled = true;
            OnElapsedTime(null, null); // Servis başladığında ilk logu yaz
        }

        protected override void OnStop()
        {
            // Timerı durduruyoruz
            WriteToFile("Servis Durduruldu");
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            // Timer çalıştığında şu anki zamanı, makine adını ve kullanıcı adını loga yazıyoruz
            WriteToFile("Servis Çalışıyor: " + logData);
        }

        public void WriteToFile(string Message)
        {
            // Dosyaya yazma işlemi
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = Path.Combine(path, "VeriketAppTest_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt");

            // Dosya varsa sonuna ekleme yapıyoruz
            using (StreamWriter sw = File.AppendText(filepath))
            {
                sw.WriteLine(Message);
            }
        }

        private string GetLoggedInUser()
        {
            IntPtr buffer;
            int sessionId = -1; // -1 oturum açık olan kullanıcıyı almak için kullanılır
            int bytesReturned;

            if (WTSQuerySessionInformation(IntPtr.Zero, sessionId, WTS_INFO_CLASS.WTSUserName, out buffer, out bytesReturned))
            {
                string loggedInUser = Marshal.PtrToStringAnsi(buffer);
                WTSFreeMemory(buffer);
                return loggedInUser;
            }
            else
            {
                return "Kullanıcı Bulunamadı";
            }
        }

        [DllImport("WTSAPI32.DLL", SetLastError = true)]
        static extern bool WTSQuerySessionInformation(
            IntPtr hServer,
            int sessionId,
            WTS_INFO_CLASS wtsInfoClass,
            out IntPtr ppBuffer,
            out int pBytesReturned);

        [DllImport("WTSAPI32.DLL", SetLastError = true)]
        static extern void WTSFreeMemory(IntPtr pointer);

        enum WTS_INFO_CLASS
        {
            WTSUserName = 5
        }
    }
}
