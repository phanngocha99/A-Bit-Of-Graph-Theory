using DoAn_LyThuyetDoThi.Entity;
using DoAn_LyThuyetDoThi.XuLyYeuCau;
using System.Text;

do
{
    Console.Clear();

    Console.WriteLine("\nDo an - Ly Thuyet Do Thi");
    Console.WriteLine("---------------------------------");
    Console.WriteLine("Yeu cau 1: Nhan dang mot so dang do thi dac biet");
    Console.WriteLine("Yeu cau 2: Xac dinh thanh phan lien thong manh");
    Console.WriteLine("Yeu cau 3a: Tim cay khung lon nhat (Prim)");
    Console.WriteLine("Yeu cau 3b: Tim cay khung lon nhat (Kruskal)");
    Console.WriteLine("Yeu cau 4: Tim duong di ngan nhat");
    Console.WriteLine("Yeu cau 5: Tim chu trinh hoac duong di Euler");
    Console.WriteLine("\nNhan phim 0: Thoat chuong trinh");
    Console.WriteLine("----------------------");
    Console.Write("Lua chon yeu cau: ");

    string option = Console.ReadLine();
    
    switch (option)
    {
        case "1":
            Console.WriteLine("\nYeu cau 1: Nhan dang mot so dang do thi dac biet\n");
            Graph g1 = new Graph("C:\\MT1B.txt");
            //nhớ sửa lại đường path đúng trong file gửi cô
            YeuCau1.Run(g1);
            break;
        case "2":
            Console.WriteLine("\nYeu cau 2: Xac dinh thanh phan lien thong manh\n");
            Graph g2 = new Graph("C:\\MT2C.txt");
            YeuCau2.Run(g2);
            break;
        case "3a":
            Console.WriteLine("\nYeu cau 3a: Tim cay khung lon nhat (Prim)\n");
            YeuCau3a.Run();
            break;
        case "3b":
            Console.WriteLine("\nYeu cau 3b: Tim cay khung lon nhat (Kruskal)\n");
            YeuCau3b.Run();
            break;
        case "4":
            Console.WriteLine("\nYeu cau 4: Tim duong di ngan nhat\n");
            YeuCau4.Run();
            break;
        case "5":
            Console.WriteLine("\nYeu cau 5: Tim chu trinh hoac duong di Euler\n");
            YeuCau5.Run();
            break;
        case "0":
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Yeu cau khong ton tai, moi chon lai...");
            break;
    }
    Console.Write("\nNhan Enter de tiep tuc... ");
    Console.ReadLine();
}
while (true);