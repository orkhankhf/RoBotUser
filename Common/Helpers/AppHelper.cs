using NLog;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Entities.Models;
using NLogLogger = NLog.ILogger;
using System.Text;
using Common.Resources;
using System.Runtime.InteropServices;

namespace Common.Helpers
{
    public static class AppHelper
    {
        private static readonly NLogLogger Logger = LogManager.GetCurrentClassLogger();

        public static string[] LoadMessages()
        {
            return new[]
            {
                "Salam hər vaxtınız xeyir olsun!\r\n\r\nAvtomobilinizi daha tez satmaq üçün sizə özəl təklifimiz var!\r\n\r\nPremium elan xidmətimizdən faydalanaraq avtomobilinizə müştəri tapa bilərsiniz.\r\n\r\n-Saytlardakı premium elanla bizim xidmətin əsas fərqi nədir❓\r\n📌 Siz saytda premium elan qoyduğunuz zaman sayta daxil olan hər kəsə elan ana səhifədə göstərilir. Elanınızın baxış sayı artsa da, müştəri tapılmır. Çünki, sizə lazım olan hər kəsin yox məhz sizin satdığınız avtomobil markası ilə maraqlanan insanların görməsidir. Biz sosial şəbəkələrdə targeting vasitəsilə məhz sizin avtomobilinizlə maraqlanan şəxsləri hədəf seçirik!\r\n\r\nSiz avtomobilinizi satmısınız ya qalır?",
                "Azərbaycanda ilk!\r\n\r\nSizə avtomobilinizi tez satmaq üçün gözəl təklifimiz var. Məsələn siz turbo.az-ın özündə və ya məşhur səhifələrdə reklam verdiyiniz zaman, elanınızı başqa modellərlə maraqlanan və ya ümumiyyətlə texnika/avtobus ilə maraqlanan sizə uyğun müştəri olmayan insanlar görür. Boş yerə sadəcə elanın baxış sayı artır.\r\n\r\nAncaq bizim SMM/IT komandamız konkret hədəf auditoriyası müəyyən edərək paylaşım etdiyi üçün aşağıdakı kriteriyalara görə müştərilərə göstəririk:\r\n- Hansı yaş aralığında insanlar görsün?\r\n- Hansı şəhərdə yaşayan?\r\n- Ən çox hansı model avtomobillərlə maraqlanan?\r\n\r\nEndirimdən yararlanmaq üçün sizə özəl təklif:\r\nStandart+ post: 10 AZN\r\nPremium post:   13 AZN (video postu və + tiktok səhifəsində tanıtım reklamı)\r\n24 saat sonra da reklam nəticəsi sizə təqdim edilir.\r\n\r\nSizin avtomobiliniz qalır?"
            };
        }

        public static string[] LoadHashTags()
        {
            return new[]
            {
                "#avto",
                "#avtosatis",
                "#avtobazar",
                "#avtomobil",
                "#elanlari",
                "#masin",
                "#bazari",
                "#avtoal",
                "#avtoelan",
                "#avtomobilelanlari",
                "#lizinqavtomobil",
                "#ikincielmasinlar",
                "#azecars",
                "#vipcars",
                "#autoaz",
                "#autoazerbaijan",
                "#masinalanmasatmaq",
                "#avtomobilbazari",
                "#baki",
                "#avtomobilsatisi",
                "#avtoaz",
                "#azerbaycan",
                "#avtomobilalqi",
                "#vipmasinlar",
                "#avtosalon",
                "#masinbazari",
                "#avtomobillər",
                "#luxmasinlar",
                "#avtomobilseverler",
                "#masinsatisi",
                "#avtomasin",
                "#masinalqi",
                "#avtolux",
                "#avtoalmaq",
                "#avtomobilkreditlə",
                "#avtoicarə",
                "#masinlarkreditle",
                "#avtoservis",
                "#avtomobilbazari",
                "#avtomobilkredit",
                "#avtokredit",
                "#dusmeMasin",
                "#elimyandi",
                "#elimyandiAvto",
                "#elimyandiAvtomobil",
                "#tecilimasinsatis"
            };
        }

        public static string FormatPhoneNumberToCountryCode(string phoneNumber)
        {
            // Ensure the phone number starts with a zero if it's local (e.g., 0XXXXXXXXX)
            return "+994" + phoneNumber.TrimStart('0').Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
        }

        public static void OpenUrlBrowser(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // Required to open URLs
            });
        }

        public static void CloseChrome()
        {
            // Get all running processes with the name "chrome"
            var chromeProcesses = Process.GetProcessesByName(AppSettings.App.Chrome);
            foreach (var process in chromeProcesses)
            {
                try
                {
                    process.Kill(); // Close the Chrome process
                    process.WaitForExit(); // Optional: wait for the process to exit
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, string.Format(LogMessagesRes.ErrorWhileClosingChrome, ex.Message));
                }
            }
        }

        public static string ExtractNumbers(string input)
        {
            // Use regular expression to match only digits
            return Regex.Replace(input, @"[^\d]", string.Empty);
        }


        public static string FormatPhoneNumberToSpaces(string phoneNumber)
        {
            if (phoneNumber.Length != 10)
            {
                throw new ArgumentException(LogMessagesRes.PhoneNumberDigitLongError);
            }

            // Split the phone number into the desired format
            return $"{phoneNumber.Substring(0, 3)} {phoneNumber.Substring(3, 3)} {phoneNumber.Substring(6, 2)} {phoneNumber.Substring(8, 2)}";
        }

        public static string GetFirstWordUntilNonLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            int index = 0;
            while (index < input.Length && char.IsLetter(input[index]))
            {
                index++;
            }

            return input.Substring(0, index); // Return the substring until the non-letter character
        }

        public static async Task<List<string>> CollectAllCarUrls(TaskCompletionSource<bool> tcs)
        {
            var options = new ChromeOptions();
            options.AddArgument("test-type");
            options.AddArgument("--remote-debugging-port=9222");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-infobars");

            var commandTimeout = TimeSpan.FromMinutes(30);
            List<string> carUrls = new List<string>();

            try
            {
                using (var driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, commandTimeout))
                {
                    driver.Navigate().GoToUrl(AppSettings.App.TurboAzUrl);

                    // Wait for the Close Browser button to be clicked
                    await tcs.Task;

                    // Collect URLs from open tabs after the button is clicked
                    var windowHandles = driver.WindowHandles;
                    foreach (var handle in windowHandles)
                    {
                        driver.SwitchTo().Window(handle);

                        await Task.Delay(100); // Small delay to ensure proper switching

                        string url = driver.Url;

                        if (url.StartsWith(AppSettings.App.UrlStartsWith))
                            carUrls.Add(url);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format(LogMessagesRes.AnErrorOccurred, ex.Message));
            }

            return carUrls;
        }

        public static string GetActiveWindowName()
        {
            IntPtr handle = GetForegroundWindow();

            StringBuilder windowTitle = new StringBuilder(256); // Allocate buffer for the window title

            if (GetWindowText(handle, windowTitle, windowTitle.Capacity) > 0)
                return windowTitle.ToString();

            Logger.Error(LogMessagesRes.ActiveWindowNotFound);
            return "";
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    }
}
