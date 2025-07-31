using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoAn_LyThuyetDoThi.Entity;

namespace DoAn_LyThuyetDoThi.NghiepVu
{
    internal class XuLyDanhSachKe
    {
        // In chi tiết danh sách kề
        public void inChiTiet(DanhSachKe danhSackKe)
        {
            Console.Write("Dinh dau: " + danhSackKe.DinhDau + " ; Dinh cuoi: ");
            for (var i = 0; i < danhSackKe.DanhSachDinhKe.Count; i++)
            {
                Console.Write(danhSackKe.DanhSachDinhKe[i] + ", ");
            }
            Console.WriteLine();
        }
    }
}
