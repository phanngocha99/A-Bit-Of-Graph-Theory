using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_LyThuyetDoThi.Entity
{
    struct Edge
    {
        public int a; //đỉnh đầu
        public int b; //đỉnh cuối
        public int w; //trọng số
    }
    internal class Graph
    {
        private string filePath; //đường dẫn file
        private int n; //số đỉnh
        private List<List<int>> listConvert; // dùng cho các hàm tạo ma trận kề, danh sách kề, danh sách cạnh
        private int soCanh;// số cạnh
        private int[] SoDinhKe; // số đỉnh kề
        private int[] degree; //bậc của đỉnh
        private int[,] MT_Ke; // ma trận kề
        private int[,] MT_TrongSo; // ma trận kề
        private List<Edge> DS_Canh; // danh sách cạnh
        private List<List<int>> DS_Ke; // danh sách (đỉnh) kề
        private List<List<Edge>> DS_CanhKe; // danh sách CẠNH kề
        private List<int> articulationPointList; // Danh sách đỉnh khớp
        private List<Edge> bridgeList; // Danh sách cạnh cầu

        private void SetFilePath(string filePath)
        {
            this.filePath = filePath;
        }



        //-----Hàm tạo------------------------------------------------------------------
        //Tham số đường dẫn được truyền trực tiếp khi khởi tạo Graph, hàm tạo sẽ tự động gọi đến hàm SetFilePath
        public Graph(string filePath)
        {
            SetFilePath(filePath);

            if (ConvertFileToList())
            {
                //Khởi tạo theo thứ tự sau
                CheckNumberAdjacentVertices(); //khởi tạo mảng Số đỉnh kề
                Init(); // Khởi tạo ma trận kề, ma trận trọng số, danh sách (đỉnh) kề, danh sách cạnh kề
                CheckEdgeList(); //Khởi tạo danh sách cạnh
                CheckDegree(); //Khởi tạo mảng bậc của đỉnh (đồ thị vô hướng) & đếm số lượng cạnh khuyên
            }
            else
            {
                Console.WriteLine("File Error !!!");
            }
        }
        
        
        
        //-----Getter-------------------------------------------------------------------

        public int GetNumberVertex()
        {
            return n;
        }

        //Lấy mảng số lượng các đỉnh kề (là cột đầu tiên trong file input)
        public int[] GetArrayNumberAdjacentVertices()
        {
            return SoDinhKe;
        }

        //Lấy mảng bậc của đỉnh
        //Nếu bậc của đỉnh thứ i==0 thì đỉnh đó là đỉnh cô lập
        public int[] GetArrayDegree()
        {
            return degree;
        }

        //Lấy được số lượng đỉnh kề của 1 đỉnh bất kỳ được truyền qua tham số
        public int GetNumberAdjacentVertices(int vertex)
        {
            if (vertex >= 0 && vertex < n)
                return SoDinhKe[vertex];
            else
            {
                throw new ArgumentException("Tham so truyen vao khong hop le");
            }
        }
        public int GetNumberEdge()
        {
            if (IsUndirectedGraph())
                soCanh = degree.Sum() / 2; //định lý bắt tay (đồ thị vô hướng): tổng số bậc = 2 x số cạnh
            else
                soCanh = SoDinhKe.Sum();

            return soCanh;
        }
        public int[,] GetAdjacencyMatrix(bool showWeight = false)
        {
            if (showWeight)
                return MT_TrongSo;
            else
                return MT_Ke;
        }
        public List<Edge> GetEdgeList()
        {
            return DS_Canh;
        }
        public List<List<int>> GetAdjacencyList()
        {
            return DS_Ke;
        }
        public List<List<Edge>> GetAdjacencyEdgeList()
        {
            return DS_CanhKe;
        }
        //---Lấy ra số thành phần liên thông của đồ thị
        //nếu kết quả >1 thì đồ thị không liên thông
        //nếu kết quả = 0 thì đồ thị có hướng
        public int GetNumberConnectedComponents(List<List<int>> sourceDS_Ke, bool showEachComponent = false)
        {
            int count = 0;

            //khởi tạo mảng có n phần từ, với giá trị mặc định là false
            bool[] visited = Enumerable.Repeat(false, n).ToArray();

            for (int i = 0; i < n; i++)
            {
                if (!visited[i])
                {
                    count++;
                    List<int> dfsFromI = new List<int>();
                    
                    // nếu muốn in ra các đỉnh thuộc cùng 1 thành phần liên thông
                    //Việc in ra danh sách chỉ phục vụ mục đích kiểm tra khi code, không được sử dụng trong các yêu cầu
                    if (showEachComponent)
                    {
                        DFSRecursive(n, sourceDS_Ke, visited, i, dfsFromI); //ở đây gọi DFS đệ quy (nếu gọi DFS(i) thì mảng visited sẽ bị reset => sai)
                        Console.Write($"Thanh phan lien thong thu {count}: ");
                        ShowList(dfsFromI);
                        Console.WriteLine();
                    }
                    else DFSRecursive(n, sourceDS_Ke, visited, i, dfsFromI);
                }
            }
            return count;
        }


        //---Liệt kê các đỉnh khớp vào 1 List, nếu trả về List rỗng thì đó là đồ thị song liên thông (là đồ thị không có đỉnh khớp)
        //---Nếu cần đếm số lượng đỉnh khớp, thì lấy chiều dài của List trả về
        public List<int> GetArticulationPointList()
        {
            articulationPointList = new List<int>();

            if (IsUndirectedGraph())
            {
                //số thành phần liên thông ban đầu
                int numberConnectedComponents = GetNumberConnectedComponents(DS_Ke);

                // duyệt qua 1 đỉnh và thử loại bỏ đỉnh đó, rồi đếm lại số thành phần liên thông
                for (int i = 0; i < n; i++)
                {
                    // reset danh sách thăm đỉnh
                    bool[] visited = Enumerable.Repeat(false, n).ToArray();

                    // giả sử muốn loại bỏ đỉnh i thì chỉ cần không duyệt qua đỉnh đó
                    visited[i] = true;

                    int count = 0; // đếm lại số thành phần liên thông sau khi loại bỏ đỉnh i
                    for (int j = 0; j < n; j++)
                    {
                        if (!visited[j])
                        {
                            count++;
                            List<int> dfsFromJ = new List<int>();
                            DFSRecursive(n, DS_Ke, visited, j, dfsFromJ); //ở đây gọi DFS đệ quy (nếu gọi DFS(j) thì mảng visited sẽ bị reset => sai)
                        }
                    }
                    
                    // nếu đồ thị tăng số thành phần liên thông, thì đỉnh i chính là đỉnh khớp
                    if (count > numberConnectedComponents)
                    {
                        articulationPointList.Add(i);
                    }
                }
            }
            else
                Console.WriteLine("Ham khong ho tro tren do thi co huong");

            return articulationPointList;
        }

        //---Liệt kê các cạnh cầu vào 1 List, nếu trả về List rỗng nếu đồ thị không có cạnh cầu
        //---Nếu cần đếm số lượng cạnh cầu, thì lấy chiều dài của List trả về
        public List<Edge> GetBridgeList()
        {
            bridgeList = new List<Edge>();

            if (IsUndirectedGraph())
            {
                int numberConnectedComponents = GetNumberConnectedComponents(DS_Ke);
                Edge edge = new Edge();
                bool[] visited;

                for (int i = 0; i < DS_Canh.Count; i++)
                {
                    //tìm cạnh cầu không dùng trọng số
                    int x = DS_Canh[i].a;
                    int y = DS_Canh[i].b;

                    edge.a = x; edge.b = y;

                    // reset danh sách thăm đỉnh
                    visited = Enumerable.Repeat(false, n).ToArray();

                    int count = 0; // đếm lại số thành phần liên thông sau khi loại bỏ đỉnh i
                    for (int j = 0; j < n; j++)
                    {
                        if (!visited[j])
                        {
                            count++;
                            DFSEdge(DS_Ke, visited, j, x, y); //gọi DFS cạnh
                        }
                    }
                    // nếu đồ thị tăng số thành phần liên thông, thì cạnh i chính là cạnh cầu
                    if (count > numberConnectedComponents)
                    {
                        bridgeList.Add(edge);
                    }
                }
            }
            else
                Console.WriteLine("Ham khong ho tro tren do thi co huong");
            
            return bridgeList;
        }
        // DFS duyệt cạnh - hàm sử dụng cho việc kiểm tra cạnh cầu, không cần lưu các bước kết quả như duyệt đỉnh
        private void DFSEdge(List<List<int>> DS_Ke, bool[] visited, int u, int a, int b) // truyền vào cạnh ab
        {
            visited[u] = true;

            foreach (int v in DS_Ke[u])
            {
                if ((u == a && v == b) || (u == b && v == a)) //đỉnh u và v trùng với 2 đỉnh của cạnh đang xét thì bỏ qua
                    continue;
                if (!visited[v])
                {
                    DFSEdge(DS_Ke, visited, v, a, b);
                }
            }

        }

        //-----Các hàm void - xem file, các danh sách, các ma trận...------------------------------------

        public void ShowAllFile()
        {
            foreach (List<int> list in listConvert)
            {
                foreach (int i in list)
                {
                    Console.Write(i + " ");
                }
                Console.WriteLine();
            }
        }
        public void ShowAdjacencyMatrix(bool showWeight = false)
        {
            int[,] MT = new int[n, n];

            if (showWeight)
                MT = MT_TrongSo;
            else
                MT = MT_Ke;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(MT[i, j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
        public void ShowEdgeList(bool showWeight = false)
        {
            foreach (Edge edge in DS_Canh)
            {
                if (showWeight)
                    Console.Write($"{edge.a} - {edge.b} : {edge.w}\n");
                else
                    Console.Write($"{edge.a} {edge.b}\n");
            }
        }
        public void ShowAdjacencyList()
        {
            foreach (List<int> i in DS_Ke)
            {
                Console.Write($"{DS_Ke.IndexOf(i)} : ");
                Console.WriteLine(string.Join(", ", i));
            }
        }
        public void ShowAdjacencyEdgeList()
        {
            foreach (List<Edge> i in DS_CanhKe)
            {
                Console.Write($"{DS_CanhKe.IndexOf(i)} : ");
                foreach (Edge edge in i)
                {
                    Console.Write($"({edge.a} - {edge.b} : {edge.w}) ");
                }
                Console.WriteLine();
            }
        }
        public void ShowList(List<int> list) // Truyền vào 1 list các số nguyên
        {
            if (list.Count != 0)
            {
                Console.Write(string.Join(", ", list));
            }
        }



        //-----Các hàm Boolean - kiểm tra thuộc tính đồ thị------------------------------------

        //Kiểm tra đồ thị vô hướng (cần phải có ma trận kề để kiểm tra)
        public bool IsUndirectedGraph()
        {
            bool hasLoops = IsGraphHasLoops();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    //kiểm tra đường chéo chính, nếu đồ thị có cạnh khuyên thì không kiểm tra đường chéo chính
                    if (i == j && !hasLoops) 
                    {
                        if (MT_Ke[i, i] != 0) return false;
                    }
                    else
                    {
                        //kiểm tra ma trận đối xứng
                        if (MT_Ke[i, j] != MT_Ke[j, i]) return false;
                    }
                }
            }
            return true;
        }
        
        //kiểm tra đồ thị có cạnh khuyên hay không (cần có ma trận kề để kiểm tra, kiểm tra đường chéo chính có giá trị nào khác 0 hay không)
        public bool IsGraphHasLoops() // trả về true nếu tồn tại cạnh khuyên
        {
            for (int i = 0; i < n; i++)
            {
                if (MT_Ke[i, i] != 0) return true;
            }
            return false;
        }

        //Kiểm tra đồ thị có tồn tại trọng số âm hay không?
        public bool IsWeightNegative()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (MT_TrongSo[i, j] < 0) return true;
                }
            }
            return false;
        }

        //Kiểm tra đồ thị có cạnh cạnh bội không
        //kiểm tra trên DS kề, nếu trong cùng 1 mảng con có 2 số giống nhau thì có cạnh bội
        public bool IsGraphHasParallel() // trả về true nếu tồn tại cạnh song song
        {
            //duyệt qua từng mảng con trong list
            for (int i = 0; i < n; i++)
            {
                //nếu chỉ có 1 hoặc 0 phần từ trong 1 mảng con thì sẽ không có cạnh bội
                if (DS_Ke[i].Count > 1)
                {
                    //kiểm  tra trùng lặp trong từng mảng con
                    //chạy từ 0 đến phần tử kế cuối
                    for (int j = 0; j < DS_Ke[i].Count - 1; j++)
                    {
                        //chạy từ 1 đến cuối
                        for (int k = j + 1; k < DS_Ke[i].Count; k++)
                        {
                            if (DS_Ke[i][j] == DS_Ke[i][k])
                                return true;
                        }
                    }
                }
            }
            return false;
        }



        //-----Các hàm Private - khởi tạo------------------------------------

        //kiểm tra đường dẫn file, chuyển file input thành list & khởi tạo luôn n (số lượng đỉnh)
        private bool ConvertFileToList()
        {
            //kiểm tra đường dẫn file
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File khong ton tai!");
                return false;
            }

            //đọc dòng thứ nhất để lấy số lượng đỉnh
            StreamReader file = new StreamReader(filePath);
            string? line = file.ReadLine();

            if (int.TryParse(line, out n))
            {
                //đã lấy thành công giá trị n
                if (n <= 2)
                {
                    Console.WriteLine("So luong dinh n phai lon hon 2");
                    file.Close();
                    return false;
                }
            }
            else
            {
                Console.WriteLine("File input: co ky tu khong phai la so nguyen");
                file.Close();
                return false;
            }

            //đọc các dòng tiếp theo và đưa vào listConvert
            string[] arrayLine;
            int num;
            listConvert = new List<List<int>>();

            for (int i=0; i<n; i++)
            {
                line = file.ReadLine();
                arrayLine = line.Split(" ");

                if(arrayLine.Length % 2 != 0)
                {
                    //Có thể dùng listConvert[i] = arrayLine.ToList();
                    //nhưng không kiểm tra bên trong được, nên cần duyệt hết các phần tử để kiểm tra

                    // khởi tạo list
                    listConvert.Add(new List<int>());
                    foreach (string s in arrayLine)
                    {
                        if (int.TryParse(s, out num))
                        {
                            listConvert[i].Add(num);
                        }
                        else
                        {
                            Console.WriteLine("File input: co ky tu khong phai la so nguyen");
                            file.Close();
                            return false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"File input: co so luong phan tu tren dong {i+1} sai");
                    file.Close();
                    return false;
                }
                
            }
            file.Close();
            return true;
        }

        //khởi tạo mảng SoDinhKe từ cột đầu tiên trong file input
        private void CheckNumberAdjacentVertices()
        {
            SoDinhKe = new int[n];
            for (int i = 0; i < n; i++)
            {
                SoDinhKe[i] = listConvert[i][0];
            }
        }

        //--- Khởi tạo Ma trận kề & Ma trận trọng số
        //--- Khởi tạo Danh sách đỉnh kề
        //--- Khởi tạo Danh sách CẠNH kề (dùng cho yêu cầu 4 có sử dụng trọng số)
        private void Init()
        {
            MT_Ke = new int[n,n];
            MT_TrongSo = new int[n,n];

            DS_Ke = new List<List<int>>();

            DS_CanhKe = new List<List<Edge>>();
            Edge edge = new Edge();

            for (int i=0; i<n; i++) // duyệt các dòng trong file
            {
                int numAdjVer = GetNumberAdjacentVertices(i);

                DS_Ke.Add(new List<int>()); // vẫn khởi tạo 1 list rỗng nếu không có đỉnh kề
                DS_CanhKe.Add(new List<Edge>()); // vẫn khởi tạo 1 list rỗng nếu không có cạnh kề

                if (numAdjVer > 0)
                {
                    int dinhKe, trongSo;
                    for (int j = 1; j <= 2*numAdjVer; j+=2)
                    {
                        //index lẻ là tên đỉnh, index sau nó là trọng số
                        dinhKe = listConvert[i][j];
                        trongSo = listConvert[i][j + 1];
                        
                        //phần của ma trận kề & ma trận trọng số
                        MT_Ke[i, dinhKe] = 1;
                        MT_TrongSo[i, dinhKe] = trongSo;

                        //phần của DS kề
                        DS_Ke[i].Add(listConvert[i][j]);

                        //phần của DS cạnh kề
                        edge.a = i;
                        edge.b = listConvert[i][j];
                        edge.w = listConvert[i][j + 1];
                        DS_CanhKe[i].Add(edge);
                    }
                }
            }
        }
        
        //--- Khởi tạo Danh sách cạnh
        //cần phải có ma trận kề để kiểm tra đồ thị vô hướng nên không viết gộp với hàm Init
        private void CheckEdgeList()
        {
            DS_Canh = new List<Edge>();
            Edge edge = new Edge();
            for (int i = 0; i < n; i++) // duyệt các dòng trong listConvert
            {
                int numAdjVer = GetNumberAdjacentVertices(i);

                if (numAdjVer > 0)
                {
                    for (int k = 1; k < 2 * numAdjVer; k += 2) // duyệt trên 1 dòng
                    {
                        int vertex = listConvert[i][k];
                        int weight = listConvert[i][k + 1];
                        //Nếu là đồ thị vô hướng thì 1 cạnh bắt đầu từ đỉnh có số nhỏ hơn
                        //VD cạnh 0-3, 1-2, không tính cạnh 3-0, 2-1
                        if (IsUndirectedGraph())
                        {
                            if (i <= vertex)
                            {
                                edge.a = i;
                                edge.b = vertex;
                                edge.w = weight;
                                DS_Canh.Add(edge);
                            }
                        }
                        else
                        {
                            edge.a = i;
                            edge.b = vertex;
                            edge.w = weight;
                            DS_Canh.Add(edge);
                        }
                    }
                }
            }
        }

        //--- Khởi tạo mảng bậc của đỉnh
        //sử dụng DS kề + kiểm tra đồ thị vô hướng nên không viết gộp với hàm Init
        private void CheckDegree()
        {
            degree = new int[n];
            if(IsUndirectedGraph())
            {
                for (int i = 0; i < n; i++)
                {
                    foreach (int j in DS_Ke[i])
                    {
                        //cạnh khuyên thì bậc tính bằng 2
                        if (i == j) degree[i] += 2;
                        else degree[i] += 1;
                    }
                }
            }
        }

        //--- start: DFS
        //DFS khởi tạo các biến & mảng cần dùng, ở Main sẽ sử dụng trực tiếp hàm này, trả về 1 list các bước duyệt DFS
        public List<int> DFS(int u) // gọi u là đỉnh bắt đầu duyệt
        {
            //khởi tạo list để lưu kết quả DFS
            List<int> resultDFS = new List<int>();
            
            //khởi tạo mảng có n phần từ, với giá trị mặc định là false
            bool[] visited = Enumerable.Repeat(false, n).ToArray();

            //gọi DFS đệ quy
            DFSRecursive(n, DS_Ke, visited, u, resultDFS);

            return resultDFS;
        }

        //DFS thực hiện đệ quy (duyệt đỉnh)
        public void DFSRecursive(int n, List<List<int>> DS_Ke, bool[] visited, int u, List<int> resultDFS)
        {
            visited[u] = true;

            resultDFS.Add(u); // lưu đỉnh được thăm vào mảng kết quả
            foreach (int v in DS_Ke[u])
            {
                if (!visited[v])
                {
                    DFSRecursive(n, DS_Ke, visited, v, resultDFS);
                }
            }
        }
        //--- end: DFS     
    }
    //---end class
}
