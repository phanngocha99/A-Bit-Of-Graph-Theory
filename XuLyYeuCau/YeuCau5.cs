using DoAn_LyThuyetDoThi.Entity;
using DoAn_LyThuyetDoThi.NghiepVu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_LyThuyetDoThi.XuLyYeuCau
{
    internal class YeuCau5
    {
        public static void Run()
        {
            // Đọc đồ thị
            string filePath = "C:\\MT5A.txt";
            DoThi? doThi = XuLyDoThi.DocDoThi(filePath);
            if (doThi == null) return;

            // Kiểm tra đồ thị có vô hướng hay không
            bool doThiVoHuong = XuLyDoThi.KiemTraDoThiVoHuong(doThi.Value);
            if (doThiVoHuong == false)
            {
                Console.WriteLine("- Day la do thi co huong. Khong the kiem tra tinh chat Euler.");
                return;
            }

            // Kiểm tra đồ thị có liên thông hay không
            bool doThiLienThong = XuLyDoThi.KiemTraDoThiLienThong(doThi.Value);
            if (!doThiLienThong)
            {
                Console.WriteLine("- Day la do thi khong lien thong. Khong the kiem tra tinh chat Euler.");
                return;
            }

            // Kiểm tra tính chất Euler
            string tinhChatEuler = ChuTrinhEuler.TinhChatEuler(doThi.Value);
            Console.WriteLine(tinhChatEuler);

            // Nếu là đồ thị Euler, tiếp tục xác định chu trình Euler xuất phát từ đỉnh source.
            // Người dùng nhập đỉnh bắt đầu source (chỉ mục bắt đầu từ 0).
            if (tinhChatEuler == "Do thi Euler")
            {
                List<Canh>? chuTrinhEuler = ChuTrinhEuler.ThuatToanFleuryTimChuTrinhHoacDuongDiEuler(doThi.Value, true);
                if (chuTrinhEuler != null)
                {
                    Console.Write("Chu trinh Euler: ");
                    for (int i = 0; i < chuTrinhEuler.Count; i++)
                    {
                        if (i != (chuTrinhEuler.Count - 1))
                        {
                            Console.Write($"{chuTrinhEuler[i].DinhDau} ");
                        }else
                        {
                            Console.Write($"{chuTrinhEuler[i].DinhDau} {chuTrinhEuler[i].DinhCuoi} ");
                        }
                    }
                }
            }

            // Nếu là đồ thị nửa Euler, tiếp tục xác định đường đi Euler.
            // Người dùng nhập đỉnh bắt đầu source, chỉ mục bắt đầu từ 0
            if (tinhChatEuler == "Do thi nua Euler")
            {
                List<Canh>? duongDiEuler = ChuTrinhEuler.ThuatToanFleuryTimChuTrinhHoacDuongDiEuler(doThi.Value, false);
                if (duongDiEuler != null)
                {
                    Console.Write("Duong di Euler: ");
                    for (int i = 0; i < duongDiEuler.Count; i++)
                    {
                        if (i != (duongDiEuler.Count - 1))
                        {
                            Console.Write($"{duongDiEuler[i].DinhDau} ");
                        }
                        else
                        {
                            Console.Write($"{duongDiEuler[i].DinhDau} {duongDiEuler[i].DinhCuoi} ");
                        }
                    }
                }
            }
        }
    }
}
