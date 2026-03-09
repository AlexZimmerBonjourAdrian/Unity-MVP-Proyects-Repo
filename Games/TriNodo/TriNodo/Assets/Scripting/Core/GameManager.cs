using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

namespace TriNodo.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Prefabs")]
        public GameObject nodePrefab;
        public GameObject linePrefab;

        [Header("Configuración")]
        public int nodeCount = 12;
        public Vector2 bounds = new Vector2(10, 6);
        public float lineWidth = 0.1f;

        [Header("Turnos y Jugadores")]
        public Color player1Color = Color.cyan;
        public Color player2Color = Color.magenta;
        private int _currentPlayerIndex = 0;
        private int _remainingLines = 0;
        private int[] _scores = new int[2];

        private List<Node> _allNodes = new List<Node>();
        private List<Vector4> _allLines = new List<Vector4>(); // X,Y = A; Z,W = B
        private List<Vector4[]> _allTriangles = new List<Vector4[]>(); // Cada Vector4 es un vértice

        private Node _selectedNode;
        private Camera _mainCam;

        private void Awake()
        {
            Instance = this;
            _mainCam = Camera.main;
        }

        private void Start()
        {
            GenerateBoard();
            StartNewTurn();
        }

        private void GenerateBoard()
        {
            // Limpiar todo lo anterior
            foreach (var n in _allNodes) if (n != null) Destroy(n.gameObject);
            _allNodes.Clear();
            _allLines.Clear();
            _allTriangles.Clear();
            _scores = new int[2];
            _selectedNode = null;

            // Limpiar líneas y áreas visuales
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < nodeCount; i++)
            {
                Vector3 pos = new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y), 0);
                GameObject nodeObj = Instantiate(nodePrefab, pos, Quaternion.identity, transform);
                Node node = nodeObj.GetComponent<Node>();
                node.Id = i;
                node.name = $"Node_{i}";
                _allNodes.Add(node);
            }
            Debug.Log($"Tablero de {nodeCount} nodos generado. Puntajes reiniciados.");
        }

        private void StartNewTurn()
        {
            // Cambiado a un solo dado de 6 caras (1d6) según petición
            _remainingLines = Random.Range(1, 7);
            
            // Cambiar al siguiente jugador
            _currentPlayerIndex = (_currentPlayerIndex + 1) % 2;
            
            Debug.Log($"--- NUEVO TURNO ---");
            Debug.Log($"Jugador {_currentPlayerIndex + 1} (Color: {CurrentColor}). Dado: {_remainingLines}");
        }

        private Color CurrentColor => _currentPlayerIndex == 0 ? player1Color : player2Color;

        private void Update()
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                HandleInput();
            }
        }

        private void HandleInput()
        {
            Vector2 mousePos2D = Mouse.current.position.ReadValue();
            Vector3 worldPos = _mainCam.ScreenToWorldPoint(new Vector3(mousePos2D.x, mousePos2D.y, _mainCam.nearClipPlane));
            worldPos.z = 0;
            
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null && hit.collider.TryGetComponent<Node>(out Node clickedNode))
            {
                // REGLA: No se pueden usar nodos dentro de áreas conquistadas
                if (IsNodeCaptured(clickedNode))
                {
                    Debug.Log("Este nodo está capturado y no puede usarse.");
                    return;
                }

                if (_selectedNode == null)
                {
                    _selectedNode = clickedNode;
                    Debug.Log($"Seleccionado: {clickedNode.Id}");
                }
                else if (_selectedNode != clickedNode)
                {
                    TryConnect(_selectedNode, clickedNode);
                    _selectedNode = null;
                }
                else
                {
                    _selectedNode = null;
                }
            }
        }

        private bool IsNodeCaptured(Node node)
        {
            Vector2 p = node.transform.position;
            foreach (var tri in _allTriangles)
            {
                // Ignorar si el nodo es un vértice del triángulo
                if (p == (Vector2)tri[0] || p == (Vector2)tri[1] || p == (Vector2)tri[2]) continue;

                if (ValidationService.IsPointInTriangle(p, tri[0], tri[1], tri[2])) return true;
            }
            return false;
        }

        private void TryConnect(Node a, Node b)
        {
            if (a.Neighbors.Contains(b)) return;

            // REGLA: No se pueden cruzar líneas existentes
            Vector2 aPos = a.transform.position;
            Vector2 bPos = b.transform.position;

            foreach (var line in _allLines)
            {
                if (ValidationService.Intersects(aPos, bPos, new Vector2(line.x, line.y), new Vector2(line.z, line.w)))
                {
                    Debug.Log("No puedes cruzar una línea existente.");
                    return;
                }
            }

            // Lógica de conexión
            a.Connect(b);
            _allLines.Add(new Vector4(aPos.x, aPos.y, bPos.x, bPos.y));
            
            CreateLine(a, b, CurrentColor);
            
            // Check Triángulos y creación de área
            CheckTriangles(a, b);

            // Verificar fin de juego INMEDIATO después de cada línea
            if (CheckGameOver())
            {
                Invoke("RestartGame", 2.0f);
                return;
            }

            _remainingLines--;
            Debug.Log($"Líneas restantes: {_remainingLines}");

            if (_remainingLines <= 0) 
            {
                StartNewTurn();
            }
        }

        private bool CheckGameOver()
        {
            // El juego termina si no quedan movimientos válidos
            // Para este MVP, verificamos si existe AL MENOS una conexión posible que no cruce líneas
            for (int i = 0; i < _allNodes.Count; i++)
            {
                for (int j = i + 1; j < _allNodes.Count; j++)
                {
                    Node a = _allNodes[i];
                    Node b = _allNodes[j];

                    if (a.Neighbors.Contains(b)) continue;
                    if (IsNodeCaptured(a) || IsNodeCaptured(b)) continue;

                    bool crosses = false;
                    foreach (var line in _allLines)
                    {
                        if (ValidationService.Intersects(a.transform.position, b.transform.position, 
                            new Vector2(line.x, line.y), new Vector2(line.z, line.w)))
                        {
                            crosses = true;
                            break;
                        }
                    }
                    
                    if (!crosses) return false; // Todavía hay al menos una jugada legal
                }
            }
            
            Debug.Log("--- JUEGO TERMINADO ---");
            string winner = _scores[0] > _scores[1] ? "Jugador 1" : (_scores[1] > _scores[0] ? "Jugador 2" : "Empate");
            Debug.Log($"El ganador es: {winner}");
            return true;
        }

        private void RestartGame()
        {
            Debug.Log("Reiniciando partida...");
            GenerateBoard();
            _currentPlayerIndex = 0;
            StartNewTurn();
        }

        private void CreateLine(Node a, Node b, Color color)
        {
            if (linePrefab == null) return;
            GameObject lineObj = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, transform);
            LineRenderer lr = lineObj.GetComponent<LineRenderer>();
            
            // Aplicar grosor del inspector
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            
            lr.startColor = lr.endColor = color;
            lr.SetPosition(0, a.transform.position);
            lr.SetPosition(1, b.transform.position);
        }

        private void CheckTriangles(Node a, Node b)
        {
            // Busca vecinos comunes para cerrar triángulos
            int found = 0;
            foreach (Node neighbor in a.Neighbors)
            {
                if (neighbor != a && neighbor != b && b.Neighbors.Contains(neighbor))
                {
                    found++;
                    Debug.Log($"¡TRIÁNGULO DETECTADO! Jugador {_currentPlayerIndex + 1} reclama el área.");
                    
                    Vector3 v1 = a.transform.position;
                    Vector3 v2 = b.transform.position;
                    Vector3 v3 = neighbor.transform.position;
                    
                    // Registrar el triángulo para bloquear nodos interiores
                    _allTriangles.Add(new Vector4[] { v1, v2, v3 });
                    
                    CreateTriangleArea(v1, v2, v3, CurrentColor);
                }
            }
            _scores[_currentPlayerIndex] += found;
            if (found > 0) Debug.Log($"Puntajes: P1: {_scores[0]} | P2: {_scores[1]}");
        }

        private void CreateTriangleArea(Vector3 v1, Vector3 v2, Vector3 v3, Color color)
        {
            // Crear un objeto para representar el área reclamada
            GameObject areaObj = new GameObject("TriangleArea");
            areaObj.transform.SetParent(transform);
            
            MeshFilter mf = areaObj.AddComponent<MeshFilter>();
            MeshRenderer mr = areaObj.AddComponent<MeshRenderer>();

            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[] { v1, v2, v3 };
            mesh.triangles = new int[] { 0, 1, 2 };
            mesh.RecalculateNormals();

            mf.mesh = mesh;
            
            // Usar un material simple (puedes asignar uno por defecto en el inspector si prefieres)
            mr.material = new Material(Shader.Find("Sprites/Default"));
            color.a = 0.3f; // Hacer el área semi-transparente
            mr.material.color = color;
        }
    }
}
