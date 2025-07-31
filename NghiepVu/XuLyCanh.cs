using DoAn_LyThuyetDoThi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_LyThuyetDoThi.NghiepVu
{
    internal class XuLyCanh
    {
        // Tạo cạnh khi biết đỉnh đầu và đỉnh cuối
        public static Canh TaoCanh(DoThi doThi, int dinhDau, int dinhCuoi)
        {
            // Tìm trọng số
            int trongSo = -1;
            for (var i = 0; i < doThi.MangDanhSachKe.Count; i++)
            {
                DanhSachKe danhSachKe = doThi.MangDanhSachKe[i];
                if (danhSachKe.DinhDau == dinhDau)
                {
                    for (var j = 0; j < danhSachKe.DanhSachDinhKe.Count; j++)
                    {
                        if (danhSachKe.DanhSachDinhKe[j] == dinhCuoi)
                        {
                            trongSo = danhSachKe.DanhSachTrongSo[j];
                        }
                    }
                }
            }

            // Trả kết quả
            Canh canh;
            canh.DinhDau = dinhDau;
            canh.DinhCuoi = dinhCuoi;
            canh.TrongSo = trongSo;
            return canh;
        }

        // Tạo Danh sách cạnh vô hướng từ mảng danh sách kề
        public static List<Canh> TaoDanhSachCanhVoHuong(List<DanhSachKe> mangDanhSachKe)
        {
            List<Canh> danhSachCanh = new List<Canh>();

            for (var i = 0; i < mangDanhSachKe.Count; i++)
            {
                DanhSachKe danhSachKe = mangDanhSachKe[i];
                int dinhDau = danhSachKe.DinhDau;
                for (var j = 0; j < danhSachKe.DanhSachDinhKe.Count; j++)
                {
                    int dinhCuoi = danhSachKe.DanhSachDinhKe[j];
                    int trongSo = danhSachKe.DanhSachTrongSo[j];
                    // Tạo cạnh và cập nhật vào danh sách cạnh
                    Canh canh, canhDoi;

                    canh.DinhDau = dinhDau;
                    canh.DinhCuoi = dinhCuoi;
                    canh.TrongSo = trongSo;

                    canhDoi.DinhDau = dinhCuoi;
                    canhDoi.DinhCuoi = dinhDau;
                    canhDoi.TrongSo = trongSo;

                    // điều kiện để không bị lặp cạnh
                    if (!danhSachCanh.Contains(canhDoi))
                    {
                        danhSachCanh.Add(canh);
                    }
                }
            }

            return danhSachCanh;
        }

        // Kiểm tra cạnh cầu trong đồ thị vô hướng
        public static bool KiemTraCanhCau(DoThi doThi, Canh canh)
        {
            int soTPLTBanDau, soTPLTSauXoa;

            // Số thành phần liên thông ban đầu
            soTPLTBanDau = XuLyDoThi.DemSoThanhPhanLienThong(doThi);

            // Xóa cạnh
            DoThi doThiSauXoaCanh = XuLyDoThi.XoaCanhTrongDoThiVoHuong(doThi, canh);

            // Số thành phân liên thông sau khi xóa
            soTPLTSauXoa = XuLyDoThi.DemSoThanhPhanLienThong(doThiSauXoaCanh);

            if (soTPLTSauXoa > soTPLTBanDau)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
    }
}
