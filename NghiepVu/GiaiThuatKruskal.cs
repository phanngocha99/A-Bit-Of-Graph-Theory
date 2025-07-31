using DoAn_LyThuyetDoThi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_LyThuyetDoThi.NghiepVu
{
    internal class GiaiThuatKruskal
    {
        public static List<Canh> TimCayKhungLonNhat(DoThi doThi)
        {
            Console.WriteLine("Giai thuat Kruskal");

            // Tạo danh sách cạnh vô hướng
            List<Canh> danhSachCanhVoHuong = XuLyCanh.TaoDanhSachCanhVoHuong(doThi.MangDanhSachKe);

            // Bước 1: sắp xếp các cạnh theo thứ tự độ dài (trọng số giảm dần)
            // và khởi tạo E
            List<Canh> danhSachCanhGiamDan = danhSachCanhVoHuong.OrderByDescending(Canh => Canh.TrongSo).ToList();

            List<Canh> E = new List<Canh>();

            // Bước 2: duyệt từng cạnh e và kiểm tra xem E hợp với e có tạo thành chu trình không, nếu không thì thêm e vào E
            for (var i = 0; i < danhSachCanhGiamDan.Count; i++)
            {
                // Bước 3: Nếu E đủ n - 1 phần tử thì dừng, ngược lại làm tiếp bước 2
                // Console.WriteLine("E count = " + E.Count);
                // Console.WriteLine("So luong dinh = " + doThi.SoLuongDinh);
                // Console.WriteLine("Duyet canh " + danhSachCanhGiamDan[i].DinhDau + " - " + danhSachCanhGiamDan[i].DinhCuoi + ": " + danhSachCanhGiamDan[i].TrongSo);
                if (E.Count == (doThi.SoLuongDinh - 1))
                {
                    break;
                }
                else
                {
                    List<Canh> ETest = E.ToList();
                    ETest.Add(danhSachCanhGiamDan[i]);
                    bool coChuTrinh = KiemTraCoChuTrinhHayKhong(ETest);
                    // Console.WriteLine("Co chu trinh: " + coChuTrinh);
                    if (coChuTrinh == false)
                    {
                        E.Add(danhSachCanhGiamDan[i]);
                    }
                }
                // Console.WriteLine("-------");
            }

            //  Trả kết quả
            return E;
        }

        private static bool KiemTraCoChuTrinhHayKhong(List<Canh> danhSachCanh)
        {
            bool coChuTrinh = false;

            // Đệ quy kiểm tra có chu trình
            for (var i = 0; i < danhSachCanh.Count; i++)
            {
                List<int> danhSachDinhDaDuyet = new List<int>();
                Canh canhDuyet = danhSachCanh[i];
                danhSachDinhDaDuyet.Add(canhDuyet.DinhDau);
                coChuTrinh = DeQuiKiemTraCoChuTrinh(canhDuyet, danhSachCanh, ref danhSachDinhDaDuyet);
                if (coChuTrinh == true)
                {
                    return coChuTrinh;
                }
            }

            // Trả kết quả
            return coChuTrinh;
        }

        private static bool DeQuiKiemTraCoChuTrinh(Canh canhDuyet, List<Canh> danhSachCanh, ref List<int> danhSachDinhDaDuyet)
        {
            bool coChuTrinh = false;

            // Kiểm tra có chu trình không
            if (danhSachDinhDaDuyet.Contains(canhDuyet.DinhCuoi))
            {
                coChuTrinh = true;
                return coChuTrinh;
            }
            else
            {
                danhSachDinhDaDuyet.Add(canhDuyet.DinhCuoi);
                // Tìm cạnh kề tiếp theo
                for (var i = 0; i < danhSachCanh.Count; i++)
                {
                    if (danhSachCanh[i].DinhDau == canhDuyet.DinhCuoi)
                    {
                        coChuTrinh = DeQuiKiemTraCoChuTrinh(danhSachCanh[i], danhSachCanh, ref danhSachDinhDaDuyet);
                        if (coChuTrinh == true)
                        {
                            return coChuTrinh;
                        }
                    }
                }
            }

            return coChuTrinh;
        }
    }
}
