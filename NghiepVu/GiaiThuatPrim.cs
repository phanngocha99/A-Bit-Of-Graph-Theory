using DoAn_LyThuyetDoThi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_LyThuyetDoThi.NghiepVu
{
    internal class GiaiThuatPrim
    {

        public static void TimCayKhungLonNhat(DoThi doThi)
        {
            Console.WriteLine("Giai thuat Prim");
            // Danh sach đỉnh (v)
            List<int> v = new List<int>();
            for (var i = 0; i < doThi.MangDanhSachKe.Count; i++)
            {
                v.Add(doThi.MangDanhSachKe[i].DinhDau);
            }

            // Bước 1: chọn đỉnh bất kỳ và khởi tạo tập Y (đỉnh) và E (cạnh)
            DanhSachKe danhSachKeBatKy = doThi.MangDanhSachKe[0];
            int dinhBatKy = danhSachKeBatKy.DinhDau;
            List<int> y = new List<int>() { dinhBatKy };
            List<Canh> e = new List<Canh>();

            // Bước 2: Chọn cạnh kề có độ dài lớn nhất
            while (e.Count < (v.Count - 1))
            {
                // Tạo mảng v\y
                List<int> vSubY = new List<int>();
                for (var i = 0; i < v.Count; i++)
                {
                    if (!y.Contains(v[i]))
                    {
                        vSubY.Add(v[i]);
                    }
                }

                // Tạo danh sách cạnh {u, v} trong đó u thuộc y, v thuộc vSuby
                List<Canh> danhSachCanh = new List<Canh>();
                for (var i = 0; i < y.Count; i++)
                {
                    for (var j = 0; j < vSubY.Count; j++)
                    {
                        Canh canh = XuLyCanh.TaoCanh(doThi: doThi, dinhDau: y[i], dinhCuoi: vSubY[j]);
                        if (canh.TrongSo != -1)
                        {
                            danhSachCanh.Add(canh);
                        }
                    }
                }

                // Tìm cạnh có độ dài (trọng số) lớn nhất
                Canh canhMax = danhSachCanh[0];
                for (var i = 0; i < danhSachCanh.Count; i++)
                {
                    if (danhSachCanh[i].TrongSo > canhMax.TrongSo)
                    {
                        canhMax = danhSachCanh[i];
                    }
                }

                // Cập nhật y, e
                y.Add(canhMax.DinhCuoi);
                e.Add(canhMax);

            }

            // In danh sách cạnh
            Console.WriteLine("Tap canh cay khung lon nhat:");
            for (var i = 0; i < e.Count; i++)
            {
                Console.WriteLine(e[i].DinhDau + "-" + e[i].DinhCuoi + ": " + e[i].TrongSo);
            }

            // In trọng số của cây khung
            int trongSoCayKhung = 0;
            for (var i = 0; i < e.Count; i++)
            {
                trongSoCayKhung = trongSoCayKhung + e[i].TrongSo;
            }
            Console.Write("Trong so cay khung lon nhat: " + trongSoCayKhung);
            Console.WriteLine();
        }
    }
}
