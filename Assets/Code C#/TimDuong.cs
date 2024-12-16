using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Unity.Burst.Intrinsics.X86.Avx;
/******************************* GIAI THICH CODE *****************/
//Giải thích chi tiết về các phần trong code:
//Lớp PriorityQueueComparer:

//Lớp này dùng để so sánh các phần tử trong SortedSet (priority queue). Các phần tử cần có dạng (int, Vector2Int), trong đó int là trọng số (distance) và Vector2Int là tọa độ.
//So sánh chủ yếu theo trọng số (int), nếu trọng số bằng nhau, so sánh theo tọa độ x và y.
//Các biến công khai:

//startTransform và endTransform: Là vị trí bắt đầu và kết thúc của đường đi, được gán từ ngoài (từ các đối tượng trong Unity).
//pathTilemap: Tilemap nơi vẽ đường đi.
//pathTile: Tile đại diện cho đường đi.
//Phương thức TimDuongThoatMeCung():

//Chuyển đổi vị trí start và end từ không gian thế giới (world space) sang không gian ô (tilemap cell space).
//Gọi hàm Dijkstra để tính toán đường đi ngắn nhất, sau đó gọi DrawPathOnTilemap để vẽ đường đi lên Tilemap.
//Phương thức Dijkstra:

//Cài đặt thuật toán Dijkstra để tìm đường đi ngắn nhất.
//Dijkstra sử dụng một bảng distances lưu trữ khoảng cách từ điểm bắt đầu đến tất cả các điểm khác. Dùng previous để lưu trữ các điểm trước mỗi điểm trong đường đi.
//Sử dụng priority queue để luôn lấy điểm có khoảng cách ngắn nhất để xử lý tiếp.
//Phương thức GetNeighbors:

//Trả về các ô lân cận của một điểm theo 4 hướng di chuyển (lên, xuống, trái, phải).
//Phương thức DrawPathOnTilemap:

//Dùng để vẽ đường đi lên Tilemap. Đảm bảo Tilemap được đặt tại vị trí (0, 0) trước khi vẽ. Sau khi vẽ xong, nó sẽ dời Tilemap về vị trí ban đầu của maze.
/****************************************************************/
public class TimDuong : MonoBehaviour
{
    // Lớp PriorityQueueComparer dùng để so sánh các phần tử trong priority queue
    // kế thừa lớp IComparere với Kiểu dữ liệu là tuple int, vector2int
    public class PriorityQueueComparer : IComparer<(int, Vector2Int)>
    {
        public int Compare((int, Vector2Int) x, (int, Vector2Int) y)
        {
            // So sánh theo trọng số (int) trước
            int comparison = x.Item1.CompareTo(y.Item1);

            // Nếu trọng số bằng nhau, so sánh theo Vector2Int ( ví dụ là 2 ô kề nhau có trọng số là 1)
            if (comparison == 0)
            {
                comparison = x.Item2.x.CompareTo(y.Item2.x);
                if (comparison == 0)
                {
                    comparison = x.Item2.y.CompareTo(y.Item2.y);
                }
            }

            return comparison;
        }
    }

    public static TimDuong Instance;   // Biến tĩnh tham chiếu đến đối tượng TimDuong duy nhất
    private List<Vector2Int> path;     // Danh sách chứa đường đi ngắn nhất
    public Transform startTransform;    // Vị trí bắt đầu
    public Transform endTransform;      // Vị trí kết thúc
    public Tilemap pathTilemap;         // Tilemap dùng để vẽ đường đi
    public TileBase pathTile;           // TileBase đại diện cho đường đi

    private void Awake()
    {
        Instance = this;   // Gán Instance cho đối tượng TimDuong
        pathTilemap.ClearAllTiles(); // Xóa sprite

    }

