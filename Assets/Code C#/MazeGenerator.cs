using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;
/************** GIẢI THÍCH CODE ******************/
//Các biến công khai:

//tilemap: Là Tilemap của mê cung. Tilemap chứa các ô mà bạn dùng để tạo mê cung.
//wallTile: Là tile đại diện cho các bức tường. Bạn sẽ dùng nó để nhận diện các ô tường trong Tilemap.
//maze: Là ma trận lưu trữ thông tin về mê cung. Mỗi ô trong ma trận này sẽ có giá trị 1 nếu là tường và 0 nếu là đường.
//Phương thức Awake:

//Là một phương thức Unity, được gọi khi đối tượng được tạo ra.
//Nó sẽ thiết lập Instance của MazeGenerator (để có thể truy cập từ bất kỳ nơi nào trong game).
//Sau đó gọi GenerateMazeFromTilemap() để tạo mê cung từ Tilemap và PrintMaze() để in ra ma trận mê cung giúp debug.
//Phương thức GenerateMazeFromTilemap:

//Duyệt qua tất cả các ô của Tilemap (sử dụng cellBounds để lấy phạm vi ô trong Tilemap).
//Kiểm tra từng ô là tường hay đường và gán giá trị vào ma trận maze.
//Phương thức PrintMaze:

//Dùng để in ma trận mê cung ra console Unity. Phương thức này chỉ có mục đích debug và kiểm tra tính đúng đắn của ma trận mê cung.
//Phương thức ConvertToWorldPosition:

//Chuyển một tọa độ trong ma trận (là gridPosition, kiểu Vector2Int) sang tọa độ thế giới 2D (Vector3).
//Cái này sẽ hữu ích khi bạn muốn vẽ các đối tượng hoặc kiểm tra các vị trí trong thế giới 2D của game.
//Phương thức TilemapToMazeCoordinates:

//Chuyển đổi một tọa độ thế giới (worldPosition, kiểu Vector3) thành tọa độ ma trận trong Tilemap.
//Để thực hiện điều này, nó sử dụng tilemap.WorldToCell(worldPosition) để tìm tọa độ của ô trong Tilemap, sau đó chuyển tọa độ này thành chỉ số tương ứng trong ma trận.
//Phương thức GetMaze:

//Trả về ma trận maze hiện tại, giúp các script khác có thể truy cập vào ma trận mê cung để sử dụng khi cần.
/*************************************************/
public class MazeGenerator : MonoBehaviour
{
    public static MazeGenerator Instance;            // Biến tĩnh để tham chiếu đến đối tượng MazeGenerator
    public Tilemap tilemap;                          // Tham chiếu đến Tilemap của mê cung, nơi chứa các tile
    public TileBase wallTile;                        // Tile đại diện cho các bức tường trong mê cung

    private int[,] maze;                             // Ma trận 2D dùng để lưu trữ mê cung (0 là đường, 1 là tường)

    private void Awake()
    {
        // Khi đối tượng này được tạo, Instance sẽ tham chiếu đến MazeGenerator duy nhất
        Instance = this;
        tilemap.GetComponent<TilemapRenderer>().enabled = false; //Tắt render ra cái này trên Scene
        GenerateMazeFromTilemap();                   // Gọi phương thức tạo mê cung từ Tilemap
        PrintMaze();                                 // Gọi phương thức in ra ma trận mê cung để debug
    }

    // Phương thức này duyệt qua các tile trong Tilemap để tạo ma trận mê cung
    void GenerateMazeFromTilemap()
    {
        // Lấy kích thước của Tilemap (bao gồm các ô mà Tilemap chiếm dụng)
        BoundsInt bounds = tilemap.cellBounds;

        // Khởi tạo ma trận mê cung với kích thước bằng kích thước của Tilemap
        maze = new int[bounds.size.x, bounds.size.y];

        // Duyệt qua các ô trong Tilemap để gán giá trị cho ma trận
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                // Tạo tọa độ 3D cho mỗi ô trong Tilemap (chỉ có X, Y là quan trọng)
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                // Lấy tile tại vị trí đó
                TileBase tile = tilemap.GetTile(cellPosition);

                // Kiểm tra xem tile có phải là tường hay không
                if (tile == wallTile)
                {
                    maze[x - bounds.xMin, y - bounds.yMin] = 1; // Nếu là tường, gán giá trị 1
                }
                else
                {
                    maze[x - bounds.xMin, y - bounds.yMin] = 0; // Nếu là đường, gán giá trị 0
                }
            }
        }
    }

    // Phương thức này in ra ma trận mê cung để debug (chỉ dùng trong quá trình phát triển)
    void PrintMaze()
    {
        // Duyệt qua ma trận mê cung từ dưới lên trên (do Tilemap gốc ở góc trái dưới)
        for (int y = maze.GetLength(1) - 1; y >= 0; y--)
        {
            string line = "";  // Dùng để xây dựng chuỗi biểu diễn một dòng của mê cung
            for (int x = 0; x < maze.GetLength(0); x++)
            {
                line += maze[x, y] + " ";  // Thêm giá trị của ma trận vào dòng (0 cho đường, 1 cho tường)
            }
            Debug.Log(line);  // In dòng mê cung ra console
        }
    }

    // Phương thức này chuyển tọa độ trong ma trận (Grid Position) thành tọa độ thế giới
    public Vector3 ConvertToWorldPosition(Vector2Int gridPosition)
    {
        // Trong trường hợp này, mỗi ô trong ma trận có kích thước là 1x1 trong không gian thế giới
        float worldX = gridPosition.x;  // Chuyển X trong ma trận thành tọa độ X trong không gian thế giới
        float worldY = gridPosition.y;  // Chuyển Y trong ma trận thành tọa độ Y trong không gian thế giới

        // Tạo và trả về một Vector3 đại diện cho tọa độ trong không gian thế giới
        return new Vector3(worldX, 0, worldY);
    }

    // Phương thức này chuyển tọa độ trong không gian thế giới thành tọa độ trong ma trận
    public Vector2Int TilemapToMazeCoordinates(Vector3 worldPosition)
    {
        // Chuyển tọa độ thế giới thành tọa độ của ô trong Tilemap (lưới Tilemap)
        Vector3Int tilemapCell = tilemap.WorldToCell(worldPosition);

        // Lấy phạm vi của Tilemap (cell bounds) để xác định offset
        BoundsInt bounds = tilemap.cellBounds;

        // Chuyển tọa độ ô của Tilemap thành chỉ số tương ứng trong ma trận (lấy offset của Tilemap)
        int x = tilemapCell.x - bounds.xMin;
        int y = tilemapCell.y - bounds.yMin;

        // Trả về tọa độ dưới dạng Vector2Int
        return new Vector2Int(x, y);
    }

    // Phương thức này trả về ma trận mê cung để các script khác có thể truy cập
    public int[,] GetMaze()
    {
        return maze;
    }
}
