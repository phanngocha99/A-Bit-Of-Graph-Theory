using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DoAn_LyThuyetDoThi.Entity;

namespace DoAn_LyThuyetDoThi.LuuTru
{
    internal class LuuTruDoThi
    {
        public static DoThi? DocDoThi(string filePath)
        {
            DoThi doThi;
            string chuoiLoi = "";// Dùng để ghi nhận lỗi

            // Đọc file
            if (File.Exists(filePath) == false)
            {
                Console.WriteLine("File khong ton tai");
                return null;
            };
            StreamReader file = new StreamReader(filePath);

            // Đọc số lượng đỉnh từ dòng đầu tiên
            int soLuongDinh = -1;
            string? dongDauTien = file.ReadLine();
            if (dongDauTien != null)
            {
                int.TryParse(dongDauTien, out soLuongDinh);
            } else
            {
                Console.WriteLine("Khong doc duoc dong dau tien.");
                return null;
            }

            // Thêm số lượng đỉnh vào đồ thị
            if (soLuongDinh != -1)
            {
                doThi.SoLuongDinh = soLuongDinh;
            }else
            {
                Console.WriteLine("Khong doc duoc so luong dinh");
                return null;
            }

            // Đọc và chuyển các dòng còn lại vào mảng Danh Sách Kề
            DanhSachKe[] mangDanhSachKe = new DanhSachKe[soLuongDinh];
            for (int i = 0; i < soLuongDinh; i++)
            {
                // Tạo danh sách kề và thêm đỉnh đầu
                DanhSachKe danhSachKe;
                danhSachKe.DinhDau = i;

                // Đọc nội dung dòng
                string? line = file.ReadLine();
                if (line != null)
                {
                    string[] mang = line.Split(" ");

                    // Đọc số lượng đỉnh kề
                    int soLuongDinhKe = -1;
                    int.TryParse(mang[0], out soLuongDinhKe);
                    if (soLuongDinhKe == -1)
                    {
                        chuoiLoi += $"Loi o dong {i + 2}";
                        break;
                    }

                    // Kiểm tra cấu trúc có chính xác hay không
                    if ( (mang.Length % 2 != 1) || (soLuongDinhKe * 2 + 1 != mang.Length))
                    {
                        chuoiLoi += $"Loi o dong {i + 2}";
                        break;
                    } 

                    // Đọc các đỉnh kề và trọng số
                    int[] danhSachDinhKe = new int[soLuongDinhKe];
                    int[] danhSachTrongSo = new int[soLuongDinhKe];

                    for (int j = 0; j < soLuongDinhKe; j++)
                    {
                        int dinhKe = -1;
                        int trongSo = -1;
                        int.TryParse(mang[2*j + 1], out dinhKe);
                        int.TryParse(mang[2*j + 2], out trongSo);
                        if (dinhKe == -1 || trongSo == -1)
                        {
                            chuoiLoi += $"Loi o dong {i + 2}";
                            break;
                        } else
                        {
                            danhSachDinhKe[j] = dinhKe;
                            danhSachTrongSo[j] = trongSo;
                        }
                    }

                    // Thêm danh sách đỉnh kề và danh sách trọng số vào Danh Sách Kề
                    danhSachKe.DanhSachDinhKe = danhSachDinhKe.ToList();
                    danhSachKe.DanhSachTrongSo = danhSachTrongSo.ToList();
                    mangDanhSachKe[i] = danhSachKe;
                }
            }

            // Báo lỗi
            if (chuoiLoi != string.Empty)
            {
                Console.WriteLine(chuoiLoi);
                return null;
            }

            // Thêm Mảng Danh Sách Kề vào Đồ Thị
            doThi.MangDanhSachKe = mangDanhSachKe.ToList();

            // Trả kết quả
            return doThi;
        }
    }
}
