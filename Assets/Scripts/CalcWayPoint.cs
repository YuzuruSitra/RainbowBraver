using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WayPoint‚ÌŒvŽZ‹@
public class CalcWayPoint
{
    private int _allRoomCount = 0;
    private int _floorRoomCount;
    private float _roomWidth;
    private float _roomHeight;
    private List<Vector3> _roomInPoints = new List<Vector3>();
    public List<Vector3> RoomInPoints => _roomInPoints;
    private List<Vector3> _roomExitPoints = new List<Vector3>();
    public List<Vector3> RoomExitPoints => _roomExitPoints;
    private List<Vector3> _roomOutPoints = new List<Vector3>();
    public List<Vector3> RoomOutPoints => _roomOutPoints;

    public CalcWayPoint(int floorRoomCount, float roomWidth, float roomHeight)
    {
        _floorRoomCount = floorRoomCount;
        _roomWidth = roomWidth;
        _roomHeight = roomHeight;
    }

    public void SetWayPoints(int newRoomCount, Vector3 room0InPoint, Vector3 room0ExitPoint, Vector3 room0OutPoint)
    {
        for (int i = _allRoomCount; i < newRoomCount; i++)
        {
            _roomInPoints.Add(CalculateWayPoint(i, room0InPoint));
            _roomExitPoints.Add(CalculateWayPoint(i, room0ExitPoint));
            _roomOutPoints.Add(CalculateWayPoint(i, room0OutPoint));
        }
        _allRoomCount = newRoomCount;
        
    }

    private Vector3 CalculateWayPoint(int roomIndex, Vector3 basePoint)
    {
        float offsetX = (roomIndex % _floorRoomCount) * _roomWidth;
        float offsetY = roomIndex / (_floorRoomCount + 1) * _roomHeight;
        if (roomIndex > _floorRoomCount) offsetX -= _roomWidth;
        Vector3 roomInPoint = Vector3.zero;
        roomInPoint.x = basePoint.x + offsetX;
        roomInPoint.y = basePoint.y + offsetY;
        roomInPoint.z = basePoint.z;

        return roomInPoint;
    }

}
