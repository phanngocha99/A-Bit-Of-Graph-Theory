using DoAn_LyThuyetDoThi.Entity;
using DoAn_LyThuyetDoThi.LuuTru;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_LyThuyetDoThi.NghiepVu
{
    internal class XuLyDoThi
    {
        // Đọc đồ thị từ file
        public static DoThi? DocDoThi(string filePath)
        {
            DoThi? doThi = LuuTruDoThi.DocDoThi(filePath);
            return doThi;
        }

        // In đồ thị
        public static void InDoThi(DoThi doThi)
        {
            Console.WriteLine($"so luong dinh: {doThi.SoLuongDinh}");
            for (int i = 0; i < doThi.MangDanhSachKe.Count; i++)
            {
                Console.WriteLine($"Dinh dau: {doThi.MangDanhSachKe[i].DinhDau}");
                if (doThi.MangDanhSachKe[i].DanhSachDinhKe != null)
                {
                    Console.Write("Dinh ke: ");
                    foreach (var dinhKe in doThi.MangDanhSachKe[i].DanhSachDinhKe)
                    {
                        Console.Write($"{dinhKe}, ");
                    }
                    Console.WriteLine();
                    Console.Write("Trong so: ");
                }
                if (doThi.MangDanhSachKe[i].DanhSachTrongSo != null)
                {
                    foreach (var trongSo in doThi.MangDanhSachKe[i].DanhSachTrongSo)
                    {
                        Console.Write($"{trongSo}, ");
                    }
                    Console.WriteLine();
                }
            }
        }

        // Kiểm tra đồ thị có vô hướng không
        public static bool KiemTraDoThiVoHuong(DoThi doThi)
        {
            // Tạo mảng cạnh từ mảng danh sách kề
            List<Canh> danhSachCanh = new List<Canh>();

            for (int i = 0; i < doThi.MangDanhSachKe.Count; i++)
            {
                DanhSachKe danhSachKe = doThi.MangDanhSachKe[i];
                for (int j = 0; j < danhSachKe.DanhSachDinhKe.Count; j++)
                {
                    Canh canh;
                    canh.DinhDau = danhSachKe.DinhDau;
                    canh.DinhCuoi = danhSachKe.DanhSachDinhKe[j];
                    canh.TrongSo = danhSachKe.DanhSachTrongSo[j];

                    danhSachCanh.Add(canh);
                }
            }

            // b1: Kiểm tra tất cả cạnh trong danh sách cạnh, tìm bất kỳ cạnh dạng (x, y) mà không có tương ứng 1 cạnh dạng (y, x) với cùng trọng số.
            // b2: Nếu có bất kỳ 1 cạnh nào thỏa điều kiện trên thì đồ thị là có hướng, ngược lại đồ thị vô hướng.
            bool voHuong = true;
            for (int i = 0; i < danhSachCanh.Count; i++)
            {
                Canh canhGoc = danhSachCanh[i];

                int count = 0;
                for (int j = 0; j < danhSachCanh.Count; j++)
                {
                    Canh canhSoSanh = danhSachCanh[j];

                    if (canhSoSanh.DinhDau == canhGoc.DinhCuoi && canhSoSanh.DinhCuoi == canhGoc.DinhDau && canhSoSanh.TrongSo == canhGoc.TrongSo)
                    {
                        count += 1;

                    }
                }
                if (count == 0)
                {
                    voHuong = false;
                    Console.WriteLine($"{canhGoc.DinhDau}, {canhGoc.DinhCuoi} ({canhGoc.TrongSo})");
                    // break;
                }

            }

            // Trả về kết quả
            return voHuong;
        }

        // Kiểm tra đồ thị có liên thông không
        public static bool KiemTraDoThiLienThong(DoThi doThi)
        {
            List<DanhSachKe> mangDanhSachKeCanKiemTra = doThi.MangDanhSachKe;
            List<DanhSachKe> mangDanhSachKeDaDuyet = new List<DanhSachKe>();
            // Duyệt qua danh sách kề của từng đỉnh
            DeQuyKiemTraDoThiLienThong(mangDanhSachKeCanKiemTra[0], doThi.MangDanhSachKe, ref mangDanhSachKeDaDuyet);

            // Kiểm tra xem có đỉnh nào chưa được duyệt không
            bool coDinhChuaDuyet = false;
            for (var i = 0; i < mangDanhSachKeCanKiemTra.Count; i++)
            {
                bool namTrongDSDuyet = false;
                for (var j = 0; j < mangDanhSachKeDaDuyet.Count; j++)
                {
                    if (mangDanhSachKeDaDuyet[j].DinhDau == mangDanhSachKeCanKiemTra[i].DinhDau)
                    {
                        namTrongDSDuyet = true;
                    }
                }
                if (namTrongDSDuyet == false)
                {
                    coDinhChuaDuyet = true;
                    break;
                }
            }

            if (coDinhChuaDuyet)
            {
                // Đồ thị không liên thông
                return false;
            }
            else
            {
                // Đồ thị liên thông
                return true;
            }
        }

        // Đệ quy kiểm tra đồ thị liên thông
        private static void DeQuyKiemTraDoThiLienThong(DanhSachKe danhSachKe, List<DanhSachKe> mangDanhSachKeCanKiemTra, ref List<DanhSachKe> mangDanhSachKeDaDuyet)
        {
            // Duyệt danhSachKe
            mangDanhSachKeDaDuyet.Add(danhSachKe);

            // Duyệt danhSachDinhKe nếu cần
            List<int> danhSachDinhKe = danhSachKe.DanhSachDinhKe;
            for (var i = 0; i < danhSachDinhKe.Count; i++)
            {
                for (var j = 0; j < mangDanhSachKeCanKiemTra.Count; j++)
                {
                    // Kiểm tra xem danhSachKe có trong mangDanhSachkeDaDuyet chưa, nếu chưa thì duyệt
                    if (mangDanhSachKeCanKiemTra[j].DinhDau == danhSachDinhKe[i] && !mangDanhSachKeDaDuyet.Contains(mangDanhSachKeCanKiemTra[j]))
                    {
                        DeQuyKiemTraDoThiLienThong(mangDanhSachKeCanKiemTra[j], mangDanhSachKeCanKiemTra, ref mangDanhSachKeDaDuyet);
                    }
                }
            }
        }

        // Xóa cạnh trong đồ thị vô hướng
        public static DoThi XoaCanhTrongDoThiVoHuong(DoThi doThi, Canh canhCanXoa)
        {
            List<DanhSachKe> mangDanhSachKeGoc = doThi.MangDanhSachKe;
            List<DanhSachKe> mangDanhSachKeMoi = mangDanhSachKeGoc.ToList();

            for (int i = 0; i < mangDanhSachKeGoc.Count; i++)
            {
                DanhSachKe danhSachKeGoc = mangDanhSachKeGoc[i];
                if (danhSachKeGoc.DinhDau == canhCanXoa.DinhDau)
                {
                    for (int j = 0; j < danhSachKeGoc.DanhSachDinhKe.Count; j++)
                    {
                        int dinhKe = danhSachKeGoc.DanhSachDinhKe[j];
                        int trongSo = danhSachKeGoc.DanhSachTrongSo[j];
                        if (dinhKe == canhCanXoa.DinhCuoi)
                        {
                            List<int> danhSachDinhKeMoi = danhSachKeGoc.DanhSachDinhKe.ToList();
                            danhSachDinhKeMoi.RemoveAt(j);
                            List<int> danhSachTrongSoMoi = danhSachKeGoc.DanhSachTrongSo.ToList();
                            danhSachTrongSoMoi.RemoveAt(j);

                            DanhSachKe danhSachKeMoi;
                            danhSachKeMoi.DinhDau = danhSachKeGoc.DinhDau;
                            danhSachKeMoi.DanhSachDinhKe = danhSachDinhKeMoi;
                            danhSachKeMoi.DanhSachTrongSo = danhSachTrongSoMoi;

                            mangDanhSachKeMoi[i] = danhSachKeMoi;
                        }
                    }
                }

                if (danhSachKeGoc.DinhDau == canhCanXoa.DinhCuoi)
                {
                    for (int j = 0; j < danhSachKeGoc.DanhSachDinhKe.Count; j++)
                    {
                        int dinhKe = danhSachKeGoc.DanhSachDinhKe[j];
                        int trongSo = danhSachKeGoc.DanhSachTrongSo[j];
                        if (dinhKe == canhCanXoa.DinhDau)
                        {
                            List<int> danhSachDinhKeMoi = danhSachKeGoc.DanhSachDinhKe.ToList();
                            danhSachDinhKeMoi.RemoveAt(j);
                            List<int> danhSachTrongSoMoi = danhSachKeGoc.DanhSachTrongSo.ToList();
                            danhSachTrongSoMoi.RemoveAt(j);

                            DanhSachKe danhSachKeMoi;
                            danhSachKeMoi.DinhDau = danhSachKeGoc.DinhDau;
                            danhSachKeMoi.DanhSachDinhKe = danhSachDinhKeMoi;
                            danhSachKeMoi.DanhSachTrongSo = danhSachTrongSoMoi;

                            mangDanhSachKeMoi[i] = danhSachKeMoi;
                        }
                    }
                }
            }

            // Trả kết quả
            DoThi doThiMoi;
            doThiMoi.SoLuongDinh = doThi.SoLuongDinh;
            doThiMoi.MangDanhSachKe = mangDanhSachKeMoi;

            return doThiMoi;
        }

        // Đếm số thành phần liên thông trong đồ thị
        public static int DemSoThanhPhanLienThong(DoThi doThi)
        {
            List<DanhSachKe> mangDanhSachKe = doThi.MangDanhSachKe;
            List<int> danhSachDinh = Enumerable.Range(0, doThi.SoLuongDinh).ToList();
            List<int> danhSachDinhDaDuyet = new List<int>();

            // Nếu chỉ có 1 đỉnh thì số tplt là 1
            //if (mangDanhSachKe.Count == 1)
            //{
            //    return 1;
            //}

            int soThanhPhanLienThong = 0;
            while (danhSachDinhDaDuyet.Count < danhSachDinh.Count)
            {
                // Chọn 1 đỉnh chưa có trong danh sách đã duyệt
                int dinh = -1;
                foreach (int x in danhSachDinh)
                {
                    if (!danhSachDinhDaDuyet.Contains(x))
                    {
                        dinh = x;
                        break;
                    }
                }

                // Đệ quy
                if (dinh != -1)
                {
                    DeQuyDemSoThanhPhanLienThong(mangDanhSachKe, dinh, ref danhSachDinhDaDuyet);
                    soThanhPhanLienThong += 1;
                }
            }

            // Trả kết quả
            return soThanhPhanLienThong;
        }

        private static void DeQuyDemSoThanhPhanLienThong(List<DanhSachKe> mangDanhSackKe, int dinhDuyet, ref List<int> danhSachDinhDaDuyet)
        {

            // Duyệt đỉnh
            danhSachDinhDaDuyet.Add(dinhDuyet);

            // Đọc danh sách đỉnh kề
            List<int> danhSachDinhKe = new List<int>();
            foreach (var danhSachKe in mangDanhSackKe)
            {
                if (danhSachKe.DinhDau == dinhDuyet)
                {
                    danhSachDinhKe = danhSachKe.DanhSachDinhKe;
                    break;
                }
            }

            // Nếu có đỉnh chưa duyệt thì đệ quy tiếp
            foreach (int dinh in danhSachDinhKe)
            {
                if (!danhSachDinhDaDuyet.Contains(dinh))
                {
                    DeQuyDemSoThanhPhanLienThong(mangDanhSackKe, dinh, ref danhSachDinhDaDuyet);
                }
            }
        }

        // Lấy ma trận trọng lượng
        public static int[,] LayMaTranTrongLuong(DoThi doThi)
        {
            var n = doThi.SoLuongDinh;
            var MT_TrongLuong = new int[n, n];

            //Những đỉnh không liên kết với nhau có trọng sô dương vô cực, ở đây set là 1000000
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    MT_TrongLuong[i, j] = 1000000;
                }
            }

            //Gán giá trị đỉnh kề và trọng số và ma trận trọng lượng
            for (int i = 0; i < doThi.MangDanhSachKe.Count; i++)
            {
                for (int j = 0; j < doThi.MangDanhSachKe[i].DanhSachDinhKe.Count; j++)
                {
                    MT_TrongLuong[i, doThi.MangDanhSachKe[i].DanhSachDinhKe[j]] = doThi.MangDanhSachKe[i].DanhSachTrongSo[j];
                }
                MT_TrongLuong[i, i] = 0;

            }


            //In Ma trận trọng lượng 
            /*for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(MT_TrongLuong[i, j] + " ");
                }
                Console.WriteLine();
            }
            */

            return MT_TrongLuong;
        }

        //Kiểm tra Đồ Thị có Trọng Số Dương hay Trọng số Âm
        public static bool KiemTraDoThiCoTrongSoAm(DoThi doThi)
        {
            var n = doThi.SoLuongDinh;
            int[,] MT_TrongLuong = LayMaTranTrongLuong(doThi);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if(MT_TrongLuong[i, j] < 0){
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
