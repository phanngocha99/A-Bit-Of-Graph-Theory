using DoAn_LyThuyetDoThi.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn_LyThuyetDoThi.XuLyYeuCau
{
    internal class YeuCau1
    {
        public static void Run(Graph g)
        {
            bool check = true;
            if (!g.IsUndirectedGraph())
            {
                Console.WriteLine("Khong thuc hien duoc tren do thi co huong");
                check = false;
            }
            if(g.IsGraphHasParallel())
            {
                Console.WriteLine("Khong thuc hien duoc tren do thi co canh boi");
                check = false;
            }
            if (g.IsGraphHasLoops())
            {
                Console.WriteLine("Khong thuc hien duoc tren do thi co canh khuyen");
                check = false;
            }
            if (check)
            {
                //mặc định nếu code chạy được vào đây thì đồ thị hợp lệ, nên các hàm bên dưới không cần kiểm tra lại
                WindmillGraph(g);
                BarbellGraph(g);
                K_PartiteGraph(g);
            }
        }
        private static void WindmillGraph(Graph g)
        {
            bool result = true;
            
            //số đỉnh của đồ thị
            int m = g.GetNumberVertex();

            //Với n cố định = 3, đồ thị cối xay gió Wd(3,n) trở thành đồ thị tình bạn Fn
            //số đỉnh m = 2n+1 => n = (m-1)/2 (với n là số lượng cánh quạt)
            int n = (m - 1) / 2;

            //số cạnh của đồ thị
            int soCanh = g.GetNumberEdge();
            if (soCanh != 3 * n) result = false;

            if(result)
            {
                List<int> danhSachDinhKhop = g.GetArticulationPointList();
                if (danhSachDinhKhop.Count != 1)
                    result = false;
                else
                {
                    int dinhKhop = danhSachDinhKhop[0];
                    int[] arrayBacCuaDinh = g.GetArrayDegree();
                    int bacDinhKhop = arrayBacCuaDinh[dinhKhop];
                    if (bacDinhKhop != m - 1)
                        result = false;
                    else
                    {
                        //duyệt các đỉnh còn lại để kiểm tra bậc
                        for (int i = 0; i < m; i++)
                        {
                            if (i != dinhKhop)
                            {
                                int bacDinhi = arrayBacCuaDinh[i];
                                if (bacDinhi != 2)
                                {
                                    result = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (result)
                Console.WriteLine($"Do thi coi xay gio: Wd(3,{n})");
            else
                Console.WriteLine("Do thi coi xay gio: Khong");
        }

        private static void BarbellGraph(Graph g)
        {
            bool result = true;
            int n = g.GetNumberVertex();

            //theo giả thuyết n = 2k => k = n/2;
            int k = n / 2;

            int soCanh = g.GetNumberEdge();
            if (soCanh != k * (k - 1) + 1) result = false;


            if (result)
            {
                List<Edge> danhSachCanhCau = g.GetBridgeList();

                if (danhSachCanhCau.Count != 1)
                    result = false;
                else
                {
                    int dinhA = danhSachCanhCau[0].a;
                    int dinhB = danhSachCanhCau[0].b;
                    int[] arrayBacCuaDinh = g.GetArrayDegree();
                    int bacDinhA = arrayBacCuaDinh[dinhA];
                    int bacDinhB = arrayBacCuaDinh[dinhB];

                    if (bacDinhA != k || bacDinhB != k) result = false;
                    else
                    {
                        //duyệt các đỉnh còn lại để kiểm tra bậc
                        for (int i = 0; i < n; i++)
                        {
                            if (i != dinhA && i != dinhB)
                            {
                                int bacDinhi = arrayBacCuaDinh[i];
                                if (bacDinhi != k-1)
                                {
                                    result = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (result)
                Console.WriteLine($"Do thi Barbell: Bac {k}");
            else
                Console.WriteLine($"Do thi Barbell: Khong");
            }

        private static void K_PartiteGraph(Graph g)
        {
            int n = g.GetNumberVertex();
            List<List<int>> DS_Ke = g.GetAdjacencyList();

            //Khởi tạo List chứa tất cả tập hợp con (group con)
            List<List<int>> groups = new List<List<int>>();

            /* Ví dụ kết quả trả về trong List như sau:
            Group 1: 0, 4, 7, 8
            Group 2: 1, 3, 6, 9
            Group 3: 2, 5
             */

            //Lưu ý: phân biệt groupS (có chữ s, số nhiều) và group
            //group là 1 phần tử con trong groupS

            //Thêm đỉnh số 0 vào group đầu tiên
            groups.Add(new List<int> { 0 });

            //duyệt từ đỉnh số 1
            for (int i = 1; i < n; i++)
            {
                //tạo biến để đếm xem đang duyệt tại group con thứ mấy trong groupS
                int count = 0;

                foreach (List<int> group in groups.ToList())
                {
                    count++; // đang duyệt ở group con thứ count

                    //đưa List về dạng Array để so sánh được với danh sách kề của đỉnh i
                    int[] groupArray = group.ToArray();

                    //kiểm tra có phần tử chung nào của DS_Ke[i] và groupArray không
                    //nếu có, có nghĩa là đỉnh i kề với group con trước đó
                    //cần tạo 1 List mới trong groups để chứa phần tử đó
                    //sử dụng .Any() để trả về dạng boolean
                    //yêu cầu thư viện : using System.Linq; (có sẵn trong IDE)
                    bool anyInBoth = DS_Ke[i].Intersect(groupArray).Any();

                    if (anyInBoth)
                    {
                        if (count < groups.Count)
                        {
                            continue;
                        }
                        else
                        {
                            groups.Add(new List<int> { i });
                        }
                    }
                    else
                    {
                        group.Add(i);
                        break;
                    }
                }
            }

            //xử lý in ra kết quả
            string result = "";
            int k = groups.Count();

            //duyệt qua groups để in ra kết quả
            foreach (var group in groups)
            {
                //Join các phần tử con trong group thành string, mỗi string cách nhau 1 dấu phẩy
                string value = "{" + string.Join(", ", group) + "}";
                result += value + " ";
            }
            Console.WriteLine($"Do thi k-phan: {k}-partite {result}");
        }
    }
}