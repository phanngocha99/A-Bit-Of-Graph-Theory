using DoAn_LyThuyetDoThi.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_LyThuyetDoThi.XuLyYeuCau
{
    internal class YeuCau2
    {
        public static void Run(Graph g)
        {
            bool check = true;
            if (g.IsUndirectedGraph())
            {
                Console.WriteLine("Khong thuc hien duoc tren do thi vo huong");
                check = false;
            }
            if (g.IsGraphHasParallel())
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
                int n = g.GetNumberVertex();
                List<List<int>> DS_Ke = g.GetAdjacencyList();

                //Tạo DS kề của đồ thị vô hướng nền UUG (Underlying Undirected Graph)
                //Ban đầu nó bằng DS_Ke, sau đó mới duyệt và thêm các đỉnh khác vào
                //không được dùng dấu bằng như này:  List<List<int>> DS_KeUUG = DS_Ke;
                //vì nó sẽ lấy địa chỉ vùng nhớ của nhau, khiến cho DS_Ke bị biến đổi theo DS_KeUUG
                //thay vào đó chỉ nên dùng vòng lặp copy giá trị
                List<List<int>> DS_KeUUG = new List<List<int>>();

                //DS kề của đồ thị ngược ReverseGraph (là đồ thị lật ngược các cung)
                List<List<int>> DS_KeRG = new List<List<int>>();
                
                //khởi tạo ngăn xếp, object được thêm vào cuối cùng sẽ được lấy ra đầu tiên
                Stack<int> stack = new Stack<int>();
                
                //Tạo list để chứa các đỉnh mà DFS(u) có thể duyệt tới
                List<List<int>> resultDFS1 = new List<List<int>>();

                // Đẩy kết quả DFS từng đỉnh vào
                // nhân tiện khởi tạo số lượng các List rỗng cho các list trên
                for (int i = 0; i < n; i++)
                {
                    DS_KeRG.Add(new List<int>());
                    DS_KeUUG.Add(new List<int>());
                    resultDFS1.Add(g.DFS(i));
                }

                //gọi hàm để thiết lập DS_KeUUG, DS_KeRG
                SetAdjList_UUG_and_RG(DS_Ke, DS_KeUUG, DS_KeRG);

                //liệt kê các thành phần liên thông mạnh
                List<List<int>> SCClist = new List<List<int>>();
                GetListSCC(n, DS_Ke, DS_KeRG, stack, SCClist);

                //Kiểm tra đồ thị thuộc loại nào và in ra màn hình
                Console.WriteLine(GetGraphType(g, n, DS_KeUUG, SCClist, resultDFS1));
                Console.WriteLine();

                //In ra các thành phần liên thông mạnh
                ShowSCCList(SCClist);
            }
        }

        //thiết lập DS_KeUUG, DS_KeRG
        public static void SetAdjList_UUG_and_RG(List<List<int>> DS_Ke, List<List<int>> DS_KeUUG, List<List<int>> DS_KeRG)
        {
            int n = DS_Ke.Count;

            for (int i = 0; i < n; i++)
            {
                foreach (int j in DS_Ke[i])
                {
                    DS_KeUUG[i].Add(j);

                    //hiện tại đỉnh 0 đã có trong DS_Ke của 1 thì cũng phải đẩy đỉnh 1 vào DS_Ke của 0
                    //kiểm tra trong list đã tồn lại đỉnh i chưa, nếu chưa mới thêm
                    if (!DS_KeUUG[j].Contains(i))
                    {
                        DS_KeUUG[j].Add(i);
                        //Đồng thời cũng tạo được DS kề của đồ thị ngược
                        DS_KeRG[j].Add(i);
                    }
                }
                //sắp xếp lại danh sách các đỉnh trong từng list con theo thứ tự tăng dần
                DS_KeUUG[i].Sort();
                DS_KeRG[i].Sort();
            }
        }

        //duyệt DFS trên đồ thị gốc để tạo được thứ tự trong stack
        private static void InitializeStack(List<List<int>> DS_Ke, bool[] visited, int u, Stack<int> stack)
        {
            visited[u] = true;

            foreach (int v in DS_Ke[u])
            {
                if (!visited[v])
                {
                    InitializeStack(DS_Ke, visited, v, stack);
                }
            }
            //đỉnh u này không phải là đỉnh ban đầu được truyền vào
            //đỉnh nào được gọi đệ quy sau cùng thì đẩy nó vào stack
            //Console.Write(u + " ");
            stack.Push(u);
        }

        //Duyệt DFS trên đồ thị ngược và trả ra kết quả các thành phần liên thông mạnh
        private static void DfsOnRG(List<List<int>> DS_KeRG, bool[] visited, int u, List<int> SCClist)
        {
            visited[u] = true;
            
            SCClist.Add(u);

            foreach (int v in DS_KeRG[u])
            {
                if (!visited[v])
                {
                    DfsOnRG(DS_KeRG, visited, v, SCClist);
                }
            }
        }

        //Hàm chính để gọi đến, hàm chạy xong thì sẽ có được danh sách các thành phần liên thông mạnh
        private static void GetListSCC(int n, List<List<int>> DS_Ke, List<List<int>> DS_KeRG, Stack<int> stack, List<List<int>> SCClist)
        {
            //khởi tạo mảng đánh dấu các đỉnh được thăm hay chưa
            bool[] visited = Enumerable.Repeat(false, n).ToArray();

            //duyệt DFS trên đồ thị gốc để tạo được thứ tự stack
            for (int i=0; i < n; i++)
            {
                if (!visited[i])
                {
                    InitializeStack(DS_Ke,visited, i, stack);
                }
            }

            //reset lại các đỉnh về trạng thái chưa được thăm
            visited = Enumerable.Repeat(false, n).ToArray();
            int count = 0;
            while (stack.Count != 0)
            {
                int u = stack.Pop();
                if (!visited[u])
                {
                    SCClist.Add(new List<int>());
                    DfsOnRG(DS_KeRG,visited, u, SCClist[count]);
                    count++;
                }
            }
        }

        private static string GetGraphType(Graph g, int n, List<List<int>> DS_KeUUG, List<List<int>> SCClist, List<List<int>> resultDFS1)
        {
            //Nếu UUG không liên thông
            //mượn hàm kiểm tra số thành phần liên thông của đồ thị G để kiểm tra số lượng thành phần liên thông của UUG
            if (g.GetNumberConnectedComponents(DS_KeUUG) != 1)
            {
                return "Do thi khong lien thong";
            }
            else
            {
                if (SCClist.Count == 1)
                {
                    return "Do thi lien thong manh";
                }
                else if (SCClist.Count > 1)
                {
                    for (int i = 0; i < n - 1; i++)
                    {
                        for (int j = i + 1; j < n; j++) //chỉ cần duyệt tịnh tiến, các cặp xét rồi thì không xét lại
                        {
                            //Nếu tồn tại 1 cặp đỉnh bất kỳ mà không có đường đi (i không đến được j và j cũng không đến được i)
                            if (!resultDFS1[i].Contains(j) && !resultDFS1[j].Contains(i))
                            {
                                //Console.WriteLine($"{i}-{j} khong co duong di");
                                return "Do thi lien thong yeu";
                            }
                        }
                    }
                }
                return "Do thi lien thong tung phan";
            }
        }

        private static void ShowSCCList(List<List<int>> SCClist)
        {
            int count = 0;
            //trước khi in ra thì sort lại theo thứ tự tăng dần như trong đề bài
            SCClist = SCClist.OrderBy(list => list.First()).ToList();

            foreach (List<int> scc in SCClist)
            {
                scc.Sort();
                count++;
                Console.Write($"Thanh phan lien thong manh {count}: ");
                Console.WriteLine(string.Join(", ", scc));
            }
        }

        /*
        Nếu không dùng arrow function thì có thể viết tường minh phương thức so sánh thông qua 2 hàm như sau
        
        Gọi: SortListByFirstValue(SCClist); ở trong ShowSCCList

        //sắp xếp dựa vào index đầu tiên
        private static void SortListByFirstValue(List<List<int>> list)
        {
            //hàm Sort truyền vào 1 phương thức so sánh, 
            list.Sort((a, b) => a[0].CompareTo(b[0]));
        }

        private static int CompareListsByFirstValue(List<int> a, List<int> b)
        {
            //dùng CompareTo để so sánh 2 giá trị, trả về số dương nếu a>b, ấm nếu a<b, =0 nếu chúng bằng nhau
            return a[0].CompareTo(b[0]);
        }
        */


        /*
        //Chuyển Ma trận kề (không trọng số) của đồ thị có hướng G thành ma trận kề của đồ thị vô hướng nền UUG (Underlying Undirected Graph)
        //Get Adjacency Matrix of Underlying Undirected Graph
        public int[,] GetAdjMatrixOfUUG()
        {
            if (CheckAdjacencyMatrix() && !IsUndirectedGraph())
            {
                MT_KeUUG = new int[n,n];
                for(int i = 0; i < n; i++)
                {
                    for(int j = 0;j < n; j++)
                    {
                        if (MT_Ke[i,j] != 0)
                        {
                            MT_KeUUG[i,j] = MT_Ke[i,j];
                            MT_KeUUG[j,i] = MT_Ke[i,j];
                        }
                    }
                }
                return MT_KeUUG;
            }
            else
                return new int[0,0];
        }
        
        */
    }
}
