using DoAn_LyThuyetDoThi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_LyThuyetDoThi.NghiepVu
{
    internal class ThuatToanFloydWarshall
    {
        public static void Run(int[,] graph ,int n)
        {
            //Tìm đường đi ngắn nhất bằng giải thuật Floy-Warshall
            FloydWarshall(graph, n);

            static void FloydWarshall(int[,] maTranTrongSo, int soDinh)
            {
                int[,] khoangCach = new int[soDinh, soDinh];
                int[,] p = new int[soDinh, soDinh];

                for (int i = 0; i < soDinh; ++i)
                {
                    for (int j = 0; j < soDinh; ++j)
                    {
                        khoangCach[i, j] = maTranTrongSo[i, j];
                        p[i, j] = j;
                    }
                }

                for (int k = 0; k < soDinh; ++k)
                {
                    for (int i = 0; i < soDinh; ++i)
                    {
                        for (int j = 0; j < soDinh; ++j)
                        {
                            if (khoangCach[i, k] + khoangCach[k, j] < khoangCach[i, j])
                            {
                                khoangCach[i, j] = khoangCach[i, k] + khoangCach[k, j];
                                p[i, j] = p[i, k];
                            }
                        }
                    }
                }

                //In ra màn hình kết quả
                for (int i = 0; i < soDinh; ++i)
                {
                    Console.WriteLine("Duong di xuat phat tu: " + i);

                    for (int j = 0; j < soDinh; ++j)
                    {
                        if (i != j)
                        {
                            //In ra đường đi ngắn nhất từ đình i đến đỉnh j
                            InDuongDiNganNhat(p, i, j);
                            if (khoangCach[i,j] == 1000000)
                            {
                                Console.Write(":" + "Khong co duong di");
                            }
                            else
                            {
                                Console.Write(":" + khoangCach[i, j]);
                            }
                            Console.WriteLine();
                        }

                    }
                }

                static void InDuongDiNganNhat(int[,] p, int i, int j)
                {
                    List<int> duongDi = new List<int> { i };
                    while (i != j)
                    {
                        i = p[i, j];
                        duongDi.Add(i);
                    }
                    foreach (var dinh in duongDi)
                    {
                        if (dinh != j)
                        {
                            Console.Write(dinh + "->");
                        }
                        else
                        {
                            Console.Write(dinh);
                        }
                    }
                } 
            }
        }
    }
}
