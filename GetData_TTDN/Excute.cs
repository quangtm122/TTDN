using GetData_TTDN;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Test2schedule
{
    public class Excute
    {
        private readonly Timer time;

        public Excute()
        {
            time = new Timer(5000) { AutoReset = true };
            time.Elapsed += TimerElapsed;

        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            using (var httpClient = new HttpClient())
            {
                // LOGIN
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;
                HttpClient client = new HttpClient(handler);

                Uri uri = new Uri("http://192.168.68.72:8888/xac-thuc/dang-nhap?returnUrl=%2F");

                client.BaseAddress = uri;
                HttpResponseMessage response = await client.GetAsync(uri);
                string operationLocation = response.Headers.GetValues("Set-Cookie").FirstOrDefault();

                var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.45 Safari/537.36");
                    httpRequestMessage.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    httpRequestMessage.Headers.Add("Accept-Encoding", "gzip, deflate");
                    httpRequestMessage.Headers.Add("Accept-Language", "vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5");
                    httpRequestMessage.Headers.Add("Connection", "keep-alive");
                var parameters = new List<KeyValuePair<string, string>>()
                {
                };
                httpRequestMessage.Content = new System.Net.Http.FormUrlEncodedContent(parameters);


                var _response = await client.SendAsync(httpRequestMessage);


                var result = await _response.Content.ReadAsStringAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(result);

                var signupFormIdElement = doc.DocumentNode.SelectSingleNode("//input[@name='__RequestVerificationToken']");

                var signupFormId = signupFormIdElement.GetAttributeValue("value", "");

                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                client.DefaultRequestHeaders.Add("Cookie", operationLocation.Split(';')[0]);


                var httpRequestMessage_repo = new HttpRequestMessage();
                httpRequestMessage_repo.Method = HttpMethod.Post;
                var parameters_repo = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("__RequestVerificationToken",signupFormId),
                    new KeyValuePair<string, string>("Username", "admin"),
                    new KeyValuePair<string, string>("Password", "123")
                };
                httpRequestMessage_repo.Content = new System.Net.Http.FormUrlEncodedContent(parameters_repo);

                HttpContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>> {
                                                            new KeyValuePair<string, string>("__RequestVerificationToken", signupFormId),
                                                            new KeyValuePair<string,string>("Username","admin"),
                                                            new KeyValuePair<string,string>("Password","123")
                                                                    });
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                content.Headers.ContentType.CharSet = "UTF-8";
                HttpResponseMessage response_ms = await client.PostAsync(uri, content);


                var _result = await response_ms.Content.ReadAsStringAsync();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(_result);

                var thongtins = new List<ThongTin>();

                //LẤY DỮ LIỆU TỪ WEB
                var products = htmlDocument.DocumentNode.SelectNodes("//div[@class='m-widget1']");
                foreach (HtmlNode product in products)
                {
                    ThongTin tt = new ThongTin();

                    tt.MT_Name = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[1]").InnerText.Trim('\r', '\n', ' ');
                    tt.MT_HT = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[2]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.MT_CSLN = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[3]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.MT_TK = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[4]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.MT_SLN = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[5]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);

                    tt.Gio_Name = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[6]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.Gio_HT = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[7]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.Gio_CSLN = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[8]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.Gio_TK = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[9]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.Gio_SLN = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[10]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);

                    tt.SK_Name = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[11]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.SK_HT = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[12]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.SK_CSLN = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[13]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.SK_TK = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[14]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.SK_SLN = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[15]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);

                    tt.Tong_Name = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[16]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.Tong_HT = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[17]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.Tong_CSLN = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[18]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.Tong_TK = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[19]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);
                    tt.Tong_SLN = product.SelectSingleNode("(//div[contains(@class,'col')]//h3)[20]").InnerText.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace(" ", String.Empty);

           
                    /*thongtins.Add(thongtin);
                    
                    Console.Write("\n" + "Mat Troi" + ":" + "\t");
                    Console.Write(thongtin.MT_HT + ";" + "\t");
                    Console.Write(thongtin.MT_CSLN + ";" + "\t");
                    Console.Write(thongtin.MT_TK + ";" + "\t");
                    Console.Write(thongtin.MT_SLN + "\n");

                    Console.Write("Gio" + ":" + "\t\t");
                    Console.Write(thongtin.Gio_HT + ";" + "\t");
                    Console.Write(thongtin.Gio_CSLN + ";" + "\t");
                    Console.Write(thongtin.Gio_TK + ";" + "\t");
                    Console.Write(thongtin.MT_SLN + "\n");

                    Console.Write("Sinh Khoi" + ":" + "\t");
                    Console.Write(thongtin.SK_HT + ";" + "\t");
                    Console.Write(thongtin.SK_CSLN + ";" + "\t");
                    Console.Write(thongtin.SK_TK + ";" + "\t");
                    Console.Write(thongtin.SK_SLN + "\n");

                    Console.Write("Tong" + ":" + "\t\t");
                    Console.Write(thongtin.Tong_HT + ";" + "\t");
                    Console.Write(thongtin.Tong_CSLN + ";" + "\t");
                    Console.Write(thongtin.Tong_TK + ";" + "\t");
                    Console.Write(thongtin.Tong_SLN + "\n");*/
                    string[] lines = new string[] { tt.ToString() + "\n" + "Get Data At: " + DateTime.Now.ToString() };
                    File.AppendAllLines(@"C:\Window Sevice\Test1.txt", lines);

                    /* string MyConnection = "Data Source=DESKTOP-6G6474Q\\SQLEXPRESS;Initial Catalog=GetData_Web;Integrated Security=True";
                   SqlConnection conn = new SqlConnection(MyConnection);
                   conn.Open();
                   DateTime dateTimeVariable = DateTime.Now;

                   using (SqlCommand sqlCmd1 = new SqlCommand { CommandText = "INSERT INTO detail ([HienTai], [CongSuatLonNhat], [ThietKe], [SanLuongNgay], [energy_name_id], [time]) VALUES (@HienTai, @CongSuatLonNhat, @ThietKe, @SanLuongNgay, @energy_name_id, @time)", Connection = conn })
                   {
                       sqlCmd1.Parameters.AddWithValue("@HienTai", thongtin.MT_HT);
                       sqlCmd1.Parameters.AddWithValue("@CongSuatLonNhat", thongtin.MT_CSLN);
                       sqlCmd1.Parameters.AddWithValue("@ThietKe", thongtin.MT_TK);
                       sqlCmd1.Parameters.AddWithValue("@SanLuongNgay", thongtin.MT_SLN);
                       sqlCmd1.Parameters.AddWithValue("@energy_name_id", '1');
                       sqlCmd1.Parameters.AddWithValue("@time", dateTimeVariable);
                       sqlCmd1.ExecuteNonQuery();
                   }

                   using (SqlCommand sqlCmd2 = new SqlCommand { CommandText = "INSERT INTO detail ([HienTai], [CongSuatLonNhat], [ThietKe], [SanLuongNgay], [energy_name_id], [time]) VALUES (@HienTai, @CongSuatLonNhat, @ThietKe, @SanLuongNgay, @energy_name_id, @time)", Connection = conn })
                   {
                       sqlCmd2.Parameters.AddWithValue("@HienTai", thongtin.Gio_HT);
                       sqlCmd2.Parameters.AddWithValue("@CongSuatLonNhat", thongtin.Gio_CSLN);
                       sqlCmd2.Parameters.AddWithValue("@ThietKe", thongtin.Gio_TK);
                       sqlCmd2.Parameters.AddWithValue("@SanLuongNgay", thongtin.Gio_SLN);
                       sqlCmd2.Parameters.AddWithValue("@energy_name_id", '2');
                       sqlCmd2.Parameters.AddWithValue("@time", dateTimeVariable);
                       sqlCmd2.ExecuteNonQuery();
                   }

                   using (SqlCommand sqlCmd3 = new SqlCommand { CommandText = "INSERT INTO detail ([HienTai], [CongSuatLonNhat], [ThietKe], [SanLuongNgay], [energy_name_id], [time]) VALUES (@HienTai, @CongSuatLonNhat, @ThietKe, @SanLuongNgay, @energy_name_id, @time)", Connection = conn })
                   {
                       sqlCmd3.Parameters.AddWithValue("@HienTai", thongtin.SK_HT);
                       sqlCmd3.Parameters.AddWithValue("@CongSuatLonNhat", thongtin.SK_CSLN);
                       sqlCmd3.Parameters.AddWithValue("@ThietKe", thongtin.SK_TK);
                       sqlCmd3.Parameters.AddWithValue("@SanLuongNgay", thongtin.SK_SLN);
                       sqlCmd3.Parameters.AddWithValue("@energy_name_id", '3');
                       sqlCmd3.Parameters.AddWithValue("@time", dateTimeVariable);
                       sqlCmd3.ExecuteNonQuery();
                   }

                   using (SqlCommand sqlCmd4 = new SqlCommand { CommandText = "INSERT INTO detail ([HienTai], [CongSuatLonNhat], [ThietKe], [SanLuongNgay], [energy_name_id], [time]) VALUES (@HienTai, @CongSuatLonNhat, @ThietKe, @SanLuongNgay, @energy_name_id, @time)", Connection = conn })
                   {
                       sqlCmd4.Parameters.AddWithValue("@HienTai", thongtin.Tong_HT);
                       sqlCmd4.Parameters.AddWithValue("@CongSuatLonNhat", thongtin.Tong_CSLN);
                       sqlCmd4.Parameters.AddWithValue("@ThietKe", thongtin.Tong_TK);
                       sqlCmd4.Parameters.AddWithValue("@SanLuongNgay", thongtin.Tong_SLN);
                       sqlCmd4.Parameters.AddWithValue("@energy_name_id", '4');
                       sqlCmd4.Parameters.AddWithValue("@time", dateTimeVariable);
                       sqlCmd4.ExecuteNonQuery();
                   }
                   conn.Close();*/
                }
            }
        }

        public void Start()
        {
            time.Start();
        }

        public void Stop()
        {
            time.Stop();
        }
    }
}
