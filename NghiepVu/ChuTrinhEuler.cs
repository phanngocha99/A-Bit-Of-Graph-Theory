using DoAn_LyThuyetDoThi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_LyThuyetDoThi.NghiepVu
{
    internal class ChuTrinhEuler
    {
        // Kiểm tra tính chất đồ thị Euler
        public static string TinhChatEuler(DoThi doThi)
        {
            // Tìm số lượng đỉnh bậc chẵn, lẻ
            int soLuongDinhBacChan = 0;
            int soLuongDinhBacLe = 0;
            foreach (var danhSachKe in doThi.MangDanhSachKe)
            {
                int bacCuaDinh = 0;
                int dinh = danhSachKe.DinhDau;
                // Tính bậc của đỉnh
                foreach (int x in danhSachKe.DanhSachDinhKe)
                {
                    if (x == dinh)
                    {
                        bacCuaDinh += 2;
                    }
                    else
                    {
                        bacCuaDinh += 1;
                    }
                }

                // Thêm đỉnh bậc chẵn hoặc lẻ
                if (bacCuaDinh % 2 == 0)
                {
                    soLuongDinhBacChan += 1;
                }
                else
                {
                    soLuongDinhBacLe += 1;
                }
            }

            // Tính chất đồ thị
            if (soLuongDinhBacChan == doThi.MangDanhSachKe.Count)
            {
                return "Do thi Euler";
            } else if (soLuongDinhBacLe <= 2)
            {
                return "Do thi nua Euler";
            } else
            {
                return "Do thi khong Euler";
            }
        }

        // Tìm chu trình hoặc đường đi Euler bằng thuật toán Fleury
        // Dùng cho đồ thị Euler (doThiEuler == true) hoặc đồ thị nửa Euler (doThiEuler == false)
        public static List<Canh>? ThuatToanFleuryTimChuTrinhHoacDuongDiEuler(DoThi doThi, bool doThiEuler)
        {
            List<Canh> danhSachCanhDaDuyet = new List<Canh>();// Danh sách cạnh đã duyệt

            // Tìm tồng số cạnh vô hướng
            List<Canh> danhSachCanhVoHuong = XuLyCanh.TaoDanhSachCanhVoHuong(doThi.MangDanhSachKe);
            int soLuongCanh = danhSachCanhVoHuong.Count;

            // Bước 1: chọn đỉnh u tùy ý để bắt đầu
            int soLuongDinh = doThi.MangDanhSachKe.Count;
            Console.WriteLine($"Vui long nhap dinh source (tu 0 den {soLuongDinh - 1})");
            string? chuoiU = Console.ReadLine();
            int u = -1;
            int.TryParse( chuoiU, out u);
            if (u == -1 || (u < 0 && u > (soLuongDinh - 1)))
            {
                Console.WriteLine("Dinh source khong chinh xac, vui long thu lai sau");
                return null;
            }

            // Bước 2-3-4-5: chọn 1 cạnh để đi tiếp, chỉ chọn cạnh cầu khi không còn lựa chọn khác
            DeQuythuatToanFleuryTimChuTrinhHoacDuongDiEuler(doThi, u, null, ref danhSachCanhDaDuyet);

            // Kết quả
            if (danhSachCanhDaDuyet.Count == soLuongCanh)
            {
                Canh canhCuoi = danhSachCanhDaDuyet.Last<Canh>(); 
                if (doThiEuler)
                {
                    if (canhCuoi.DinhCuoi == u)
                    {
                        return danhSachCanhDaDuyet; // Đồ thị Euler
                    }
                    else
                    {
                        Console.WriteLine("Khong co loi giai");
                        return null;
                    }
                } else
                {
                    return danhSachCanhDaDuyet; // Đồ thị nửa Euler
                }
            } else
            {
                Console.WriteLine("Khong co loi giai");
                return null;
            }
        }

        // Đệ quy thuật toán Fleury tìm chu trình Euler
        private static void DeQuythuatToanFleuryTimChuTrinhHoacDuongDiEuler(DoThi doThi, int diemSource, Canh? canhDuyet, ref List<Canh> danhSachCanhDaDuyet)
        {
            // Duyệt cạnh
            if (canhDuyet != null)
            {
                danhSachCanhDaDuyet.Add(canhDuyet.Value);
            }

            // Đỉnh đầu
            int u = -1;
            if (danhSachCanhDaDuyet.Count == 0)
            {
                u = diemSource;
            }else
            {
                if (canhDuyet != null)
                {
                    u = canhDuyet.Value.DinhCuoi;
                }
            }

            // Tìm cạnh đi tiếp

            Canh? canhDiTiep = null;

            if ( u == -1)
            {
                return;
            }

            List<int> danhSachDinhKe = doThi.MangDanhSachKe[u].DanhSachDinhKe;
            List<Canh> danhSachCanhCau = new List<Canh>();
            for (int i = 0; i < danhSachDinhKe.Count; i++)
            {
                Canh canh;
                canh.DinhDau = doThi.MangDanhSachKe[u].DinhDau;
                canh.DinhCuoi = danhSachDinhKe[i];
                canh.TrongSo = doThi.MangDanhSachKe[u].DanhSachTrongSo[i];

                Canh canhDoi;
                canhDoi.DinhDau = danhSachDinhKe[i];
                canhDoi.DinhCuoi = doThi.MangDanhSachKe[u].DinhDau;
                canhDoi.TrongSo = doThi.MangDanhSachKe[u].DanhSachTrongSo[i];

                bool canhCau = XuLyCanh.KiemTraCanhCau(doThi, canh);
                if (!canhCau)
                {
                    // Kiểm tra xem cạnh này đã từng đi qua chưa (bao gồm cạnh đối)
                    if (!danhSachCanhDaDuyet.Contains(canh) && !danhSachCanhDaDuyet.Contains(canhDoi))
                    {
                        canhDiTiep = canh;
                    }
                }
                else
                {
                    danhSachCanhCau.Add(canh);
                }
            }

            // Chọn cạnh cầu nều không có lựa chọn khác
            if (canhDiTiep == null)
            {
                foreach (var canhCau in danhSachCanhCau)
                {
                    Canh canhDoi;
                    canhDoi.DinhDau = canhCau.DinhCuoi;
                    canhDoi.DinhCuoi = canhCau.DinhDau;
                    canhDoi.TrongSo = canhCau.TrongSo;
                    // Kiểm tra xem cạnh này đã từng đi qua chưa (bao gồm cạnh đối)
                    if (!danhSachCanhDaDuyet.Contains(canhCau) && !danhSachCanhDaDuyet.Contains(canhDoi))
                    {
                        canhDiTiep = canhCau;
                    }
                }
            }

            // Đệ quy
            // Tiếp tục đệ quy
            if (canhDiTiep != null)
            {
                DeQuythuatToanFleuryTimChuTrinhHoacDuongDiEuler(doThi, diemSource, canhDiTiep, ref danhSachCanhDaDuyet);
            }
        }
    }
}
