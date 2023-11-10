using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace WalkerScripts
{
    public class WalkerMovement : MonoBehaviour
    {
        public Vector3Int targetPosition;
        public List<Vector3Int> moveList = new List<Vector3Int>();
        public float speed = 2;
        public Vector3Int GetVector1;

        private void Update()
        {
            var step = speed * Time.deltaTime;
            var currentPos = transform.position;
            targetPosition = targetPosition+ GetVector1;  
            transform.position = Vector3.MoveTowards(currentPos, targetPosition, step);

            var difference = targetPosition - currentPos;
            if (difference != Vector3.zero)
            {
                transform.rotation=Quaternion.LookRotation(targetPosition - currentPos);
            }
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                if (!moveList.Contains(targetPosition))
                {
                    moveList.Add(targetPosition);
                    print("targetPosition: " + targetPosition);
                }

                targetPosition = FindClosestRoad();
            }
        }

        public Vector3Int FindClosestRoad()
        {
            var neighbourTiles =
                PlacementManager.inst.placementGrid.GetWalkableAdjacentCells(targetPosition.x, targetPosition.z, true);
            if (moveList.Count > 1)
            {
                var lastPosition = moveList[0];
                if (neighbourTiles.Count > 1)
                {
                    neighbourTiles.Remove(new Point(lastPosition.x, lastPosition.z));
                }
                moveList.RemoveAt(0);
            }
            if (neighbourTiles.Count > 0)
            {
                Random rng = new Random();
                Utility.Shuffle(neighbourTiles, rng);
                var chosenPoint = neighbourTiles[0];
                return new Vector3Int(chosenPoint.X, 0, chosenPoint.Y);
            }

            return new Vector3Int(-300, 0, -300);
        }
    }
}