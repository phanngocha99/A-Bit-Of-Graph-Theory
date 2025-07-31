using DoAn_LyThuyetDoThi.Entity;
using DoAn_LyThuyetDoThi.NghiepVu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_LyThuyetDoThi.XuLyYeuCau
{
    internal class YeuCau3a
    {
        public static void Run()
        {
            // Đọc file và tạo đồ thị
            string filePath = "C:\\MT3A.txt";
            DoThi? doThi = XuLyDoThi.DocDoThi(filePath);
            if (doThi == null) return;

            //XuLyDoThi.InDoThi(doThi.Value);
            //return;

            // Kiểm tra đồ thị có vô hướng hay không
            bool doThiVoHuong = XuLyDoThi.KiemTraDoThiVoHuong(doThi.Value);
            if (doThiVoHuong == false)
            {
                Console.WriteLine("- Day la do thi co huong. Khong the tim cay khung lon nhat");
                return;
            }

            // Kiểm tra đồ thị có liên thông hay không
            bool doThiLienThong = XuLyDoThi.KiemTraDoThiLienThong(doThi.Value);
            if (!doThiLienThong)
            {
                Console.WriteLine("- Day la do thi khong lien thong. Khong the tim cay khung lon nhat");
                return;
            }

            // Tìm cây khung lớn nhất theo giải thuật Prim
            GiaiThuatPrim.TimCayKhungLonNhat(doThi.Value);
        }
    }

    internal class YeuCau3b
    {
        public static void Run()
        {
            // Đọc file và tạo đồ thị
            string filePath = "C:\\MT3A.txt";
            DoThi? doThi = XuLyDoThi.DocDoThi(filePath);
            if (doThi == null) return;

            // Kiểm tra đồ thị có vô hướng hay không
            bool doThiVoHuong = XuLyDoThi.KiemTraDoThiVoHuong(doThi.Value);
            if (!doThiVoHuong)
            {
                Console.WriteLine("- Day la do thi co huong. Khong the tim cay khung lon nhat");
                return;
            }

            // Kiểm tra đồ thị có liên thông hay không
            bool doThiLienThong = XuLyDoThi.KiemTraDoThiLienThong(doThi.Value);
            if (!doThiLienThong)
            {
                Console.WriteLine("- Day la do thi khong lien thong. Khong the tim cay khung lon nhat");
                return;
            }

            // Tìm cây khung lớn nhất theo giải thuật Kruskal
            List<Canh> cayKhungLonNhat = GiaiThuatKruskal.TimCayKhungLonNhat(doThi.Value);

            // In danh sách cạnh
            Console.WriteLine("Tap canh cay khung lon nhat:");
            for (var i = 0; i < cayKhungLonNhat.Count; i++)
            {
                Console.WriteLine(cayKhungLonNhat[i].DinhDau + "-" + cayKhungLonNhat[i].DinhCuoi + ": " + cayKhungLonNhat[i].TrongSo);
            }

            // In trọng số của cây khung
            int trongSoCayKhung = 0;
            for (var i = 0; i < cayKhungLonNhat.Count; i++)
            {
                trongSoCayKhung = trongSoCayKhung + cayKhungLonNhat[i].TrongSo;
            }
            Console.Write("Trong so cay khung lon nhat: " + trongSoCayKhung);
            Console.WriteLine();

        }
    }

}
