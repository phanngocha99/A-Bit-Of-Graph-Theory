using DoAn_LyThuyetDoThi.Entity;
using DoAn_LyThuyetDoThi.NghiepVu;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_LyThuyetDoThi.XuLyYeuCau
{
    internal class YeuCau4
    {
        public static void Run()
        {
            // Đọc file và tạo đồ thị
            string filePath1 = "E:\\1. iamenjoying\\2023-now\\cntt\\cntt hcmus\\mon-hoc\\9.ly-thuyet-do-thi\\Thuc hanh\\DoAnMonHoc\\ThucHanhDoAn\\DoAn_LyThuyetDoThi\\SampleData\\MT4.txt";
            DoThi? doThi = XuLyDoThi.DocDoThi(filePath1);
            if (doThi == null) return;

            //Lấy ma trận trọng lượng
            int [,] MT_TrongLuong = XuLyDoThi.LayMaTranTrongLuong(doThi.Value);

            //Kiểm tra đồ thị có trọng số dương
            if (XuLyDoThi.KiemTraDoThiCoTrongSoAm(doThi.Value))
            {
                Console.WriteLine("\"- Day la do thi co trong so am. Khong the tim duong di ngan nhat theo thuat toan Floyd-Warshall");
                return;
            }

            //Tìm đường đi ngắn nhất bằng Thuật Toán Floyd-Warshall và in ra màn hình
            Console.WriteLine("Ket qua yeu cau 4: ");
            ThuatToanFloydWarshall.Run(MT_TrongLuong, doThi.Value.SoLuongDinh);

        }
    }
}

