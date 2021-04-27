using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partition 
{
    private int MIN_LEAF_SIZE = 6;

    public int x, y, width, height; //Position and dimension of the partition

    public Partition leftChild;
    public Partition rightChild;
    public Room room; //Room that is inside this partition
    public List<Hall> halls; //halls that connect this partition to other partitions

    Random r;

    public Partition(int X, int Y, int Width, int Height, int MIN_LEAF_SIZE)
    {
        this.MIN_LEAF_SIZE = MIN_LEAF_SIZE;
        x = X;
        y = Y;
        width = Width;
        height = Height;

        halls = new List<Hall>();

    }

    public bool Split()
    {
        // begin splitting the leaf into two children
        if (leftChild != null || rightChild != null)
            return false; // we're already split! Abort!

        // determine direction of split
        // if the width is >25% larger than height, we split vertically
        // if the height is >25% larger than the width, we split horizontally
        // otherwise we split randomly
        bool splitH = Random.value > 0.5;

        if (width > height && width / height >= 1.25)
            splitH = false;
        else if (height > width && height / width >= 1.25)
            splitH = true;

        int max = (splitH ? height : width) - MIN_LEAF_SIZE; // determine the maximum height or width
        if (max <= MIN_LEAF_SIZE)
            return false; // the area is too small to split any more...

        int split = Random.Range(MIN_LEAF_SIZE, max); // determine where we're going to split

        // create our left and right children based on the direction of the split
        if (splitH)
        {
            leftChild = new Partition(x, y, width, split, MIN_LEAF_SIZE);
            rightChild = new Partition(x, y + split, width, height - split, MIN_LEAF_SIZE);
        }
        else
        {
            leftChild = new Partition(x, y, split, height, MIN_LEAF_SIZE);
            rightChild = new Partition(x + split, y, width - split, height, MIN_LEAF_SIZE);
        }
        return true; // split successful!
    }


    public void createRooms()
    {
        // this function generates all the rooms and hallways for this Leaf and all of its children.
        if (leftChild != null || rightChild != null)
        {
            // this leaf has been split, so go into the children leafs
            if (leftChild != null)
            {
                leftChild.createRooms();
            }
            if (rightChild != null)
            {
                rightChild.createRooms();
            }

            // if there are both left and right children in this Leaf, create a hallway between them
            if (leftChild != null && rightChild != null)
            {
                createHall(leftChild.getRoom(), rightChild.getRoom());
            }
        }
        else
        {
            // this Leaf is the ready to make a room
            int roomSizeX;
            int roomSizeY;

            int roomPosX;
            int roomPosY;

            // the room can be between 3 x 3 tiles to the size of the leaf - 2.
            roomSizeX = Random.Range(3, width - 2);
            roomSizeY = Random.Range(3, height - 2);
            // place the room within the Leaf, but don't put it right 
            // against the side of the Leaf (that would merge rooms together)
            roomPosX = Random.Range(1, width - roomSizeX - 1);
            roomPosY = Random.Range(1, height - roomSizeY - 1);

            room = new Room(x + roomPosX, y + roomPosY, roomSizeX, roomSizeY);
        }
    }

    public Room getRoom()
    {
        // iterate all the way through these leafs to find a room, if one exists.
        if (room != null)
            return room;
        else
        {
            Room lRoom = null;
            Room rRoom = null;
            if (leftChild != null)
            {
                lRoom = leftChild.getRoom();
            }
            if (rightChild != null)
            {
                rRoom = rightChild.getRoom();
            }
            if (lRoom == null && rRoom == null)
                return null;
            else if (rRoom == null)
                return lRoom;
            else if (lRoom == null)
                return rRoom;
            else if (Random.value > .5)
                return lRoom;
            else
                return rRoom;
        }
    }


    public void createHall(Room l, Room r)
    {
        // connect these two rooms together with hallways.
        // it's  trying to figure out which point is where and then either draw a straight line, or a pair of lines to make a right-angle to connect them.

        int point1X = Random.Range(l.left + 1, l.right - 2);
        int point1Y = Random.Range(l.top + 1, l.bottom - 2);

        int point2X = Random.Range(r.left + 1, r.right - 2);
        int point2Y = Random.Range(r.top + 1, r.bottom - 2);

        int w = point2X - point1X;
        int h = point2Y - point1Y;

        if (w < 0)
        {
            if (h < 0)
            {


                if (Random.value < 0.5)
                {
                    halls.Add(new Hall(point2X, point1Y, Mathf.Abs(w), 1, l, r));
                    halls.Add(new Hall(point2X, point2Y, 1, Mathf.Abs(h), l, r));
                }
                else
                {
                    halls.Add(new Hall(point2X, point2Y, Mathf.Abs(w), 1, l, r));
                    halls.Add(new Hall(point1X, point2Y, 1, Mathf.Abs(h), l, r));
                }
            }

            //-----------------------------------------------------------------------------------------------------------------
            else if (h > 0)
            {
                halls.Add(new Hall(point2X, point1Y, Mathf.Abs(w), 1, l, r));
                halls.Add(new Hall(point2X, point1Y, 1, Mathf.Abs(h), l, r));

                //if (Random.value < 0.5)
                //{

                //}
                //else
                //{
                //    halls.Add(new Hall(point2X, point2Y, Mathf.Abs(w), 1, l, r));
                //    halls.Add(new Hall(point1X, point1Y, 1, Mathf.Abs(h), l, r));
                //}
        }
            else // if (h == 0)
            {
                halls.Add(new Hall(point2X, point2Y, Mathf.Abs(w), 1, l, r));
            }
        }
        else if (w > 0)
        {
            if (h < 0)
            {
                halls.Add(new Hall(point1X, point2Y, Mathf.Abs(w), 1, l, r));
                halls.Add(new Hall(point1X, point2Y, 1, Mathf.Abs(h), l, r));

                //if (Random.value < 0.5)
                //{

                //}
                //else
                //{
                //    halls.Add(new Hall(point1X, point1Y, Mathf.Abs(w), 1, l, r));
                //    halls.Add(new Hall(point2X, point2Y, 1, Mathf.Abs(h), l, r));
                //}
            }
            else if (h > 0)
            {
                halls.Add(new Hall(point1X, point1Y, Mathf.Abs(w), 1, l, r));
                halls.Add(new Hall(point2X, point1Y, 1, Mathf.Abs(h), l, r));

                //if (Random.value < 0.5)
                //{

                //}
                //else
                //{
                //    halls.Add(new Hall(point1X, point2Y, Mathf.Abs(w), 1, l, r));
                //    halls.Add(new Hall(point1X, point1Y, 1, Mathf.Abs(h), l, r));
                //}
            }
            else // if (h == 0)
            {
                halls.Add(new Hall(point1X, point1Y, Mathf.Abs(w), 1, l, r));
            }
        }
        else // if (w == 0)
        {
            if (h < 0)
            {
                halls.Add(new Hall(point2X, point2Y, 1, Mathf.Abs(h), l, r));
            }
            else if (h > 0)
            {
                halls.Add(new Hall(point1X, point1Y, 1, Mathf.Abs(h), l, r));
            }
        }
    }
}