    // Phương thức gọi thuật toán Dijkstra để tìm đường
    public void TimDuongThoatMeCung()
    {
        // Chuyển start và end từ thế giới (world space) sang tọa độ ô trong Tilemap (cell space)
        Vector2Int start = MazeGenerator.Instance.TilemapToMazeCoordinates(startTransform.position);
        Vector2Int end = MazeGenerator.Instance.TilemapToMazeCoordinates(endTransform.position);
        Debug.Log(start);
        Debug.Log(end);

        // Gọi phương thức tìm đường đi ngắn nhất bằng dijkstra
        path = Dijkstra(start, end);

        // Vẽ đường đi trên Tilemap
        DrawPathOnTilemap(path);

        // Nếu không tìm được đường, in cảnh báo
        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("No path found!");
        }
        else
        {
            // In đường đi tìm được (dành cho debug)
            Debug.Log("Path found: " + string.Join(", ", path.Select(p => p.ToString()).ToArray()));
        }
    }

    // Phương thức chính thực hiện thuật toán Dijkstra để tìm đường
    List<Vector2Int> Dijkstra(Vector2Int start, Vector2Int end)
    {
        int rows = MazeGenerator.Instance.GetMaze().GetLength(0);
        int cols = MazeGenerator.Instance.GetMaze().GetLength(1);

        // Khởi tạo các mảng hỗ trợ: distances, previous và visited
        // trong c# thì [,] là mảng 2 chiều :))))
        int[,] distances = new int[rows, cols];
        Vector2Int[,] previous = new Vector2Int[rows, cols];
        bool[,] visited = new bool[rows, cols];

        // Gán giá trị mặc định cho các ô
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                distances[i, j] = int.MaxValue;  // Đặt khoảng cách ban đầu là vô hạn
                previous[i, j] = new Vector2Int(-1, -1);  // Chưa có ô trước đó
                visited[i, j] = false;  // Chưa thăm ô nào
            }
        }

        // Thiết lập ô bắt đầu
        distances[start.x, start.y] = 0;

        // Sử dụng PriorityQueue (SortedSet) để thực hiện thuật toán Dijkstra
        var priorityQueue = new SortedSet<(int, Vector2Int)>(new PriorityQueueComparer());
        //new PriorityQueueComparer()
        //Phần này là đối tượng để so sánh các phần tử trong SortedSet.
        //Theo mặc định, nếu bạn không cung cấp một trình so sánh(IComparer), C# sẽ dùng cách sắp xếp mặc định của kiểu dữ liệu.
        //Nhưng vì phần tử trong SortedSet ở đây là tuple(int, Vector2Int), bạn cần chỉ định cách so sánh tùy chỉnh.

        //Là một đối tượng của lớp PriorityQueueComparer.
        //Lớp này được thiết kế để thực hiện việc so sánh hai tuple(int, Vector2Int) dựa trên trọng số(int) và tọa độ(Vector2Int) nếu cần.
        //SortedSet là một tập hợp có thứ tự(sorted collection) trong C#.
        //Các phần tử của nó được sắp xếp theo một trật tự cụ thể.
        //Mỗi phần tử là duy nhất(không có phần tử trùng lặp).
        //Trật tự được quyết định bởi so sánh mặc định(nếu kiểu dữ liệu có sẵn so sánh như int hoặc string)
        //hoặc bởi một hàm so sánh tùy chỉnh(ví dụ PriorityQueueComparer trong đoạn mã này).

        // (int, Vector2Int) là 1 kiểu dữ liệu tuple trong c# mà ko cần tạo struct
        priorityQueue.Add((0, start));

        // Thuật toán Dijkstra
        while (priorityQueue.Count > 0)
        {
            // Lấy phần tử có trọng số nhỏ nhất
            var current = priorityQueue.Min;
            // Cái này giống đánh dấu * khi làm trên giấy, cái nào dấu * là nhỏ nhất và bỏ đi
            priorityQueue.Remove(current);

            Vector2Int currentNode = current.Item2; // Node hiện tại là phần PriorityQueueComparer() chứa trọng số và Position đỉnh

            // Nếu đã thăm ô này rồi, bỏ qua
            if (visited[currentNode.x, currentNode.y]) continue;

            visited[currentNode.x, currentNode.y] = true;

            // Nếu đến ô đích, dừng thuật toán
            if (currentNode == end)
                break;

            // Kiểm tra các ô lân cận của ô hiện tại
            foreach (var neighbor in GetNeighbors(currentNode, rows, cols))
            {
                // Nếu ô là tường hoặc đã thăm, bỏ qua
                if (MazeGenerator.Instance.GetMaze()[neighbor.x, neighbor.y] == 1 || visited[neighbor.x, neighbor.y])
                    continue;

                int newDistance = distances[currentNode.x, currentNode.y] + 1;  // Trọng số là 1

                // Nếu khoảng cách tìm được nhỏ hơn, cập nhật
                if (newDistance < distances[neighbor.x, neighbor.y]) // Bình thường thì neighbor sẽ là vô cực giống mấy bước đầu khi làm trên giấy
                {
                    distances[neighbor.x, neighbor.y] = newDistance;
                    previous[neighbor.x, neighbor.y] = currentNode;
                    priorityQueue.Add((newDistance, neighbor));
                }
            }
        }

        // Truy ngược đường đi từ ô đích về ô bắt đầu
        List<Vector2Int> path = new List<Vector2Int>();
        for (Vector2Int at = end; at != new Vector2Int(-1, -1); at = previous[at.x, at.y])
        {
            path.Add(at);
        }
        path.Reverse();  // Đảo lại danh sách để có đường đi từ start đến end

        // Nếu không tìm thấy đường (ô đầu tiên không phải ô start)
        if (path.Count == 0 || path[0] != start)
            return null;

        return path;  // Trả về đường đi ngắn nhất
    }

    // Phương thức lấy các ô lân cận của ô hiện tại (4 hướng: lên, xuống, trái, phải)
    List<Vector2Int> GetNeighbors(Vector2Int node, int rows, int cols)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Các hướng di chuyển: lên, xuống, trái, phải
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0)
        };

        // Thêm các ô lân cận vào danh sách nếu hợp lệ
        foreach (var dir in directions)
        {
            Vector2Int neighbor = node + dir;
            if (neighbor.x >= 0 && neighbor.x < rows && neighbor.y >= 0 && neighbor.y < cols)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    // Phương thức vẽ đường đi trên Tilemap
    private void DrawPathOnTilemap(List<Vector2Int> path)
    {
        // Đảm bảo Tilemap được đặt tại vị trí gốc (0,0)
        pathTilemap.transform.position = Vector3.zero;

        // Kiểm tra xem Tilemap và Tile có được gán hay chưa
        if (pathTilemap == null || pathTile == null)
        {
            Debug.LogError("Tilemap or pathTile is not assigned!");
            return;
        }

        // Xóa tất cả các tile hiện tại trong Tilemap
        pathTilemap.ClearAllTiles();

        // Vẽ các tile cho đường đi
        foreach (var point in path)
        {
            // Tính toán lại vị trí của tile trong Tilemap
            Vector3Int tilePosition = new Vector3Int(point.x, point.y, 0);
            // Đặt tile tại vị trí đường đi
            pathTilemap.SetTile(tilePosition, pathTile);
        }

        // Tìm vị trí của ô ở góc trái dưới của Tilemap
        Vector3Int bottomLeftCell = MazeGenerator.Instance.tilemap.cellBounds.min;
        Vector3 bottomLeftWorld = MazeGenerator.Instance.tilemap.CellToWorld(bottomLeftCell); // Chuyển sang tọa độ thế giới
        Debug.Log(bottomLeftWorld);

        // Dời đường đi về tọa độ 0,0,0 để vẽ
        pathTilemap.transform.position = bottomLeftWorld;
    }
}
